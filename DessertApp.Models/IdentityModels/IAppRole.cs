namespace DessertApp.Models.IdentityModels
{
    /// <summary>
    /// Defines a contract for application roles in the system.
    /// This interface represents the basic properties that all roles must
    /// have, including a description of the role.
    /// </summary>
    public interface IAppRole
    {
        string Id { get; set; }
        string? Name { get; set; }
        string? NormalizedName { get; set; }
        string? ConcurrencyStamp { get; set; }
        string Description { get; set; }
    }
}
