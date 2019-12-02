using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WS_FingerPrinter
{
    public static class ServiceConfig
    {
        public static string LoadConfig(string serviceName,string key, DataProvider dataProvider)
        {
            try
            {
                return dataProvider.ExecuteScalar_string($"  SELECT TOP 1 Value from ServiceConfig WHERE [Name] = N'{key}' AND ServiceName = N'{serviceName}';");
            }
            catch
            {
                return null;
            }
        }

        public static string LoadConfig(string serviceName, string key, DataProvider dataProvider, bool CreateIfNotFound, string defaultValue)
        {
            try
            {
                return dataProvider.ExecuteScalar_string($"  SELECT TOP 1 Value from ServiceConfig WHERE [Name] = N'{key}' AND ServiceName = N'{serviceName}';");
            }
            catch
            {
                if (CreateIfNotFound)
                {
                    if (Create(serviceName, key, defaultValue, "", dataProvider) > 0)
                    {
                        return defaultValue;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return defaultValue;
                }
            }
        }

        public static int SaveConfig(string serviceName ,string key, string value, DataProvider dataProvider)
        {
            try
            {
                return dataProvider.ExecuteNonQuery($"   UPDATE ServiceConfig SET Value = N'{value}' WHERE Name = N'{key}' AND ServiceName = N'{serviceName}';");
            }
            catch
            {
                return -1;
            }
        }

        public static int Create(string serviceName, string key, string value, string description ,DataProvider dataProvider)
        {
            try
            {
                return dataProvider.ExecuteNonQuery($" INSERT dbo.ServiceConfig(ServiceName,Name,Value,Description)VALUES(N'{serviceName}',N'{key}',N'{value}',N'{description}');");
            }
            catch
            {
                return -1;
            }
        }
    }
}
