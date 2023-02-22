using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
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

        public async Task<object> UploadFile(IFormFile file)
        {
            using (HttpClient client = new HttpClient())
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var bytes = ms.ToArray();

                    using (var content = new ByteArrayContent(bytes))
                    {
                        content.Headers.ContentType = new MediaTypeHeaderValue("*/*");

                        var path = Path.Combine(Directory.GetCurrentDirectory(), "FileDownloaded",  file.FileName);
                        //Send it
                        var response = await client.PostAsync(path, content);
                        response.EnsureSuccessStatusCode();
                        Stream responseStream = await response.Content.ReadAsStreamAsync();
                        StreamReader reader = new StreamReader(responseStream);
                        return reader.ReadToEnd();
                    }
                }
            }
        }
    }
}
