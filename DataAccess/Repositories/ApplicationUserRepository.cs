using Bulky_Book_Project.Dataaccess.data;
using DataAccess.IServiceContracts;
using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repositories
{
    public class ApplicationUserRepository:Repository<ApplicationUser>,IApplicationUser
    {
        private readonly ApplicationDbContext dbContext;
        public ApplicationUserRepository(ApplicationDbContext _dbContext):base(_dbContext)
        {
            dbContext = _dbContext;
        }

        
    }
}
