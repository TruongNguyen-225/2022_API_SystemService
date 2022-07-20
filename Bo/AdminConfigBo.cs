using System.Data;
using System.Net;
using SystemServiceAPI.Bo.Interface;
using SystemServiceAPI.Dto.BaseResult;
using SystemServiceAPI.Helpers;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using SystemServiceAPICore3.Utilities;
using System.Reflection;
using System.Linq;

namespace SystemServiceAPI.Bo
{
    public class AdminConfigBo : IAdminConfig
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        public AdminConfigBo(IConfiguration configuration, AppDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<object> GetAllTable()
        {
            string connectStr = _configuration.GetValue<String>("ConnectionStrings:DefaultConnection");
            ResponseResults response = new ResponseResults();
            List<object> results = new List<object>();

            try
            {
                DataTable data = new DataTable();
                string cmd = @"select schema_name(t.schema_id) as schema_name,
                                                            t.name as table_name,
                                                            t.create_date,
                                                            t.modify_date
                                                    from sys.tables t
                                                    order by schema_name, table_name;";

                data = SqlHelper.GetDataReturnDataTable(connectStr, cmd);
                if (data != null && data.Rows != null && data.Rows.Count > 0)
                {
                    response.Result = Utility.DataTableToJSONWithStringBuilder(data);
                }
                else
                {
                    response.Result = null;
                }

                response.Code = (int)HttpStatusCode.OK;
                response.Msg = "SUCCESS";
            }
            catch
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Msg = "ERROR";
                throw;
            }

            return await Task.FromResult(response);
        }

        public async Task<object> GetColumns(string tableName)
        {
            string connectStr = _configuration.GetValue<String>("ConnectionStrings:DefaultConnection");
            ResponseResults response = new ResponseResults();
            List<object> results = new List<object>();

            try
            {
                DataTable data = new DataTable();
                string cmd = @"SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'" + tableName + "'";

                data = SqlHelper.GetDataReturnDataTable(connectStr, cmd);
                if (data != null && data.Rows != null && data.Rows.Count > 0)
                {
                    response.Result = Utility.DataTableToJSONWithStringBuilder(data);
                }
                else
                {
                    response.Result = null;
                }

                response.Code = (int)HttpStatusCode.OK;
                response.Msg = "SUCCESS";
            }
            catch
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Msg = "ERROR";
                throw;
            }

            return await Task.FromResult(response);
        }

        public async Task<object> ExcuteQuery(string query)
        {
            string key = _configuration.GetValue<String>("Encrypt:Key");
            //string query = Utility.DecryptString(cmd, key);
            string connectStr = _configuration.GetValue<String>("ConnectionStrings:DefaultConnection");
            ResponseResults response = new ResponseResults();
            List<object> results = new List<object>();

            try
            {

                if (query.ToLower().Contains("select"))
                {
                    DataTable data = new DataTable();
                    data = SqlHelper.GetDataReturnDataTable(connectStr, query);
                    if (data != null && data.Rows != null && data.Rows.Count > 0)
                    {
                        response.Result = Utility.DataTableToJSONWithStringBuilder(data);
                    }
                    else
                    {
                        response.Result = null;
                    }
                }
                else
                {
                    bool result = SqlHelper.ExcuteNonQuerySQL(query, connectStr);
                    response.Result = result;
                }

                response.Code = (int)HttpStatusCode.OK;
                response.Msg = "SUCCESS";
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Msg = "ERROR:\n" + ex.Message;
                response.Result = null;

                throw;
            }

            return await Task.FromResult(response);
        }
    }
}
