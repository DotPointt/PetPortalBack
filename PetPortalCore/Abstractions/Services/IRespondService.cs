using PetPortalCore.DTOs;

namespace PetPortalCore.Abstractions.Services;

/// <summary>
/// Интерфейс работы откликов.
/// </summary>
public interface IRespondService
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<List<RespondDto>> GetAllResponds();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<List<RespondDto>> GetRespondsByUserId(Guid userId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="projectId"></param>
    /// <returns></returns>
    Task<List<RespondDto>> GetRespondsByProjectId(Guid projectId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="respondDto"></param>
    /// <returns></returns>
    Task<bool> CreateRespond(RespondDto respondDto);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="respondId"></param>
    /// <returns></returns>
    Task<bool> DeleteRespond(Guid respondId);
}