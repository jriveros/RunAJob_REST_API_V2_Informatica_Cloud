using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Xml;
//using System.IO;
//using System.Web;
//using System.Net;


namespace ICS.Framework.RESTful

{
    public class ICSLogin : IClogin
    {
        // Class members: 
        // Property. 
        public string username { get; set; }
        public string password { get; set; }
        public string icSessionId { get; set; }
        public DateTime updateTime { get; set; }
        

        public ICSLogin()
        {
            this.ICServerUrl = "";
            this.ICServerPort = "";
            username       = "";
            password       = "";
            icSessionId = "";
            updateTime = System.DateTime.Now.ToUniversalTime();
        }



        public string postData
        {
            get
            {
                // Using the login class defined from XSD to send user login information 
                
                login l = new login();
                l.username = this.username;
                l.password = this.password;
                return l.Serialize();

            }
        }


        public  bool LogIn()
        {

            string contentOut;
            HttpPostRequest request = new HttpPostRequest(this.ICUri + this.ICHttpPath, this.postData);
            if (WebInvoker.Invoke(request, out contentOut)) 
            {
                // Using the user class to get connection data, can be extended to get 
                // other values related to this user connection.

                user u = user.Deserialize(contentOut);
                this.icSessionId = u.icSessionId;
                this.updateTime = u.updateTime;
                return true;
            };

            return false;
            
        }
    }
}
