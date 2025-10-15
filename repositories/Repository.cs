namespace SanVicenteHospital.repositories;


using SanVicenteHospital.interfaces;
using SanVicenteHospital.data;
using SanVicenteHospital.models;

/// <summary>
/// Generic repository for managing entities in lists within the SanVicenteHospital system.
/// Allows you to add, query, search, and delete entities from the in-memory database.
/// </summary>
/// <typeparam name="Entity">Type of entity managed by the repository.</typeparam>
public class Repository<Entity> : IRepository<Entity> where Entity : class, IEntity
{
    /// <summary>
    /// Internal list that stores entities.
    /// </summary>
    private readonly List<Entity> _dataList;

    /// <summary>
    /// Initialize the repository and link the corresponding list from the database.
    /// </summary>
    public Repository()
    {
        if (typeof(Entity) == typeof(Appointment))
            _dataList = (List<Entity>)(object)Database.Appointments;
        else if (typeof(Entity) == typeof(EmailLog))
            _dataList = (List<Entity>)(object)Database.EmailLogs;
        else
            throw new InvalidOperationException($"There is no list for {typeof(Entity).Name} in Database");
    }

    /// <summary>
    /// Add a new entity to the list.
    /// </summary>
    /// <param name="entity">Entity to add</param>
    public void Add(Entity entity) => _dataList.Add(entity);

    /// <summary>
    /// Gets all entities stored in the list.
    /// </summary>
    /// <returns>List of entities</returns>
    public List<Entity> GetAll() => _dataList;

    /// <summary>
    /// Search for an entity by its unique identifier.
    /// </summary>
    /// <param name="id">Entity identifier</param>
    /// <returns>Entity found or null if it does not exist</returns>
    public Entity? GetById(Guid id) => _dataList.FirstOrDefault(e => e.Id == id);

    /// <summary>
    /// Removes an entity from the list by its unique identifier.
    /// </summary>
    /// <param name="id">Identifier of the entity to be deleted</param>
    public void Remove(Guid id)
    {
        var entity = GetById(id);
        if (entity != null)
            _dataList.Remove(entity);
    }

    /// <summary>
    /// Update an existing entity in the list.
    /// </summary>
    /// <param name="entity">The updated entity</param>
    public void Update(Entity entity)
    {
        var index = _dataList.FindIndex(e => e.Id == entity.Id);

        if (index >= 0)
        {
            _dataList[index] = entity;
        }
        else
        {
            Console.WriteLine("❌ Entity not found for update.");
        }
    }

}

