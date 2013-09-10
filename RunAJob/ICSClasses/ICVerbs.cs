using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    /// <summary>
        //activityLog. 
        //    Returns job details from the Informatica Cloud activity log.
        //activityMonitor. 
        //    Returns job details from the Informatica Cloud activity monitor.
        //agent. 
        //    Returns the details of a Secure Agent or the details of all Secure Agents in the organization. Also deletes a Secure Agent.
        //connection. 
        //    Returns the details of a connection or the details of all connections in the organization. Returns
        //    available source or target objects for a specified connection. Returns all connections of a specified type associated
        //    with a Secure Agent. Creates or updates a connection. Also deletes a connection.
        //customFunc. 
        //    Returns the details of a plug-in or of all plug-ins in the organization. Creates or updates a plug-in.
        //    Also deletes a plug-in.
        //dnbWorkflow. 
        //    Returns the details of a D&B360 workflow or the details of all D&B360 workflows in the
        //    organization. Creates or updates a D&B360 workflow. Also deletes a D&B360 workflow. (Available for Informatica
        //    Cloud D&B360 partners only.)
        //field. 
        //    Returns the field details for a source or target object.
        //fileRecord. 
        //    Uploads an integration template XML file or image file. Also deletes an integration template XML file or
        //    image file.
        //job. 
        //    Starts a task or task flow.
        //login. 
        //    Logs in to an Informatica Cloud organization with Informatica Cloud or Salesforce credentials. Returns an
        //    Informatica Cloud REST API session ID that you can use for subsequent REST API requests.
        //masterTemplate. 
        //    Returns the details of an integration template or the details of all integration templates in the
        //    organization. Creates or updates an integration template. Also deletes an integration template.
        //mttask. 
        //    Returns the details of a custom integration task. Creates or updates a custom integration task. Also
        //    deletes a custom integration task.
        //org. 
        //    Returns details of an Informatica Cloud organization or related sub-organization. Updates an organization or
        //    related sub-organization. Also deletes a related sub-organization.
        //register. 
        //    Creates an Informatica Cloud organization or sub-organization using organization details. Also creates
        //    an organization using Salesforce credentials. (Available for Informatica Cloud partners only.)
        //salesforceVersion. 
        //    Returns the Salesforce version used by Informatica Cloud.
        //schedule. 
        //    Returns the details of a schedule or the details of all schedules in the organization. Creates or updates a
        //    schedule. Also deletes a schedule.
        //serverTime. 
        //    Returns the local time of the Informatica Cloud server.
        //user. 
        //    Returns the details of a user account or the details of all user accounts in the organization. Creates or
        //    updates a user account. Also deletes a user account.
        //workflow. 
        //    Returns the details of a task flow or the details of all task flows in the organization. Creates or updates a
        //task flow. Also deletes a task flow.
    /// </summary>    

namespace ICS.Framework.RESTful
{
    public abstract class ICBase {

        public string ICServerUrl { get; set; }
        public string ICServerPort { get; set; }
        
        public string ICUri
        {
            get
            {
                string value = this.ICServerUrl;
                value = value + this.ICServerPort;
                return value;
            }
        }

    
    }
    public class IClogin : ICBase
    {
 
        public string ICHttpPath        
        {
            get
            {
                return "/ma/api/v2/user/login";
            }
        }

        public string IChttpVerb
        {
            get
            {
                return "POST";
            }
        }

    }

    public class ICjob : ICBase
    {
        public string ICHttpPath
        {
            get
            {
                return "/saas/api/v2/job/";
            }
        }

        public string IChttpVerb() 
        {
            return "POST";
        }
    }

    public class ICactivityLog : ICBase
    {
        public string IChttppath
        { 
            get 
            {
                return "/saas/api/v2/activity/activityLog";
            }
        }

        public string IChttpVerb()
        {
            return "GET";
        }
    }

    public class ICactivityMonitor : ICBase
    {
        public string IChttppath
        {
            get
            {
                return "/saas/api/v2/activity/activityMonitor";
            }
        }

        public string IChttpVerb()
        {
            return "GET";
        }
    }


    //To be implemented indexer the future..... :D
        //public string agent()               { return ""; }
        //public string connection()          { return ""; }
        //public string customFunc()          { return ""; }
        //public string dnbWorkflow()         { return ""; }
        //public string field()               { return ""; }
        //public string fileRecord()          { return ""; }
        //public string masterTemplate()      { return ""; }
        //public string mttask()              { return ""; }
        //public string org()                 { return ""; }
        //public string register()            { return ""; }
        //public string salesforceVersion()   { return ""; }
        //public string schedule()            { return ""; }
        //public string serverTime()          { return ""; }
        //public string user()                { return ""; }
        //public string workflow()            { return ""; }

    }

