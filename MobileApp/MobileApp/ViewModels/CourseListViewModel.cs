using System;
using System.Collections.Generic;
using System.Text;

namespace MobileApp.ViewModels
{
    public class CourseListViewModel
    {
        public int TermId { get; set; }
        public List<CoursesViewModel> Courses {get;set;}
    }
}
