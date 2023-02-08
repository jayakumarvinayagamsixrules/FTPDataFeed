using DataFeed.Model;
using DataFeed.Persistence;
using DataFeed.Service.FolderProcess;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;

namespace FTPDataFeed
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var setting = new MongoOptions()
            {
                ConnectionString = "mongodb://localhost:27017",
                Db = "catalog",
            };

            //var db = new MongoClient(setting);

            var services = new ServiceCollection();
            services.AddSingleton<IMoveFile, MoveFile>();
            services.AddSingleton<DataFeedProcess>();
            services.AddScoped<IMongoContext, MongoContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IECCHRepository, ECCHRepository>();
            // Read configuration file
            FileSource fileSource = new FileSource
            {
                Destination = ConfigurationManager.AppSettings["DataFeedDestination"],
                Source = ConfigurationManager.AppSettings["DataFeedSource"],
                ProcessClientA = ConfigurationManager.AppSettings["DataFeedProcessA"],
                ProcessClientB = ConfigurationManager.AppSettings["DataFeedProcessB"]
            };

            var dataFeedServiceProvider = services.BuildServiceProvider();
            var dataFeedService = dataFeedServiceProvider.GetService<DataFeedProcess>();
    

            dataFeedService.Start(fileSource);
            Console.ReadKey(true);

        }
    }
}
