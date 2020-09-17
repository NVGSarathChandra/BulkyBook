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

        public ICategory category { get; private set; }

        public IStoredProcedureCall storedProcedureCall { get; private set; }

        public ICoverType coverType { get; private set; }
        public IProduct product { get; private set; }

        public IOrganization organization { get; set; }
        public IApplicationUser applicationUser { get; set; }


        public UnitOfWorkRepository(ApplicationDbContext _dbContext)
        {
            this.dbContext = _dbContext;
            category = new CategoryRepository(dbContext);
            storedProcedureCall = new StoredProcedureCallRepository(dbContext);
            coverType = new CoverTypeRepository(dbContext);
            product = new ProductRepository(dbContext);
            organization = new OrganizationRepository(dbContext);
            applicationUser = new ApplicationUserRepository(dbContext);
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
