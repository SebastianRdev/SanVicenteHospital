namespace SanVicenteHospital.repositories;

using SanVicenteHospital.data;
using SanVicenteHospital.models;
using SanVicenteHospital.interfaces;

// Repository specializing in managing entities using dictionaries in the SanVicenteHospital system.
// Allows you to add, query, search, delete, and update entities by their unique identifier.
public class RepositoryDict<Entity> : IRepository<Entity> where Entity : class, IEntity
{
    // Internal dictionary that stores entities indexed by Guid.
    private readonly Dictionary<Guid, Entity> _dataDict;

    // Initialize the repository and link the corresponding dictionary from the database.
    public RepositoryDict()
    {
        if (typeof(Entity) == typeof(Patient))
            _dataDict = (Dictionary<Guid, Entity>)(object)Database.PatientsDict;
        else if (typeof(Entity) == typeof(Doctor))
            _dataDict = (Dictionary<Guid, Entity>)(object)Database.DoctorsDict;
        else
            throw new InvalidOperationException($"No dictionary defined for {typeof(Entity).Name}");
    }

    // Adds a new entity to the dictionary.
    public void Add(Entity entity)
    {
        _dataDict.TryAdd(entity.Id, entity);
    }

    // Search for an entity by its unique identifier
    public Entity? GetById(Guid id)
    {
        _dataDict.TryGetValue(id, out var entity);
        return entity;
    }

    // Gets all entities stored in the dictionary.
    public List<Entity> GetAll()
    {
        return _dataDict.Values.ToList();
    }

    // Get the complete dictionary of entities.
    public Dictionary<Guid, Entity> GetDictionary()
    {
        return _dataDict;
    }

    // Removes an entity from the dictionary by its unique identifier.
    public void Remove(Guid id)
    {
        _dataDict.Remove(id);
    }

    // Updates an existing entity in the dictionary.
    public void Update(Entity entity)
    {
        if (_dataDict.ContainsKey(entity.Id))
            _dataDict[entity.Id] = entity;
    }
}
