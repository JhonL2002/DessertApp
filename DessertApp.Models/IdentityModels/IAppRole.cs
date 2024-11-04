namespace DessertApp.Models.IdentityModels
{
    /// <summary>
    /// Defines a contract for application roles in the system.
    /// This interface represents the basic properties that all roles must
    /// have, including a description of the role.
    /// </summary>
    public interface IAppRole
    {
        string Description { get; set; }
    }
}
