using Pocos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private string _connectionString;

        public EmployeeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool CreateNewRow(Employee employee)
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
                        var sqlStmt = "INSERT INTO [CompanyFdm].[dbo].EMPLOYEES (EMP_FST_NAME, EMP_LST_NAME, FK_DEPT_NO) VALUES (@fName, @lName, @dNumber)";
                        cmd.CommandText = sqlStmt;
                        
                        IDbDataParameter param = new SqlParameter();
                        param.ParameterName = "@fName";
                        param.Value = employee.FirstName;
                        cmd.Parameters.Add(param);
                        
                        param = new SqlParameter();
                        param.ParameterName = "@lName";
                        param.Value = employee.LastName;
                        cmd.Parameters.Add(param);

                        param = new SqlParameter();
                        param.ParameterName = "@dNumber";
                        param.Value = employee.Dept.Id;
                        cmd.Parameters.Add(param);

                        var numberAffectedRows = cmd.ExecuteNonQuery();
                        Console.WriteLine($"Created {numberAffectedRows} row(s)");
                        if (numberAffectedRows > 0)
                        {
                            isCreated = true;
                        }
                    }
                }
                catch (SqlException e)
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
                        var sqlStmt = "DELETE FROM [CompanyFdm].[dbo].EMPLOYEES WHERE EMP_ID = @Id";
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

        public bool DeleteRow(Employee entity)
        {
            return DeleteRow(entity.Id);
        }

        public IEnumerable<Employee> ReadAllRows()
        {
            List<Employee> employees = new List<Employee>();
            using (IDbConnection conn = new SqlConnection())
            {
                conn.ConnectionString = _connectionString;
                try
                {
                    conn.Open();
                    using (IDbCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        var sqlQuery = "SELECT EMP_ID, EMP_FST_NAME, EMP_LST_NAME, DEPT_NO, DEPT_NAME " +
                            "FROM [CompanyFdm].[dbo].EMPLOYEES e JOIN [CompanyFdm].[dbo].DEPARTMENTS d ON (e.FK_DEPT_NO = d.DEPT_NO)";
                        cmd.CommandText = sqlQuery;
                        using (IDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var employee = new Employee();
                                employee.Id = reader.GetInt32(0);
                                employee.FirstName = reader.GetString(1);
                                employee.LastName = reader.GetString(2);
                                employee.Dept = new Department { Id = reader.GetInt32(3), DeptName = reader.GetString(4) };
                                employees.Add(employee);
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
            return employees;
        }

        public Employee? ReadRowById(int id)
        {
            Employee? employee = null;
            using (IDbConnection conn = new SqlConnection())
            {
                conn.ConnectionString = _connectionString;
                try
                {
                    conn.Open();
                    using (IDbCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        var sqlQuery = "SELECT EMP_ID, EMP_FST_NAME, EMP_LST_NAME, DEPT_NO, DEPT_NAME " +
                            "FROM [CompanyFdm].[dbo].EMPLOYEES e JOIN [CompanyFdm].[dbo].DEPARTMENTS d ON (e.FK_DEPT_NO = d.DEPT_NO) " +
                            "WHERE EMP_ID = @Id;";

                        cmd.CommandText = sqlQuery;
                        IDbDataParameter param = new SqlParameter();
                        param.ParameterName = "@Id";
                        param.Value = id;
                        cmd.Parameters.Add(param);
                        using (IDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                employee = new Employee();
                                employee.Id = (int)reader["DEPT_NO"];
                                employee.FirstName = (string)reader["EMP_FST_NAME"];
                                employee.LastName = (string)reader["EMP_LST_NAME"];
                                employee.Dept = new Department { Id = (int)reader["DEPT_NO"], DeptName = (string)reader["DEPT_NAME"] };
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
            return employee;
        }

        public Employee? ReadRowByName(string firstName, string lastName)
        {
            Employee? employee = null;
            using (IDbConnection conn = new SqlConnection())
            {
                conn.ConnectionString = _connectionString;
                try
                {
                    conn.Open();
                    using (IDbCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        var sqlQuery = "SELECT EMP_ID, EMP_FST_NAME, EMP_LST_NAME, DEPT_NO, DEPT_NAME " +
                            "FROM [CompanyFdm].[dbo].EMPLOYEES e JOIN [CompanyFdm].[dbo].DEPARTMENTS d ON (e.FK_DEPT_NO = d.DEPT_NO) " +
                            "WHERE EMP_FST_NAME = @fName AND EMP_LST_NAME = @lName;";

                        cmd.CommandText = sqlQuery;
                        IDbDataParameter param = new SqlParameter();
                        param.ParameterName = "@fName";
                        param.Value = firstName;
                        cmd.Parameters.Add(param);

                        param = new SqlParameter();
                        param.ParameterName = "@lName";
                        param.Value = lastName;
                        cmd.Parameters.Add(param);

                        using (IDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                employee = new Employee();
                                employee.Id = (int)reader["DEPT_NO"];
                                employee.FirstName = (string)reader["EMP_FST_NAME"];
                                employee.LastName = (string)reader["EMP_LST_NAME"];
                                employee.Dept = new Department { Id = (int)reader["DEPT_NO"], DeptName = (string)reader["DEPT_NAME"] };
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
            return employee;
        }

        public bool UpdateRow(Employee employee)
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
                        var sqlStmt = "UPDATE [CompanyFdm].[dbo].EMPLOYEES SET EMP_FST_NAME = @fName, EMP_LST_NAME = @lName, FK_DEPT_NO = @dId WHERE EMP_ID = @eId";
                        cmd.CommandText = sqlStmt;

                        IDbDataParameter param = new SqlParameter("@fName", SqlDbType.VarChar);
                        param.Value = employee.FirstName;
                        cmd.Parameters.Add(param);

                        param = new SqlParameter("@lName", SqlDbType.VarChar);
                        param.Value = employee.LastName;
                        cmd.Parameters.Add(param);

                        param = new SqlParameter("@dId", SqlDbType.Int);
                        param.Value = employee.Dept.Id;
                        cmd.Parameters.Add(param);

                        param = new SqlParameter("@eId", SqlDbType.Int);
                        param.Value = employee.Id;
                        cmd.Parameters.Add(param);

                        var numberAffectedRows = cmd.ExecuteNonQuery();
                        if (numberAffectedRows > 0)
                        {
                            isUpdated = true;
                        }
                    }
                }
                catch (SqlException e)
                {
                    Console.WriteLine($"caught - {e.GetType()}: {e.Message}");
                }
            }
            return isUpdated;
        }
    }
}
