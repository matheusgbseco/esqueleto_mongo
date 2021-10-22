using Common.Domain.Interfaces;
using Common.Domain.Model;
using MongoDB.Driver;
using Project.Core.Data.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Data.Context
{
    public class DbContextMongoDb : IContextMongoDb
    {
        public string DatabaseName { get ; set; }
        public string ConnectionString { get; set; }
    }
}
