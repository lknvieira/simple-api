namespace simple_api.Models
{
    public abstract class Entity
    {
        public Guid Id { get; init; }

        public Entity()
        {
            Id = Guid.NewGuid();
        }
    }
}
