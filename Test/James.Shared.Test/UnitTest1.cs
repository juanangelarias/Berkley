using James.Shared.Model;
using System;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Diagnostics;
using James.Data.Server.Model;
using Xunit.Abstractions;

namespace James.Shared.Test
{
    public class UnitTest1
    {

        private readonly ITestOutputHelper output;
        public UnitTest1(ITestOutputHelper output)
        {  this.output = output; }

        private Random _rnd = new Random((int)DateTime.Now.Ticks);
        [Fact]
        //public async Task Test1()
        public async Task Test1()
        {
            //var randomDbName = $"Test1Db{_rnd.Next(1000000,9999999)}";
            //var jamesDbContext = new JamesDatabaseContext(randomDbName, true, true);
            ////Debug.WriteLine("Creating database: " + randomDbName);
            //output.WriteLine("Creating database: " + randomDbName);
            //await jamesDbContext.Database.MigrateAsync();
            //Assert.NotEqual(0, jamesDbContext.AccountRates.Count());
            //await jamesDbContext.Database.EnsureDeletedAsync();
        }
        [Fact]
        public void Test2()
        {
            output.WriteLine("Ultra simple test to prove tests are running.");
            Assert.True(true);
        }
    }
}