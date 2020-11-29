using Bulky_Book_Project.Dataaccess.data;
using DataAccess.IServiceContracts;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Repositories
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCart
    {
        private readonly ApplicationDbContext dbContext;
        public ShoppingCartRepository(ApplicationDbContext _dbContext) : base(_dbContext)
        {
            this.dbContext = _dbContext;
        }

        public void Update(ShoppingCart shoppingCart)
        {
            dbContext.Update(shoppingCart);
        }
    }

}
