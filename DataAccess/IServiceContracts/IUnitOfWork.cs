using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.IServiceContracts
{
    public interface IUnitOfWork : IDisposable
    {
        ICategory category { get; }
        IStoredProcedureCall storedProcedureCall { get; }
        void Save();
        
    }
}
  