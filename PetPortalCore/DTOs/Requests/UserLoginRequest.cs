namespace PetPortalCore.DTOs.Requests;

/// <summary>
/// UserLoginRequest
/// </summary>
public class UserLoginRequest
{
    /// <summary>
    /// Email
    /// </summary>
    public string Email  { get; set;  }
    
    /// <summary>
    /// Password
    /// </summary>
    public string Password { get; set;  }
}