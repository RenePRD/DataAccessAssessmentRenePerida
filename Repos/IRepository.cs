using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos
{
    public interface IRepository<T>
    {
        bool CreateNewRow(T entity);
        IEnumerable<T> ReadAllRows();
        T? ReadRowById(int id);
        bool UpdateRow(T entity);
        bool DeleteRow(int id);
        bool DeleteRow(T entity);
    }
}
