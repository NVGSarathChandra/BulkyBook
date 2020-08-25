using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.IServiceContracts
{
    public interface ICategory :IRepository<Category>
    {
        void Update(Category category);
    }
}
