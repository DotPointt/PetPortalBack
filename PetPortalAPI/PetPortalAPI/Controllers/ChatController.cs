using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PetPortalAPI.Hubs;
using PetPortalCore.Abstractions;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.DTOs;
using PetPortalCore.DTOs.Requests;

namespace PetPortalAPI.Controllers;

/// <summary>
/// Контроллер чатов.
/// </summary>
[ApiController]
[Route("api/chat")]
public class ChatController : ControllerBase
{
    /// <summary>
    /// Сервис по чатам.
    /// </summary>
    private readonly IChatRoomService _chatRoomService;
    
    /// <summary>
    /// Сервис по сообщениям.
    /// </summary>
    private readonly IChatMessageService _chatMessageService;
    
    /// <summary>
    /// Хаб чатов.
    /// </summary>
    private readonly IHubContext<ChatHub, IChatClient> _hubContext;

    /// <summary>
    /// Конструктор контроллера.
    /// </summary>
    /// <param name="chatRoomService">Сервис по чатам.</param>
    /// <param name="chatMessageService">Сервис по сообщениям.</param>
    /// <param name="hubContext">Хаб чатов.</param>
    public ChatController(
        IChatRoomService chatRoomService,
        IChatMessageService chatMessageService,
        IHubContext<ChatHub, IChatClient> hubContext)
    {
        _chatRoomService = chatRoomService;
        _chatMessageService = chatMessageService;
        _hubContext = hubContext;
    }

    /// <summary>
    /// Получить чаты пользователя.
    /// </summary>
    [HttpGet("rooms/{userId:guid}")]
    public async Task<ActionResult<List<ChatRoomDto>>> GetUserChatRooms(Guid userId)
    {
        var rooms = await _chatRoomService.GetUserChatRoomsAsync(userId);
        return Ok(rooms);
    }

    /// <summary>
    /// Получить чат по идентификатору.
    /// </summary>
    [HttpGet("room/{roomId:guid}")]
    public async Task<ActionResult<ChatRoomDto>> GetRoom(Guid roomId)
    {
        var room = await _chatRoomService.GetByIdAsync(roomId);
        if (room is null)
            return NotFound();
        return Ok(room);
    }

    /// <summary>
    /// Создать чат с именем и участниками.
    /// </summary>
    [HttpPost("room")]
    public async Task<ActionResult<ChatRoomDto>> CreateRoom([FromBody] CreateChatRoomRequest request)
    {
        var chat = await _chatRoomService.CreateNamedChatAsync(request.Name, request.UserIds);
        return CreatedAtAction(nameof(GetRoom), new { roomId = chat.Id }, chat);
    }

    /// <summary>
    /// Получить все сообщения из комнаты.
    /// </summary>
    [HttpGet("messages/{roomId:guid}")]
    public async Task<ActionResult<List<ChatMessageDto>>> GetRoomMessages(Guid roomId)
    {
        var messages = await _chatMessageService.GetMessagesByRoomIdAsync(roomId);
        return Ok(messages);
    }

    /// <summary>
    /// Отправить сообщение в чат.
    /// </summary>
    [HttpPost("messages/send")]
    public async Task<ActionResult<Guid>> SendMessage([FromBody] SendMessageRequest request)
    {
        var messageId = await _chatMessageService.AddAsync(request.Message, request.SenderId, request.ChatRoomId);
        return Ok(messageId);
    }

    /// <summary>
    /// Получить последние N сообщений.
    /// </summary>
    [HttpGet("messages/{roomId:guid}/last/{count:int}")]
    public async Task<ActionResult<List<ChatMessageDto>>> GetLastMessages(Guid roomId, int count)
    {
        var messages = await _chatMessageService.GetLastMessagesAsync(roomId, count);
        return Ok(messages);
    }

    /// <summary>
    /// Удалить сообщение.
    /// </summary>
    [HttpDelete("messages/{messageId:guid}")]
    public async Task<ActionResult<Guid>> DeleteMessage(Guid messageId)
    {
        var deletedId = await _chatMessageService.DeleteAsync(messageId);
        return Ok(deletedId);
    }
}
