using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WS_FingerPrinter;

namespace MapreduceService
{
    class Git
    {
        public void clone()
        {
            var co = new CloneOptions();
            co.CredentialsProvider = (_url, _user, _cred) => new UsernamePasswordCredentials { Username = "47d8127730c93d264327891ea15a42221d29b3b9", Password = string.Empty};

            string folderName = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
            File_Read_Write.Create_Exits_Folder(folderName);
            Repository.Clone("https://github.com/chungcien/MapReduceDemo.git", AppDomain.CurrentDomain.BaseDirectory + @"repo\" + folderName, co);
        }
    }
}
