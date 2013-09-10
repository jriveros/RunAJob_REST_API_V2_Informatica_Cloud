using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ICS.Framework.RESTful
{
    class ICSactivityMonitor : ICactivityMonitor
    {
        public string id { get; set; }

        public ICSactivityMonitor(string iServerUrl, string iServerPort)
        {
            this.ICServerUrl = iServerUrl;
            this.ICServerPort = iServerPort;

        }

        public bool CheckActivityforTaskName(string iicSessionId, string iActivityName)
        {

            HttpGetRequest getRequest = new HttpGetRequest(this.ICUri + this.IChttppath, "");
            getRequest.QueryStringParams.Add("details", "false");
            getRequest.Headers.Add("icSessionId", iicSessionId);

            string contentOut;

            if (WebInvoker.Invoke(getRequest, out contentOut))
            {
                if (contentOut != "")
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(contentOut);
                    int i = 0;
                    // Load each node and check against the task name.
                    // when found exit? or Save Values.?
                    foreach (XmlNode node in doc.DocumentElement.ChildNodes)
                    {
                        i++;
                        activityMonitoryEntry a = activityMonitoryEntry.Deserialize(node.OuterXml);
                        if (a.taskName == iActivityName)
                        {
                            // Get the run ID 
                            this.id = a.id;
                            return true;

                        }
                    }
                }
            }
            return false;
        }


        
    }
}

