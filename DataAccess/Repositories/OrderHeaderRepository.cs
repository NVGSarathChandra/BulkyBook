using Bulky_Book_Project.Dataaccess.data;
using DataAccess.IServiceContracts;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Repositories
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeader
    {
        private readonly ApplicationDbContext dbContext;
        public OrderHeaderRepository(ApplicationDbContext _dbContext) : base(_dbContext)
        {
            this.dbContext = _dbContext;
        }

        public void Update(OrderHeader orderHeader)
        {
            dbContext.Update(orderHeader);


        }
    }

}
