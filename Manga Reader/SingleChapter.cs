using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Manga_Reader
{
    public class SingleChapter : ListViewItem
    {
        public string chapter = "";
        public string link = "";
        public string done = "no";
        private HtmlAgilityPack.HtmlNode node;

        private string id = "";
        public SingleChapter(HtmlAgilityPack.HtmlNode _node,string _id)
        {
            this.node = _node;
            for (int i = 0; i < 3; i++)
            {
                this.SubItems.Add(new ListViewSubItem());
            }

            chapter = node.InnerText;
            this.SubItems[0].Text = chapter;
            link = node.Attributes.First(o => o.Name == "href").Value;
            this.SubItems[1].Text = link;
            this.SubItems[2].Text = done;
            id = _id;

        }

        public void Download()
        {

           this.downloadImages();
            ////backgroundThread.Start();

        }

        public void downloadImages()
        {
            DownloadList.currentchapter = chapter.Replace(" ", "_").Replace("\n", "").Replace("\t", "");
            HtmlWeb web = new HtmlWeb();
            var htmlDoc1 = web.Load(link);
            var images = htmlDoc1.DocumentNode.SelectNodes("//*[contains(@class, 'wp-manga-chapter-img')]");
            var i = 1;
            Debug.WriteLine("Downloading for " + id + "at " + DateTime.Now);
            DownloadList.chapters = images.Count;
            DownloadList.downloadedChapters = 0;
            foreach (var item in images)
            {
                using(WebClient client = new WebClient())
                {
                    var p = id + @"\" + chapter.Replace(" ", "_").Replace("\n", "").Replace("\t", "");
                    if (!Directory.Exists(id))
                    {
                        Directory.CreateDirectory(id);
                    }
                    if (!Directory.Exists(p))
                    {
                        Directory.CreateDirectory(p);
                    }
                    client.DownloadFile(
                        new Uri(item.Attributes.First(o => o.Name == "data-src").Value),
                        p + @"\" + i.ToString("000") + ".jpeg");
                }
                //SaveImage(item.Attributes.First(o => o.Name == "data-src").Value,id+"\\"+chapter.Replace(" ","_").Replace("\n","").Replace("\t","")+"\\"+ i.ToString("000")+".jpg",ImageFormat.Jpeg);
                i += 1;
                DownloadList.downloadedChapters +=1;

            }
        }

        public void SaveImage(string  imageUrl,string filename, ImageFormat format)
        {
            WebClient client = new WebClient();
            Stream stream = client.OpenRead(imageUrl);
            Bitmap bitmap; bitmap = new Bitmap(stream);

            if (bitmap != null)
            {
                bitmap.Save(filename, format);
            }

            stream.Flush();
            stream.Close();
            client.Dispose();
        }

    }
}
