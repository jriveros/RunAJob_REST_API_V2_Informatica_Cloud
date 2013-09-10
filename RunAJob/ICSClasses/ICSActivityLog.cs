using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ICS.Framework.RESTful
{
    class ICSActivityLog : ICactivityLog
    {
        public string errorMsg { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public int state { get; set; }
        public string stateString { get; set; }
        public long successRows { get; set; }
        public long failedRows { get; set; }


        public ICSActivityLog(string iServerUrl, string iServerPort)
        {
            this.ICServerUrl = iServerUrl;
            this.ICServerPort = iServerPort;

        }

        public bool CheckActivityforTaskName(string iicSessionId, string iActivityName) 
        {

            HttpGetRequest getRequest = new HttpGetRequest(this.ICUri + this.IChttppath, "");
            getRequest.QueryStringParams.Add("rowLimit", "50");
            getRequest.Headers.Add("icSessionId", iicSessionId);
            
            string contentOut;

            if (WebInvoker.Invoke(getRequest, out contentOut))
            {
                if (contentOut == "")
                {
                    return false;
                }
                else
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(contentOut);
                    int i = 0;
                    // Load each node and check against the task name.
                    // when found exit? or Save Values.?
                    foreach (XmlNode node in doc.DocumentElement.ChildNodes)
                    {
                        i++;
                        activityLogEntry a = activityLogEntry.Deserialize(node.OuterXml);
                        if (a.objectName == iActivityName)
                        { 
                            // Do something 
                            this.errorMsg = a.errorMsg;
                            this.endTime= (DateTime)a.endTime;
                            this.startTime=(DateTime)a.startTime;
                            this.state=a.state;
                            this.successRows = a.successTargetRows;
                            this.failedRows = a.failedTargetRows;
                           

                            //  Whether the task completed successfully. Returns one of the following codes:
                            //- 1. The task completed successfully
                            //- 2. The task completed with errors.
                            //- 3. The task failed to complete.

                            switch (this.state)
                            {
                                case 1:
                                    this.stateString = "Success";
                                    break;
                                case 2:
                                    this.stateString = "With Errors";
                                    break;
                                case 3:
                                    this.stateString = "Failed";
                                    break;
                                default:
                                    this.stateString = "No translated " + this.state;
                                    break;
                            }

                            return true;
                            
                        }
                    }

                    
                    return true;
                }
            }
            else { return false; }



        //return false;
        }



    }
}
