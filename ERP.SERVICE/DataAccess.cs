
using ERP.UTILITY;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Dynamic;
using System.Reflection;
using System.Xml;

namespace ERP.SERVICE
{
    public class DataAccess
    {
        private readonly string connectionString;

        public DataAccess(string connectionString)
        {
            this.connectionString = connectionString;
        }
        //below code add in program.cs file
        //string connectionString = builder.Configuration.GetConnectionString("data");
        //builder.Services.AddSingleton<ProcessAccess>(provider => new ProcessAccess(connectionString));
        //end 


        public async Task<List<T>> GetTransInfo<T>(string comcod, string procedurename, string calltype, params string[] descriptions)
        {
            List<T> entities = new List<T>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand(procedurename, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Add input parameters
                    command.Parameters.AddWithValue("@Comp1", comcod);
                    command.Parameters.AddWithValue("@CallType", calltype);

                    // Add description parameters
                    for (int i = 0; i < descriptions.Length; i++)
                    {
                        string paramName = $"@Desc{i + 1}";
                        command.Parameters.AddWithValue(paramName, descriptions[i]);
                    }

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            T entity = MapDataToEntity<T>(reader);
                            entities.Add(entity);
                        }
                    }
                }
            }

            return entities;
        }

        private T MapDataToEntity<T>(SqlDataReader reader)
        {
            T entity = Activator.CreateInstance<T>();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                string columnName = reader.GetName(i);
                object value = reader.GetValue(i);

                PropertyInfo prop = typeof(T).GetProperty(columnName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (prop != null && value != DBNull.Value)
                {
                    prop.SetValue(entity, value);
                }
            }

            return entity;
        }

        // Extension method to check if a column exists in the data reader






        public async Task<bool> UpdateXmlTransData(string comcod, string procedurename, string calltype, IEnumerable<string> xmlParameters, params string[] descriptions)
        {
            try
            {
                using (SqlCommand command = new SqlCommand(procedurename))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Add input parameters
                    command.Parameters.AddWithValue("@Comp1", comcod);
                    command.Parameters.AddWithValue("@CallType", calltype);

                    // Add description parameters
                    for (int i = 0; i < descriptions.Length; i++)
                    {
                        string paramName = $"@Desc{i + 1}";
                        command.Parameters.AddWithValue(paramName, descriptions[i]);
                    }

                    // Add XML parameters
                    int xmlParamIndex = 1;
                    foreach (string xmlParameter in xmlParameters)
                    {
                        string paramName = $"@XmlParam{xmlParamIndex++}";

                        using (XmlReader xmlReader = XmlReader.Create(new StringReader(xmlParameter)))
                        {
                            command.Parameters.Add(paramName, SqlDbType.Xml).Value = new SqlXml(xmlReader);
                        }
                    }

                    return await ExecuteCommand(command);
                }
            }
            catch (Exception ex)
            {
                // Log the exception details for debugging and analysis
                Console.WriteLine($"Error in UpdateTransDataAsync: {ex.Message}");
                // You might want to log the exception using a logging framework
                // Log.Error($"Error in UpdateTransDataAsync: {ex.Message}", ex);
                return false;
            }
        }
        //string xmlData1 = "<root1><item>value1</item></root1>";
        //string xmlData2 = "<root2><item>value2</item></root2>";
        //bool result = await UpdateTransDataAsync("3101", "YourStoredProcedure", "YourCallType", new List<string> { xmlData1, xmlData2 }, "desc1", "desc2");

        public async Task<bool> UpdateTransData(string comcod, string procedurename, string calltype, params string[] descriptions)
        {
            try
            {
                using (SqlCommand command = new SqlCommand(procedurename))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Add input parameters
                    command.Parameters.AddWithValue("@Comp1", comcod);
                    command.Parameters.AddWithValue("@CallType", calltype);

                    // Add description parameters
                    for (int i = 0; i < descriptions.Length; i++)
                    {
                        string paramName = $"@Desc{i + 1}";
                        command.Parameters.AddWithValue(paramName, descriptions[i]);
                    }

                    return await ExecuteCommand(command);
                }
            }
            catch (Exception ex)
            {
                // Log the exception details for debugging and analysis
                Console.WriteLine($"Error in UpdateTransDataAsync: {ex.Message}");
                // You might want to log the exception using a logging framework
                // Log.Error($"Error in UpdateTransDataAsync: {ex.Message}", ex);
                return false;
            }
        }




        public async Task<bool> ExecuteCommand(SqlCommand cmd)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                cmd.Connection = connection;
                cmd.CommandTimeout = 120;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    // Handle error if needed
                    return false;
                }
            }
        }


        public SqlTransaction BeginTransaction()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            return connection.BeginTransaction();
        }

        public void CommitTransaction(SqlTransaction transaction)
        {
            transaction.Commit();
            transaction.Connection?.Close();
        }

        public void RollbackTransaction(SqlTransaction transaction)
        {
            transaction.Rollback();
            transaction.Connection?.Close();
        }


        //example uses transaction method
        //public void InsertMultipleEmployees(List<Employee> employees)
        //{
        //    using (SqlTransaction transaction = _processAccess.BeginTransaction())
        //    {
        //        try
        //        {
        //            foreach (var employee in employees)
        //            {
        //                // Perform the insertion using the UpdateTransData method within the transaction
        //                _processAccess.UpdateTransData(transaction, "", "InsertEmployee", "INSERT_EMPLOYEE",
        //                    employee.FirstName, employee.LastName, employee.Email);
        //            }

        //            // Commit the transaction if all insertions are successful
        //            _processAccess.CommitTransaction(transaction);
        //        }
        //        catch (Exception ex)
        //        {
        //            // Handle exceptions, and roll back the transaction if an error occurs
        //            _processAccess.RollbackTransaction(transaction);
        //            // Optionally log the exception or handle it according to your application's needs
        //        }
        //    }
        //}
        //Example end

        public object GetScalarValue(string procedureName, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(procedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters);

                    return command.ExecuteScalar();
                }
            }
        }

        //example using scalarValue
        //public object GetScalarValue(string procedureName, params SqlParameter[] parameters)
        //{
        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();

        //        using (SqlCommand command = new SqlCommand(procedureName, connection))
        //        {
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.Parameters.AddRange(parameters);

        //            return command.ExecuteScalar();
        //        }
        //    }
        //}
        //Example end
        public int ExecuteNonQueryWithParameters(string commandText, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    command.Parameters.AddRange(parameters);
                    return command.ExecuteNonQuery();
                }
            }
        }
        //Example ExecuteNonQueryWithParameters
        //public IActionResult UpdateRecord()
        //{
        //    string updateQuery = "UPDATE YourTable SET Column1 = @Value1 WHERE ID = @RecordID";

        //    SqlParameter paramValue1 = new SqlParameter("@Value1", SqlDbType.NVarChar)
        //    {
        //        Value = "NewValue"
        //    };

        //    SqlParameter paramRecordID = new SqlParameter("@RecordID", SqlDbType.Int)
        //    {
        //        Value = 123 // Replace with the actual record ID
        //    };

        //    int rowsAffected = _processAccess.ExecuteNonQueryWithParameters(updateQuery, paramValue1, paramRecordID);

        //    // Now 'rowsAffected' contains the number of rows affected by the update operation

        //    return View();
        //}

        public SqlDataReader ExecuteReaderWithParameters(string commandText, params SqlParameter[] parameters)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            SqlCommand command = new SqlCommand(commandText, connection);
            command.Parameters.AddRange(parameters);

            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }
        //Example ExecuteReaderWithParameters()
        //public IActionResult GetRecords()
        //{
        //    string selectQuery = "SELECT Column1, Column2 FROM YourTable WHERE ConditionColumn = @ConditionValue";

        //    SqlParameter paramConditionValue = new SqlParameter("@ConditionValue", SqlDbType.NVarChar)
        //    {
        //        Value = "SomeValue" // Replace with the actual condition value
        //    };

        //    using (SqlDataReader reader = _processAccess.ExecuteReaderWithParameters(selectQuery, paramConditionValue))
        //    {
        //        List<YourModel> records = new List<YourModel>();

        //        while (reader.Read())
        //        {
        //            YourModel record = new YourModel
        //            {
        //                Column1 = reader["Column1"].ToString(),
        //                Column2 = reader["Column2"].ToString()
        //                // Map other columns as needed
        //            };

        //            records.Add(record);
        //        }

        //        // Now 'records' contains the data retrieved from the database

        //        return View(records);
        //    }
        //}




    }



}
