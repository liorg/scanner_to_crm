﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18034
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace testdotnettwain.UploadLargeImages {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="UploadLargeImages.IFileTransferService")]
    public interface IFileTransferService {
        
        // CODEGEN: Generating message contract since the operation UploadFile is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IFileTransferService/UploadFile", ReplyAction="http://tempuri.org/IFileTransferService/UploadFileResponse")]
        testdotnettwain.UploadLargeImages.UploadFileResponse UploadFile(testdotnettwain.UploadLargeImages.RemoteFileInfo request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="RemoteFileInfo", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class RemoteFileInfo {
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://tempuri.org/")]
        public string FileName;
        
        [System.ServiceModel.MessageHeaderAttribute(Namespace="http://tempuri.org/")]
        public long Length;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public System.IO.Stream FileByteStream;
        
        public RemoteFileInfo() {
        }
        
        public RemoteFileInfo(string FileName, long Length, System.IO.Stream FileByteStream) {
            this.FileName = FileName;
            this.Length = Length;
            this.FileByteStream = FileByteStream;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class UploadFileResponse {
        
        public UploadFileResponse() {
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IFileTransferServiceChannel : testdotnettwain.UploadLargeImages.IFileTransferService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class FileTransferServiceClient : System.ServiceModel.ClientBase<testdotnettwain.UploadLargeImages.IFileTransferService>, testdotnettwain.UploadLargeImages.IFileTransferService {
        
        public FileTransferServiceClient() {
        }
        
        public FileTransferServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public FileTransferServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public FileTransferServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public FileTransferServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        testdotnettwain.UploadLargeImages.UploadFileResponse testdotnettwain.UploadLargeImages.IFileTransferService.UploadFile(testdotnettwain.UploadLargeImages.RemoteFileInfo request) {
            return base.Channel.UploadFile(request);
        }
        
        public void UploadFile(string FileName, long Length, System.IO.Stream FileByteStream) {
            testdotnettwain.UploadLargeImages.RemoteFileInfo inValue = new testdotnettwain.UploadLargeImages.RemoteFileInfo();
            inValue.FileName = FileName;
            inValue.Length = Length;
            inValue.FileByteStream = FileByteStream;
            testdotnettwain.UploadLargeImages.UploadFileResponse retVal = ((testdotnettwain.UploadLargeImages.IFileTransferService)(this)).UploadFile(inValue);
        }
    }
}
