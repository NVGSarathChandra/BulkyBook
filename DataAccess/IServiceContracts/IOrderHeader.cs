using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.IServiceContracts
{
    public interface IOrderHeader : IRepository<OrderHeader>
    {
        void Update(OrderHeader OoderHeader);
    }
}
