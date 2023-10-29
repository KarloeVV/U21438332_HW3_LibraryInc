using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryInc.Models
{
    public class ReportViewModel
    {
        public ChartData ChartData { get; set; }
        public List<students> Students { get; set; }
        public List<books> Books { get; set; }
    }
}