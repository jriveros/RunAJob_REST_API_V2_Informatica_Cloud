//using System;
//using System.Collections.Generic;
//using System.Text;
using System.Xml;

namespace ICS.Framework.RESTful
{
    class InitialParameters : ICBase
    {
        public string username { get; set; }
        public string password { get; set; }
        public bool debug { get; set; }

        public InitialParameters(out bool loaded) 
        {

            loaded = false;
            try
            {
                XmlDocument InProp = new XmlDocument();
                InProp.Load("InitialProperties.xml");

                this.username = InProp.DocumentElement.SelectSingleNode("/root/username").InnerXml;
                this.password = InProp.DocumentElement.SelectSingleNode("/root/password").InnerXml;
                this.ICServerUrl = InProp.DocumentElement.SelectSingleNode("/root/baseurl").InnerXml;
                this.ICServerPort = InProp.DocumentElement.SelectSingleNode("/root/port").InnerXml;
                this.debug = bool.Parse(InProp.DocumentElement.SelectSingleNode("/root/doDebug").InnerXml);
                loaded = true;
            }
            catch  { 
            
               
            
            }
            
        }


    }
}
