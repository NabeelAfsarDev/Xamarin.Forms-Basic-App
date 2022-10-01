using MobileApp.Models;
using MobileApp.Services;
using MobileApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CourseList : ContentPage
    {
        private readonly int _termId;
        private readonly string _termTitle;
        public CourseList(Term term)
        {
            InitializeComponent();
            _termId = term.Id;
            _termTitle = term.Title;
            lblTermId.Text = term.Id.ToString();
            lblTermTitle.Text = term.Title;

            this.OnAppearing();
            
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            
            var courses = await DatabaseService.GetCourses(_termId);

            //Assign the label for term title

            var coursesViewModel = new CourseListViewModel();
            var coursesVm = new List<CoursesViewModel>();
            foreach(var course in courses)
            {
                var courseToAdd = new CoursesViewModel
                {
                    Title = course.Title,
                    CourseStartEnd = $"{course.CourseStart.ToShortDateString()}-{course.CourseEnd.ToShortDateString()}",
                    Status = course.Status,
                    InstructorName = course.InstructorName,
                    InstructorEmail = course.InstructorEmail,
                    InstructorPhone = course.InstructorPhone,
                    ObjectiveAssessment = course.ObjectiveAssessment,
                    PerformanceAssessment = course.PerformanceAssessment,
                    OaStartEnd = $"{course.OaStart.ToShortDateString()}-{course.OaEnd.ToShortDateString()}",
                    PaStartEnd = $"{course.PaStart.ToShortDateString()}-{course.PaEnd.ToShortDateString()}"
                };

                coursesVm.Add(courseToAdd);
            }
            coursesViewModel.Courses = coursesVm;
            
            CourseCollection.ItemsSource = coursesVm;
        }

        private void CourseCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AddCourses_Clicked(object sender, EventArgs e)
        {

        }
    }
}