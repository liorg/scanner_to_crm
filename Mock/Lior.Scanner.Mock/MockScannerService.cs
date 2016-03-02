using Lior.Scanner.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lior.Scanner.Mock
{
    public class MockScannerService : IScannerService
    {
        public MockScannerService()
        {

        }
        public void Upload(byte[] data, string field1, string field2, string field3)
        {
            var file = field1 + "_" + field2 + "_" + field3 + "_" + DateTime.Now.ToString("yyyyMMddHHmmss")+".tif";
            var temp = @"C:\\temp\scanner\mock";
            var path = Path.Combine(temp, file);
            File.WriteAllBytes(path, data);
        }

        
    }
}
