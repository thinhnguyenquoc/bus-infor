using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bus_Info.Entities
{
    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public string Tag { get; set; }
        public string TopImage { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? Publish { get; set; }
        public bool? New { get; set; }
        public int? UserId { get; set; }
        public string Genre { get; set; }
        public string TopDetailImage { get; set; }
        public DateTime? PublishDate { get; set; }
    }
}
