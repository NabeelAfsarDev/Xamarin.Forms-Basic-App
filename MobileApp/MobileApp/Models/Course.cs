using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobileApp.Models
{
    public class Course
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CourseStart { get; set; }
        public DateTime CourseEnd { get; set; }
        [Indexed]
        public int TermId { get; set; }
        public string Status { get; set; }

        public string InstructorName { get; set; }
        public string InstructorPhone { get; set; }
        public string InstructorEmail { get; set; }
        public string ObjectiveAssessment{ get; set; }
        public string PerformanceAssessment { get; set; }
        public DateTime OaStart { get; set; }
        public DateTime OaEnd { get; set; }
        public DateTime PaStart { get; set; }
        public DateTime PaEnd { get; set; }
    }
}
