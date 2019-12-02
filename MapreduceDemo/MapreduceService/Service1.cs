using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using WS_FingerPrinter;


namespace MapreduceService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();

            timer = new System.Timers.Timer();
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.timer_Tick);
            timer.Interval = 60000;

        }

        public void Start()
        {
            OnStart(new string[0]);

            Running(true, DateTime.Now.ToString("HH-mm"));
        }

        private void timer_Tick(object sender, EventArgs e)
        {


            if (ServiceConfig.LoadConfig(ServiceName, "RunNow", dataProvider) == "false")
            {
                string date_time = DateTime.Now.ToString("HH:mm");

                for (int i = 0; i < List_ScanAt.Count; i++)
                {
                    if (date_time == List_ScanAt[i])
                    {
                        Running(true, DateTime.Now.ToString("HH-mm"));
                    }
                }
            }
            else if (ServiceConfig.LoadConfig(ServiceName, "RunNow", dataProvider) == "true")
            {
                Running(true, DateTime.Now.ToString("HH-mm"));

                ServiceConfig.SaveConfig(ServiceName, "RunNow", "false", dataProvider);
            }
            else
            {
                ServiceConfig.Create(ServiceName, "RunNow", "false", "Khi giá trị là true thì service sẽ thực thi ngay nhiệm vụ trong 1 phút sắp tới", dataProvider);
            }

        }

        System.Timers.Timer timer = null;

        string logpath = "";

        List<string> List_ScanAt = new List<string>();

        static Info_Instances instanse = new Info_Instances();

        DataProvider dataProvider = new DataProvider();

        List<string> admin = new List<string>();

        public static List<string> mail_content = new List<string>();

        int isWarnning = 0;

        private string MailToAdmin;

        Model.MapreduceDemoEntities db = new Model.MapreduceDemoEntities();


        protected override void OnStart(string[] args)
        {
            try
            {

                Initialize(DateTime.Now.ToString("HH-mm"));

                File_Read_Write.Write_File(logpath, "-----------------------------------------------------", true, ref mail_content);
                File_Read_Write.Write_File(logpath, DateTime.Now.ToString() + ": Service Starting...", true, ref mail_content);

                // load email admin để gửi thông báo
                try
                {
                    admin.AddRange(ServiceConfig.LoadConfig(ServiceName, "Email_Admin", dataProvider).Split(',').ToArray());
                }
                catch
                {
                    isWarnning = 1;
                    throw new Exception("Load Config Email_Admin failed!");
                }


                MailToAdmin = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "EmailToAdmin.htm");

                List_ScanAt.AddRange(ServiceConfig.LoadConfig(ServiceName, "ListScanAt", dataProvider).Split(',').ToList());

                File_Read_Write.Write_File(logpath, DateTime.Now.ToString() + ": Load Config...Done! - Scan at (No SendLogs): " + string.Join(", ", List_ScanAt.ToArray()), true, ref mail_content);

                timer.Start();

            }
            catch (Exception ex)
            {
                ServiceLog.SaveLog_Result(ServiceName, DateTime.Now.ToString("yyyy/MM/dd HH-mm"), "Error", logpath, dataProvider);

                File_Read_Write.Write_File(logpath, DateTime.Now + ": Error - Load Config Failed! " + ex.Message, true, ref mail_content);
            }

        }

        void Initialize(string time)
        {
            logpath = AppDomain.CurrentDomain.BaseDirectory + @"Log\" + ServiceName + @"\" + DateTime.Now.ToString("yyyy-MM-dd_") + time;

            //new Git().clone();

            isWarnning = 0;

            // khởi tạo các thiết lập từ file setting

            instanse.Server_Name = ConfigurationManager.AppSettings["SQL_Server_Name"].ToString();
            instanse.DBName = ConfigurationManager.AppSettings["SQL_DBName"].ToString();
            instanse.User = ConfigurationManager.AppSettings["SQL_User"].ToString();
            instanse.Pass = ConfigurationManager.AppSettings["SQL_Pass"].ToString();

            if (dataProvider.cnn.State == ConnectionState.Open)
            {
                dataProvider.cnn.Close();
            }
            dataProvider.cnn.ConnectionString = (@"Data Source = " + instanse.Server_Name + "; Initial Catalog = " + instanse.DBName + "; Persist Security Info = True; User ID = " + instanse.User + "; Password = " + instanse.Pass + "; Connection Timeout = 20");

        }

        void Running(bool isSendLog, string time)
        {
            //timer.Stop();
            try
            {
                Initialize(time);

                List<Model.Repository> repositories = db.Repositories.ToList();



                
            }
            catch (Exception op)
            {
                isWarnning = 2;

                File_Read_Write.Write_File(logpath, DateTime.Now + ": Error - " + op.Message, true, ref mail_content);
            }

            // gưi log cho admin
            if (isSendLog == true)
            {
                SendEmail.Send_Email("FPM Sync Check InOut Service", admin, null, "FPM Verify Data Log", MailToAdmin.Replace("#content#", "</br>" + string.Join("</br>", mail_content) + "</br>"), true, logpath, ref mail_content);
            }

            if (isWarnning == 0)
            {
                ServiceLog.SaveLog_Result(ServiceName, DateTime.Now.ToString("yyyy/MM/dd ") + time, "Success", logpath, dataProvider);
            }
            else if (isWarnning == 1)
            {
                ServiceLog.SaveLog_Result(ServiceName, DateTime.Now.ToString("yyyy/MM/dd ") + time, "Warnning", logpath, dataProvider);
            }
            else
            {
                ServiceLog.SaveLog_Result(ServiceName, DateTime.Now.ToString("yyyy/MM/dd ") + time, "Error", logpath, dataProvider);
            }


            mail_content.Clear();

            timer.Start();
        }



        protected override void OnStop()
        {
        }
    }
}
