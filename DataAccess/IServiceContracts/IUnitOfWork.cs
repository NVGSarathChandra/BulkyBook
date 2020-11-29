using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.IServiceContracts
{
    public interface IUnitOfWork : IDisposable
    {
        ICategory Category { get; }
        IStoredProcedureCall StoredProcedureCall { get; }
        ICoverType CoverType { get; }
        IProduct Product { get; }
        IOrganization Organization { get; }
        IApplicationUser ApplicationUser { get; }
        IOrderHeader OrderHeader { get; }
        IOrderDetails OrderDetails { get; }
        IShoppingCart ShoppingCart { get; }




        void Save();
    }
}
