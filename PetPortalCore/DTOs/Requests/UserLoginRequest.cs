namespace PetPortalCore.DTOs.Requests;

/// <summary>
/// UserLoginRequest
/// </summary>
public class UserLoginRequest
{
    /// <summary>
    /// Username
    /// </summary>
    public string Username { get; set;  }
    
    /// <summary>
    /// Email
    /// </summary>
    public string Email  { get; set;  }
    
    /// <summary>
    /// Password
    /// </summary>
    public string Password { get; set;  }
}