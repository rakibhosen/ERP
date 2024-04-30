using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.ENTITY.Models.Menu
{
    public class ModuleInfoDTO
    {

            public int ModuleId { get; set; }
        public int mid { get; set; }
        public string area { get; set; }
            public string name { get; set; }
            public string controller { get; set; }
            public string action { get; set; }
            public string modicon { get; set; }
        public string icon { get; set; }
        public string submodname { get; set; }
        public string qtype { get; set; }

    }

    public class MenuInfoDTO
    {

        public string comcod { get; set; }

        public int MenuId { get; set; }
        public string userid { get; set; }

        public int ModuleId { get; set; }
        public string area { get; set; }
        public string name { get; set; }
        public string controller { get; set; }
        public string action { get; set; }
        public string pagetype { get; set; }
        public string icon { get; set; }
        public bool permission { get; set; }



    }

}
