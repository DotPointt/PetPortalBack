using PetPortalCore.Abstractions.Repositories;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.DTOs;

namespace PetPortalApplication.Services;

/// <summary>
/// Сервис работы откликов.
/// </summary>
public class RespondService : IRespondService
{
    /// <summary>
    /// 
    /// </summary>
    private readonly IRespondRepository _respondRepository;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="respondRepository"></param>
    public RespondService(IRespondRepository respondRepository)
    {
        _respondRepository = respondRepository;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Task<List<RespondDto>> GetAllResponds()
    {
        return _respondRepository.GetAllResponds();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public Task<List<RespondDto>> GetRespondsByUserId(Guid userId)
    {
        return _respondRepository.GetRespondsByUserId(userId);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="projectId"></param>
    /// <returns></returns>
    public Task<List<RespondDto>> GetRespondsByProjectId(Guid projectId)
    {
        return _respondRepository.GetRespondsByProjectId(projectId);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="respondDto"></param>
    /// <returns></returns>
    public Task<bool> CreateRespond(RespondDto respondDto)
    {
        return _respondRepository.CreateRespond(respondDto);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="respondId"></param>
    /// <returns></returns>
    public Task<bool> DeleteRespond(Guid respondId)
    {
        return _respondRepository.DeleteRespond(respondId);
    }
}