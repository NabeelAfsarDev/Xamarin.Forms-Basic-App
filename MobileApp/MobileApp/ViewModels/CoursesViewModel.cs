using System;
using System.Collections.Generic;
using System.Text;

namespace MobileApp.ViewModels
{
    public class CoursesViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CourseStartEnd { get; set; }
        public string Status { get; set; }
        public string InstructorName { get; set; }
        public string InstructorPhone { get; set; }
        public string InstructorEmail { get; set; }
        public string ObjectiveAssessment { get; set; }
        public string PerformanceAssessment { get; set; }
        public string OaStartEnd { get; set; }
        public string PaStartEnd { get; set; }
    }
}
