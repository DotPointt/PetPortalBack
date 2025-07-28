using PetPortalCore.DTOs;

namespace PetPortalCore.Abstractions.Repositories;

/// <summary>
/// Интерфейс работы откликов на базе данных.
/// </summary>
public interface IRespondRepository
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
    /// <exception cref="Exception"></exception>
    Task<bool> DeleteRespond(Guid respondId);
}