using Bulky_Book_Project.Dataaccess.data;
using DataAccess.IServiceContracts;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess.Repositories
{
    public class CoverTypeRepository: Repository<CoverType>, ICoverType
    {
        private ApplicationDbContext dbContext;

        public CoverTypeRepository(ApplicationDbContext dbContext):base(dbContext)
        {
            this.dbContext = dbContext;
        }

       
        public void Update(CoverType coverType)
        {
            var objFromDB = dbContext.CoverTypes.FirstOrDefault(s => s.CoverTypeId == coverType.CoverTypeId);
            if (objFromDB != null)
            {
                objFromDB.CoverTypeName = coverType.CoverTypeName;
                dbContext.SaveChanges();
            }
        }
    }
}
