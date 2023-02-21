using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using SystemServiceAPI.Bo.Interface;
using SystemServiceAPI.Helpers;
using SystemServiceAPICore3.Utilities;

namespace SystemServiceAPI.Bo
{
    public class AdminConfigBo : IAdminConfig
    {
        private readonly IConfiguration _configuration;
        private string connectionString;
        public AdminConfigBo(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetValue<String>("ConnectionStrings:DefaultConnection");
        }

        public object GetAllTable()
        {
            string cmd = @"select schema_name(t.schema_id) as schema_name, t.name as table_name, t.create_date, t.modify_date from sys.tables t order by schema_name, table_name;";

            return ExcuteQuery(cmd);
        }

        public object GetColumns(string tableName)
        {
            string cmd = @"SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'" + tableName + "'";

            return ExcuteQuery(cmd);
        }

        public object ExcuteQuery(string query)
        {
            if (query.ToLower().Contains("select"))
            {
                DataTable data = SqlHelper.GetDataReturnDataTable(connectionString, query);

                if (data != null && data.Rows != null && data.Rows.Count > 0)
                {
                    var result = Utility.DataTableToJSONWithStringBuilder(data);

                    return result;
                }

                return null;
            }
            else
            {
                bool result = SqlHelper.ExcuteNonQuerySQL(query, connectionString);

                return result;
            }
        }
    }
}
