using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShortLinkAppV2._0.DTO
{
    public class URI
    {
        public string Id { get; set; }
        public string FullURI { get; set; }
        public string ShortURI { get; set; }
        public string Token { get; set; }
        public int Clicked { get; set; } = 0;
        public DateTime Created { get; set; }
        public string Creater { get; set; }
    }
}
