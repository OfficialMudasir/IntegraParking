//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18034
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.18034.
// 
#pragma warning disable 1591

namespace GTechnaWS.StandardThirdPartyFineWS {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="PayByPhoneListenerSoapBinding", Namespace="http://192.168.3.91:8081/paybyphone-demo/services/PayByPhoneListener")]
    public partial class PayByPhoneSoapImplService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback queryFinePaymentQuantityOperationCompleted;
        
        private System.Threading.SendOrPostCallback queryListOfFinesOperationCompleted;
        
        private System.Threading.SendOrPostCallback confirmFinePaymentOperationCompleted;
        
        private System.Threading.SendOrPostCallback athentificationHashOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public PayByPhoneSoapImplService() {
            this.Url = global::GTechnaWS.Properties.Settings.Default.GTechnaWS_StandardThirdPartyFineWS_PayByPhoneSoapImplService;
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
        public event queryFinePaymentQuantityCompletedEventHandler queryFinePaymentQuantityCompleted;
        
        /// <remarks/>
        public event queryListOfFinesCompletedEventHandler queryListOfFinesCompleted;
        
        /// <remarks/>
        public event confirmFinePaymentCompletedEventHandler confirmFinePaymentCompleted;
        
        /// <remarks/>
        public event athentificationHashCompletedEventHandler athentificationHashCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace="http://paybyphone.cc.gti.com", ResponseNamespace="http://192.168.3.91:8081/paybyphone-demo/services/PayByPhoneListener")]
        [return: System.Xml.Serialization.SoapElementAttribute("queryFinePaymentQuantityReturn")]
        public string queryFinePaymentQuantity(string xmlIn) {
            object[] results = this.Invoke("queryFinePaymentQuantity", new object[] {
                        xmlIn});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void queryFinePaymentQuantityAsync(string xmlIn) {
            this.queryFinePaymentQuantityAsync(xmlIn, null);
        }
        
        /// <remarks/>
        public void queryFinePaymentQuantityAsync(string xmlIn, object userState) {
            if ((this.queryFinePaymentQuantityOperationCompleted == null)) {
                this.queryFinePaymentQuantityOperationCompleted = new System.Threading.SendOrPostCallback(this.OnqueryFinePaymentQuantityOperationCompleted);
            }
            this.InvokeAsync("queryFinePaymentQuantity", new object[] {
                        xmlIn}, this.queryFinePaymentQuantityOperationCompleted, userState);
        }
        
        private void OnqueryFinePaymentQuantityOperationCompleted(object arg) {
            if ((this.queryFinePaymentQuantityCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.queryFinePaymentQuantityCompleted(this, new queryFinePaymentQuantityCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace="http://paybyphone.cc.gti.com", ResponseNamespace="http://192.168.3.91:8081/paybyphone-demo/services/PayByPhoneListener")]
        [return: System.Xml.Serialization.SoapElementAttribute("queryListOfFinesReturn")]
        public string queryListOfFines(string xmlIn) {
            object[] results = this.Invoke("queryListOfFines", new object[] {
                        xmlIn});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void queryListOfFinesAsync(string xmlIn) {
            this.queryListOfFinesAsync(xmlIn, null);
        }
        
        /// <remarks/>
        public void queryListOfFinesAsync(string xmlIn, object userState) {
            if ((this.queryListOfFinesOperationCompleted == null)) {
                this.queryListOfFinesOperationCompleted = new System.Threading.SendOrPostCallback(this.OnqueryListOfFinesOperationCompleted);
            }
            this.InvokeAsync("queryListOfFines", new object[] {
                        xmlIn}, this.queryListOfFinesOperationCompleted, userState);
        }
        
        private void OnqueryListOfFinesOperationCompleted(object arg) {
            if ((this.queryListOfFinesCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.queryListOfFinesCompleted(this, new queryListOfFinesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace="http://paybyphone.cc.gti.com", ResponseNamespace="http://192.168.3.91:8081/paybyphone-demo/services/PayByPhoneListener")]
        [return: System.Xml.Serialization.SoapElementAttribute("confirmFinePaymentReturn")]
        public string confirmFinePayment(string xmlIn) {
            object[] results = this.Invoke("confirmFinePayment", new object[] {
                        xmlIn});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void confirmFinePaymentAsync(string xmlIn) {
            this.confirmFinePaymentAsync(xmlIn, null);
        }
        
        /// <remarks/>
        public void confirmFinePaymentAsync(string xmlIn, object userState) {
            if ((this.confirmFinePaymentOperationCompleted == null)) {
                this.confirmFinePaymentOperationCompleted = new System.Threading.SendOrPostCallback(this.OnconfirmFinePaymentOperationCompleted);
            }
            this.InvokeAsync("confirmFinePayment", new object[] {
                        xmlIn}, this.confirmFinePaymentOperationCompleted, userState);
        }
        
        private void OnconfirmFinePaymentOperationCompleted(object arg) {
            if ((this.confirmFinePaymentCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.confirmFinePaymentCompleted(this, new confirmFinePaymentCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace="http://paybyphone.cc.gti.com", ResponseNamespace="http://192.168.3.91:8081/paybyphone-demo/services/PayByPhoneListener")]
        [return: System.Xml.Serialization.SoapElementAttribute("athentificationHashReturn")]
        public string athentificationHash(string xmlIn) {
            object[] results = this.Invoke("athentificationHash", new object[] {
                        xmlIn});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void athentificationHashAsync(string xmlIn) {
            this.athentificationHashAsync(xmlIn, null);
        }
        
        /// <remarks/>
        public void athentificationHashAsync(string xmlIn, object userState) {
            if ((this.athentificationHashOperationCompleted == null)) {
                this.athentificationHashOperationCompleted = new System.Threading.SendOrPostCallback(this.OnathentificationHashOperationCompleted);
            }
            this.InvokeAsync("athentificationHash", new object[] {
                        xmlIn}, this.athentificationHashOperationCompleted, userState);
        }
        
        private void OnathentificationHashOperationCompleted(object arg) {
            if ((this.athentificationHashCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.athentificationHashCompleted(this, new athentificationHashCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void queryFinePaymentQuantityCompletedEventHandler(object sender, queryFinePaymentQuantityCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class queryFinePaymentQuantityCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal queryFinePaymentQuantityCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void queryListOfFinesCompletedEventHandler(object sender, queryListOfFinesCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class queryListOfFinesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal queryListOfFinesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void confirmFinePaymentCompletedEventHandler(object sender, confirmFinePaymentCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class confirmFinePaymentCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal confirmFinePaymentCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void athentificationHashCompletedEventHandler(object sender, athentificationHashCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class athentificationHashCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal athentificationHashCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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