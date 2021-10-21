using System.ServiceModel;

namespace Lab2
{ 
    [ServiceContract]
    public interface IFileTransferService
    {
        string UploadFolder { get; set; }

        [OperationContract]
        void UploadFile(RemoteFileInfo request);

        [OperationContract]
        RemoteFileInfo DownloadFile(DownloadRequest request);

        [OperationContract]
        string[] GetFileNames();
    }
}
