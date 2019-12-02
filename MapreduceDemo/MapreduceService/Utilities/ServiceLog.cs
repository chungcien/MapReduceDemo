using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WS_FingerPrinter
{
    public static class ServiceLog
    {
        public static int SaveLog_Result(string serviceName, string RunTime, string State, string Logfile, DataProvider dataProvider)
        {
            try
            {
                return dataProvider.ExecuteNonQuery($"  INSERT dbo.ServiceLog (ServiceName,RunTime,Status,Logfile ) VALUES (N'{serviceName}','{RunTime}',N'{State}',N'{Logfile}');");
            }
            catch
            {
                return -1;
            }
        }
    }
}
