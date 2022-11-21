using System;
using System.IO;
using System.Text;
using System.Web.Services.Protocols;
using System.Xml;
using System.Diagnostics;

namespace WSIparksuiteTest
{

    /// <summary>
    /// Interface that a proxy class should implement to support tracing
    /// </summary>
    public interface ITraceable
    {
        bool IsTraceRequestEnabled { get; set; }
        bool IsTraceResponseEnabled { get; set; }
        string ComponentName { get; set; }
    }

    /// <summary>
    /// Type of a traced message
    /// </summary>
    enum DumpType
    {
        Request,
        Response,
    }

    #region TraceExtension

    public class TraceExtension : SoapExtension
    {
        TraceExtensionStream traceStream;

        /// <summary>
        /// Replaces soap stream with our smart stream
        /// </summary>
        public override Stream ChainStream(Stream stream)
        {
            traceStream = new TraceExtensionStream(stream);
            return traceStream;
        }

        public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute)
        {
            return null;
        }

        public override object GetInitializer(Type WebServiceType)
        {
            return null;
        }

        public override void Initialize(object initializer)
        {
        }


        public override void ProcessMessage(SoapMessage message)
        {
            ITraceable traceable = GetTraceable(message);
            //If proxy is not configured to be traced, return
            if (traceable == null) return;
            switch (message.Stage)
            {
                case SoapMessageStage.BeforeSerialize:
                    //If tracing is enabled, switch to memory buffer
                    if (traceable.IsTraceRequestEnabled)
                    {
                        traceStream.SwitchToNewStream();
                    }
                    break;
                case SoapMessageStage.AfterSerialize:
                    //If message was written to memory buffer, write its content to log and copy to the SOAP stream
                    if (traceStream.IsNewStream)
                    {
                        traceStream.Position = 0;
                        WriteToLog(DumpType.Request, traceable);
                        traceStream.Position = 0;
                        traceStream.CopyNewToOld();
                    }
                    break;
                case SoapMessageStage.BeforeDeserialize:
                    //If tracing is enabled, copy SOAP stream to the new stream and write its content to log
                    if (traceable.IsTraceResponseEnabled)
                    {
                        traceStream.SwitchToNewStream();
                        traceStream.CopyOldToNew();
                        WriteToLog(DumpType.Response, traceable);
                        traceStream.Position = 0;
                    }
                    break;
            }
        }

        /// <summary>
        /// Tries to get ITraceable instance 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private ITraceable GetTraceable(SoapMessage message)
        {
            SoapClientMessage clientMessage = message as SoapClientMessage;
            if (clientMessage != null)
            {
                return clientMessage.Client as ITraceable;
            }

            return null;
        }

        private void WriteToLog(DumpType type, ITraceable traceable)
        {
            try
            {
                XmlTextReader xmlTextReader = new XmlTextReader(traceStream.InnerStream);
                StringWriter writer = new StringWriter(new StringBuilder((int)(traceStream.InnerStream.Length * 1.51)));
                XmlTextWriter xmlTextWriter = new XmlTextWriter(writer);
                xmlTextWriter.Formatting = Formatting.Indented;
                xmlTextWriter.IndentChar = '\t';
                xmlTextWriter.Indentation = 1;
                xmlTextReader.MoveToContent();
                xmlTextWriter.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"utf-8\"");
                while (!xmlTextReader.EOF)
                {
                    if (xmlTextReader.NodeType != XmlNodeType.XmlDeclaration)
                        xmlTextWriter.WriteNode(xmlTextReader, false);
                }
                Trace.WriteLine(writer.ToString(), String.Format("{0} {1}", traceable.ComponentName, type));
            }
            catch (Exception e)
            {
                Trace.TraceError("Error in TraceExtension: {0}", e);
            }
        }
    }

    #endregion

    #region TraceExtensionStream

    /// <summary>
    /// Special switchable stream
    /// </summary>
    internal class TraceExtensionStream : Stream
    {
        #region Fields

        private Stream innerStream;
        private readonly Stream originalStream;

        #endregion

        #region .ctor

        /// <summary>
        /// Constructs an instance of the stream wrapping the original stream into it
        /// </summary>
        internal TraceExtensionStream(Stream originalStream)
        {
            innerStream = this.originalStream = originalStream;
        }

        #endregion

        #region New public members

        /// <summary>
        /// Creates a new memory stream and makes it active 
        /// </summary>
        public void SwitchToNewStream()
        {
            innerStream = new MemoryStream();
        }

        /// <summary>
        /// Copies data from the old stream to the new in-memory stream 
        /// </summary>
        public void CopyOldToNew()
        {
            //innerStream = new MemoryStream((int)originalStream.Length);
            Copy(originalStream, innerStream);
            innerStream.Position = 0;
        }

        /// <summary>
        /// Copies data from the new stream to the old stream
        /// </summary>
        public void CopyNewToOld()
        {
            Copy(innerStream, originalStream);
        }

        /// <summary>
        /// Returns <c>true</c> if the active inner stream is a new stream, i.e. <see cref="SwitchToNewStream"/> has been called
        /// </summary>
        public bool IsNewStream
        {
            get
            {
                return (innerStream != originalStream);
            }
        }

        /// <summary>
        /// A link to the active inner stream
        /// </summary>
        public Stream InnerStream
        {
            get { return innerStream; }
        }

        #endregion

        #region Private members

        private static void Copy(Stream from, Stream to)
        {
            const int size = 4096;
            byte[] bytes = new byte[4096];
            int numBytes;
            while ((numBytes = from.Read(bytes, 0, size)) > 0)
                to.Write(bytes, 0, numBytes);
        }

        #endregion

        #region Overridden members

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return innerStream.BeginRead(buffer, offset, count, callback, state);
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return innerStream.BeginWrite(buffer, offset, count, callback, state);
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    innerStream.Close();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            return innerStream.EndRead(asyncResult);
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            innerStream.EndWrite(asyncResult);
        }

        public override void Flush()
        {
            innerStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return innerStream.Read(buffer, offset, count);
        }

        public override int ReadByte()
        {
            return innerStream.ReadByte();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return innerStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            innerStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            innerStream.Write(buffer, offset, count);
        }

        public override void WriteByte(byte value)
        {
            innerStream.WriteByte(value);
        }

        // Properties
        public override bool CanRead
        {
            get
            {
                return innerStream.CanRead;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return innerStream.CanSeek;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return innerStream.CanWrite;
            }
        }

        public override long Length
        {
            get
            {
                return innerStream.Length;
            }
        }

        public override long Position
        {
            get
            {
                return innerStream.Position;
            }
            set
            {
                innerStream.Position = value;
            }
        }

        #endregion
    }

    #endregion
}