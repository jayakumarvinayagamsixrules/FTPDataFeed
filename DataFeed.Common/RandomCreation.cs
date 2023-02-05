using System;
using System.IO;

namespace DataFeed.Common
{
    public static class RandomCreation
    {
        public static string GetFileAccessTime()
        {            
            Random generator = new Random();
            String feedRandom = generator.Next(0, 1000000).ToString(DataFeedValue.RANDOMRANGE);
            return DateTime.Now.ToString(DataFeedValue.DATETIMEFORMATE) + "_" + feedRandom;
        }

        public static bool IsFileLocked(FileInfo file)
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
