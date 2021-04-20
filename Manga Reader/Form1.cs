using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace Manga_Reader
{
    public partial class Form1 : Form
    {
        string id = "";
        string name = "";
        string image = "";
        private readonly HttpClient client = new HttpClient();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {



        }

        private void button_download_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {

                selitems = listView1.SelectedItems.Cast<SingleChapter>().ToList();
                DownloadList.total = selitems.Count;
                DownloadList.downloaded = 0;
                timer1.Enabled = true;
                listView1.Enabled = false;
                button_download.Enabled = false;
                button_start.Enabled = false;
                backgroundWorker1.RunWorkerAsync();
            }
        }


        private async void button_start_Click(object sender, EventArgs e)
        {
            try
            {

                var html = textBox_url.Text;

                HtmlWeb web = new HtmlWeb();
                var htmlDoc1 = web.Load(html);
                var node = htmlDoc1.DocumentNode.SelectSingleNode("//*[contains(@class, 'wp-manga-action-button')]");
                var node1 = htmlDoc1.DocumentNode.SelectSingleNode("//*[contains(@class, 'summary_image')]/a/img");
                var node2 = htmlDoc1.DocumentNode.SelectSingleNode("//*[contains(@class, 'sContent')]");
                name = htmlDoc1.DocumentNode.SelectSingleNode("//*[contains(@class, 'post-title')]").InnerText;
                label1.Text = name;
                id = node.Attributes.First(o => o.Name == "data-post").Value;
                image = node1.Attributes.First(o => o.Name == "data-src").Value;
                if (node2 != null)
                {
                    textBox1.Text = node2.InnerText;
                }
                pictureBox1.ImageLocation = image;

                var values = new Dictionary<string, string>
            {
                { "action", "manga_get_chapters" },
                { "manga", id }
            };

                var content = new FormUrlEncodedContent(values);
                var response = await client.PostAsync("https://mangafoxfull.com/wp-admin/admin-ajax.php", content);
                var responseString = await response.Content.ReadAsStringAsync();



                var htmlDoc = new HtmlAgilityPack.HtmlDocument();
                htmlDoc.LoadHtml(responseString);
                var nodes = htmlDoc.DocumentNode.SelectNodes(@"//*[contains(@class, 'wp-manga-chapter')]/a");
                foreach (var _node in nodes)
                {
                    listView1.Items.Add(new SingleChapter(_node, name.Trim().Replace(" ","_")));
                }
            }
            catch (Exception)
            {

            }
        }

        List<SingleChapter> selitems;
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

            foreach (var item in selitems)
            {
                (item as SingleChapter).Download();
                DownloadList.downloaded +=1;

            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            listView1.Enabled = true;
            button_download.Enabled = true;
            button_start.Enabled = true;
            timer1.Enabled = false;
        }

        private void button_open_Click(object sender, EventArgs e)
        {
            new Form2().Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label_info.Text = DownloadList.getInfo();
        }
    }
}
