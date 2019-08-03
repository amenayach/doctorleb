using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using Health.Configuration;
using System.Threading.Tasks;

namespace Health.Doctors.Sql
{
    /// <summary>
    /// Handles SQL operations
    /// </summary>
    public class SqlDataProvider
    {
        /// <summary>
        /// The SQL connection string
        /// </summary>
        private readonly string connectionString;

        /// <summary>
        /// Initialize the provider
        /// </summary>
        public SqlDataProvider(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Returns a new instance of SQL connection
        /// </summary>
        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        /// <summary>
        /// Execute a SQL query for a generic .Net type and map the query columns with the it properties
        /// </summary>
        /// <typeparam name="T">The targeted generic type</typeparam>
        /// <param name="query">The SQL query to execute</param>
        /// <param name="isProcedure">Deal with query text as procedure or not</param>
        /// <param name="parameters">An array of SqlParamters used with the SQL query</param>
        /// <returns>A list of the generic type T</returns>
        public async Task<List<T>> ExecuteQueryAsync<T>(string query, bool isProcedure, params SqlParameter[] parameters)
        {
            List<T> result = new List<T>();
            SqlDataReader reader = null;

            try
            {
                using (var connection = GetConnection())
                {

                    if (string.IsNullOrEmpty(query))
                    {
                        throw new ArgumentNullException(nameof(query));
                    }

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        if (isProcedure)
                        {
                            command.CommandType = CommandType.StoredProcedure;
                        }

                        if (parameters.Length > 0)
                        {
                            command.Parameters.AddRange(parameters);
                        }
                        if (command.Connection.State != ConnectionState.Open)
                        {
                            await command.Connection.OpenAsync();
                        }

                        reader = await command.ExecuteReaderAsync();

                        var properties = typeof(T).GetProperties();
                        var propertiesNames = properties.Select(f => f.Name).ToList();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                if (typeof(T).FullName.Contains("System."))
                                {
                                    result.Add((T)reader[0]);
                                }
                                else
                                {
                                    var returnedTypeInstance = (T)Activator.CreateInstance(typeof(T));

                                    for (int j = 0; j <= reader.FieldCount - 1; j++)
                                    {

                                        var fieldName = reader.GetName(j).ToString();
                                        var fieldType = reader.GetProviderSpecificFieldType(j).ToString().ToLower();
                                        var property = properties.Where(f => f.Name.Trim().ToLower() == fieldName.ToLower()).ToList();

                                        if (property.Count > 0)
                                        {
                                            //Date or DateTime
                                            if (fieldType.Contains("date"))
                                            {

                                                if (!property[0].PropertyType.FullName.ToLower().Contains("system.nullable") && (reader[fieldName] == null || reader[fieldName].Equals(System.DBNull.Value)))
                                                {
                                                    property[0].SetValue(returnedTypeInstance, DateTime.Now, null);
                                                }
                                                else
                                                {
                                                    if (!(property[0].PropertyType.FullName.ToLower().Contains("system.nullable") && (reader[fieldName] == null || reader[fieldName].Equals(System.DBNull.Value))))
                                                    {
                                                        property[0].SetValue(returnedTypeInstance, reader[fieldName], null);
                                                    }
                                                }

                                            }
                                            // Strings
                                            else if (fieldType.Contains("string"))
                                            {
                                                if (reader[fieldName] == null || reader[fieldName].Equals(System.DBNull.Value))
                                                {
                                                    property[0].SetValue(returnedTypeInstance, "", null);
                                                }
                                                else
                                                {
                                                    property[0].SetValue(returnedTypeInstance, reader[fieldName], null);
                                                }
                                            }
                                            // Binary/Blob/Files
                                            else if (fieldType.Contains("sqlbinary") && !(reader[fieldName] == null || reader[fieldName].Equals(DBNull.Value)))
                                            {
                                                property[0].SetValue(returnedTypeInstance, (byte[])reader[fieldName], null);
                                            }
                                            //Handles the SQL guid into string property
                                            else if (!(reader[fieldName] == null || reader[fieldName].Equals(DBNull.Value)) &&
                                                        reader[fieldName].GetType() != property.GetType() &&
                                                        property[0].PropertyType == typeof(string))
                                            {
                                                property[0].SetValue(returnedTypeInstance, reader[fieldName].ToString(), null);
                                            }
                                            else
                                            {
                                                if (!(reader[fieldName] == null || reader[fieldName].Equals(DBNull.Value)))
                                                {
                                                    property[0].SetValue(returnedTypeInstance, reader[fieldName], null);
                                                }
                                            }
                                        }
                                    }
                                    result.Add(returnedTypeInstance);
                                }
                            }
                        }

                        reader.Close();

                    }

                }

            }
            catch (Exception ex)
            {
                if (reader != null && reader.IsClosed == false)
                {
                    reader.Close();
                }

                Logger.Log(ex);

                throw;
            }
            return result;
        }

        /// <summary>
        /// Execute a SQL query for a generic .Net type and map the query result with the generic type
        /// </summary>
        /// <typeparam name="T">The targeted generic type</typeparam>
        /// <param name="query">The SQL query to execute</param>
        /// <param name="isProcedure">Deal with query text as procedure or not</param>
        /// <param name="parameters">An array of SqlParamters used with the SQL query</param>
        /// <returns>A single value of the generic type T</returns>
        public async Task<T> ExecuteScalarAsync<T>(string query, bool isProcedure, params SqlParameter[] parameters)
        {
            T result;

            try
            {
                using (var connection = GetConnection())
                {

                    if (string.IsNullOrEmpty(query))
                    {
                        throw new ArgumentNullException(nameof(query));
                    }

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        if (isProcedure)
                        {
                            command.CommandType = CommandType.StoredProcedure;
                        }

                        if (parameters.Length > 0)
                        {
                            command.Parameters.AddRange(parameters);
                        }
                        if (command.Connection.State != ConnectionState.Open)
                        {
                            await command.Connection.OpenAsync();
                        }

                        result = (T)(await command.ExecuteScalarAsync());

                    }

                }

            }
            catch (Exception ex)
            {
                Logger.Log(ex);

                throw;
            }
            return result;
        }

        /// <summary>
        /// Execute a SQL non query
        /// </summary>
        /// <param name="query">The SQL query to execute</param>
        /// <param name="isProcedure">Deal with query text as procedure or not</param>
        /// <param name="parameters">An array of SqlParamters used with the SQL query</param>
        public async Task ExecuteNonQuery(string query, bool isProcedure, params SqlParameter[] parameters)
        {
            try
            {
                using (var connection = GetConnection())
                {

                    if (string.IsNullOrEmpty(query))
                    {
                        throw new ArgumentNullException(nameof(query));
                    }

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        if (isProcedure)
                        {
                            command.CommandType = CommandType.StoredProcedure;
                        }

                        if (parameters.Length > 0)
                        {
                            command.Parameters.AddRange(parameters);
                        }
                        if (command.Connection.State != ConnectionState.Open)
                        {
                            await command.Connection.OpenAsync();
                        }

                        await command.ExecuteNonQueryAsync();

                    }

                }

            }
            catch (Exception ex)
            {
                Logger.Log(ex);

                throw;
            }
        }
    }
}
