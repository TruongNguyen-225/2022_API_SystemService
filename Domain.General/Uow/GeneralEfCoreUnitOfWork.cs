using System;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using SystemServiceAPICore3.Logging.Interfaces;
using SystemServiceAPICore3.Uow.Interface;
using System.Threading.Tasks;
using System.Threading;
using MySqlConnector;
using Npgsql;

namespace SystemServiceAPICore3.Domain.General.Uow
{
    public class GeneralEfCoreUnitOfWork<TU> : EfCoreUnitOfWork<TU>
    {
        #region -- Variables --
        #endregion

        #region -- Properties --
        #endregion

        #region -- Constructors --

        public GeneralEfCoreUnitOfWork(DbContext context, ISscLogger logger)
            : base(context, logger)
        {
        }

        #endregion

        #region -- Overrides --

        public override DataSet ExecuteSpDataSet(string sql, object param)
        {
            var ds = new DataSet();

            var connStr = ConnectionString;
            var providerName = ProviderName;

            switch (providerName)
            {
                case "Microsoft.EntityFrameworkCore.SqlServer":
                    ds = SqlServerExecuteSpDataSet(connStr, sql, param);
                    break;
                case "Npgsql.EntityFrameworkCore.PostgreSQL":
                    ds = PostgreSqlExecuteSpDataSet(connStr, sql, param);
                    break;
                case "Pomelo.EntityFrameworkCore.MySql":
                case "MySql.Data.EntityFrameworkCore":
                    ds = MySqlExecuteSpDataSet(connStr, sql, param);
                    break;
            }

            return ds;
        }

        public override DataTable ExecuteSpDataTable(string sql, object param)
        {
            var dt = new DataTable();

            var connStr = ConnectionString;
            var providerName = ProviderName;

            switch (providerName)
            {
                case "Microsoft.EntityFrameworkCore.SqlServer":
                    dt = SqlServerExecuteSpDataTable(connStr, sql, param);
                    break;
                case "Npgsql.EntityFrameworkCore.PostgreSQL":
                    dt = PostgreSqlExecuteSpDataTable(connStr, sql, param);
                    break;
                case "Pomelo.EntityFrameworkCore.MySql":
                case "MySql.Data.EntityFrameworkCore":
                    dt = MySqlExecuteSpDataTable(connStr, sql, param);
                    break;
            }

            return dt;
        }

        public override void ExecuteSp(string sql, object param)
        {
            var connStr = ConnectionString;
            var providerName = ProviderName;

            switch (providerName)
            {
                case "Microsoft.EntityFrameworkCore.SqlServer":
                    SqlServerExecuteSp(connStr, sql, param);
                    break;
                case "Npgsql.EntityFrameworkCore.PostgreSQL":
                    PostgreSqlExecuteSp(connStr, sql, param);
                    break;
                case "Pomelo.EntityFrameworkCore.MySql":
                case "MySql.Data.EntityFrameworkCore":
                    MySqlExecuteSp(connStr, sql, param);
                    break;
            }
        }

        protected override Tuple<string, object[]> GetExecutionParams(string sql, object param, int? type)
        {
            var providerName = ProviderName;

            switch (providerName)
            {
                case "Microsoft.EntityFrameworkCore.SqlServer":
                    return SqlServerGetExecutionParams(sql, param, type);
                case "Npgsql.EntityFrameworkCore.PostgreSQL":
                    return PostgreSqlGetExecutionParams(sql, param, type);
                case "Pomelo.EntityFrameworkCore.MySql":
                case "MySql.Data.EntityFrameworkCore":
                    return MySqlGetExecutionParams(sql, param, type);
            }

            return null;
        }

        protected override object[] GetExecutionParams(object param)
        {
            var providerName = ProviderName;

            switch (providerName)
            {
                case "Microsoft.EntityFrameworkCore.SqlServer":
                    return SqlServerGetExecutionParams(param);
                case "Npgsql.EntityFrameworkCore.PostgreSQL":
                    return PostgreSqlGetExecutionParams(param);
                case "Pomelo.EntityFrameworkCore.MySql":
                case "MySql.Data.EntityFrameworkCore":
                    return MySqlGetExecutionParams(param);
            }

            return null;
        }

        #endregion

        #region -- Methods --

        private DataSet SqlServerExecuteSpDataSet(string connStr, string sql, object param)
        {
            var ds = new DataSet();

            using (var objConn = new SqlConnection(connStr))
            {
                var objCmd = new SqlCommand
                {
                    Connection = objConn,
                    CommandText = sql,
                    CommandType = CommandType.StoredProcedure
                };

                objCmd.Parameters.AddRange(GetExecutionParams(param));

                try
                {
                    // Open connection.
                    objConn.Open();

                    // Get data set.
                    var da = new SqlDataAdapter(objCmd);
                    da.Fill(ds);
                }
                finally
                {
                    objConn.Close();
                }
            }

            return ds;
        }

        private DataSet PostgreSqlExecuteSpDataSet(string connStr, string sql, object param)
        {
            var ds = new DataSet();

            using (var objConn = new NpgsqlConnection(connStr))
            {
                var objCmd = new NpgsqlCommand
                {
                    Connection = objConn,
                    CommandText = sql,
                    CommandType = CommandType.StoredProcedure
                };

                objCmd.Parameters.AddRange(GetExecutionParams(param));

                try
                {
                    // Open connection.
                    objConn.Open();

                    // Get data set.
                    var da = new NpgsqlDataAdapter(objCmd);
                    da.Fill(ds);
                }
                finally
                {
                    objConn.Close();
                }
            }

            return ds;
        }

        private DataSet MySqlExecuteSpDataSet(string connStr, string sql, object param)
        {
            var ds = new DataSet();

            using (var objConn = new MySqlConnection(connStr))
            {
                var objCmd = new MySqlCommand
                {
                    Connection = objConn,
                    CommandText = sql,
                    CommandType = CommandType.StoredProcedure
                };

                objCmd.Parameters.AddRange(GetExecutionParams(param));

                try
                {
                    // Open connection.
                    objConn.Open();

                    // Get data set.
                    var da = new MySqlDataAdapter(objCmd);
                    da.Fill(ds);
                }
                finally
                {
                    objConn.Close();
                }
            }

            return ds;
        }

        private DataTable SqlServerExecuteSpDataTable(string connStr, string sql, object param)
        {
            var dt = new DataTable();

            using (var objConn = new SqlConnection(connStr))
            {
                var objCmd = new SqlCommand
                {
                    Connection = objConn,
                    CommandText = sql,
                    CommandType = CommandType.StoredProcedure
                };

                objCmd.Parameters.AddRange(GetExecutionParams(param));

                try
                {
                    // Open connection.
                    objConn.Open();

                    // Get data set.
                    var da = new SqlDataAdapter(objCmd);
                    da.Fill(dt);
                }
                finally
                {
                    objConn.Close();
                }
            }

            return dt;
        }

        private DataTable PostgreSqlExecuteSpDataTable(string connStr, string sql, object param)
        {
            var dt = new DataTable();

            using (var objConn = new NpgsqlConnection(connStr))
            {
                var objCmd = new NpgsqlCommand
                {
                    Connection = objConn,
                    CommandText = sql,
                    CommandType = CommandType.StoredProcedure
                };

                objCmd.Parameters.AddRange(GetExecutionParams(param));

                try
                {
                    // Open connection.
                    objConn.Open();

                    // Get data set.
                    var da = new NpgsqlDataAdapter(objCmd);
                    da.Fill(dt);
                }
                finally
                {
                    objConn.Close();
                }
            }

            return dt;
        }

        private DataTable MySqlExecuteSpDataTable(string connStr, string sql, object param)
        {
            var dt = new DataTable();

            using (var objConn = new MySqlConnection(connStr))
            {
                var objCmd = new MySqlCommand
                {
                    Connection = objConn,
                    CommandText = sql,
                    CommandType = CommandType.StoredProcedure
                };

                objCmd.Parameters.AddRange(GetExecutionParams(param));

                try
                {
                    // Open connection.
                    objConn.Open();

                    // Get data set.
                    var da = new MySqlDataAdapter(objCmd);
                    da.Fill(dt);
                }
                finally
                {
                    objConn.Close();
                }
            }

            return dt;
        }

        private void SqlServerExecuteSp(string connStr, string sql, object param)
        {
            using (var objConn = new SqlConnection(connStr))
            {
                var objCmd = new SqlCommand
                {
                    Connection = objConn,
                    CommandText = sql,
                    CommandType = CommandType.StoredProcedure
                };

                objCmd.Parameters.AddRange(GetExecutionParams(param));

                try
                {
                    // Open connection.
                    objConn.Open();

                    // Execute
                    objCmd.ExecuteNonQuery();
                }
                finally
                {
                    objConn.Close();
                }
            }
        }

        private void PostgreSqlExecuteSp(string connStr, string sql, object param)
        {
            using (var objConn = new NpgsqlConnection(connStr))
            {
                var objCmd = new NpgsqlCommand
                {
                    Connection = objConn,
                    CommandText = sql,
                    CommandType = CommandType.StoredProcedure
                };

                objCmd.Parameters.AddRange(GetExecutionParams(param));

                try
                {
                    // Open connection.
                    objConn.Open();

                    // Execute
                    objCmd.ExecuteNonQuery();
                }
                finally
                {
                    objConn.Close();
                }
            }
        }

        private void MySqlExecuteSp(string connStr, string sql, object param)
        {
            using (var objConn = new MySqlConnection(connStr))
            {
                var objCmd = new MySqlCommand
                {
                    Connection = objConn,
                    CommandText = sql,
                    CommandType = CommandType.StoredProcedure
                };

                objCmd.Parameters.AddRange(GetExecutionParams(param));

                try
                {
                    // Open connection.
                    objConn.Open();

                    // Execute
                    objCmd.ExecuteNonQuery();
                }
                finally
                {
                    objConn.Close();
                }
            }
        }

        private Tuple<string, object[]> SqlServerGetExecutionParams(string sql, object param, int? type)
        {
            var sqlParams = new List<object>();
            var paramStr = "";

            // Get sql param.
            var properties = param.GetType().GetProperties();
            foreach (var property in properties)
            {
                paramStr += string.IsNullOrWhiteSpace(paramStr)
                        ? $" @{property.Name}"
                        : $", @{property.Name}";

                var value = property.GetValue(param);
                sqlParams.Add(value == null
                    ? new SqlParameter(property.Name, DBNull.Value)
                    : new SqlParameter(property.Name, value));
            }

            string queryScript = sql;
            if (type == (int)CommandType.StoredProcedure)
            {
                // Incase store proceduce.
                queryScript = $"EXEC {sql} {paramStr}";
            }

            return new Tuple<string, object[]>(queryScript, sqlParams.ToArray());
        }

        private Tuple<string, object[]> PostgreSqlGetExecutionParams(string sql, object param, int? type)
        {
            var sqlParams = new List<object>();
            var paramStr = "";

            // Get sql param.
            var properties = param.GetType().GetProperties();
            foreach (var property in properties)
            {
                paramStr += string.IsNullOrWhiteSpace(paramStr)
                        ? $" @{property.Name}"
                        : $", @{property.Name}";

                var value = property.GetValue(param);
                sqlParams.Add(value == null
                    ? new NpgsqlParameter(property.Name, DBNull.Value)
                    : new NpgsqlParameter(property.Name, value));
            }

            string queryScript = sql;
            if (type == (int)CommandType.StoredProcedure)
            {
                // Incase store proceduce.
                queryScript = $"CALL {sql} ({paramStr});";
            }

            return new Tuple<string, object[]>(queryScript, sqlParams.ToArray());
        }

        private Tuple<string, object[]> MySqlGetExecutionParams(string sql, object param, int? type)
        {
            var sqlParams = new List<object>();
            var paramStr = "";

            // Get sql param.
            var properties = param.GetType().GetProperties();
            foreach (var property in properties)
            {
                paramStr += string.IsNullOrWhiteSpace(paramStr)
                ? $" @{property.Name}"
                : $", @{property.Name}";

                var value = property.GetValue(param);
                sqlParams.Add(value == null
                    ? new MySqlParameter(property.Name, DBNull.Value)
                    : new MySqlParameter(property.Name, value));
            }

            string queryScript = sql;
            if (type == (int)CommandType.StoredProcedure)
            {
                // Incase store proceduce.
                queryScript = $"CALL {sql} ({paramStr});";
            }

            return new Tuple<string, object[]>(queryScript, sqlParams.ToArray());
        }

        private object[] SqlServerGetExecutionParams(object param)
        {
            var sqlParams = new List<object>();

            // Get sql param.
            var properties = param.GetType().GetProperties();
            foreach (var property in properties)
            {
                var value = property.GetValue(param);
                sqlParams.Add(value == null
                    ? new SqlParameter(property.Name, DBNull.Value)
                    : new SqlParameter(property.Name, value));
            }

            return sqlParams.ToArray();
        }

        private object[] PostgreSqlGetExecutionParams(object param)
        {
            var sqlParams = new List<object>();

            // Get sql param.
            var properties = param.GetType().GetProperties();
            foreach (var property in properties)
            {
                var value = property.GetValue(param);
                sqlParams.Add(value == null
                    ? new NpgsqlParameter(property.Name, DBNull.Value)
                    : new NpgsqlParameter(property.Name, value));
            }

            return sqlParams.ToArray();
        }

        private object[] MySqlGetExecutionParams(object param)
        {
            var sqlParams = new List<object>();

            // Get sql param.
            var properties = param.GetType().GetProperties();
            foreach (var property in properties)
            {
                var value = property.GetValue(param);
                sqlParams.Add(value == null
                    ? new MySqlParameter(property.Name, DBNull.Value)
                    : new MySqlParameter(property.Name, value));
            }

            return sqlParams.ToArray();
        }

        public override void CreateSavepoint(string savePoint)
        {
            throw new NotImplementedException();
        }

        public override void RollbackToSavepoint(string savePoint)
        {
            throw new NotImplementedException();
        }

        public override Task CreateSavepointAsync(string savePoint, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task RollbackToSavepointAsync(string savePoint, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
