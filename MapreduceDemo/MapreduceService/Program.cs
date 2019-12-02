﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace MapreduceService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            //ServiceBase[] ServicesToRun;
            //ServicesToRun = new ServiceBase[]
            //{
            //    new Service1()
            //};
            //ServiceBase.Run(ServicesToRun);

            var myService3 = new Service1();

            if (Environment.UserInteractive)
            {
                Console.WriteLine("Starting service...");
                //myService.Start();

                myService3.Start();
                Console.WriteLine("Service is running.");
                System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
                //Console.WriteLine("Press any key to stop...");
                //Console.ReadKey(true);
                //Console.WriteLine("Stopping service...");
                //myService.Stop();
                //Console.WriteLine("Service stopped.");
            }
            else
            {
                var servicesToRun = new ServiceBase[] {  myService3 };
                ServiceBase.Run(servicesToRun);
            }
        }
    }
}
