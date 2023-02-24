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
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using var ms = new MemoryStream();
            file.CopyTo(ms);
            var bytes = ms.ToArray();

            using var form = new MultipartFormDataContent();
            using var fileContent = new ByteArrayContent(bytes);

            string hostAddress = @"https://pss.itdvgroup.com/template";
            string localAddress = @"https://localhost:5001";

            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType);

            // here it is important that second parameter matches with name given in API.
            form.Add(content: fileContent, name: "file", fileName: file.FileName);

            var httpClient = new HttpClient(clientHandler)
            {
                BaseAddress = new Uri(hostAddress)
            };

            var response = await httpClient.PostAsync("", form);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();

            return await response.Content.ReadAsStringAsync();
        }
    }
}