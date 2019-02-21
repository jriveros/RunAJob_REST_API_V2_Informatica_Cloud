//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using System.Collections.Specialized;

namespace ICS.Framework.RESTful
{
    public interface IHttpFormParams
    {
        NameValueCollection FormParams { get; }
    }
}