using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.IServiceContracts
{
    public interface IProduct : IRepository<Product>
    {
        void Update(Product product);
    }
}
