using System;
using System.Collections.Generic;
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


        public void UploadFile(RemoteFileInfo request)
        {
            // report start
            Console.WriteLine("Start uploading " + request.FileName);
            Console.WriteLine("Size " + request.Length);
            var filePathSving = System.Configuration.ConfigurationManager.AppSettings["FilePathSving"];
            filePathSving += @"\Upload";
            // create output folder, if does not exist       
            if (!System.IO.Directory.Exists(filePathSving)) System.IO.Directory.CreateDirectory(filePathSving);
          
            // kill target file, if already exists
            string filePath = System.IO.Path.Combine(filePathSving, request.FileName);
            if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);

            int chunkSize = 2048;
            byte[] buffer = new byte[chunkSize];

            using (System.IO.FileStream writeStream = new System.IO.FileStream(filePath, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write))
            {
                do
                {
                    // read bytes from input stream
                    int bytesRead = request.FileByteStream.Read(buffer, 0, chunkSize);
                    if (bytesRead == 0) break;

                    // write bytes to output stream
                    writeStream.Write(buffer, 0, bytesRead);
                } while (true);

                // report end
                Console.WriteLine("Done!");

                writeStream.Close();
            }
        }

       
    }
}
