namespace WebService.Models;

/// <summary>
/// Any request that just needs to specify a person,
/// such as nominating someone for membership or a
/// position.
/// </summary>
public class GenericPersonRequest : GenericUserRequest
{
    public string? PersonId { get; set; }
}