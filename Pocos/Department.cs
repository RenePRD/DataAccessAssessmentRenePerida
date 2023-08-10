namespace Pocos
{
    //public class Department
    //{
    //    public int Id { get; set; }
    //    public string DeptName { get; set; }
    //    public ISet<Employee> Employees { get; set; }

    //    public override string ToString()
    //    {
    //        return $"[ id={Id}, name={DeptName} ]";
    //    }
    //}

    public class Department
    {
        public Department()
        {
            Employees = new HashSet<Employee>();
        }

        public int Id { get; set; }
        public string DeptName { get; set; }
        public ISet<Employee> Employees { get; set; }

        public override string ToString()
        {
            string employeeNames = string.Join(", ", Employees.Select(e => e.FirstName + " " + e.LastName));
            return $"[ id={Id}, name={DeptName}, employees=[ {employeeNames} ] ]";
        }
    }

}