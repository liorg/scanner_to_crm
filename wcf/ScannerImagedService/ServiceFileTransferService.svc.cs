using Lior.Scanner.Contract;
using Lior.Scanner.Mock;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace FileService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class ServiceFileTransferService : IFileTransferService
    {


        public ResponseBase UploadFile(RemoteFileInfo request)
        {
            ResponseBase res = new ResponseBase();
            try
            {
               
                 int chunkSize = 2048;
                 byte[] content = null; string filePath = "";
                System.IO.MemoryStream writeStreamMemo = new System.IO.MemoryStream();
                byte[] buffer = new byte[chunkSize];
                
                    // report start
                    Console.WriteLine("Start uploading " + request.FileName);
                    Console.WriteLine("Size " + request.Length);
                    var filePathSving = System.Configuration.ConfigurationManager.AppSettings["FilePathSving"];

                    if (!String.IsNullOrWhiteSpace(filePathSving))
                    {
                        filePathSving += @"\Upload";
                        // create output folder, if does not exist       
                        if (!System.IO.Directory.Exists(filePathSving)) System.IO.Directory.CreateDirectory(filePathSving);

                        // kill target file, if already exists
                        filePath = System.IO.Path.Combine(filePathSving, request.FileName + "_" + request.Field2);

                        if (System.IO.File.Exists(filePath))
                            System.IO.File.Delete(filePath);
                    }
                    using (MemoryStream writeStream = new MemoryStream())
                    {
                        do
                        {
                            // read bytes from input stream
                            int bytesRead = request.FileByteStream.Read(buffer, 0, chunkSize);
                            if (bytesRead == 0) break;

                            // write bytes to output stream
                            writeStream.Write(buffer, 0, bytesRead);
                        } 
                        while (true);

                        if (!String.IsNullOrWhiteSpace(filePath))
                            System.IO.File.WriteAllBytes(filePath + "1.tiff", writeStream.ToArray());
                        
                        content = writeStream.ToArray();
                        writeStream.Close();
                    }

                   
                IScannerService scannerService = new MockScannerService();
                scannerService.Upload(content, request.Field1, request.Field2, request.Field3);
            }
            catch (Exception e)
            {

                res.IsError = true;
                res.ErrorDesc = e.ToString();
            }

            return res;

        }


    }
}
