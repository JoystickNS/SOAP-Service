using Lab2;
using System.IO;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IFileTransferService client = new FileTransferService();

            string path = "D:\\Test\\files\\test.txt";

            await UploadFile(client, path);

            string downloadFolder = "D:\\Test\\download";

            string[] fileNames = client.GetFileNames();
            foreach(string fileName in fileNames)
            {
                await DownloadFile(client, downloadFolder, fileName);
            }
        }

        public static async Task UploadFile(IFileTransferService client, string path)
        {
            await Task.Run(() =>
            {
                FileInfo fileinfo = new FileInfo(path);

                RemoteFileInfo uploadRequestInfo = new RemoteFileInfo();

                using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    uploadRequestInfo.FileName = path;
                    uploadRequestInfo.Length = fileinfo.Length;
                    uploadRequestInfo.FileByteStream = stream;
                    client.UploadFile(uploadRequestInfo);
                }
            });

        }

        public static async Task DownloadFile(IFileTransferService client, string downloadFolder, string fileName)
        {
            await Task.Run(() =>
            {
                DownloadRequest downloadRequest = new DownloadRequest();
                downloadRequest.FileName = fileName;

                RemoteFileInfo downloadRequestInfo = client.DownloadFile(downloadRequest);

                string filePath = Path.Combine(downloadFolder, Path.GetFileName(fileName));

                using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    FileStream sourceStream = downloadRequestInfo.FileByteStream;
                    const int bufferLen = 65000;
                    byte[] buffer = new byte[bufferLen];
                    int count = 0;
                    while ((count = sourceStream.Read(buffer, 0, bufferLen)) > 0)
                    {
                        stream.Write(buffer, 0, count);
                    }
                }
            });
        }
    }
}
