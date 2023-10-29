using LibraryInc.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static LibraryInc.Models.ChartData;

namespace LibraryInc.Controllers
{
    public class ChartsController : Controller
    {
        private LibraryEntities1 db = new LibraryEntities1();

        // GET: Charts
        public ActionResult Index()
        {
            // Retrieve data for creating charts related to book types, students, books, borrows, and more.

            // Get the count of books by their types (grouped by type name).
            var bookCountsByType = db.books
                .GroupBy(b => b.types.name) // Use the TypeName property of the related Type entity
                .Select(group => new
                {
                    TypeName = group.Key,
                    Count = group.Count()
                })
                .ToList();

            // Create a ChartData object to hold the data for rendering charts in the view.
            var chartData = new ChartData
            {
                Labels = bookCountsByType.Select(x => x.TypeName).ToList(),
                Values = bookCountsByType.Select(x => x.Count).ToList(),
                TotalStudents = db.students.Count(),
                TotalBooks = db.books.Count(),
                TotalBorrows = db.borrows.Count(),
                MostPopularBooks = GetMostPopularBooks(), // Get a list of most popular books
                StudentsByClass = GetStudentsByClass() // Get the count of students by class
            };

            return View(chartData);
        }

        // Get the list of most popular books based on the number of borrows.
        private List<MostPopularBookViewModel> GetMostPopularBooks()
        {
            // Implement your logic to get the most popular books here.
            // Example: Return a list of books with the highest borrows.
            return db.books.OrderByDescending(b => b.borrows.Count)
                .Select(book => new MostPopularBookViewModel
                {
                    BookName = book.name,
                    TotalBorrows = book.borrows.Count
                })
                .Take(5) // Limit to the top 5 books
                .ToList();
        }

        // Get the count of students by class.
        private List<StudentsByClassViewModel> GetStudentsByClass()
        {
            // Implement your logic to get the count of students by class.
            // Example: Return the count of students in each class.
            return db.students.GroupBy(s => s.@class)
                .Select(group => new StudentsByClassViewModel
                {
                    ClassName = group.Key,
                    TotalStudents = group.Count()
                })
                .ToList();
        }

        // Display a chart showing gender distribution of students.
        public ActionResult Gender()
        {
            var genderCounts = db.students
                .GroupBy(s => s.gender)
                .Select(group => new
                {
                    Gender = group.Key,
                    Count = group.Count()
                })
                .ToList();

            var chartData = new ChartData
            {
                Labels = genderCounts.Select(x => x.Gender).ToList(),
                Values = genderCounts.Select(x => x.Count).ToList(),
            };

            return View(chartData);
        }

        // Display a chart showing the distribution of students by classes.
        public ActionResult Classes()
        {
            // Retrieve class information and student counts.
            var classInfo = db.students
                .GroupBy(s => s.@class)
                .Select(group => new
                {
                    ClassName = group.Key,
                    StudentCount = group.Count()
                })
                .ToList();

            // Create the ChartData object.
            var chartData = new ChartData
            {
                ClassNames = classInfo.Select(item => item.ClassName).ToList(),
                StudentCounts = classInfo.Select(item => item.StudentCount).ToList(),
                TotalStudents = db.students.Count(),
                TotalBooks = db.books.Count(),
                TotalBorrows = db.borrows.Count(),
                MostPopularBooks = GetMostPopularBooks(), // Get a list of most popular books
                StudentsByClass = GetStudentsByClass() // Get the count of students by class
            };

            return View(chartData);
        }
    }
}

