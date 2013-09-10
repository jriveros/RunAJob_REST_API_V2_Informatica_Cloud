using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml;
using System.IO;


namespace ICS.Framework.RESTful
{
    class MainV
    {
        enum ExitCode : int
        {
            //  Whether the task completed successfully. Returns one of the following codes:
            //- 1. The task completed successfully
            //- 2. The task completed with errors.
            //- 3. The task failed to complete.
            Success = 1,
            WithErrors = 2,
            Failed = 3,
            UnhandledError = 999
        }


        public static void WriteToConsole(string msg, bool line,bool doDebug, params object[] values ) 
        {
            if (doDebug) 
            {
                string lmsg = "";
                if (msg == "." || msg == " ") { lmsg = msg; } else { lmsg = DateTime.Now + " " + msg; }

                if (line)

                {
                    Console.WriteLine(lmsg, values);     
                } else {
                    Console.Write(lmsg,values);
                }
            }
        }

        static void  Main(string[] args)
        {   
          
            string iTaskId = args[0];
            string iTaskName = args[1];
            string iTaskType = args[2];
            bool idoActivityCheck = bool.Parse(args[3]);
            bool doDebug = false;
            bool loaded = false;
            
            InitialParameters vInitialLoad = new InitialParameters(out loaded);
            if (!loaded) {
                Console.WriteLine("Exiting, Could not parse the XML file, check InitialProperties.xml exist!, Exiting with code 999");
                Environment.Exit((int)ExitCode.UnhandledError);
            }
           
            doDebug=vInitialLoad.debug;
                     
            ICSLogin vLogin = new ICSLogin();
            vLogin.ICServerUrl = vInitialLoad.ICServerUrl;
            vLogin.ICServerPort = vInitialLoad.ICServerPort;
            vLogin.username = vInitialLoad.username;
            vLogin.password = vInitialLoad.password;

            //Login to ICS and get a connection ID
            int f = 0;
            while (!vLogin.LogIn())
            {
                if (f > 10) {
                    WriteToConsole("LogIn           => To Many retries, please check InitialProperties.xml values are ok. exiting!", true, true);
                    Environment.Exit((int)ExitCode.UnhandledError); 
                }
                f++;
                WriteToConsole("LogIn           => Could not log into Informatica Cloud Services...Retrying.",true,doDebug);
            }

            // Now the process has loged into ICS and icsessionId has been returned. Using this sessionId
            // for Log and do the task in ICS
            WriteToConsole("LogIn           => Loged into Informatica Cloud Services Using REST API 2.0, now scheduling the job",true,doDebug);

            ICSJob vJob = new ICSJob(vInitialLoad.ICServerUrl, vInitialLoad.ICServerPort);

            f= 0;
            while (!vJob.Schedule(vLogin.icSessionId, iTaskId, iTaskType))
            {

                if (f > 10) {
                    WriteToConsole("Job             => To Many retries, exiting!", true, true);
                    Environment.Exit((int)ExitCode.UnhandledError); 
                }
                f++;
                WriteToConsole("Job             => Retrying to schedule the task.",true,doDebug);

            }

            string doActivityCheck ="";
            if (idoActivityCheck) { doActivityCheck = ", Now checking Activity Monitor"; }
            WriteToConsole("Job             => Task Scheduled {0} {1}", true, doDebug, iTaskName, doActivityCheck);

            if (idoActivityCheck)
            {
                int i = 0; int j = 0;
                bool run = true;

                ICSactivityMonitor vActivityMonitor =
                    new ICSactivityMonitor(vInitialLoad.ICServerUrl, vInitialLoad.ICServerPort);

                // check if the task is running, until go out from ActivityMonitor!
                // In Some cases it take a while to show up in Activity Monitor.
                // For that reason this loop . Maybe could be better! improve this.

                while (run)
                {
                    if (vActivityMonitor.CheckActivityforTaskName(vLogin.icSessionId, iTaskName))
                    {
                        // In case has been found in the activityMonitor more than once.
                        if (i > 0) { WriteToConsole(".", false, doDebug); }
                        //In case has been found in activity Monitor, showing just one time, later just shows dots....
                        else { WriteToConsole("ActivityMonitor => {0} Still running ", false, doDebug, iTaskName); }
                        i++;
                    }
                    else
                    {
                        // if i>0 found at least once and now exit from while.
                        if (i > 0)
                        {
                            run = false;
                            WriteToConsole(".", true, doDebug);
                        }
                        else
                        {
                            // If i=0 means not found and time out, increment j for end while in case many time out 
                            // Maybe the task run pretty fast and could not found in ActivityMonitor.
                            //WriteToConsole(".",false,doDebug);
                            if (i == 0) { j++; }
                            if (j > 100)
                            {
                                // if j > retries, then go out and check in activityLog, to see if the task actually run. 
                                run = false;
                                WriteToConsole(".", true, doDebug);
                                WriteToConsole("ActivityMonitor => Something went wrong here, Skipping to ActivityLog to see if the task actually run", true, doDebug);
                            }

                        }

                    }
                }

                ICSActivityLog vActivityLog =
                    new ICSActivityLog(vInitialLoad.ICServerUrl, vInitialLoad.ICServerPort);

                // Now the task has been run, but we need to know if was Sucessful or Failed.
                // and also we get other values to show up.

                f = 0;
                while (!vActivityLog.CheckActivityforTaskName(vLogin.icSessionId, iTaskName))
                {
                    if (f > 50)
                    {
                        WriteToConsole("LogIn           => To Many retries, exiting!", true, true);
                        Environment.Exit((int)ExitCode.UnhandledError);
                    }
                    f++;
                    WriteToConsole("ActivityLog     => Time Out ? Retrying !!!!!", true, doDebug);
                }

                WriteToConsole("ActivityLog     => {0} success rows:{1}  failed rows:{2} start at:{3} end at:{4} took:{5} seconds", true, doDebug,
                        vActivityLog.stateString,
                        vActivityLog.successRows,
                        vActivityLog.failedRows,
                        vActivityLog.startTime.TimeOfDay,
                        vActivityLog.endTime.TimeOfDay,
                        (vActivityLog.endTime - vActivityLog.startTime).Seconds);


                //if (!string.IsNullOrWhiteSpace(vActivityLog.errorMsg))
                if (vActivityLog.state != 1)
                {
                    WriteToConsole("ActivityLog     => error message : {0} ", true, doDebug, vActivityLog.errorMsg);
                    WriteToConsole(" ", true, doDebug);
                    Environment.Exit(vActivityLog.state);
                }
                else {
                    WriteToConsole(" ", true, doDebug);
                    Environment.Exit(vActivityLog.state); 
                }
            }
            else {
                WriteToConsole(" ", true, doDebug);
                Environment.Exit((int)ExitCode.Success); 
            }
            
        }
    }
}

