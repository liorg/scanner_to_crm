using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace testdotnettwain.Mechanism.DataModel
{
    public class SendObjDetail
    {
        public string Path { get; set; }
        public Guid  ObjId { get; set; }
        public string ObjType { get; set; }
        //public string Header { get; set; }
    }
}
