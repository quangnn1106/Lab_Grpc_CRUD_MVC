using System;
using System.Collections.Generic;

#nullable disable

namespace GrpcService.Models
{
    public partial class Author
    {
        public Author()
        {
            Books = new HashSet<Book>();
        }

        public int Aid { get; set; }
        public string Aname { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
