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
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace integraMobile.ExternalWS.EysaThirdPartyConfirmParkWS {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="TicketSoap", Namespace="http://tempuri.org/")]
    public partial class Ticket : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private ConsolaSoapHeader consolaSoapHeaderValueField;
        
        private System.Threading.SendOrPostCallback rdPConfirmParkingOperationOperationCompleted;
        
        private System.Threading.SendOrPostCallback rdPQueryUnParkingOperationOperationCompleted;
        
        private System.Threading.SendOrPostCallback rdPConfirmUnParkingOperationOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public Ticket() {
            this.Url = global::integraMobile.ExternalWS.Properties.Settings.Default.integraMobile_ExternalWS_EysaThirdPartyConfirmParkWS_Ticket;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public ConsolaSoapHeader ConsolaSoapHeaderValue {
            get {
                return this.consolaSoapHeaderValueField;
            }
            set {
                this.consolaSoapHeaderValueField = value;
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
        public event rdPConfirmParkingOperationCompletedEventHandler rdPConfirmParkingOperationCompleted;
        
        /// <remarks/>
        public event rdPQueryUnParkingOperationCompletedEventHandler rdPQueryUnParkingOperationCompleted;
        
        /// <remarks/>
        public event rdPConfirmUnParkingOperationCompletedEventHandler rdPConfirmUnParkingOperationCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ConsolaSoapHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/rdPConfirmParkingOperation", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string rdPConfirmParkingOperation(string xmlIn) {
            object[] results = this.Invoke("rdPConfirmParkingOperation", new object[] {
                        xmlIn});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void rdPConfirmParkingOperationAsync(string xmlIn) {
            this.rdPConfirmParkingOperationAsync(xmlIn, null);
        }
        
        /// <remarks/>
        public void rdPConfirmParkingOperationAsync(string xmlIn, object userState) {
            if ((this.rdPConfirmParkingOperationOperationCompleted == null)) {
                this.rdPConfirmParkingOperationOperationCompleted = new System.Threading.SendOrPostCallback(this.OnrdPConfirmParkingOperationOperationCompleted);
            }
            this.InvokeAsync("rdPConfirmParkingOperation", new object[] {
                        xmlIn}, this.rdPConfirmParkingOperationOperationCompleted, userState);
        }
        
        private void OnrdPConfirmParkingOperationOperationCompleted(object arg) {
            if ((this.rdPConfirmParkingOperationCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.rdPConfirmParkingOperationCompleted(this, new rdPConfirmParkingOperationCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ConsolaSoapHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/rdPQueryUnParkingOperation", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string rdPQueryUnParkingOperation(string xmlIn) {
            object[] results = this.Invoke("rdPQueryUnParkingOperation", new object[] {
                        xmlIn});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void rdPQueryUnParkingOperationAsync(string xmlIn) {
            this.rdPQueryUnParkingOperationAsync(xmlIn, null);
        }
        
        /// <remarks/>
        public void rdPQueryUnParkingOperationAsync(string xmlIn, object userState) {
            if ((this.rdPQueryUnParkingOperationOperationCompleted == null)) {
                this.rdPQueryUnParkingOperationOperationCompleted = new System.Threading.SendOrPostCallback(this.OnrdPQueryUnParkingOperationOperationCompleted);
            }
            this.InvokeAsync("rdPQueryUnParkingOperation", new object[] {
                        xmlIn}, this.rdPQueryUnParkingOperationOperationCompleted, userState);
        }
        
        private void OnrdPQueryUnParkingOperationOperationCompleted(object arg) {
            if ((this.rdPQueryUnParkingOperationCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.rdPQueryUnParkingOperationCompleted(this, new rdPQueryUnParkingOperationCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("ConsolaSoapHeaderValue")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/rdPConfirmUnParkingOperation", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string rdPConfirmUnParkingOperation(string xmlIn) {
            object[] results = this.Invoke("rdPConfirmUnParkingOperation", new object[] {
                        xmlIn});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void rdPConfirmUnParkingOperationAsync(string xmlIn) {
            this.rdPConfirmUnParkingOperationAsync(xmlIn, null);
        }
        
        /// <remarks/>
        public void rdPConfirmUnParkingOperationAsync(string xmlIn, object userState) {
            if ((this.rdPConfirmUnParkingOperationOperationCompleted == null)) {
                this.rdPConfirmUnParkingOperationOperationCompleted = new System.Threading.SendOrPostCallback(this.OnrdPConfirmUnParkingOperationOperationCompleted);
            }
            this.InvokeAsync("rdPConfirmUnParkingOperation", new object[] {
                        xmlIn}, this.rdPConfirmUnParkingOperationOperationCompleted, userState);
        }
        
        private void OnrdPConfirmUnParkingOperationOperationCompleted(object arg) {
            if ((this.rdPConfirmUnParkingOperationCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.rdPConfirmUnParkingOperationCompleted(this, new rdPConfirmUnParkingOperationCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://tempuri.org/", IsNullable=false)]
    public partial class ConsolaSoapHeader : System.Web.Services.Protocols.SoapHeader {
        
        private int idContrataField;
        
        private System.DateTime localTimeField;
        
        private string nomUsuarioField;
        
        private string numSerieField;
        
        private string idUsuarioField;
        
        private string passwordField;
        
        private System.Xml.XmlAttribute[] anyAttrField;
        
        /// <remarks/>
        public int IdContrata {
            get {
                return this.idContrataField;
            }
            set {
                this.idContrataField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime LocalTime {
            get {
                return this.localTimeField;
            }
            set {
                this.localTimeField = value;
            }
        }
        
        /// <remarks/>
        public string NomUsuario {
            get {
                return this.nomUsuarioField;
            }
            set {
                this.nomUsuarioField = value;
            }
        }
        
        /// <remarks/>
        public string NumSerie {
            get {
                return this.numSerieField;
            }
            set {
                this.numSerieField = value;
            }
        }
        
        /// <remarks/>
        public string IdUsuario {
            get {
                return this.idUsuarioField;
            }
            set {
                this.idUsuarioField = value;
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
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr {
            get {
                return this.anyAttrField;
            }
            set {
                this.anyAttrField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    public delegate void rdPConfirmParkingOperationCompletedEventHandler(object sender, rdPConfirmParkingOperationCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class rdPConfirmParkingOperationCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal rdPConfirmParkingOperationCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    public delegate void rdPQueryUnParkingOperationCompletedEventHandler(object sender, rdPQueryUnParkingOperationCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class rdPQueryUnParkingOperationCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal rdPQueryUnParkingOperationCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    public delegate void rdPConfirmUnParkingOperationCompletedEventHandler(object sender, rdPConfirmUnParkingOperationCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class rdPConfirmUnParkingOperationCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal rdPConfirmUnParkingOperationCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591