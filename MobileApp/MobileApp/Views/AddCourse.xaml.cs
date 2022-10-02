using MobileApp.Models;
using MobileApp.Services;
using Plugin.LocalNotifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            //Validate Course information and course intructor information
            if(string.IsNullOrWhiteSpace(CourseTitle.Text) || string.IsNullOrWhiteSpace(CI.Text) || string.IsNullOrWhiteSpace(CiEmail.Text) || string.IsNullOrWhiteSpace(CiPhone.Text))
            {
                await App.Current.MainPage.DisplayAlert("Incomplete Entries", "Please make sure you provide a course title, course instructor, instructor email, and instructor phone number.", "OK");
                return;
            }
            if(CourseStart.Date >= CourseEnd.Date)
            {
                await App.Current.MainPage.DisplayAlert("Invalid Dates", "The start date can't be greater than the end date", "OK");
                return;
            }

            //Validate Email Address for course instructor
            Regex EmailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (!EmailRegex.IsMatch(CiEmail.Text.Trim()))
            {
                await App.Current.MainPage.DisplayAlert("Invalid Email", "Invalid course instructor email.", "OK");
                return;
            }

            //Validate the phone number
            string ciPhoneNumber = CiPhone.Text.Replace("-", string.Empty).Trim();
  
            if (!ciPhoneNumber.All(char.IsDigit))
            {
                await App.Current.MainPage.DisplayAlert("Invalid Phone", "Invalid course instructor phone number.", "OK");
                return;
            }
           
            //We need to make sure that only 6 courses are entered per term
            var termCourses = await DatabaseService.GetCourses(_termId) as ICollection<Course>;
            if(termCourses.Count == 6)
            {
                await App.Current.MainPage.DisplayAlert("Course Limit Reached", "There are already six courses in this term.", "OK");
                return;
            }

            //Create Notifications for course
            Random random = new Random();

            CrossLocalNotifications.Current.Show("Course Starts", $"{CourseTitle.Text} will start on {CourseStart.Date.ToShortDateString()}!", random.Next(1,1000), CourseStart.Date);
            CrossLocalNotifications.Current.Show("Course Due", $"{CourseTitle.Text} is due on {CourseEnd.Date.ToShortDateString()}!", random.Next(1, 1000), CourseEnd.Date.AddDays(-1));

            //Check to see if assessments are provided
            if (!string.IsNullOrEmpty(OA.Text))
            {
                if (OaStart.Date > OaEnd.Date)
                {
                    await App.Current.MainPage.DisplayAlert("Invalid Dates", "The O.A. start date can't be greater than the O.A. end date.", "OK");
                    return;
                }

                //Create Notifications for objective assessment
                CrossLocalNotifications.Current.Show("Objective Assessment", $"{OA.Text} will start on {OaStart.Date.ToShortDateString()}!", random.Next(1, 100), OaStart.Date);
                CrossLocalNotifications.Current.Show("Objective Assessment Due", $"{OA.Text} is due on {OaEnd.Date.ToShortDateString()}!", random.Next(1, 100) + 1, OaEnd.Date);

            }
            if (!string.IsNullOrEmpty(PA.Text))
            {
                if (PaStart.Date > PaEnd.Date)
                {
                    await App.Current.MainPage.DisplayAlert("Invalid Dates", "The P.A. start date can't be greater than the O.A. end date.", "OK");
                    return;
                }

                //Create Notifications for performance assessment
                CrossLocalNotifications.Current.Show("Performance Assessment", $"{PA.Text} will start on {OaStart.Date.ToShortDateString()}!", random.Next(1, 100), PaStart.Date);
                CrossLocalNotifications.Current.Show("Performance Assessment Due", $"{PA.Text} is due on {OaEnd.Date.ToShortDateString()}!", random.Next(1, 100) + 1, PaEnd.Date);

            }

            await DatabaseService.AddCourse(CourseTitle.Text, _termId, CourseNotes.Text, CourseStart.Date, CourseEnd.Date, (string)picker.SelectedItem, CI.Text, CiPhone.Text, CiEmail.Text, OA.Text, PA.Text, OaStart.Date, OaEnd.Date, PaStart.Date, PaEnd.Date);
            await Navigation.PopAsync();
        }

        private void CancelCourse_Clicked(object sender, EventArgs e)
        {

        }
    }
}