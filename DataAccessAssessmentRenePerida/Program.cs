using Pocos;
using Repos;

var connectionString = @"Data Source=.\SQLEXPRESS; Initial Catalog=CompanyFdm;Integrated Security=sspi;";

var employeeRepo = new EmployeeRepository(connectionString);

var departmentRepo = new DepartmentRepository(connectionString);

var academy = new Department { Id = 1, DeptName = "Academy"};
var sales = new Department { Id = 2, DeptName = "Sales" };
var temporary = new Department { Id = 3, DeptName = "Temporary" };

departmentRepo.CreateNewRow(academy);
departmentRepo.CreateNewRow(sales);
departmentRepo.CreateNewRow(temporary);

var johnDoe = new Employee { FirstName = "John", LastName = "Doe", Dept = academy};
var janeSmith = new Employee { FirstName = "Jane", LastName = "Smith", Dept = sales };
var joeBloggs = new Employee { FirstName = "Joe", LastName = "Bloggs", Dept = academy };
var joeSchmoe = new Employee { FirstName = "Joe", LastName = "Schmoe", Dept = temporary };

employeeRepo.CreateNewRow(johnDoe);
employeeRepo.CreateNewRow(janeSmith);
employeeRepo.CreateNewRow(joeBloggs);
employeeRepo.CreateNewRow(joeSchmoe);

var retrievedDepartment = departmentRepo.ReadRowById(1);
if(!object.ReferenceEquals(retrievedDepartment, null))
{
    Console.WriteLine($"\n The retrieved deparment is : {retrievedDepartment}");
}

var retrievedEmployee = employeeRepo.ReadRowById(1);
if (!object.ReferenceEquals(retrievedEmployee, null))
{
    Console.WriteLine($"\n The retrieved employee is : {retrievedEmployee}");
}

var permanentDepartment = new Department { Id = 3, DeptName = "Permanent" };
departmentRepo.UpdateRow(permanentDepartment);

var renePerida = new Employee();
renePerida.Id = 4;
renePerida.FirstName = "Rene";
renePerida.LastName = "Perida";
renePerida.Dept = permanentDepartment;
employeeRepo.UpdateRow(renePerida);

var retrievedDepartmentByName = departmentRepo.ReadRowByName("Sales");
if (!object.ReferenceEquals(retrievedDepartmentByName, null))
{
    Console.WriteLine($"\n The retrieved first deparment is : {retrievedDepartmentByName}");
}

var retrievedEmployeeByName = employeeRepo.ReadRowByName("Jane", "Smith");
if (!object.ReferenceEquals(retrievedEmployeeByName, null))
{
    Console.WriteLine($"\n The retrieved first employee is : {retrievedEmployeeByName}");
}

//employeeRepo.DeleteRow(4);
//departmentRepo.DeleteRow(3);

//employeeRepo.DeleteRow(renePerida);
//departmentRepo.DeleteRow(permanentDepartment);

var departments = departmentRepo.ReadAllRows();
foreach (Department department in departments)
{
    Console.WriteLine(department);
}

var employees = employeeRepo.ReadAllRows();
foreach (Employee employee in employees)
{
    Console.WriteLine(employee);
}