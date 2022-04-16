﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This code was auto-generated by Microsoft.VisualStudio.ServiceReference.Platforms, version 17.0.32113.165
// 
namespace Projent.PMServer2 {
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Message", Namespace="http://schemas.datacontract.org/2004/07/PMService2.Model")]
    public partial class Message : object, System.ComponentModel.INotifyPropertyChanged {
        
        private System.Collections.ObjectModel.ObservableCollection<string> MentionedUsersField;
        
        private string MessageContentField;
        
        private System.DateTime TimeField;
        
        private bool isStickerField;
        
        private string receiverField;
        
        private string senderField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Collections.ObjectModel.ObservableCollection<string> MentionedUsers {
            get {
                return this.MentionedUsersField;
            }
            set {
                if ((object.ReferenceEquals(this.MentionedUsersField, value) != true)) {
                    this.MentionedUsersField = value;
                    this.RaisePropertyChanged("MentionedUsers");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string MessageContent {
            get {
                return this.MessageContentField;
            }
            set {
                if ((object.ReferenceEquals(this.MessageContentField, value) != true)) {
                    this.MessageContentField = value;
                    this.RaisePropertyChanged("MessageContent");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime Time {
            get {
                return this.TimeField;
            }
            set {
                if ((this.TimeField.Equals(value) != true)) {
                    this.TimeField = value;
                    this.RaisePropertyChanged("Time");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool isSticker {
            get {
                return this.isStickerField;
            }
            set {
                if ((this.isStickerField.Equals(value) != true)) {
                    this.isStickerField = value;
                    this.RaisePropertyChanged("isSticker");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string receiver {
            get {
                return this.receiverField;
            }
            set {
                if ((object.ReferenceEquals(this.receiverField, value) != true)) {
                    this.receiverField = value;
                    this.RaisePropertyChanged("receiver");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string sender {
            get {
                return this.senderField;
            }
            set {
                if ((object.ReferenceEquals(this.senderField, value) != true)) {
                    this.senderField = value;
                    this.RaisePropertyChanged("sender");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="PMServer2.IProjectService")]
    public interface IProjectService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProjectService/IntializeDatabaseService", ReplyAction="http://tempuri.org/IProjectService/IntializeDatabaseServiceResponse")]
        System.Threading.Tasks.Task IntializeDatabaseServiceAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProjectService/FindDirectMessagesFor", ReplyAction="http://tempuri.org/IProjectService/FindDirectMessagesForResponse")]
        System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<Projent.PMServer2.Message>> FindDirectMessagesForAsync(string sender, System.DateTime lastMessage);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProjectService/NewMessage", ReplyAction="http://tempuri.org/IProjectService/NewMessageResponse")]
        System.Threading.Tasks.Task<bool> NewMessageAsync(Projent.PMServer2.Message message);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProjectService/RequestState", ReplyAction="http://tempuri.org/IProjectService/RequestStateResponse")]
        System.Threading.Tasks.Task<bool> RequestStateAsync(string DeviceID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProjectService/CheckNewMessagesFor", ReplyAction="http://tempuri.org/IProjectService/CheckNewMessagesForResponse")]
        System.Threading.Tasks.Task<bool> CheckNewMessagesForAsync(string username, System.DateTime latestMessageTime);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IProjectService/DeleteMessagesFrom", ReplyAction="http://tempuri.org/IProjectService/DeleteMessagesFromResponse")]
        System.Threading.Tasks.Task<bool> DeleteMessagesFromAsync(string sender, string receiver);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IProjectServiceChannel : Projent.PMServer2.IProjectService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ProjectServiceClient : System.ServiceModel.ClientBase<Projent.PMServer2.IProjectService>, Projent.PMServer2.IProjectService {
        
        /// <summary>
        /// Implement this partial method to configure the service endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint to configure</param>
        /// <param name="clientCredentials">The client credentials</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public ProjectServiceClient(EndpointConfiguration endpointConfiguration) : 
                base(ProjectServiceClient.GetBindingForEndpoint(endpointConfiguration), ProjectServiceClient.GetEndpointAddress(endpointConfiguration)) {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public ProjectServiceClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(ProjectServiceClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress)) {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public ProjectServiceClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(ProjectServiceClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress) {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public ProjectServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Threading.Tasks.Task IntializeDatabaseServiceAsync() {
            return base.Channel.IntializeDatabaseServiceAsync();
        }
        
        public System.Threading.Tasks.Task<System.Collections.ObjectModel.ObservableCollection<Projent.PMServer2.Message>> FindDirectMessagesForAsync(string sender, System.DateTime lastMessage) {
            return base.Channel.FindDirectMessagesForAsync(sender, lastMessage);
        }
        
        public System.Threading.Tasks.Task<bool> NewMessageAsync(Projent.PMServer2.Message message) {
            return base.Channel.NewMessageAsync(message);
        }
        
        public System.Threading.Tasks.Task<bool> RequestStateAsync(string DeviceID) {
            return base.Channel.RequestStateAsync(DeviceID);
        }
        
        public System.Threading.Tasks.Task<bool> CheckNewMessagesForAsync(string username, System.DateTime latestMessageTime) {
            return base.Channel.CheckNewMessagesForAsync(username, latestMessageTime);
        }
        
        public System.Threading.Tasks.Task<bool> DeleteMessagesFromAsync(string sender, string receiver) {
            return base.Channel.DeleteMessagesFromAsync(sender, receiver);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync() {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        public virtual System.Threading.Tasks.Task CloseAsync() {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginClose(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndClose));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration) {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IProjectService)) {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            if ((endpointConfiguration == EndpointConfiguration.NetTcpBinding_IProjectService)) {
                System.ServiceModel.NetTcpBinding result = new System.ServiceModel.NetTcpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration) {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IProjectService)) {
                return new System.ServiceModel.EndpointAddress("http://pasan-desktop:8076/ProjectServiceReference");
            }
            if ((endpointConfiguration == EndpointConfiguration.NetTcpBinding_IProjectService)) {
                return new System.ServiceModel.EndpointAddress(new System.Uri("net.tcp://pasan-desktop:8070/ProjectServiceReference"), new System.ServiceModel.UpnEndpointIdentity("PASAN-DESKTOP\\Pasan"));
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        public enum EndpointConfiguration {
            
            BasicHttpBinding_IProjectService,
            
            NetTcpBinding_IProjectService,
        }
    }
}
