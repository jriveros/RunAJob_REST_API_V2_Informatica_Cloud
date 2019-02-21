using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Xml;


namespace ICS.Framework.RESTful
{
    public class ICSJob : ICjob
    {
        // Class members: 
        // Property. 
        public string TaskID { get; set; }
        public string TaskName { get; set; }
        public string icSessionId { get; set; }
        public string TaskType { get; set; }

        public ICSJob(string iServerUrl, string iServerPort)
        {
            this.ICServerUrl = iServerUrl;
            this.ICServerPort = iServerPort;
            TaskName = "";
            TaskID = "";
            icSessionId = "";
            TaskType = "";
        }

        public string postData
        {
            get
            {
                job j = new job();
                j.taskId = this.TaskID;
                j.taskType = (JobType) Enum.Parse(typeof(JobType),this.TaskType, true);
                return j.Serialize();
            }
        }

        public bool Schedule(string iicSessionId, string iTaskId, string iTaskType)
        {
            this.icSessionId = iicSessionId;
            this.TaskID = iTaskId;
            this.TaskType = iTaskType;

            HttpPostRequest request = new HttpPostRequest(this.ICUri + this.ICHttpPath, this.postData);
            request.Headers.Add("icSessionId", this.icSessionId);
                        
            string contentOut;

            if (WebInvoker.Invoke(request, out contentOut))
            {
                if (contentOut == "")
                {
                    return true;
                }
                else
                {
                    error e = error.Deserialize(contentOut);
                    Console.WriteLine(e.description);
                    //Select the cd node with the matching title
                    return false;
                }
            }
            else { return false; }

        }


    }
}
