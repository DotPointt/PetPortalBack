using System.ComponentModel;

namespace PetPortalCore.DTOs.Requests;

public class ProjectRequest
{
    /// <summary>
    /// asc or desc
    /// </summary>
    public string? SortOrder { get; set; }
    
    /// <summary>
    /// Item by which projects sorted: "date", "name", "applyingdeadline"
    /// </summary>
    /// <example>date</example>
    public string? SortItem { get; set; }
    [DefaultValue(10)]
    public int offset { get; set; } = 10;
    [DefaultValue(1)]
    public int page { get; set; } = 1;
}