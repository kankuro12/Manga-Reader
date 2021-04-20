using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Manga_Reader
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            foreach(var dir in Directory.GetDirectories(Application.StartupPath))
            {
                var vv = dir.LastIndexOf("\\")+1;
                var sub = dir.Substring(vv);
                listBox1.Items.Add(sub);
            }
        }

        string current = "";
       
        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count > 0)
            {
                current = listBox1.SelectedItem as string;
                foreach (var dir in Directory.GetDirectories(current).OrderBy(o => {
                    return Convert.ToInt32( o.Substring(o.LastIndexOf("_")+1));
                }).ToList())
                {
                    var vv = dir.LastIndexOf("\\") + 1;
                    var sub = dir.Substring(vv);
                    listBox2.Items.Add(sub);
                }
            }
        }

        private string GetBase64(string f)
        {
            string ret = "";
            {
                var s = new StreamReader(f);

                using (BinaryReader br = new BinaryReader(s.BaseStream))
                {
                    byte[] data = br.ReadBytes((int)s.BaseStream.Length);
                    ret = System.Convert.ToBase64String(data);
                }
            }
            return ret;
        }
        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            //panel1.Controls.Clear();
            if (listBox2.SelectedItems.Count > 0)
            {
                var path = current +@"\"+listBox2.SelectedItem as string;
                var html = "<html><body>";
                foreach (var dir in Directory.GetFiles(path))
                {
                    //Image b = Image.FromFile(dir);

                    //PictureBox p = new PictureBox();
                    //p.SizeMode = PictureBoxSizeMode.StretchImage;
                    //p.Dock = DockStyle.Top;
                    //p.Image = b;
                    //panel1.Controls.Add(p);
                    //p.Show();
                    //p.Height = p.Width/ b.Width   * b.Height;
                    Debug.WriteLine(dir);
                    String b64 = GetBase64(dir);
                    html += "<div   style='width:100%;text-align:center;'><img  style='max-width:100%'  src='data:image/jpeg;base64," + b64  + "' /></div>";
                }
                html += "</body></html>";
                webBrowser1.DocumentText = html;
            }
        }
    }
}
