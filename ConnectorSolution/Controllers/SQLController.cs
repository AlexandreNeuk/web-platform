using Connector.Controllers.Framework;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace Connector.Controllers
{
    public class SQLController : BaseController
    {
        private string stringConexao = string.Empty;
        SqlConnection oSqlConnection;

        #region SQL
        //
        private string StringConexao
        {
            get
            {
                string sessao = string.Empty;
                if (Session == null)
                {
                    sessao = SetaConexao();
                }
                else
                {
                    if (Session["stringConexao"] == null)
                    {
                        Session["stringConexao"] = SetaConexao();
                    }
                    sessao = Session["stringConexao"].ToString();
                }
                return sessao;
            }

            set
            {
                if (Session != null)
                {
                    Session["stringConexao"] = value;
                }
            }
        }

        private string SetaConexao()
        {
            DatabaseConfiguration databaseConfiguration = System.Configuration.ConfigurationManager.GetSection("DatabaseConfiguration") as DatabaseConfiguration;
            return databaseConfiguration.ConnectionString.String;
        }

        public bool StringSqlValida(string textoBusca)
        {
            if (textoBusca.Contains(";")) { return false; }
            if (textoBusca.Contains("'")) { return false; }
            if (textoBusca.Contains("-")) { return false; }
            if (textoBusca.Contains("/")) { return false; }
            if (textoBusca.Contains("*")) { return false; }
            if (textoBusca.Contains("\"")) { return false; }
            //
            return true;
        }

        public DataTable ExecutaSQL(string sql)
        {
            oSqlConnection = new SqlConnection("server=sql5018.site4now.net;database=db_a4925a_connector;user id=DB_A4925A_connector_admin;password=HUdgf!@S45G");
            SqlCommand oSqlCommand = new SqlCommand();
            oSqlCommand.CommandText = sql;
            oSqlCommand.CommandType = CommandType.Text;
            oSqlCommand.Connection = oSqlConnection;
            oSqlCommand.CommandTimeout = 300;
            oSqlConnection.Open();
            System.Data.Common.DbDataReader dbDataReader = oSqlCommand.ExecuteReader();
            DataTable oDataTable = ObterTabela(dbDataReader);
            oSqlConnection.Close();
            //
            return oDataTable;
        }

        public bool ExecutaSQL(string sql, out DataTable datatable)
        {
            oSqlConnection = new SqlConnection("server=sql5018.site4now.net;database=db_a4925a_connector;user id=DB_A4925A_connector_admin;password=HUdgf!@S45G");
            SqlCommand oSqlCommand = new SqlCommand();
            oSqlCommand.CommandText = sql;
            oSqlCommand.CommandType = CommandType.Text;
            oSqlCommand.Connection = oSqlConnection;
            oSqlCommand.CommandTimeout = 300;
            oSqlConnection.Open();
            System.Data.Common.DbDataReader dbDataReader = oSqlCommand.ExecuteReader();
            datatable = ObterTabela(dbDataReader);
            oSqlConnection.Close();
            //
            if (datatable != null && datatable.Rows.Count > 0)
            {
                return true;
            }
            //
            return false;
        }

        public bool ExecutaSQL(string sql, out DataTable datatable, string data)
        {
            try
            {
                oSqlConnection = new SqlConnection(data);
                SqlCommand oSqlCommand = new SqlCommand();
                oSqlCommand.CommandText = sql;
                oSqlCommand.CommandType = CommandType.Text;
                oSqlCommand.Connection = oSqlConnection;
                oSqlCommand.CommandTimeout = 300;
                oSqlConnection.Open();
                System.Data.Common.DbDataReader dbDataReader = oSqlCommand.ExecuteReader();
                datatable = ObterTabela(dbDataReader);
                oSqlConnection.Close();
                //
                if (datatable != null && datatable.Rows.Count > 0)
                {
                    return true;
                }
            }
            catch (Exception exc)
            {
                // Utils.Utils.Log.GravaLogMesmo(true, "Erro: " + exc.Message);
            }
            datatable = null;
            //
            return false;
        }

        public int ExecutaSQLNonQuery(string sql, out string erro)
        {
            int iRet = 0;
            erro = "";
            //
            try
            {
                oSqlConnection = new SqlConnection(getConString());
                SqlCommand oSqlCommand = new SqlCommand();
                oSqlConnection.Open();
                SqlTransaction oSqlTransaction = oSqlConnection.BeginTransaction();
                oSqlCommand.Transaction = oSqlTransaction;
                oSqlCommand.CommandText = sql;
                oSqlCommand.CommandType = CommandType.Text;
                oSqlCommand.Connection = oSqlConnection;
                oSqlCommand.CommandTimeout = 300; // 300 / 60 = 5 minutos
                iRet = oSqlCommand.ExecuteNonQuery();
                oSqlCommand.Transaction.Commit();
                oSqlConnection.Close();
            }
            catch (Exception exc)
            {
                erro = exc.Message;
            }
            //
            return iRet;
        }

        public DataSet ExecutaSQLToDataSet(string sql)
        {
            oSqlConnection = new SqlConnection(StringConexao);
            SqlDataAdapter da = new SqlDataAdapter(sql, oSqlConnection);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        private DataTable ObterTabela(System.Data.Common.DbDataReader reader)
        {
            //while (reader.NextResult())
            //{

            //}
            DataTable tbEsquema = reader.GetSchemaTable();
            DataTable tbRetorno = new DataTable();

            foreach (DataRow r in tbEsquema.Rows)
            {
                if (!tbRetorno.Columns.Contains(r["ColumnName"].ToString()))
                {
                    DataColumn col = new DataColumn()
                    {
                        ColumnName = r["ColumnName"].ToString(),
                        Unique = Convert.ToBoolean(r["IsUnique"]),
                        AllowDBNull = Convert.ToBoolean(r["AllowDBNull"])
                        //ReadOnly = false //Convert.ToBoolean(r["IsReadOnly"])
                    };
                    tbRetorno.Columns.Add(col);
                }
                else
                {
                    /* break point aqui quando der louca na montagem da estrutura da tabela dinâmica */
                }
            }

            while (reader.Read())
            {
                DataRow novaLinha = tbRetorno.NewRow();
                for (int i = 0; i < tbRetorno.Columns.Count; i++)
                {
                    novaLinha[i] = reader.GetValue(i);
                }
                tbRetorno.Rows.Add(novaLinha);
            }

            return tbRetorno;
        }
        //
        #endregion
    }

    namespace Framework
    {
        public class DatabaseConfiguration : ConfigurationSection
        {
            [ConfigurationProperty("DatabaseType", IsRequired = true)]
            public string DatabaseType
            {
                get
                {
                    return this["DatabaseType"].ToString();
                }
                set
                {
                    this["DatabaseType"] = value;
                }
            }

            [ConfigurationProperty("ConnectionString", IsRequired = true)]
            public ConnectionString ConnectionString
            {
                get
                {
                    return (ConnectionString)this["ConnectionString"];
                }
                set
                {
                    this["ConnectionString"] = value;
                }
            }

            [ConfigurationProperty("CreateDatabaseSchema", IsRequired = true)]
            public bool CreateDatabaseSchema
            {
                get
                {
                    return (bool)this["CreateDatabaseSchema"];
                }
                set
                {
                    this["CreateDatabaseSchema"] = value;
                }
            }

            [ConfigurationProperty("Prefix", IsRequired = false)]
            public string Prefix
            {
                get
                {
                    return (string)this["Prefix"];
                }
                set
                {
                    this["Prefix"] = value;
                }
            }

            [ConfigurationProperty("DefaultData", IsRequired = true)]
            public bool DefaultData
            {
                get
                {
                    return (bool)this["DefaultData"];
                }
                set
                {
                    this["DefaultData"] = value;
                }
            }


            [ConfigurationProperty("ExportSchemaToPath", IsRequired = true)]
            public string ExportSchemaToPath
            {
                get
                {
                    return (string)this["ExportSchemaToPath"];
                }
                set
                {
                    this["ExportSchemaToPath"] = value;
                }
            }

            [ConfigurationProperty("RepositoryAssemblies", IsDefaultCollection = false)]
            [ConfigurationCollection(typeof(RepositoryAssemblies), AddItemName = "AssemblyInfo", ClearItemsName = "clear", RemoveItemName = "remove")]
            public RepositoryAssemblies RepositoryAssemblies
            {
                get
                {
                    return (RepositoryAssemblies)this["RepositoryAssemblies"];
                }
            }
        }

        public class ConnectionString : ConfigurationElement
        {
            [ConfigurationProperty("String")]
            public string String
            {
                get
                {
                    if (IsEncrypted)
                        return this["String"].ToString();
                    else
                        return this["String"].ToString();
                }
                set
                {
                    this["String"] = value;
                }
            }

            [ConfigurationProperty("IsEncrypted")]
            public bool IsEncrypted
            {
                get
                {
                    return (bool)this["IsEncrypted"];
                }
                set
                { this["IsEncrypted"] = value; }
            }
        }

        public class AssemblyInfo : ConfigurationElement
        {
            public AssemblyInfo() { }

            public AssemblyInfo(string assemblyName, string assemblyPath)
            {
                AssemblyName = assemblyName;
                AssemblyPath = assemblyPath;
            }

            [ConfigurationProperty("AssemblyName", IsRequired = true)]
            public String AssemblyName
            {
                get
                {
                    return (string)this["AssemblyName"];
                }
                set
                {
                    this["AssemblyName"] = value;
                }
            }

            [ConfigurationProperty("AssemblyPath", DefaultValue = ".", IsRequired = true)]
            public String AssemblyPath
            {
                get
                {
                    return (string)this["AssemblyPath"];
                }
                set
                {
                    this["AssemblyPath"] = value;
                }
            }
        }

        public class RepositoryAssemblies : ConfigurationElementCollection
        {
            public RepositoryAssemblies() { }

            public AssemblyInfo this[int index]
            {
                get { return (AssemblyInfo)BaseGet(index); }
                set
                {
                    if (BaseGet(index) != null)
                    {
                        BaseRemoveAt(index);
                    }
                    BaseAdd(index, value);
                }
            }

            public void Add(AssemblyInfo assemblyInfo)
            {
                BaseAdd(assemblyInfo);
            }

            public void Clear()
            {
                BaseClear();
            }

            protected override ConfigurationElement CreateNewElement()
            {
                return new AssemblyInfo();
            }

            protected override object GetElementKey(ConfigurationElement element)
            {
                return ((AssemblyInfo)element).AssemblyName;
            }

            public void Remove(AssemblyInfo serviceConfig)
            {
                BaseRemove(serviceConfig.AssemblyName);
            }

            public void RemoveAt(int index)
            {
                BaseRemoveAt(index);
            }

            public void Remove(string name)
            {
                BaseRemove(name);
            }
        }
    }
}