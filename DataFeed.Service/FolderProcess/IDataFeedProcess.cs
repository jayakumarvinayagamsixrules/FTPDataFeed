using DataFeed.Model;
using System;

namespace DataFeed.Service.FolderProcess
{
    public interface IDataFeedProcess
    {
        void Start(FileSource fileSource);
    }

    public class DataFeedProcess : IDataFeedProcess
    {
        private readonly IMoveFile _moveFile;

        public DataFeedProcess(IMoveFile moveFile)
        {
            _moveFile = moveFile;
        }
        public void Start(FileSource fileSource)
        {
            Console.WriteLine("Data feed started!");

            bool isFileCopied = _moveFile.CopyAndMove(fileSource);

            Console.WriteLine("Data feed ended!");
        }
    }
}
