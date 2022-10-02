using MobileApp.Models;
using MobileApp.Services;
using MobileApp.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var dashboard = new Dashboard();
            var navPage = new NavigationPage(dashboard);
            MainPage = navPage;

        }

        protected override void OnStart()
        {
             SeedDatabase();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        private async void SeedDatabase()
        {
            //Code to setup sample term and course
            var sampleTerm = new Term
            {
                Title = "Sample Term",
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddMonths(2)
             };

             await DatabaseService.AddTerm(sampleTerm.Title, sampleTerm.StartDate, sampleTerm.EndDate);
             var terms = await DatabaseService.GetTerms();
            var termId = terms.Where(t => t.Title == sampleTerm.Title).FirstOrDefault().Id;

             var sampleCourse = new Course
             {
                 TermId = termId,
                 Title = "Sample Course",
                 CourseStart = DateTime.Today,
                 CourseEnd = DateTime.Today.AddDays(20),
                 InstructorName = "Nabeel Afsar",
                 InstructorEmail = "nafsar1@wgu.edu",
                 ObjectiveAssessment = "OA01",
                 InstructorPhone = "331-250-6258",
                 OaStart = DateTime.Today.AddDays(15),
                 OaEnd = DateTime.Today.AddDays(16),
                 PerformanceAssessment = "PA01",
                 PaStart = DateTime.Today.AddDays(17),
                 PaEnd = DateTime.Today.AddDays(18),
                 CourseNotes = "Course sample notes",
                 Status = "Plan to Take"
             };

             DatabaseService.AddCourse(sampleCourse.Title, sampleCourse.TermId, sampleCourse.CourseNotes, sampleCourse.CourseStart, sampleCourse.CourseEnd, sampleCourse.Status, sampleCourse.InstructorName, sampleCourse.InstructorPhone, sampleCourse.InstructorEmail, sampleCourse.ObjectiveAssessment, sampleCourse.PerformanceAssessment, sampleCourse.OaStart, sampleCourse.OaEnd, sampleCourse.PaStart, sampleCourse.PaEnd);
          
        }
    }
}
