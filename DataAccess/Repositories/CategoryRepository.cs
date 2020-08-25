using Bulky_Book_Project.Dataaccess.data;
using DataAccess.IServiceContracts;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategory
    {
        private readonly ApplicationDbContext dbContext;
        public CategoryRepository(ApplicationDbContext _dbContext) : base(_dbContext)
        {
            this.dbContext = _dbContext;
        }

        public void Update(Category category)
        {
            var objFromDB = dbContext.categories.FirstOrDefault(s => s.CategoryId == category.CategoryId);
            if (objFromDB != null)
            {
                objFromDB.CategoryName = category.CategoryName;
                dbContext.SaveChanges();
            }
            
        }
    }

}
