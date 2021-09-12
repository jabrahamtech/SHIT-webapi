using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SHIT_webapi.Model;

namespace SHIT_webapi.Data
{
    public class DBWebAPIRepo : IWebAPIRepo
    {
        private readonly WebAPIDBContext _dbContext;
        public DBWebAPIRepo(WebAPIDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Staff> GetAllStaff()
        {
            IEnumerable<Staff> Staff = _dbContext.Staff.ToList<Staff>();
            return Staff;
        }

        public IEnumerable<Product> GetItems()
        {
            IEnumerable<Product> Products = _dbContext.Products.ToList<Product>();
            return Products;
        }

        public IEnumerable<Product> GetItems(string name)
        {
            IEnumerable<Product> Products = _dbContext.Products.Where(e => e.Name.ToLower().Contains(name.ToLower())).ToList<Product>();
            return Products;
        }

        public Staff CheckStaff(int id)
        {
            Staff Staff = _dbContext.Staff.FirstOrDefault(e => e.Id == id);
            return Staff;
        }

        public Product CheckItem(int id)
        {
            Product Item = _dbContext.Products.FirstOrDefault(e => e.Id == id);
            return Item;
        }

        public Comments AddComment(Comments comment)
        {
            EntityEntry<Comments> e = _dbContext.Comments.Add(comment);
            Comments c = e.Entity;
            _dbContext.SaveChanges();
            return c;
        }

        public IEnumerable<Comments> GetAllComments()
        {
            IEnumerable<Comments> comments = _dbContext.Comments.ToList<Comments>();
            return comments;
        }
    }
}
