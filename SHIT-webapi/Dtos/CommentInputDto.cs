using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SHIT_webapi
{
    public class CommentInputDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Comment { get; set; }
    }
}
