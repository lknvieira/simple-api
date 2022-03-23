namespace simple_api.Models
{
    public class Student : Entity
    {
        public string Name { get; init; }

        public Student(string name)
        {
            if(string.IsNullOrEmpty(name)) throw new ArgumentNullException("The property Name can not be null");
            Name = name;
        }
    }
}
