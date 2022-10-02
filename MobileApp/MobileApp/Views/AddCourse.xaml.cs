using MobileApp.Models;
using MobileApp.Services;
using Plugin.LocalNotifications;
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
    public partial class AddCourse : ContentPage
    {
        private readonly int _termId;
        public AddCourse(int termId)
        {
            _termId = termId;
            InitializeComponent();
        }

        private async void SaveCourse_Clicked(object sender, EventArgs e)
        {
            if(CourseStart.Date >= CourseEnd.Date)
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
            var termCourses = await DatabaseService.GetCourses(_termId) as ICollection<Course>;
            if(termCourses.Count == 6)
            {
                await App.Current.MainPage.DisplayAlert("Course Limit Reached", "There are already six courses in this term.", "OK");
                return;
            }
            //Create Notifications for course
            CrossLocalNotifications.Current.Show("Course Start!", $"{CourseTitle.Text} will start on {CourseStart.Date.ToShortDateString()}!", int.Parse(CourseId.Text), CourseStart.Date.AddDays(-1));
            CrossLocalNotifications.Current.Show("Course Ends Soon!", $"{CourseTitle.Text} will start on {CourseEnd.Date.ToShortDateString()}!", int.Parse(CourseId.Text)+1, CourseEnd.Date.AddDays(-1));

            await DatabaseService.AddCourse(CourseTitle.Text, _termId, CourseNotes.Text, CourseStart.Date, CourseEnd.Date, (string)picker.SelectedItem, CI.Text, CiPhone.Text, CiEmail.Text, OA.Text, PA.Text, OaStart.Date, OaEnd.Date, PaStart.Date, PaEnd.Date);
            await Navigation.PopAsync();
        }

        private void CancelCourse_Clicked(object sender, EventArgs e)
        {

        }
    }
}