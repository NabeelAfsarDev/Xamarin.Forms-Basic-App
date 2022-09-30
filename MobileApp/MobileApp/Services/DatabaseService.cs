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
    }
}
