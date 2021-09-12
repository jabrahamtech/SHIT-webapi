using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SHIT_webapi.Model;

namespace SHIT_webapi.Data
{
    public interface IWebAPIRepo
    {
        IEnumerable<Staff> GetAllStaff();
        IEnumerable<Product> GetItems();
        IEnumerable<Product> GetItems(string name);
        Staff CheckStaff(int Id);
        Product CheckItem(int Id);
        Comments AddComment(Comments comment);
        IEnumerable<Comments> GetAllComments();
    }
}
