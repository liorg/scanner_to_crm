﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.18408.
// 
#pragma warning disable 1591

namespace testdotnettwain.Crmdiscovery {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="CrmDiscoveryServiceSoap", Namespace="http://schemas.microsoft.com/crm/2007/CrmDiscoveryService")]
    public partial class CrmDiscoveryService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback ExecuteOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public CrmDiscoveryService() {
            this.Url = "https://vanguard.crm.g-s.co.il/mscrmservices/2007/spla/Crmdiscoveryservice.asmx";
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event ExecuteCompletedEventHandler ExecuteCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://schemas.microsoft.com/crm/2007/CrmDiscoveryService/Execute", RequestNamespace="http://schemas.microsoft.com/crm/2007/CrmDiscoveryService", ResponseNamespace="http://schemas.microsoft.com/crm/2007/CrmDiscoveryService", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("Response")]
        public Response Execute(Request Request) {
            object[] results = this.Invoke("Execute", new object[] {
                        Request});
            return ((Response)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginExecute(Request Request, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("Execute", new object[] {
                        Request}, callback, asyncState);
        }
        
        /// <remarks/>
        public Response EndExecute(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((Response)(results[0]));
        }
        
        /// <remarks/>
        public void ExecuteAsync(Request Request) {
            this.ExecuteAsync(Request, null);
        }
        
        /// <remarks/>
        public void ExecuteAsync(Request Request, object userState) {
            if ((this.ExecuteOperationCompleted == null)) {
                this.ExecuteOperationCompleted = new System.Threading.SendOrPostCallback(this.OnExecuteOperationCompleted);
            }
            this.InvokeAsync("Execute", new object[] {
                        Request}, this.ExecuteOperationCompleted, userState);
        }
        
        private void OnExecuteOperationCompleted(object arg) {
            if ((this.ExecuteCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ExecuteCompleted(this, new ExecuteCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IsEndUserNotificationAvailableRequest))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RetrievePolicyRequest))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RetrieveOrganizationExtendedDetailsRequest))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RetrieveCrmTicketRequest))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RetrieveOrganizationsRequest))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RetrieveClientPatchesRequest))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18408")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.microsoft.com/crm/2007/CrmDiscoveryService")]
    public abstract partial class Request {
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18408")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.microsoft.com/crm/2007/CrmDiscoveryService")]
    public partial class OrganizationDetail {
        
        private System.Guid organizationIdField;
        
        private string organizationNameField;
        
        private string friendlyNameField;
        
        private string crmMetadataServiceUrlField;
        
        private string crmServiceUrlField;
        
        private string webApplicationUrlField;
        
        /// <remarks/>
        public System.Guid OrganizationId {
            get {
                return this.organizationIdField;
            }
            set {
                this.organizationIdField = value;
            }
        }
        
        /// <remarks/>
        public string OrganizationName {
            get {
                return this.organizationNameField;
            }
            set {
                this.organizationNameField = value;
            }
        }
        
        /// <remarks/>
        public string FriendlyName {
            get {
                return this.friendlyNameField;
            }
            set {
                this.friendlyNameField = value;
            }
        }
        
        /// <remarks/>
        public string CrmMetadataServiceUrl {
            get {
                return this.crmMetadataServiceUrlField;
            }
            set {
                this.crmMetadataServiceUrlField = value;
            }
        }
        
        /// <remarks/>
        public string CrmServiceUrl {
            get {
                return this.crmServiceUrlField;
            }
            set {
                this.crmServiceUrlField = value;
            }
        }
        
        /// <remarks/>
        public string WebApplicationUrl {
            get {
                return this.webApplicationUrlField;
            }
            set {
                this.webApplicationUrlField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18408")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.microsoft.com/crm/2007/CrmDiscoveryService")]
    public partial class ClientPatchInfo {
        
        private System.Guid patchIdField;
        
        private string titleField;
        
        private string descriptionField;
        
        private bool isMandatoryField;
        
        private int depthField;
        
        private string linkIdField;
        
        /// <remarks/>
        public System.Guid PatchId {
            get {
                return this.patchIdField;
            }
            set {
                this.patchIdField = value;
            }
        }
        
        /// <remarks/>
        public string Title {
            get {
                return this.titleField;
            }
            set {
                this.titleField = value;
            }
        }
        
        /// <remarks/>
        public string Description {
            get {
                return this.descriptionField;
            }
            set {
                this.descriptionField = value;
            }
        }
        
        /// <remarks/>
        public bool IsMandatory {
            get {
                return this.isMandatoryField;
            }
            set {
                this.isMandatoryField = value;
            }
        }
        
        /// <remarks/>
        public int Depth {
            get {
                return this.depthField;
            }
            set {
                this.depthField = value;
            }
        }
        
        /// <remarks/>
        public string LinkId {
            get {
                return this.linkIdField;
            }
            set {
                this.linkIdField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18408")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.microsoft.com/crm/2007/CrmDiscoveryService")]
    public partial class OrganizationEndpoint {
        
        private int authenticationTypeField;
        
        private string crmMetadataServiceUrlField;
        
        private string crmServiceUrlField;
        
        private string webApplicationUrlField;
        
        /// <remarks/>
        public int AuthenticationType {
            get {
                return this.authenticationTypeField;
            }
            set {
                this.authenticationTypeField = value;
            }
        }
        
        /// <remarks/>
        public string CrmMetadataServiceUrl {
            get {
                return this.crmMetadataServiceUrlField;
            }
            set {
                this.crmMetadataServiceUrlField = value;
            }
        }
        
        /// <remarks/>
        public string CrmServiceUrl {
            get {
                return this.crmServiceUrlField;
            }
            set {
                this.crmServiceUrlField = value;
            }
        }
        
        /// <remarks/>
        public string WebApplicationUrl {
            get {
                return this.webApplicationUrlField;
            }
            set {
                this.webApplicationUrlField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18408")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.microsoft.com/crm/2007/CrmDiscoveryService")]
    public partial class OrganizationExtendedDetail {
        
        private System.Guid organizationIdField;
        
        private string organizationNameField;
        
        private string friendlyNameField;
        
        private OrganizationEndpoint[] endpointsField;
        
        /// <remarks/>
        public System.Guid OrganizationId {
            get {
                return this.organizationIdField;
            }
            set {
                this.organizationIdField = value;
            }
        }
        
        /// <remarks/>
        public string OrganizationName {
            get {
                return this.organizationNameField;
            }
            set {
                this.organizationNameField = value;
            }
        }
        
        /// <remarks/>
        public string FriendlyName {
            get {
                return this.friendlyNameField;
            }
            set {
                this.friendlyNameField = value;
            }
        }
        
        /// <remarks/>
        public OrganizationEndpoint[] Endpoints {
            get {
                return this.endpointsField;
            }
            set {
                this.endpointsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RetrievePolicyResponse))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RetrieveCrmTicketResponse))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RetrieveOrganizationsResponse))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RetrieveClientPatchesResponse))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IsEndUserNotificationAvailableResponse))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RetrieveOrganizationExtendedDetailsResponse))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18408")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.microsoft.com/crm/2007/CrmDiscoveryService")]
    public abstract partial class Response {
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18408")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.microsoft.com/crm/2007/CrmDiscoveryService")]
    public partial class RetrievePolicyResponse : Response {
        
        private string policyField;
        
        /// <remarks/>
        public string Policy {
            get {
                return this.policyField;
            }
            set {
                this.policyField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18408")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.microsoft.com/crm/2007/CrmDiscoveryService")]
    public partial class RetrieveCrmTicketResponse : Response {
        
        private string crmTicketField;
        
        private OrganizationDetail organizationDetailField;
        
        private string expirationDateField;
        
        /// <remarks/>
        public string CrmTicket {
            get {
                return this.crmTicketField;
            }
            set {
                this.crmTicketField = value;
            }
        }
        
        /// <remarks/>
        public OrganizationDetail OrganizationDetail {
            get {
                return this.organizationDetailField;
            }
            set {
                this.organizationDetailField = value;
            }
        }
        
        /// <remarks/>
        public string ExpirationDate {
            get {
                return this.expirationDateField;
            }
            set {
                this.expirationDateField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18408")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.microsoft.com/crm/2007/CrmDiscoveryService")]
    public partial class RetrieveOrganizationsResponse : Response {
        
        private OrganizationDetail[] organizationDetailsField;
        
        /// <remarks/>
        public OrganizationDetail[] OrganizationDetails {
            get {
                return this.organizationDetailsField;
            }
            set {
                this.organizationDetailsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18408")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.microsoft.com/crm/2007/CrmDiscoveryService")]
    public partial class RetrieveClientPatchesResponse : Response {
        
        private ClientPatchInfo[] patchInfoField;
        
        /// <remarks/>
        public ClientPatchInfo[] PatchInfo {
            get {
                return this.patchInfoField;
            }
            set {
                this.patchInfoField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18408")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.microsoft.com/crm/2007/CrmDiscoveryService")]
    public partial class IsEndUserNotificationAvailableResponse : Response {
        
        private bool isEndUserNotificationAvailableField;
        
        /// <remarks/>
        public bool IsEndUserNotificationAvailable {
            get {
                return this.isEndUserNotificationAvailableField;
            }
            set {
                this.isEndUserNotificationAvailableField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18408")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.microsoft.com/crm/2007/CrmDiscoveryService")]
    public partial class RetrieveOrganizationExtendedDetailsResponse : Response {
        
        private OrganizationExtendedDetail[] organizationExtendedDetailsField;
        
        /// <remarks/>
        public OrganizationExtendedDetail[] OrganizationExtendedDetails {
            get {
                return this.organizationExtendedDetailsField;
            }
            set {
                this.organizationExtendedDetailsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18408")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.microsoft.com/crm/2007/CrmDiscoveryService")]
    public partial class ClientInfo {
        
        private System.Guid[] patchIdsField;
        
        private ClientTypes clientTypeField;
        
        private System.Guid userIdField;
        
        private System.Guid organizationIdField;
        
        private int languageCodeField;
        
        private string officeVersionField;
        
        private string oSVersionField;
        
        private string crmVersionField;
        
        /// <remarks/>
        public System.Guid[] PatchIds {
            get {
                return this.patchIdsField;
            }
            set {
                this.patchIdsField = value;
            }
        }
        
        /// <remarks/>
        public ClientTypes ClientType {
            get {
                return this.clientTypeField;
            }
            set {
                this.clientTypeField = value;
            }
        }
        
        /// <remarks/>
        public System.Guid UserId {
            get {
                return this.userIdField;
            }
            set {
                this.userIdField = value;
            }
        }
        
        /// <remarks/>
        public System.Guid OrganizationId {
            get {
                return this.organizationIdField;
            }
            set {
                this.organizationIdField = value;
            }
        }
        
        /// <remarks/>
        public int LanguageCode {
            get {
                return this.languageCodeField;
            }
            set {
                this.languageCodeField = value;
            }
        }
        
        /// <remarks/>
        public string OfficeVersion {
            get {
                return this.officeVersionField;
            }
            set {
                this.officeVersionField = value;
            }
        }
        
        /// <remarks/>
        public string OSVersion {
            get {
                return this.oSVersionField;
            }
            set {
                this.oSVersionField = value;
            }
        }
        
        /// <remarks/>
        public string CrmVersion {
            get {
                return this.crmVersionField;
            }
            set {
                this.crmVersionField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.FlagsAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18408")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.microsoft.com/crm/2007/CrmDiscoveryService")]
    public enum ClientTypes {
        
        /// <remarks/>
        OutlookLaptop = 1,
        
        /// <remarks/>
        OutlookDesktop = 2,
        
        /// <remarks/>
        DataMigration = 4,
        
        /// <remarks/>
        OutlookConfiguration = 8,
        
        /// <remarks/>
        DataMigrationConfiguration = 16,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18408")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.microsoft.com/crm/2007/CrmDiscoveryService")]
    public partial class IsEndUserNotificationAvailableRequest : Request {
        
        private System.Guid organizationIdField;
        
        private EndUserNotificationClient clientField;
        
        /// <remarks/>
        public System.Guid OrganizationId {
            get {
                return this.organizationIdField;
            }
            set {
                this.organizationIdField = value;
            }
        }
        
        /// <remarks/>
        public EndUserNotificationClient Client {
            get {
                return this.clientField;
            }
            set {
                this.clientField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18408")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.microsoft.com/crm/2007/CrmDiscoveryService")]
    public enum EndUserNotificationClient {
        
        /// <remarks/>
        None,
        
        /// <remarks/>
        WebApplication,
        
        /// <remarks/>
        Portal,
        
        /// <remarks/>
        Outlook,
        
        /// <remarks/>
        Email,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18408")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.microsoft.com/crm/2007/CrmDiscoveryService")]
    public partial class RetrievePolicyRequest : Request {
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18408")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.microsoft.com/crm/2007/CrmDiscoveryService")]
    public partial class RetrieveOrganizationExtendedDetailsRequest : Request {
        
        private string passportTicketField;
        
        private string userIdField;
        
        private string passwordField;
        
        /// <remarks/>
        public string PassportTicket {
            get {
                return this.passportTicketField;
            }
            set {
                this.passportTicketField = value;
            }
        }
        
        /// <remarks/>
        public string UserId {
            get {
                return this.userIdField;
            }
            set {
                this.userIdField = value;
            }
        }
        
        /// <remarks/>
        public string Password {
            get {
                return this.passwordField;
            }
            set {
                this.passwordField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18408")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.microsoft.com/crm/2007/CrmDiscoveryService")]
    public partial class RetrieveCrmTicketRequest : Request {
        
        private string organizationNameField;
        
        private string passportTicketField;
        
        private string userIdField;
        
        private string passwordField;
        
        /// <remarks/>
        public string OrganizationName {
            get {
                return this.organizationNameField;
            }
            set {
                this.organizationNameField = value;
            }
        }
        
        /// <remarks/>
        public string PassportTicket {
            get {
                return this.passportTicketField;
            }
            set {
                this.passportTicketField = value;
            }
        }
        
        /// <remarks/>
        public string UserId {
            get {
                return this.userIdField;
            }
            set {
                this.userIdField = value;
            }
        }
        
        /// <remarks/>
        public string Password {
            get {
                return this.passwordField;
            }
            set {
                this.passwordField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18408")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.microsoft.com/crm/2007/CrmDiscoveryService")]
    public partial class RetrieveOrganizationsRequest : Request {
        
        private string passportTicketField;
        
        private string userIdField;
        
        private string passwordField;
        
        /// <remarks/>
        public string PassportTicket {
            get {
                return this.passportTicketField;
            }
            set {
                this.passportTicketField = value;
            }
        }
        
        /// <remarks/>
        public string UserId {
            get {
                return this.userIdField;
            }
            set {
                this.userIdField = value;
            }
        }
        
        /// <remarks/>
        public string Password {
            get {
                return this.passwordField;
            }
            set {
                this.passwordField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18408")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.microsoft.com/crm/2007/CrmDiscoveryService")]
    public partial class RetrieveClientPatchesRequest : Request {
        
        private ClientInfo clientInfoField;
        
        /// <remarks/>
        public ClientInfo ClientInfo {
            get {
                return this.clientInfoField;
            }
            set {
                this.clientInfoField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    public delegate void ExecuteCompletedEventHandler(object sender, ExecuteCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ExecuteCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ExecuteCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public Response Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((Response)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591