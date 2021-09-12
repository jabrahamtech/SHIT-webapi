using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System;

namespace SHIT_webapi.Model
{
    public class Comments
    {
        [Key]
        public int Id { get; set; }
        public string Time { get; set; }
        public string Comment { get; set; }
        public string Name { get; set; }
        public string IP { get; set; }

    }
}
