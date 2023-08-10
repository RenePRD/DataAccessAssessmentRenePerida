using Pocos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Repos
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private string _connectionString;

        public DepartmentRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool CreateNewRow(Department department)
        {
            var isCreated = false;
            using (IDbConnection conn = new SqlConnection()) 
            {
                conn.ConnectionString = _connectionString;
                try
                {
                    conn.Open();
                    using (IDbCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        //INSERT INTO [DbNba].[dbo].players (first_name, last_name, career_points) VALUES ('Curly', 'Neal', 22);
                        var sqlStmt = "INSERT INTO [CompanyFdm].[dbo].DEPARTMENTS (DEPT_NAME) VALUES (@dName)";
                        cmd.CommandText = sqlStmt;
                        IDbDataParameter param = new SqlParameter();
                        param.ParameterName = "@dName";
                        param.Value = department.DeptName;
                        cmd.Parameters.Add(param);
                        var numberAffectedRows = cmd.ExecuteNonQuery();
                        Console.WriteLine($"Created {numberAffectedRows} row(s)");
                        if (numberAffectedRows > 0)
                        {
                            isCreated = true;
                        }
                    }
                } catch (SqlException e)
                {
                    Console.WriteLine($"caught - {e.GetType()}: {e.Message}");
                }
            }
            return isCreated;
        }

        public bool DeleteRow(int id)
        {
            var isDeleted = false;
            using (IDbConnection conn = new SqlConnection())
            {
                conn.ConnectionString = _connectionString;
                try
                {
                    conn.Open();
                    using (IDbCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        var sqlStmt = "DELETE FROM [CompanyFdm].[dbo].DEPARTMENTS WHERE DEPT_NO = @Id";
                        cmd.CommandText = sqlStmt;
                        IDbDataParameter param = new SqlParameter();
                        param.ParameterName = "@Id";
                        param.Value = id;
                        cmd.Parameters.Add(param);
                        var numberAffectedRows = cmd.ExecuteNonQuery();
                        if (numberAffectedRows > 0)
                        {
                            isDeleted = true;
                        }
                    }
                }
                catch (SqlException e)
                {
                    Console.WriteLine($"caught - {e.GetType()}: {e.Message}");
                }
                catch (System.Data.SqlTypes.SqlNullValueException e)
                {
                    Console.WriteLine($"caught - {e.GetType()}: {e.Message}");
                }
            }
            return isDeleted;
        }

        public bool DeleteRow(Department entity)
        {
            return DeleteRow(entity.Id);
        }

        public IEnumerable<Department> ReadAllRows()
        {
            List<Department> departments = new List<Department>();
            using (IDbConnection conn = new SqlConnection())
            {
                conn.ConnectionString = _connectionString;
                try
                {
                    conn.Open();
                    using (IDbCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        var sqlQuery = "SELECT d.DEPT_NO, d.DEPT_NAME, e.EMP_ID, e.EMP_FST_NAME, e.EMP_LST_NAME " +
                            "FROM [CompanyFdm].[dbo].DEPARTMENTS d " +
                            "LEFT JOIN [CompanyFdm].[dbo].EMPLOYEES e ON d.DEPT_NO = e.FK_DEPT_NO";
                        cmd.CommandText = sqlQuery;
                        using (IDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int deptId = reader.GetInt32(0);
                                string deptName = reader.GetString(1);
                                int empId = reader.GetInt32(2);
                                string empFirstName = reader.GetString(3);
                                string empLastName = reader.GetString(4);
                                
                                var department = departments.FirstOrDefault(d => d.Id == deptId);
                                if (department == null)
                                {
                                    department = new Department
                                    {
                                        Id = deptId,
                                        DeptName = deptName,
                                        Employees = new HashSet<Employee>()
                                    };
                                   departments.Add(department);
                                }

                                if(!string.IsNullOrWhiteSpace(empFirstName) && !string.IsNullOrWhiteSpace(empLastName))
                                {
                                    department.Employees.Add(new Employee
                                    {
                                        Id = empId,
                                        FirstName = empFirstName,
                                        LastName = empLastName,
                                    });
                                }
                            }
                        }
                    }
                }
                catch (SqlException e)
                {
                    Console.WriteLine($"caught - {e.GetType()}: {e.Message}");
                }
                catch (System.Data.SqlTypes.SqlNullValueException e)
                {
                    Console.WriteLine($"caught - {e.GetType()}: {e.Message}");
                }
            }
            return departments;
        }

        public Department? ReadRowById(int id)
        {
            Department? department = null;
            using (IDbConnection conn = new SqlConnection())
            {
                conn.ConnectionString = _connectionString;
                try
                {
                    conn.Open();
                    using (IDbCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        var sqlQuery = "SELECT DEPT_NO, DEPT_NAME FROM [CompanyFdm].[Dbo].DEPARTMENTS WHERE DEPT_NO = @Id";
                        cmd.CommandText = sqlQuery;
                        IDbDataParameter param = new SqlParameter();
                        param.ParameterName = "@Id";
                        param.Value = id;
                        cmd.Parameters.Add(param);
                        using(IDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                department = new Department();
                                department.Id = (int)reader["DEPT_NO"];
                                department.DeptName = (string)reader["DEPT_NAME"];
                            }
                        }
                    }

                } 
                catch (SqlException e)
                {
                    Console.WriteLine($"caught - {e.GetType()}: {e.Message}");
                }
                catch (System.Data.SqlTypes.SqlNullValueException e)
                {
                    Console.WriteLine($"caught - {e.GetType()}: {e.Message}");
                }
            }
            return department;
        }

        public Department? ReadRowByName(string name)
        {
            Department? department = null;
            using (IDbConnection conn = new SqlConnection())
            {
                conn.ConnectionString = _connectionString;
                try
                {
                    conn.Open();
                    using (IDbCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        var sqlQuery = "SELECT DEPT_NO, DEPT_NAME FROM [CompanyFdm].[Dbo].DEPARTMENTS WHERE DEPT_NAME = @dName";
                        cmd.CommandText = sqlQuery;

                        IDbDataParameter param = new SqlParameter();
                        param.ParameterName = "@dName";
                        param.Value = name;
                        cmd.Parameters.Add(param);
                        using (IDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                department = new Department();
                                department.Id = (int)reader["DEPT_NO"];
                                department.DeptName = (string)reader["DEPT_NAME"];
                            }
                        }
                    }

                }
                catch (SqlException e)
                {
                    Console.WriteLine($"caught - {e.GetType()}: {e.Message}");
                }
                catch (System.Data.SqlTypes.SqlNullValueException e)
                {
                    Console.WriteLine($"caught - {e.GetType()}: {e.Message}");
                }
            }
            return department;
        }

        public bool UpdateRow(Department department)
        {
            var isUpdated = false;
            using (IDbConnection conn = new SqlConnection())
            {
                conn.ConnectionString = _connectionString;
                try
                {
                    conn.Open();
                    using (IDbCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        var sqlStmt = "UPDATE [CompanyFdm].[dbo].DEPARTMENTS SET DEPT_NAME = @dName WHERE DEPT_NO = @Id";
                        cmd.CommandText = sqlStmt;

                        IDbDataParameter param = new SqlParameter("@dName", SqlDbType.VarChar);
                        param.Value = department.DeptName;
                        cmd.Parameters.Add(param);

                        param = new SqlParameter("@Id", SqlDbType.Int);
                        param.Value = department.Id;
                        cmd.Parameters.Add(param);
                        var numberAffectedRows = cmd.ExecuteNonQuery();
                        if (numberAffectedRows > 0)
                        {
                            isUpdated = true;
                        }
                    }
                } catch (SqlException e)
                {
                    Console.WriteLine($"caught - {e.GetType()}: {e.Message}");
                }
            }
            return isUpdated;
        }
    }
}
