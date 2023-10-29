using LibraryInc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace LibraryInc.Models
{
    public class StudentBookViewModel
    {
        
        public IEnumerable<students> Students { get; set; }

        public IEnumerable<borrows> Borrowz { get; set; }
        public IPagedList<books> Books { get; set; }

        public IPagedList<borrows> Borrows { get; set; }

        public IPagedList<types> Types { get; set; }

        public IPagedList<authors> Authors { get; set; }

        public IPagedList<students> PageStudent { get; set; }


    }
}