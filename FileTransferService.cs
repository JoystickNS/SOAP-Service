using System;
using System.IO;

namespace Lab2
{
    public class FileTransferService : IFileTransferService
    {
        private string uploadFolder = "D:\\Test\\upload";
        public string UploadFolder
        {
            get
            {
                return uploadFolder;
            }
            set
            {
                uploadFolder = value;
            }
        }
        public void UploadFile(RemoteFileInfo request)
        {
            Stream sourceStream = request.FileByteStream;

            string filePath = Path.Combine(uploadFolder, Path.GetFileName(request.FileName));

            FileStream targetStream;
            using (targetStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                const int bufferLen = 65000;
                byte[] buffer = new byte[bufferLen];
                int count = 0;

                while ((count = sourceStream.Read(buffer, 0, bufferLen)) > 0)
                {
                    targetStream.Write(buffer, 0, count);
                }

                targetStream.Close();
                sourceStream.Close();
            }
        }

        public RemoteFileInfo DownloadFile(DownloadRequest request)
        {
            RemoteFileInfo result = new RemoteFileInfo();

            try
            {
                string filePath = Path.Combine(UploadFolder, request.FileName);
                FileInfo fileInfo = new FileInfo(filePath);

                if (!fileInfo.Exists)
                {
                    throw new FileNotFoundException("File not found", request.FileName);
                }

                FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                result.FileName = request.FileName;
                result.Length = fileInfo.Length;
                result.FileByteStream = stream;
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
            return result;
        }

        public string[] GetFileNames()
        {
            return Directory.GetFiles(UploadFolder); ;
        }
    }
}
