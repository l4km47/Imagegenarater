using Simple.ImageResizer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;
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
            try
            {
                backgroundWorker1.RunWorkerAsync(progressBar1);
                button1.Enabled = false;
                button2.Enabled = false;
                progressBar1.Maximum = System.IO.Directory.GetFiles(source, "*.jpg").Length;
                progressBar1.Value = 0;
                button3.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
            try
            {
                foreach (string s in System.IO.Directory.GetFiles(source, "*.jpg"))
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
                    Invoke(new Action(() => label6.Text = "Current file > " + System.IO.Path.GetFileName(s)));
                    bitmap.Dispose();
                    string del = Application.StartupPath + "\\" + System.IO.Path.GetFileName(s);
                    System.IO.File.Delete(del);
                    var p = (ProgressBar)e.Argument;
                    Invoke(new Action(() => p.Increment(1)));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Invoke(new Action(() => button3.Enabled = false));
            }

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                MessageBox.Show("OK", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                button1.Enabled = true;
                button2.Enabled = true;
                progressBar1.Value = 0;

                System.IO.StreamWriter st = new System.IO.StreamWriter(target + "\\names.txt", false, Encoding.UTF8);

                foreach (string s in System.IO.Directory.GetFiles(source, "*.jpg", System.IO.SearchOption.TopDirectoryOnly))
                {
                    st.WriteLine(System.IO.Path.GetFileName(s));
                }
                st.Flush();
                st.Dispose();
                label6.Text = "";
                button3.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                folderBrowserDialog1.Description = "Select source path";

                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    source = folderBrowserDialog1.SelectedPath;
                    label1.Text = source;

                    label5.Text = String.Format("Images in Source folder {0}", System.IO.Directory.GetFiles(source, "*.jpg").Length.ToString());

                    folderBrowserDialog1.Description = "Select Export path";

                    if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                    {
                        target = folderBrowserDialog1.SelectedPath;
                        label2.Text = target;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }
    }
}

