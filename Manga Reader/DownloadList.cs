using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manga_Reader
{
    public class DownloadList
    {
        public static int total;
        public static int downloaded;
        public static int chapters;
        public static int downloadedChapters;
        public static string currentchapter = "__";
        public static string getInfo()
        {
            var info = "Downloading : " + currentchapter + " | " + downloaded.ToString("000") + "/" + total.ToString("000") + " Chapters | " + downloadedChapters.ToString("00") + "/" + chapters.ToString("00") + " Pages";
            return info;
        }
    }
}
