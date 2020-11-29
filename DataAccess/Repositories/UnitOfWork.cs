using Bulky_Book_Project.Dataaccess.data;
using DataAccess.IServiceContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repositories
{
    public class UnitOfWorkRepository : IUnitOfWork
    {
        private readonly ApplicationDbContext dbContext;

        public ICategory Category { get; private set; }
        public IStoredProcedureCall StoredProcedureCall { get; private set; }

        public ICoverType CoverType { get; private set; }
        public IProduct Product { get; private set; }

        public IOrganization Organization { get; set; }
        public IApplicationUser ApplicationUser { get; set; }
        public IShoppingCart ShoppingCart { get; set; }
        public IOrderDetails OrderDetails { get; set; }
        public IOrderHeader OrderHeader { get; set; }

        

        public UnitOfWorkRepository(ApplicationDbContext _dbContext)
        {
            this.dbContext = _dbContext;
            Category = new CategoryRepository(dbContext);
            StoredProcedureCall = new StoredProcedureCallRepository(dbContext);
            CoverType = new CoverTypeRepository(dbContext);
            Product = new ProductRepository(dbContext);
            Organization = new OrganizationRepository(dbContext);
            ApplicationUser = new ApplicationUserRepository(dbContext);
            ShoppingCart = new ShoppingCartRepository(dbContext);
            OrderDetails = new OrderDetailsRepository(dbContext);
            OrderHeader = new OrderHeaderRepository(dbContext);
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }
        public void Save()
        {
            dbContext.SaveChanges();
        }
    }
}
