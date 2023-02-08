using DataFeed.Common;
using DataFeed.Model;
using DataFeed.Persistence;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace DataFeed.Service.FolderProcess
{
    public interface IDataFeedProcess
    {
        void Start(FileSource fileSource);
    }

    public class DataFeedProcess : IDataFeedProcess
    {
        private readonly IMoveFile _moveFile;
        private readonly IUserRepository _userRepository;
        private readonly IMeteringRepository _meteringRepository;
        public DataFeedProcess(IMoveFile moveFile, IUserRepository userRepository, IMeteringRepository meteringRepository)
        {
            _moveFile = moveFile;
            _userRepository = userRepository;
            _meteringRepository = meteringRepository;
        }
        public void Start(FileSource fileSource)
        {
            Console.WriteLine("Data feed started!");

            bool isFileCopied = _moveFile.CopyAndMove(fileSource);

            // access files from Client_A and Client_B
            var processFiles = ReadFile.GetAllFiles(fileSource.ProcessClientA);
            foreach (var filePath in processFiles)
            {
                Console.WriteLine($"Path: {filePath}");
                var csv = new List<string[]>();
                var lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                    csv.Add(line.Split(';'));

                var properties = lines[0].Split(';');

                var listObjResult = new List<Dictionary<string, string>>();

                for (int i = 1; i < lines.Length; i++)
                {
                    var objResult = new Dictionary<string, string>();
                    for (int j = 0; j < properties.Length; j++)
                        objResult.Add(properties[j], csv[i][j]);
                    listObjResult.Add(objResult);

                }

                var feedJson = JsonConvert.SerializeObject(listObjResult);
                var meteringJsons = JsonConvert.DeserializeObject<List<MeteringJson>>(feedJson);
                _meteringRepository.AddManyAsync(meteringJsons);
                //_userRepository.GetCategories();

                //var ttt = _userRepository.GetCategories().Result;

                //var users = new List<User>() { 
                //    new User{ Age = 11, Blog = "fsdfsadf", Name = "Jaya"},
                //    new User{ Age = 15, Blog = "=======", Name = "Kumar"}
                //};
                //_userRepository.AddManyAsync(users);
            }
            Console.WriteLine("Data feed ended!");
        }
    }
}
