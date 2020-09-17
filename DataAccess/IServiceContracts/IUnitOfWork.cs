using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.IServiceContracts
{
    public interface IUnitOfWork : IDisposable
    {
        ICategory category { get; }
        IStoredProcedureCall storedProcedureCall { get; }
        ICoverType coverType { get; }
        IProduct product { get; }
        IOrganization organization { get; }
        IApplicationUser applicationUser { get; }
        void Save();
    }
}
