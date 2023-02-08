using System.Collections.Generic;
using System.IO;

namespace DataFeed.Common
{
    public static class ReadFile
    {
        public static ICollection<string> GetAllFiles(string processPath) => Directory.GetFiles(processPath, DataFeedValue.PROCESSFILETYPE);
    }
}
