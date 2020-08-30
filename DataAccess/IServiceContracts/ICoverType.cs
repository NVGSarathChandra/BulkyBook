using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.IServiceContracts
{
    public interface ICoverType:IRepository<CoverType>
    {
        void Update(CoverType coverType);
    }
}
