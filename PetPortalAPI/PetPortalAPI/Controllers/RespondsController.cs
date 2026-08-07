using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PetPortalAPI.Hubs;
using PetPortalApplication.Helpers;
using PetPortalCore.Abstractions;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.Contracts;
using PetPortalCore.DTOs;
using PetPortalDAL;

namespace PetPortalAPI.Controllers;
/// <summary>
/// Контроллер работы откликов.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class RespondsController : ControllerBase
{
    private readonly IRespondService _respondService;
    private readonly IUserService _userService;
    private readonly IProjectsService _projectsService;
    private readonly IUserProjectService _membersService;
    private readonly IChatRoomService _chatRoomService;
    private readonly IChatMessageService _chatMessageService;
    private readonly IHubContext<ChatHub, IChatClient> _hubContext;
    private readonly IMailSenderService _mailSender;
    private readonly PetPortalDbContext _db;

    public RespondsController(
        IRespondService respondService,
        IUserService userService,
        IProjectsService projectsService,
        IUserProjectService membersService,
        IChatRoomService chatRoomService,
        IChatMessageService chatMessageService,
        IHubContext<ChatHub, IChatClient> hubContext,
        IMailSenderService mailSender,
        PetPortalDbContext db)
    {
        _respondService = respondService;
        _userService = userService;
        _projectsService = projectsService;
        _membersService = membersService;
        _chatRoomService = chatRoomService;
        _chatMessageService = chatMessageService;
        _hubContext = hubContext;
        _mailSender = mailSender;
        _db = db;
    }
    [HttpGet("AllResponds")]
    [Authorize]
    public async Task<ActionResult<List<RespondDto>>> GetAllResponds()
    {
        try
        {
            var responds = await _respondService.GetAllResponds();
            return Ok(responds);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpGet("RespondsByUser/{userId}")]
    [Authorize]
    public async Task<ActionResult<List<RespondDto>>> GetRespondsByUserId(Guid userId)
    {
        try
        {
            var responds = await _respondService.GetRespondsByUserId(userId);
            return Ok(responds);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpGet("RespondsByProject/{projectId}")]
    [Authorize]
    public async Task<ActionResult<List<RespondDto>>> GetRespondsByProjectId(Guid projectId)
    {
        try
        {
            var responds = await _respondService.GetRespondsByProjectId(projectId);
            return Ok(responds);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpPost("AddRespond")]
    [Authorize]
    public async Task<IActionResult> CreateRespond([FromBody] RespondCreateContract respondCreateContract)
    {
        try
        {
            var userId = await _userService.GetUserIdFromJWTAsync(User);
            if (userId == null)
                return Unauthorized();

            var project = await _projectsService.GetById(respondCreateContract.ProjectId);
            if (project.OwnerId == userId.Value)
                return BadRequest("Нельзя откликнуться на собственный проект.");

            var already = await _db.Responds.AnyAsync(r =>
                r.UserId == userId.Value && r.ProjectId == respondCreateContract.ProjectId);
            if (already)
                return Conflict("Вы уже откликнулись на этот проект.");

            var id = Guid.NewGuid();
            var respondDto = new RespondDto()
            {
                Id = id,
                Role = respondCreateContract.Role,
                Comment = respondCreateContract.Comment,
                UserId = userId.Value,
                ProjectId = respondCreateContract.ProjectId,
                Status = "Pending",
            };

            await _respondService.CreateRespond(respondDto);

            Guid? chatRoomId = null;
            try
            {
                var roomName = $"Отклик · {project.Id:N} · {userId.Value:N}";
                var existingId = await _chatRoomService.GetChatRoomIdByNameAsync(roomName);
                var isNewRoom = false;
                if (existingId.HasValue && existingId.Value != Guid.Empty)
                {
                    chatRoomId = existingId.Value;
                }
                else
                {
                    var room = await _chatRoomService.CreateNamedChatAsync(
                        roomName,
                        new List<Guid> { project.OwnerId, userId.Value });
                    chatRoomId = room.Id;
                    isNewRoom = true;
                }

                // Первое системное сообщение при создании чата по отклику
                if (isNewRoom && chatRoomId.HasValue)
                {
                    var responder = await _db.Users.AsNoTracking()
                        .Where(u => u.Id == userId.Value)
                        .Select(u => u.Name)
                        .FirstOrDefaultAsync();
                    var displayName = string.IsNullOrWhiteSpace(responder) ? "Пользователь" : responder.Trim();
                    var systemText = $"{displayName} Откликнулся на ваш проект.";
                    var systemMessage = await _chatMessageService.AddAsync(
                        systemText, userId.Value, chatRoomId.Value);

                    await _hubContext.Clients.Group(ChatHub.UserGroup(project.OwnerId))
                        .ReceiveMessage(systemMessage);
                    await _hubContext.Clients.Group(ChatHub.UserGroup(userId.Value))
                        .ReceiveMessage(systemMessage);
                }
            }
            catch (Exception chatEx)
            {
                Console.WriteLine($"Chat create on respond failed: {chatEx.Message}");
            }

            try
            {
                var owner = await _userService.GetUserById(project.OwnerId);
                var responder = await _userService.GetUserById(userId.Value);
                var link = $"{AppUrls.FrontendBase}/account/project-responses";
                var body = EmailTemplates.NewRespond(
                    owner.Name,
                    responder.Name,
                    project.Name,
                    respondCreateContract.Comment ?? "",
                    link);
                await _mailSender.SendEmailAsync(
                    owner.Email,
                    "Новый отклик на ваш проект — PetPortal",
                    body,
                    isBodyHtml: true);
            }
            catch (Exception mailEx)
            {
                Console.WriteLine($"Respond notification email failed: {mailEx.Message}");
            }

            return Ok(new { success = true, respondId = id, chatRoomId });        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    /// <summary>
    /// Принять отклик: добавить в команду и создать чат.
    /// </summary>
    [HttpPost("Accept/{respondId:guid}")]
    [Authorize]
    public async Task<ActionResult> AcceptRespond(Guid respondId)
    {
        try
        {
            var ownerId = await _userService.GetUserIdFromJWTAsync(User);
            if (ownerId == null)
                return Unauthorized();

            var respond = await _db.Responds.FirstOrDefaultAsync(r => r.Id == respondId);
            if (respond == null)
                return NotFound("Отклик не найден");

            var project = await _projectsService.GetById(respond.ProjectId);
            if (project.OwnerId != ownerId.Value)
                return Forbid();

            if (respond.Status == "Accepted")
                return Ok(new { alreadyAccepted = true });

            await _membersService.AddProjectMember(respond.UserId, respond.ProjectId);

            Guid chatRoomId;
            var inquiryName = $"Отклик · {project.Id:N} · {respond.UserId:N}";
            var existingInquiry = await _chatRoomService.GetChatRoomIdByNameAsync(inquiryName);
            if (existingInquiry.HasValue && existingInquiry.Value != Guid.Empty)
            {
                chatRoomId = existingInquiry.Value;
            }
            else
            {
                var teamName = $"Команда · {project.Id:N} · {respond.UserId:N}";
                var existingTeam = await _chatRoomService.GetChatRoomIdByNameAsync(teamName);
                if (existingTeam.HasValue && existingTeam.Value != Guid.Empty)
                {
                    chatRoomId = existingTeam.Value;
                }
                else
                {
                    var room = await _chatRoomService.CreateNamedChatAsync(
                        teamName,
                        new List<Guid> { ownerId.Value, respond.UserId });
                    chatRoomId = room.Id;
                }
            }

            respond.Status = "Accepted";
            await _db.SaveChangesAsync();

            return Ok(new { accepted = true, chatRoomId });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpDelete("DeleteRespond/{respondId}")]
    [Authorize]
    public async Task<ActionResult<bool>> DeleteRespond(Guid respondId)
    {
        try
        {
            var res = await _respondService.DeleteRespond(respondId);
            return Ok(res);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }
}
