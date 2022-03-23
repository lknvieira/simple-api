namespace simple_api.Models
{
    public class Teacher : Entity
    {
        public string Name { get; private set; }

        public IList<Student> Students { get; private set; } = new List<Student>();

        public Teacher(string name)
        {
            AddOrUpdateName(name);
        }

        public void AddOrUpdateName(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("The property Name can not be null");
            Name = name;
        }

        public void AddStudent(Student student)
        {
            if (student == null) throw new ArgumentNullException("Student can not be null");
            Students.Add(student);
        }

        public void RemoveStudent(Student student) 
        {
            Students.Remove(student);
        }

    }
}
