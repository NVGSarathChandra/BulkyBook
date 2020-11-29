using Bulky_Book_Project.Dataaccess.data;
using DataAccess.IServiceContracts;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Repositories
{
    public class OrderDetailsRepository : Repository<OrderDetails>, IOrderDetails
    {
        private readonly ApplicationDbContext dbContext;
        public OrderDetailsRepository(ApplicationDbContext _dbContext) : base(_dbContext)
        {
            this.dbContext = _dbContext;
        }

        public void Update(OrderDetails orderDetails)
        {
            dbContext.Update(orderDetails);

        }
    }

}
