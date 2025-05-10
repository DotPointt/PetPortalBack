using Mapster;
using PetPortalCore.Abstractions.Services;
using PetPortalCore.DTOs;
using PetPortalCore.Models;
using PetPortalDAL.Entities;

namespace PetPortalDAL.Mappers;

public class RegisterMapper : IRegister
{
    private readonly IPasswordHasher _passwordHasher;

    public RegisterMapper(IPasswordHasher passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }
    
    /** */
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UserDto, UserEntity>()
            .Map(adto => adto.PasswordHash, a => _passwordHasher.HashPassword(a.Password) )                
            .RequireDestinationMemberSource(true);
    }
}