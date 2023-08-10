namespace Pocos
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Department Dept { get; set; }

        public override string ToString()
        {
            return $"[ id = {Id}, name={FirstName} {LastName}, department= {Dept} ]";
        }
    }
}