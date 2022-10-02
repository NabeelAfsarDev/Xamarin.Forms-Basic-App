using MobileApp.Models;
using MobileApp.Services;
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
    public partial class EditCourse : ContentPage
    {
        private readonly int _termId;
        public EditCourse(Course course, int termId)
        {
            _termId = termId;
            InitializeComponent();
            CourseId.Text = course.Id.ToString();
            CourseTitle.Text = course.Title;
            CourseStart.Date = course.CourseStart;
            CourseEnd.Date = course.CourseEnd;
            CI.Text = course.InstructorName;
            CiEmail.Text = course.InstructorEmail;
            CiPhone.Text = course.InstructorPhone;
            OA.Text = course.ObjectiveAssessment;
            OaStart.Date = course.OaStart;
            OaEnd.Date = course.OaEnd;
            PA.Text = course.PerformanceAssessment;
            PaStart.Date = course.PaStart;
            PaEnd.Date = course.PaEnd;
            CourseNotes.Text = course.CourseNotes;
            picker.SelectedItem = (string)course.Status;
        }

        private async void DeleteCourse_Clicked(object sender, EventArgs e)
        {
            int id = int.Parse(CourseId.Text);
            await DatabaseService.DeleteCourse(id);
            await Navigation.PopAsync();
        }

        private async void SaveCourse_Clicked(object sender, EventArgs e)
        {
            if (CourseStart.Date >= CourseEnd.Date)
            {
                await App.Current.MainPage.DisplayAlert("Invalid Dates", "The start date can't be greater than the end date", "OK");
                return;
            }
            if (OaStart.Date > OaEnd.Date)
            {
                await App.Current.MainPage.DisplayAlert("Invalid Dates", "The O.A. start date can't be greater than the O.A. end date.", "OK");
                return;
            }
            if (PaStart.Date > PaEnd.Date)
            {
                await App.Current.MainPage.DisplayAlert("Invalid Dates", "The P.A. start date can't be greater than the P.A. end date.", "OK");
                return;
            }
            int courseId = int.Parse(CourseId.Text);
            await DatabaseService.UpdateCourse(courseId, CourseTitle.Text, CourseStart.Date, CourseEnd.Date, (string)picker.SelectedItem, CourseNotes.Text, CI.Text, CiPhone.Text, CiEmail.Text, OA.Text, PA.Text, OaStart.Date, OaEnd.Date, PaStart.Date, PaEnd.Date);
            await Navigation.PopAsync();
        }

        private async void CancelCourse_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}