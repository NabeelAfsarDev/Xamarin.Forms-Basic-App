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
            return terms;
        }

        public static async Task UpdateWidget(int id, string title, DateTime start, DateTime end)
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

        public static async Task AddCourse(string title, int termId, DateTime start, DateTime end, string status, string instructor, string instructorPhone, string instructorEmail, string oa, string pa, DateTime oastart, DateTime oaEnd, DateTime paStart, DateTime paEnd)
        {
            await Init();
            var course = new Course
            {
               TermId = termId,
               Title = title,
               CourseStart = start,
               CourseEnd = end,
               InstructorName = instructor,
               InstructorEmail = instructorEmail,
               InstructorPhone = instructorPhone,
               ObjectiveAssessment = oa,
               PerformanceAssessment = pa,
               OaStart = oastart,
               OaEnd = oaEnd,
               PaStart = paStart,
               PaEnd = paEnd
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
            return courses;
        }

        public static async Task UpdateWidget(int courseId, string title, DateTime start, DateTime end, string status, string instructor, string instructorPhone, string instructorEmail, string oa, string pa, DateTime oastart, DateTime oaEnd, DateTime paStart, DateTime paEnd)
        {
            await Init();

            var course = await _db.Table<Course>().Where(c => c.Id == courseId).FirstOrDefaultAsync();
            if (course != null)
            {
               course.Title = title;
               course.CourseStart = start;
               course.CourseEnd = end;
               course.InstructorName = instructor;
               course.InstructorEmail = instructorEmail;
               course.InstructorPhone = instructorPhone;
               course.ObjectiveAssessment = oa;
               course.PerformanceAssessment = pa;
               course.OaStart = oastart;
               course.OaEnd = oaEnd;
               course.PaStart = paStart;
                course.PaEnd = paEnd;
                await _db.UpdateAsync(course);
            }
        }
    }
}
