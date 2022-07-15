using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SystemServiceAPI.Helpers
{
    public class SqlHelper
    {
        public static DataSet GetDataReturnDbSet(List<string> selectCommand, string connectString)
        {
            DataSet data = new DataSet();

            using (SqlConnection connection = new SqlConnection(connectString))
            {
                if (selectCommand.Count() > 0)
                {
                    try
                    {
                        connection.Open();
                        foreach (string selectCommandItem in selectCommand)
                        {
                            SqlDataAdapter dataAdapter = new SqlDataAdapter(selectCommandItem, connection);
                            DataTable results = new DataTable();
                            try
                            {
                                dataAdapter.Fill(results);
                            }
                            catch
                            {
                                throw;
                            }
                            finally
                            {
                                data.Tables.Add(results);
                                dataAdapter.Dispose();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            return data;
        }
        public static DataTable GetDataReturnDataTable(string connection, string selectCommand)
        {
            DataTable results = new DataTable();

            try
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(selectCommand, connection);
                dataAdapter.Fill(results);
                dataAdapter.Dispose();
            }
            catch (Exception ex)
            {
                throw;
            }

            return results;
        }
        /// <summary>
        /// check connect
        /// </summary>
        public static bool CheckServerIsConnect(string connectString)
        {
            using (SqlConnection conn = new SqlConnection(connectString))
            {
                try
                {
                    conn.Open();
                    return true;
                }
                catch
                {
                    return false;
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }
        /// <summary>
        /// ExcuteNonQuerySQL
        /// </summary>
        /// <param name="query">query</param>
        /// <returns>bool</returns>
        public static bool ExcuteNonQuerySQL(string query, string connectString)
        {
            bool isSuccess = false;

            if (!String.IsNullOrWhiteSpace(query))
            {
                using (SqlConnection connection = new SqlConnection(connectString))
                {
                    try
                    {
                        SqlCommand command = new SqlCommand(query, connection);
                        connection.Open();
                        command.ExecuteNonQuery();
                        isSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        isSuccess = false;
                    }
                    finally
                    {
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }

            return isSuccess;
        }
    }
}
