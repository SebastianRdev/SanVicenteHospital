namespace SanVicenteHospital.repositories;

using SanVicenteHospital.data;
using SanVicenteHospital.models;
using SanVicenteHospital.interfaces;

/// <summary>
/// Repository specializing in managing entities using dictionaries in the HealthClinic system.
/// Allows you to add, query, search, delete, and update entities by their unique identifier.
/// </summary>
/// <typeparam name="Entity">Type of entity managed by the repository</typeparam>
public class RepositoryDict<Entity> : IRepository<Entity> where Entity : class, IEntity
{
    /// <summary>
    /// Internal dictionary that stores entities indexed by Guid.
    /// </summary>
    private readonly Dictionary<Guid, Entity> _dataDict;

    /// <summary>
    /// Initialize the repository and link the corresponding dictionary from the database.
    /// </summary>
    public RepositoryDict()
    {
        if (typeof(Entity) == typeof(Patient))
            _dataDict = (Dictionary<Guid, Entity>)(object)Database.PatientsDict;
        else if (typeof(Entity) == typeof(Doctor))
            _dataDict = (Dictionary<Guid, Entity>)(object)Database.DoctorsDict;
        else
            throw new InvalidOperationException($"No dictionary defined for {typeof(Entity).Name}");
    }

    /// <summary>
    /// Adds a new entity to the dictionary.
    /// </summary>
    /// <param name="entity">Entity to add</param>
    public void Add(Entity entity)
    {
        _dataDict.TryAdd(entity.Id, entity);
    }

    /// <summary>
    /// Search for an entity by its unique identifier
    /// </summary>
    /// <param name="id">Entity identifier</param>
    /// <returns>Entity found or null if it does not exist</returns>
    public Entity? GetById(Guid id)
    {
        _dataDict.TryGetValue(id, out var entity);
        return entity;
    }

    /// <summary>
    /// Gets all entities stored in the dictionary.
    /// </summary>
    /// <returns>List of entities</returns>
    public List<Entity> GetAll()
    {
        return _dataDict.Values.ToList();
    }

    /// <summary>
    /// Get the complete dictionary of entities.
    /// </summary>
    /// <returns>Dictionary of entities</returns>
    public Dictionary<Guid, Entity> GetDictionary()
    {
        return _dataDict;
    }

    /// <summary>
    /// Removes an entity from the dictionary by its unique identifier.
    /// </summary>
    /// <param name="id">Identifier of the entity to be deleted.</param>
    public void Remove(Guid id)
    {
        _dataDict.Remove(id);
    }

    /// <summary>
    /// Updates an existing entity in the dictionary.
    /// </summary>
    /// <param name="entity">Entity to be updated</param>
    public void Update(Entity entity)
    {
        if (_dataDict.ContainsKey(entity.Id))
            _dataDict[entity.Id] = entity;
    }
}
