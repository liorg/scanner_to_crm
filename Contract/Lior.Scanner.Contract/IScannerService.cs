using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Lior.Scanner.Contract
{
   public  interface IScannerService
    {
        void Upload(byte[] data, string field1, string field2, string field3);
    }
}
