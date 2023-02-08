using DataFeed.Common;
using DataFeed.Model;
using System;
using System.Diagnostics;
using System.IO;

namespace DataFeed.Service.FolderProcess
{
    public interface IMoveFile
    {
        bool CopyAndMove(FileSource fileSource);
    }

    public class MoveFile : IMoveFile
    {
        public bool CopyAndMove(FileSource fileSource)
        {
            Console.WriteLine($"Source: {fileSource.Source} , Destination: {fileSource.Destination}");
            var files = new DirectoryInfo(fileSource.Source).GetFiles(DataFeedValue.PROCESSFILETYPE);
            //Loop throught files and Copy to destination folder
            Console.WriteLine($"File count {files.Length} going to move!");
            int _fileProcessIndex = 0;
            foreach (FileInfo file in files)
            {
                if(!RandomCreation.IsFileLocked(file))
                {
                    string fileExt = "";
                    fileExt = Path.GetExtension(file.Name);
                    //Copy the file to destination folder after adding datetime
                    file.CopyTo(fileSource.Destination + file.Name.Replace(fileExt, string.Empty)
                        + "_" + RandomCreation.GetFileAccessTime() + fileExt);
                    _fileProcessIndex++;
                }                                 
            }
            Console.WriteLine($"File count {_fileProcessIndex} moved!");
            return true;
        }

        protected virtual bool IsFileLocked(FileInfo file)
        {
            try
            {
                using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                return true;
            }

            //file is not locked
            return false;
        }
    }
}
