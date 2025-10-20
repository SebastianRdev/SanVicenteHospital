namespace SanVicenteHospital.interfaces;

// Interface that defines an entity with a unique identifier in the SanVicenteHospital system.
public interface IEntity
{
    // Unique identifier for the entity.
    Guid Id { get; }
}