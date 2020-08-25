using Bulky_Book_Project.Dataaccess.data;
using Dapper;
using DataAccess.IServiceContracts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DataAccess.Repositories
{
    class StoredProcedureCallRepository:IStoredProcedureCall
    {
        private readonly ApplicationDbContext dbContext;
        private static string connectionString = "";
        public StoredProcedureCallRepository(ApplicationDbContext _dbContext)
        {
            this.dbContext = _dbContext;
            connectionString = dbContext.Database.GetDbConnection().ConnectionString;
        }

        public T Single<T>(string procedureName, DynamicParameters param = null)
        {
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
            var result = sqlCon.ExecuteScalar<T>(procedureName, param, commandType: CommandType.StoredProcedure);
            return (T)Convert.ChangeType(result, typeof(T));
        }

        public void Execute(string procedureName, DynamicParameters param = null)
        {
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
            sqlCon.Execute(procedureName, param, commandType:CommandType.StoredProcedure);
        }

        public T singleRecord<T>(string procedureName, DynamicParameters param = null)
        {
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
            var result = sqlCon.Query<T>(procedureName, param, commandType: CommandType.StoredProcedure);
            return (T)Convert.ChangeType(result.FirstOrDefault(), typeof(T));
        }

        public IEnumerable<T> RetrievTable<T>(string procedureName, DynamicParameters param = null)
        {
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
           return sqlCon.Query<T>(procedureName, param, commandType: CommandType.StoredProcedure);
        }

        public Tuple<IEnumerable<T1>, IEnumerable<T2>> List<T1, T2>(string procedureName, DynamicParameters param = null)
        {
            SqlConnection sqlCon = new SqlConnection(connectionString);
            sqlCon.Open();
            var result= SqlMapper.QueryMultiple(sqlCon,procedureName, param, commandType: CommandType.StoredProcedure);
            var item1 = result.Read<T1>();
            var item2 = result.Read<T2>();
            if(item1!=null && item2 != null)
            {
                return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(item1, item2);
            }
            
            return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(new List<T1>(), new List<T2>());
        } 

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
