using DataFeed.Common;
using DataFeed.Model;
using DataFeed.Persistence;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;

namespace DataFeed.Service.FolderProcess
{
    public interface IDataFeedProcess
    {
        void Start(FileSource fileSource);
    }

    public class DataFeedProcess : IDataFeedProcess
    {
        private readonly IMoveFile _moveFile;
        private readonly IECCHRepository _repository;
        public DataFeedProcess(IMoveFile moveFile)
        {
            _moveFile = moveFile;
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
                //_repository.AddMany(meteringJsons);
            }
            Console.WriteLine("Data feed ended!");
        }
    }
}
