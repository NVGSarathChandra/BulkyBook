using Bulky_Book_Project.Dataaccess.data;
using DataAccess.IServiceContracts;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DataAccess.Repositories
{
    public class ProductRepository : Repository<Product>, IProduct
    {
        private readonly ApplicationDbContext dbContext;
        public ProductRepository(ApplicationDbContext _dbContext) : base(_dbContext)
        {
            dbContext = _dbContext;
        }

        public void Update(Product product)
        {
            var resultFromDb = dbContext.Products.FirstOrDefault(a => a.ProductID == product.ProductID);
            if (resultFromDb != null)
            {
                if (product.ImageURL != null)
                {
                    resultFromDb.ImageURL = product.ImageURL;
                }
                resultFromDb.ProductTitle = product.ProductTitle;
                resultFromDb.ProductDescription = product.ProductDescription;
                resultFromDb.ProductAuthor = product.ProductAuthor;
                resultFromDb.ProductISBN = product.ProductISBN;
                resultFromDb.ListPrice = product.ListPrice;
                resultFromDb.Price = product.Price;
                resultFromDb.Price50 = product.Price50;
                resultFromDb.Price100 = product.Price100;
                resultFromDb.CoverTypeId = product.CoverTypeId;
                resultFromDb.CategoryId = product.CategoryId;
            }
        }
    }
}
