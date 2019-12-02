using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace JobController
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<string> repos = new List<string>();

        private void btnBrowser_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();

            txtPath.Text = folderBrowserDialog1.SelectedPath;

            repos.AddRange(Directory.GetDirectories(folderBrowserDialog1.SelectedPath));

            listBox1.Items.AddRange(Directory.GetDirectories(folderBrowserDialog1.SelectedPath)
                            .Select(System.IO.Path.GetFileName)
                            .ToArray());
        }

        private void btnStart_Click(object sender, EventArgs e)
        {

            new Thread(() => {

                foreach(string x in repos)
                { 
                    var t = new Thread(() => Mapper(x));
                    t.Start();
                }

            }).Start();


        }

        DataTable result = new DataTable();
        private void Mapper(string path)
        {
            Thread.CurrentThread.IsBackground = true;
            var p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "LocMetrics.exe";
            p.StartInfo.Arguments = "-i \"" + path + "\" - e \"*.cs\" - o \"" + path + "\"";
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            
            DataTable dt = ConvertCSVtoDataTable(path + @"\LocMetricsFolders.csv");

            result = dt.Clone();

            result.Rows.Add(dt.Rows[0].ItemArray);

            dataGridView1.Invoke(new MethodInvoker(delegate ()
            {
                //listView1.Items.Add("Mapping done: " + path + " - Files: " + dt.Rows[0]["Files"].ToString() + " - Lines of Code: " + dt.Rows[0]["LOC"].ToString() + " - Code and Comment Lines of Code: " + dt.Rows[0]["C&SLOC"].ToString() + " - Comment Only Lines of Code: " + dt.Rows[0]["CLOC"].ToString() + " - Blank Lines of Code: " + dt.Rows[0]["BLOC"].ToString());

                //listView1.Items.Add("")
                ////listView1.MultiColumn = true;
                ////listView1.Items.Add(dt.Rows[0]);
                ///
                dataGridView1.Rows.Add(new object[] { path, dt.Rows[0]["Files"].ToString(), dt.Rows[0]["LOC"].ToString(), dt.Rows[0]["BLOC"].ToString(), dt.Rows[0]["C&SLOC"].ToString(), dt.Rows[0]["CLOC"].ToString() });
            }));


        }


        private void Reduce(string path)
        {
            Thread.CurrentThread.IsBackground = true;
            var p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "LocMetrics.exe";
            p.StartInfo.Arguments = path + @"\";
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            //listBox2.Invoke(new MethodInvoker(delegate ()
            //{
            //    listBox2.Items.Add("Mapping done: " + path);
            //}));
        }


        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }
                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    dt.Rows.Add(dr);
                }

                sr.Dispose();
                sr.Close();
            }


            return dt;
        }
    }
}
