using Pocos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        Department? ReadRowByName(string name);
    }
}
