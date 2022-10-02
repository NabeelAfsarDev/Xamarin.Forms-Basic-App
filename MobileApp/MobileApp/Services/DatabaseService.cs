using MobileApp.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MobileApp.Services
{
    public static class DatabaseService
    {
        private static SQLiteAsyncConnection _db;
       
        static async Task Init()
        {
            if (_db != null)
                return;

            //Get absolute path to the database file
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "Schedule.db");

            _db = new SQLiteAsyncConnection(databasePath);
            await _db.CreateTableAsync<Term>();
            await _db.CreateTableAsync<Course>();

      
        }

        public static async Task AddTerm(string title, DateTime start, DateTime end)
        {
            await Init();
            var term = new Term
            {
                Title = title,
                StartDate = TimeZoneInfo.ConvertTimeToUtc(start,TimeZoneInfo.Local),
                EndDate = TimeZoneInfo.ConvertTimeToUtc(end, TimeZoneInfo.Local)
            };

            var id = await _db.InsertAsync(term);

        }

        public static async Task DeleteTerm(int id)
        {
            await Init();
            await _db.DeleteAsync<Term>(id);
        }

        public static async Task<IEnumerable<Term>> GetTerms()
        {
            await Init();
            var terms = await _db.Table<Term>().ToListAsync();
            foreach(var term in terms)
            {
                term.StartDate = TimeZoneInfo.ConvertTimeFromUtc(term.StartDate, TimeZoneInfo.Local);
                term.EndDate = TimeZoneInfo.ConvertTimeFromUtc(term.EndDate, TimeZoneInfo.Local);
            }
            return terms;
        }

        public static async Task UpdateTerm(int id, string title, DateTime start, DateTime end)
        {
            await Init();

            var inDb = await _db.Table<Term>().Where(w => w.Id == id).FirstOrDefaultAsync();
            if (inDb != null)
            {
                inDb.Title = title;
                inDb.StartDate = start;
                inDb.EndDate = end;
                await _db.UpdateAsync(inDb);
            }
        }

        public static async Task AddCourse(string title, int termId, string notes, DateTime start, DateTime end, string status, string instructor, string instructorPhone, string instructorEmail, string oa, string pa, DateTime oastart, DateTime oaEnd, DateTime paStart, DateTime paEnd)
        {
            await Init();
            var course = new Course
            {
               TermId = termId,
               Title = title,
               Status = status,
               CourseNotes = notes,
               CourseStart = TimeZoneInfo.ConvertTimeToUtc(start, TimeZoneInfo.Local),
               CourseEnd = TimeZoneInfo.ConvertTimeToUtc(end, TimeZoneInfo.Local),
               InstructorName = instructor,
               InstructorEmail = instructorEmail,
               InstructorPhone = instructorPhone,
               ObjectiveAssessment = oa,
               PerformanceAssessment = pa,
               OaStart = TimeZoneInfo.ConvertTimeToUtc(oastart, TimeZoneInfo.Local),
               OaEnd = TimeZoneInfo.ConvertTimeToUtc(oaEnd, TimeZoneInfo.Local),
               PaStart = TimeZoneInfo.ConvertTimeToUtc(paStart, TimeZoneInfo.Local),
               PaEnd = TimeZoneInfo.ConvertTimeToUtc(paEnd, TimeZoneInfo.Local)
            };

            var id = await _db.InsertAsync(course);

        }


        public static async Task DeleteCourse(int id)
        {
            await Init();
            await _db.DeleteAsync<Course>(id);
        }

        public static async Task<IEnumerable<Course>> GetCourses(int termId)
        {
            await Init();
            var courses = await _db.Table<Course>().Where(c => c.TermId == termId).ToListAsync();
            foreach(var course in courses)
            {
                course.CourseStart = TimeZoneInfo.ConvertTimeToUtc(course.CourseStart, TimeZoneInfo.Local);
                course.CourseEnd = TimeZoneInfo.ConvertTimeToUtc(course.CourseEnd, TimeZoneInfo.Local);
                course.OaStart = TimeZoneInfo.ConvertTimeToUtc(course.OaStart, TimeZoneInfo.Local);
                course.OaEnd = TimeZoneInfo.ConvertTimeToUtc(course.OaEnd, TimeZoneInfo.Local);
                course.PaStart = TimeZoneInfo.ConvertTimeToUtc(course.PaStart, TimeZoneInfo.Local);
                course.PaEnd = TimeZoneInfo.ConvertTimeToUtc(course.PaEnd, TimeZoneInfo.Local);

            }
            return courses;
        }

        public static async Task UpdateCourse(int courseId, string title, DateTime start, DateTime end, string status,string notes, string instructor, string instructorPhone, string instructorEmail, string oa, string pa, DateTime oastart, DateTime oaEnd, DateTime paStart, DateTime paEnd)
        {
            await Init();

            var course = await _db.Table<Course>().Where(c => c.Id == courseId).FirstOrDefaultAsync();
            if (course != null)
            {
               course.Title = title;
               course.CourseStart = TimeZoneInfo.ConvertTimeToUtc(start, TimeZoneInfo.Local);
               course.CourseEnd = TimeZoneInfo.ConvertTimeToUtc(end, TimeZoneInfo.Local);
                course.Status = status;
                course.CourseNotes = notes;
               course.InstructorName = instructor;
               course.InstructorEmail = instructorEmail;
               course.InstructorPhone = instructorPhone;
               course.ObjectiveAssessment = oa;
               course.PerformanceAssessment = pa;
               course.OaStart = TimeZoneInfo.ConvertTimeToUtc(oastart, TimeZoneInfo.Local);
                course.OaEnd = TimeZoneInfo.ConvertTimeToUtc(oaEnd, TimeZoneInfo.Local);
                course.PaStart = TimeZoneInfo.ConvertTimeToUtc(paStart, TimeZoneInfo.Local);
                course.PaEnd = TimeZoneInfo.ConvertTimeToUtc(paEnd, TimeZoneInfo.Local);
                await _db.UpdateAsync(course);
            }
        }
    }
}
