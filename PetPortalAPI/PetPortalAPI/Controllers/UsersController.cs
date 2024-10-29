using Microsoft.AspNetCore.Mvc;
using PetPortalAPI.Contracts;
using PetPortalCore.Abstractions.Services;

namespace PetPortalAPI.Controllers
{
    /// <summary>
    /// Users controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        /// <summary>
        /// Users service.
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// Users controller constructor.
        /// </summary>
        /// <param name="userService"></param>
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        
        /// <summary>
        /// Endpoint get users.
        /// </summary>
        /// <returns>
        /// Action result - List of users or
        /// Action result - error message.
        /// </returns>
        [HttpGet]
        public async Task<ActionResult<List<UsersResponse>>> GetUsers()
        {  
            try
            {
                var users = await _userService.GetAll();
                var response = users.Select(p => new UsersResponse(p.Id, p.Name, p.Email, p.PasswordHash));

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        
        /// <summary>
        /// Endpoint create user.
        /// </summary>
        /// <param name="request">User data.</param>
        /// <returns>
        /// Action result - created user guid or
        /// Action result - error message.
        /// </returns>
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateProject([FromBody] UsersRequest request)
        {
            try
            {
                var (user, error) = PetPortalCore.Models.User.Create(
                    Guid.NewGuid(),
                    request.Name,
                    request.Email,
                    request.Password);
                
                if (!string.IsNullOrEmpty(error))
                {
                    return BadRequest(error);
                }
                
                var userGuid = await _userService.Create(user);

                return Ok(userGuid);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        
        /// <summary>
        /// Endpoint update user.
        /// </summary>
        /// <param name="id">User identifier.</param>
        /// <param name="request">User data.</param>
        /// <returns>
        /// Action result - updated user guid or
        /// Action result - error message.
        /// </returns>
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Guid>> UpdateProject(Guid id, [FromBody] UsersRequest request)
        {
            try
            {
                var userId = await _userService.Update(id, request.Name, request.Email, request.Password);
                
                return Ok(userId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        
        /// <summary>
        /// Endpoint delete user.
        /// </summary>
        /// <param name="id">User identifier.</param>
        /// <returns>
        /// Action result - deleted user guid or
        /// Action result - error message.
        /// </returns>
        [HttpDelete]
        public async Task<ActionResult<Guid>> DeleteProject([FromBody] Guid id)
        {
            try
            {
                var userId = await _userService.Delete(id);

                return Ok(userId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }    
        }
    }
}