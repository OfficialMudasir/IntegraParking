﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// Microsoft.VSDesigner generó automáticamente este código fuente, versión=4.0.30319.42000.
// 
#pragma warning disable 1591

namespace integraMobile.Infrastructure.BSRedsysWs {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="SerClsWSEntradaSoapBinding", Namespace="http://webservice.sis.sermepa.es")]
    public partial class SerClsWSEntradaService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback trataPeticionOperationCompleted;
        
        private System.Threading.SendOrPostCallback trataPeticion3DESOperationCompleted;
        
        private System.Threading.SendOrPostCallback consultaDCCOperationCompleted;
        
        private System.Threading.SendOrPostCallback consultaBINOperationCompleted;
        
        private System.Threading.SendOrPostCallback iniciaPeticionOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public SerClsWSEntradaService() {
            this.Url = global::integraMobile.Infrastructure.Properties.Settings.Default.integraMobile_Infrastructure_BSRedsysWs_SerClsWSEntradaService;
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
        public event trataPeticionCompletedEventHandler trataPeticionCompleted;
        
        /// <remarks/>
        public event trataPeticion3DESCompletedEventHandler trataPeticion3DESCompleted;
        
        /// <remarks/>
        public event consultaDCCCompletedEventHandler consultaDCCCompleted;
        
        /// <remarks/>
        public event consultaBINCompletedEventHandler consultaBINCompleted;
        
        /// <remarks/>
        public event iniciaPeticionCompletedEventHandler iniciaPeticionCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://webservice.sis.sermepa.es", ResponseNamespace="http://webservice.sis.sermepa.es", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("trataPeticionReturn", IsNullable=true)]
        public string trataPeticion([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string datoEntrada) {
            object[] results = this.Invoke("trataPeticion", new object[] {
                        datoEntrada});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void trataPeticionAsync(string datoEntrada) {
            this.trataPeticionAsync(datoEntrada, null);
        }
        
        /// <remarks/>
        public void trataPeticionAsync(string datoEntrada, object userState) {
            if ((this.trataPeticionOperationCompleted == null)) {
                this.trataPeticionOperationCompleted = new System.Threading.SendOrPostCallback(this.OntrataPeticionOperationCompleted);
            }
            this.InvokeAsync("trataPeticion", new object[] {
                        datoEntrada}, this.trataPeticionOperationCompleted, userState);
        }
        
        private void OntrataPeticionOperationCompleted(object arg) {
            if ((this.trataPeticionCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.trataPeticionCompleted(this, new trataPeticionCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://webservice.sis.sermepa.es", ResponseElementName="trataPeticionResponse3DES", ResponseNamespace="http://webservice.sis.sermepa.es", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("trataPeticionReturn", IsNullable=true)]
        public string trataPeticion3DES([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string datoEntrada) {
            object[] results = this.Invoke("trataPeticion3DES", new object[] {
                        datoEntrada});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void trataPeticion3DESAsync(string datoEntrada) {
            this.trataPeticion3DESAsync(datoEntrada, null);
        }
        
        /// <remarks/>
        public void trataPeticion3DESAsync(string datoEntrada, object userState) {
            if ((this.trataPeticion3DESOperationCompleted == null)) {
                this.trataPeticion3DESOperationCompleted = new System.Threading.SendOrPostCallback(this.OntrataPeticion3DESOperationCompleted);
            }
            this.InvokeAsync("trataPeticion3DES", new object[] {
                        datoEntrada}, this.trataPeticion3DESOperationCompleted, userState);
        }
        
        private void OntrataPeticion3DESOperationCompleted(object arg) {
            if ((this.trataPeticion3DESCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.trataPeticion3DESCompleted(this, new trataPeticion3DESCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://webservice.sis.sermepa.es", ResponseNamespace="http://webservice.sis.sermepa.es", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("consultaDCCReturn", IsNullable=true)]
        public string consultaDCC([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string datoEntrada) {
            object[] results = this.Invoke("consultaDCC", new object[] {
                        datoEntrada});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void consultaDCCAsync(string datoEntrada) {
            this.consultaDCCAsync(datoEntrada, null);
        }
        
        /// <remarks/>
        public void consultaDCCAsync(string datoEntrada, object userState) {
            if ((this.consultaDCCOperationCompleted == null)) {
                this.consultaDCCOperationCompleted = new System.Threading.SendOrPostCallback(this.OnconsultaDCCOperationCompleted);
            }
            this.InvokeAsync("consultaDCC", new object[] {
                        datoEntrada}, this.consultaDCCOperationCompleted, userState);
        }
        
        private void OnconsultaDCCOperationCompleted(object arg) {
            if ((this.consultaDCCCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.consultaDCCCompleted(this, new consultaDCCCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://webservice.sis.sermepa.es", ResponseNamespace="http://webservice.sis.sermepa.es", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("consultaBINReturn", IsNullable=true)]
        public string consultaBIN([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string datoEntrada) {
            object[] results = this.Invoke("consultaBIN", new object[] {
                        datoEntrada});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void consultaBINAsync(string datoEntrada) {
            this.consultaBINAsync(datoEntrada, null);
        }
        
        /// <remarks/>
        public void consultaBINAsync(string datoEntrada, object userState) {
            if ((this.consultaBINOperationCompleted == null)) {
                this.consultaBINOperationCompleted = new System.Threading.SendOrPostCallback(this.OnconsultaBINOperationCompleted);
            }
            this.InvokeAsync("consultaBIN", new object[] {
                        datoEntrada}, this.consultaBINOperationCompleted, userState);
        }
        
        private void OnconsultaBINOperationCompleted(object arg) {
            if ((this.consultaBINCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.consultaBINCompleted(this, new consultaBINCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://webservice.sis.sermepa.es", ResponseNamespace="http://webservice.sis.sermepa.es", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("iniciaPeticionReturn", IsNullable=true)]
        public string iniciaPeticion([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string datoEntrada) {
            object[] results = this.Invoke("iniciaPeticion", new object[] {
                        datoEntrada});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void iniciaPeticionAsync(string datoEntrada) {
            this.iniciaPeticionAsync(datoEntrada, null);
        }
        
        /// <remarks/>
        public void iniciaPeticionAsync(string datoEntrada, object userState) {
            if ((this.iniciaPeticionOperationCompleted == null)) {
                this.iniciaPeticionOperationCompleted = new System.Threading.SendOrPostCallback(this.OniniciaPeticionOperationCompleted);
            }
            this.InvokeAsync("iniciaPeticion", new object[] {
                        datoEntrada}, this.iniciaPeticionOperationCompleted, userState);
        }
        
        private void OniniciaPeticionOperationCompleted(object arg) {
            if ((this.iniciaPeticionCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.iniciaPeticionCompleted(this, new iniciaPeticionCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    public delegate void trataPeticionCompletedEventHandler(object sender, trataPeticionCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class trataPeticionCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal trataPeticionCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    public delegate void trataPeticion3DESCompletedEventHandler(object sender, trataPeticion3DESCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class trataPeticion3DESCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal trataPeticion3DESCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    public delegate void consultaDCCCompletedEventHandler(object sender, consultaDCCCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class consultaDCCCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal consultaDCCCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    public delegate void consultaBINCompletedEventHandler(object sender, consultaBINCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class consultaBINCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal consultaBINCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    public delegate void iniciaPeticionCompletedEventHandler(object sender, iniciaPeticionCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class iniciaPeticionCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal iniciaPeticionCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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