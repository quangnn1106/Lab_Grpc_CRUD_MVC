using System;
using System.Collections.Generic;

#nullable disable

namespace GrpcService.Models
{
    public partial class Book
    {
        public int Bid { get; set; }
        public string Bname { get; set; }
        public int? Bversion { get; set; }
        public int? Bpages { get; set; }
        public int? Bprice { get; set; }
        public int? Byear { get; set; }
        public int? Aid { get; set; }

        public virtual Author AidNavigation { get; set; }
    }
}
