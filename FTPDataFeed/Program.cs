using DataFeed.Model;
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

            // Read configuration file
            FileSource fileSource = new FileSource
            {
                Destination = ConfigurationManager.AppSettings["DataFeedDestination"],
                Source = ConfigurationManager.AppSettings["DataFeedSource"],
            };

            var dataFeedServiceProvider = services.BuildServiceProvider();
            var dataFeedService = dataFeedServiceProvider.GetService<DataFeedProcess>();
            dataFeedService.Start(fileSource);
            Console.ReadKey(true);
        }
    }
}
