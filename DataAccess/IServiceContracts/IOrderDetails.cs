using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.IServiceContracts
{
    public interface IOrderDetails : IRepository<OrderDetails>
    {
        void Update(OrderDetails orderDetails);
    }
}
