using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.ENTITY.Models.Auth
{
    public class User
    {
        //comcod,userid,username,userfname,isvalid=1
        public string comcod { get; set; }
        public string userid { get; set; }
        public string username { get; set; }
        public string userfname { get; set; }     
        public string userrole { get; set; }
        public string empid { get; set; }
        public string mailid { get; set; }
        public string userimg { get; set; }

    }

}
