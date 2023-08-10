using Pocos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Employee? ReadRowByName(string firstName, string lastName); 
    }
}
