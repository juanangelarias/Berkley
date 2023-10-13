using JamesDAL.Server.Model;
using System;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Diagnostics;

namespace James.Shared.Test
{
    public class UnitTest1
    {
        private Random _rnd = new Random((int)DateTime.Now.Ticks);
        [Fact]
        //public async Task Test1()
        public void Test1()
        {
            var randomDbName = $"Test1Db{_rnd.Next(1000000,9999999)}";
            var jamesDbContext = new JamesDatabaseContext(randomDbName, true, true);
            //await jamesDbContext.Database.MigrateAsync();
            jamesDbContext.Database.Migrate();
            Assert.NotEqual(0, jamesDbContext.AccountRates.Count());
        }
        [Fact]
        public void Test2()
        {
            Debug.WriteLine("Ultrasimple test to prove tests are running.");
            Assert.True(true);
        }
    }
}