using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace FileService
{
    [MessageContract(IsWrapped = false)]
    public class ResponseBase
    {
        [MessageBodyMember(Order = 0)]
        public bool IsError { get; set; }
        [MessageBodyMember(Order = 1)]
        public string ErrorDesc { get; set; }
    }
}
