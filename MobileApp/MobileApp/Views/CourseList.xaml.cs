using MobileApp.Models;
using MobileApp.Services;
using MobileApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CourseList : ContentPage
    {
        private readonly int _termId;
        public CourseList(Term term)
        {
            InitializeComponent();
            _termId = term.Id;
            lblTermId.Text = term.Id.ToString();
            lblTermTitle.Text = term.Title;            
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            
            var courses = await DatabaseService.GetCourses(_termId);
            var coursesVm = new List<CoursesViewModel>();
            foreach(var course in courses)
            {
                var courseToAdd = new CoursesViewModel
                {
                    Id = course.Id,
                    Title = course.Title,
                    CourseStartEnd = $"{course.CourseStart.ToShortDateString()}-{course.CourseEnd.ToShortDateString()}",
                    Status = course.Status,
                    CourseNotes = course.CourseNotes,
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
            CourseCollection.ItemsSource = coursesVm;
        }

        private async void CourseCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null)
            {
                CoursesViewModel course = (CoursesViewModel)e.CurrentSelection.FirstOrDefault();
                var toSend = new Course
                {
                    Id = course.Id,
                    Title = course.Title,
                    CourseStart = Convert.ToDateTime(course.CourseStartEnd.Split('-')[0]),
                    CourseEnd = Convert.ToDateTime(course.CourseStartEnd.Split('-')[1]),
                    Status = course.Status,
                    CourseNotes = course.CourseNotes,
                    InstructorName = course.InstructorName,
                    InstructorEmail = course.InstructorEmail,
                    InstructorPhone = course.InstructorPhone,
                    ObjectiveAssessment = course.ObjectiveAssessment,
                    PerformanceAssessment = course.PerformanceAssessment,
                    OaStart = Convert.ToDateTime(course.OaStartEnd.Split('-')[0]),
                    OaEnd = Convert.ToDateTime(course.OaStartEnd.Split('-')[1]),
                    PaStart = Convert.ToDateTime(course.PaStartEnd.Split('-')[0]),
                    PaEnd = Convert.ToDateTime(course.PaStartEnd.Split('-')[1]),
                    TermId = _termId
                };
                int termId = int.Parse(lblTermId.Text);
                await Navigation.PushAsync(new EditCourse(toSend, termId));
            }
        }

        private async void AddCourses_Clicked(object sender, EventArgs e)
        {
            int termId = int.Parse(lblTermId.Text);
            await Navigation.PushAsync(new AddCourse(termId));
        }

        private async void ShareNotes_Clicked(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var course = btn.CommandParameter as CoursesViewModel;

            //Make sure course notes are added before sharing.
            if (string.IsNullOrWhiteSpace(course.CourseNotes))
            {
                await App.Current.MainPage.DisplayAlert("Share Error", "No assessment notes to share!", "OK");
                return;
            }
            //Share the notes.
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = course.CourseNotes,
                Title = "Course Notes!"
            });
        }
    }
}