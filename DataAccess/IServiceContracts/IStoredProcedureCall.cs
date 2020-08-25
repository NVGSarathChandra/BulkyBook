using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.IServiceContracts
{
    public interface IStoredProcedureCall : IDisposable
    {
        T Single<T>(string procedureName, DynamicParameters param= null);

        void Execute(string procedureName, DynamicParameters param= null);

        T singleRecord<T>(string procedureName, DynamicParameters param = null);

        IEnumerable<T> RetrievTable<T>(string procedureName, DynamicParameters param = null);

        Tuple<IEnumerable<T1>,IEnumerable<T2>> List<T1,T2>(string procedureName, DynamicParameters param = null);

    }
}

