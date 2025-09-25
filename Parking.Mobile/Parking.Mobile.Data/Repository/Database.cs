using System;
using System.Collections.Generic;
using System.IO;
using SQLite;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Parking.Mobile.Entity;
using Parking.Mobile.DependencyService.Interfaces;

namespace Parking.Mobile.Data.Repository
{
    public class Database
    {
        private SQLiteConnection _connection;

        public SQLiteConnection Connection
        {
            get
            {
                return _connection;
            }
            set
            {
                _connection = value;
            }
        }

        private string _dbFileName = "dbParkingMobileApp.db";

        public void DropTables()
        {
            _connection.DropTable<ConfigurationApp>();

            CreateTables();
        }

        public void CreateTables()
        {
            _connection.CreateTable<ConfigurationApp>();
        }

        public Database()
        {
            IFilePath dep = Xamarin.Forms.DependencyService.Get<IFilePath>();

            string filePath = dep.GetPath();

            string fileName = Path.Combine(filePath, _dbFileName);

            _connection = new SQLiteConnection(fileName);

            CreateTables();

            var size = GetDatabaseSize();
        }

        public long GetDatabaseSize()
        {
            IFilePath dep = Xamarin.Forms.DependencyService.Get<IFilePath>();

            string filePath = dep.GetPath();

            string fileName = Path.Combine(filePath, _dbFileName);

            return dep.GetFileSize(fileName);
        }

        private int CountRecords(string tableName)
        {
            var query = $"SELECT COUNT(*) FROM {tableName}";

            return _connection.ExecuteScalar<int>(query);
        }
    }
}

