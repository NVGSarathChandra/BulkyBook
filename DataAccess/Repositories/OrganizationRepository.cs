using Bulky_Book_Project.Dataaccess.data;
using DataAccess.IServiceContracts;
using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repositories
{
    public class OrganizationRepository : Repository<Organization>, IOrganization
    {
        private readonly ApplicationDbContext dbContext;
        public OrganizationRepository(ApplicationDbContext _dbContext):base(_dbContext)
        {
            dbContext = _dbContext;
        }
        public void Update(Organization organization)
        {
            dbContext.Organizations.Update(organization);
        }
    }
}
