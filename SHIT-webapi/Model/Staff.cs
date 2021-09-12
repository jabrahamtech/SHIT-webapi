using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SHIT_webapi.Model
{
    public class Staff
    {
        [Key]
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Url { get; set; }
        public string Research { get; set; }
    }
}
