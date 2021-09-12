using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SHIT_webapi.Data;
using SHIT_webapi.Model;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using SHIT_webapi.Helper;
using SHIT_webapi.Dtos;
using System;
using System.Net;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SHIT_webapi
{
    [Route("api")]
    [ApiController]
    public class StaffController : Controller
    {
        // GET: /<controller>/
        private readonly IWebAPIRepo _repository;

        public StaffController(IWebAPIRepo repository)
        {
            _repository = repository;
        }

        //Get: /GetLogo Q1
        [HttpGet("GetLogo")]
        public ActionResult GetLogo()
        {
            string path = Directory.GetCurrentDirectory();
            string folder = Path.Combine(path, "Images");
            string imgDir = Path.Combine(folder, "StaffPhotos");
            string fileName = Path.Combine(imgDir, "logo.png");
            string respHeader = "image/png";
            return PhysicalFile(fileName, respHeader);
        }

        //Get: /GetVersion Q2
        [HttpGet("GetVersion")]
        public ContentResult Version()
        {
            return Content("V1");
        }

        //Get: /GetAllStaff Q3
        [HttpGet("GetAllStaff")]
        public ActionResult<IEnumerable<StaffOutDto>> GetStaff()
        {
            IEnumerable<Staff> Staff = _repository.GetAllStaff();
            IEnumerable<StaffOutDto> s = Staff.Select(e=> new StaffOutDto{ Id = e.Id, FirstName = e.FirstName, LastName = e.LastName, Title = e.Title, Email = e.Email, Tel = e.Tel, Url = e.Url, Research = e.Research});
            return Ok(s);
        }

        // Get: /GetStaffPhoto Q4
        [HttpGet("GetStaffPhoto/{id}")]
        public ActionResult GetStaffPhoto(int id)
        {
            Staff Staff = _repository.CheckStaff(id);
            string path = Directory.GetCurrentDirectory();
            string folder = Path.Combine(path, "Images");
            string imgDir = Path.Combine(folder, "StaffPhotos");
            string fileName = Path.Combine(imgDir, "default.png");
            string respHeader = "image/png";
            string respHeader1 = "image/jpeg";
            string fileName1 = Path.Combine(imgDir, id + ".jpg");

            if (Staff == null)
                return PhysicalFile(fileName, respHeader);
            else
            {
                return PhysicalFile(fileName1, respHeader1);
            }
        }

        // Get: /GetCard Q5
        [HttpGet("GetCard/{id}")]
        public ActionResult GetCard(int id)
        {
            Staff staff = _repository.CheckStaff(id);
            string path = Directory.GetCurrentDirectory();
            string fileName = Path.Combine(path, "Images/StaffPhotos/" + id + ".jpg");
            string defaultCard = Path.Combine(path, "Images/StaffPhotos/" + "logo.png");
            string photoString, photoType;
            ImageFormat imageFormat;
            if (System.IO.File.Exists(fileName))
            {
                Image image = Image.FromFile(fileName);
                imageFormat = image.RawFormat;
                image = ImageHelper.Resize(image, new Size(200, 200), out photoType);
                photoString = ImageHelper.ImageToString(image, imageFormat);
            }
            else
            {
                Image image = Image.FromFile(defaultCard);
                imageFormat = image.RawFormat;
                image = ImageHelper.Resize(image, new Size(100, 100), out photoType);
                photoString = ImageHelper.ImageToString(image, imageFormat);
            }
            if (staff == null)
            {
                CardOut cardOut = new CardOut();
                cardOut.Name = null;
                cardOut.Photo = photoString;
                cardOut.PhotoType = photoType;
                cardOut.Email = null;
                cardOut.Categories = null;
                Response.Headers.Add("Content-Type", "text/vcard");
                return Ok(cardOut);
             }
            else
            {
                CardOut cardOut = new CardOut();
                cardOut.Name = staff.Title + ' ' + staff.FirstName + ' ' + staff.LastName;
                cardOut.Photo = photoString;
                cardOut.PhotoType = photoType;
                cardOut.Email = staff.Email;
                cardOut.Categories = Helper.ResearchFilter.Filter(staff.Research);
                Response.Headers.Add("Content-Type", "text/vcard");
                return Ok(cardOut);
            }
        }

        // Get: /GetItems Q6
        [HttpGet("GetItems")]
        public ActionResult<IEnumerable<ProductOutDto>> GetItems()
        {
            IEnumerable<Product> Products = _repository.GetItems();
            IEnumerable<ProductOutDto> p = Products.Select(e => new ProductOutDto { Id = e.Id, Description = e.Description, Name = e.Name, Price = e.Price });
            return Ok(p);
        }

        [HttpGet("GetItems/{name}")]
        public ActionResult<IEnumerable<ProductOutDto>> GetItems(string name)
        {
            IEnumerable<Product> Products = _repository.GetItems(name);
            IEnumerable<ProductOutDto> p = Products.Select(e => new ProductOutDto { Id = e.Id, Description = e.Description, Name = e.Name, Price = e.Price });
            return Ok(p);
        }

        // Get: /GetItemPhoto Q7
        [HttpGet("GetItemPhoto/{id}")]
        public ActionResult GetItemPhoto(int id)
        {
            Product item = _repository.CheckItem(id);
            string path = Directory.GetCurrentDirectory();
            string folder = Path.Combine(path, "Images");
            string imgDir = Path.Combine(folder, "ItemsImages");
            string fileName = Path.Combine(imgDir, "default.png");
            string respHeader = "image/png";
            string respHeader1 = "image/jpeg";
            string fileName1 = Path.Combine(imgDir, id + ".jpg");
            string fileName2 = Path.Combine(imgDir, id + ".png");

            if (item == null)
                return PhysicalFile(fileName, respHeader);
            else
            {
                if (System.IO.File.Exists(fileName1))
                    return PhysicalFile(fileName1, respHeader1);
                return PhysicalFile(fileName2, respHeader);
            }
        }

        // Put: /WriteComment Q8
        [HttpPost("WriteComment")]
        public ActionResult<CommentInputDto> AddComment(CommentInputDto comment)
        {
            Random random = new Random();
            int num = random.Next();
            string time = DateTime.Now.ToString("h:mm:ss tt");
            var ip = Request.HttpContext.Connection.RemoteIpAddress;
            Comments c = new Comments {Id = num,  Time = time, Comment = comment.Comment,  Name = comment.Name, IP =  ip.ToString()};
            Comments addedComment = _repository.AddComment(c);
            return Ok(addedComment);
        }

        //Get: /Getcomments Q9
        [HttpGet("GetComments")]
        public ContentResult GetComments()
        {
            IEnumerable<Comments> comments = _repository.GetAllComments().OrderByDescending(e => e.Time).Take(5);
            IEnumerable<CommentOutDto> c = comments.Select(e => new CommentOutDto { Comment = e.Comment, Name = e.Name });
            string comment = "<html><body>";
            foreach(var i in c)
            {
                comment += "<p>" + i.Comment + "-" + i.Name + "</p>";
            }
            comment += "</body></html>";
            ContentResult j = new ContentResult
            {
                Content = comment,
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK
            };
            return j;
        }

    }
}