using Simple.ImageResizer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }
        private void button1_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
            Hide();
        }
        private Image cropImage(string image)
        {
            ImageResizer resizer = new ImageResizer(image);
            // resize to 400 px, jpg quality 90
            var byteArray1 = resizer.Resize(1024, ImageEncoding.Jpg100);
            // save last resized image to file
            resizer.SaveToFile(Application.StartupPath + "\\" + System.IO.Path.GetFileName(image));
            Image i = Image.FromFile(Application.StartupPath + "\\" + System.IO.Path.GetFileName(image));
            return i;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

        }

        private string source;
        private string target;

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
          
            foreach (string s in System.IO.Directory.GetFiles(source, "*.jpg", System.IO.SearchOption.TopDirectoryOnly))
            {
                //string s = "D:\\1-1000\\01 ALL-IS-WELL-2015.jpg";
                Bitmap bitmap = (Bitmap)(cropImage(s));//load the image file                      
                bitmap.SetResolution(20, 20);

                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    using (Font arialFont = new Font("Arial", 150))
                    {
                        PointF firstLocation = new PointF(bitmap.Width - 90, 25);
                        System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(Color.Red);
                        graphics.FillEllipse(myBrush, new Rectangle(bitmap.Width - 105, 5, 100, 100));
                        string[] words = System.IO.Path.GetFileName(s).Split(' ');
                        graphics.DrawString(words[0], arialFont, Brushes.Black, firstLocation);
                        myBrush.Dispose();
                    }
                }
                bitmap.Save(target + "\\" + System.IO.Path.GetFileName(s));
                bitmap.Dispose();
                string del = Application.StartupPath + "\\" + System.IO.Path.GetFileName(s);
                System.IO.File.Delete(del);
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Show();
            MessageBox.Show("OK");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = "Select source path";

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                source = folderBrowserDialog1.SelectedPath;
                label1.Text = source;
                folderBrowserDialog1.Description = "Select Export path";

                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    target = folderBrowserDialog1.SelectedPath;
                    label2.Text = target;
                }
            }
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

