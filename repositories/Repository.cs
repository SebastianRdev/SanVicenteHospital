namespace SanVicenteHospital.repositories;


using SanVicenteHospital.interfaces;
using SanVicenteHospital.data;
using SanVicenteHospital.models;


// Generic repository for managing entities in lists within the SanVicenteHospital system.
// Allows you to add, query, search, and delete entities from the in-memory database.
public class Repository<Entity> : IRepository<Entity> where Entity : class, IEntity
{
    // Internal list that stores entities.
    private readonly List<Entity> _dataList;

    // Initialize the repository and link the corresponding list from the database.
    public Repository()
    {
        if (typeof(Entity) == typeof(Appointment))
            _dataList = (List<Entity>)(object)Database.Appointments;
        else if (typeof(Entity) == typeof(EmailLog))
            _dataList = (List<Entity>)(object)Database.EmailLogs;
        else
            throw new InvalidOperationException($"There is no list for {typeof(Entity).Name} in Database");
    }

    // Add a new entity to the list.
    public void Add(Entity entity) => _dataList.Add(entity);

    // Gets all entities stored in the list.
    public List<Entity> GetAll() => _dataList;

    // Search for an entity by its unique identifier.
    public Entity? GetById(Guid id) => _dataList.FirstOrDefault(e => e.Id == id);

    // Removes an entity from the list by its unique identifier.
    public void Remove(Guid id)
    {
        var entity = GetById(id);
        if (entity != null)
            _dataList.Remove(entity);
    }

    // Update an existing entity in the list.
    public void Update(Entity entity)
    {
        var index = _dataList.FindIndex(e => e.Id == entity.Id);

        if (index >= 0)
        {
            _dataList[index] = entity;
        }
        else
        {
            Console.WriteLine("❌ Entity not found for update");
        }
    }

}

