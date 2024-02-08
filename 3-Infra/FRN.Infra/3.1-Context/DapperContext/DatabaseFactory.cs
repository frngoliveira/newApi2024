
using FRN.Domain._2._1_Interface.DapperInterface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;

namespace FRN.Infra._3._1_Context.DapperContext
{
    public class DatabaseFactory : IDatabaseFactory
    {
        private readonly IConfiguration _configuration;
        private IOptions<DataSettings> dataSettings;
        protected string ConnectionString;

        public DatabaseFactory(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        public IDbConnection GetDbConnection => new SqlConnection(ConnectionString);
        public DatabaseFactory(IOptions<DataSettings> dataSettings)
        {
            this.dataSettings = dataSettings;
            ConnectionString = _configuration.GetConnectionString("");
        }
    }
}
