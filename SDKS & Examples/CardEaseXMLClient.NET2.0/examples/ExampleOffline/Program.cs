using System;

using CardEaseXML;

namespace ExampleOffline
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: " + System.Diagnostics.Process.GetCurrentProcess().ProcessName + " TerminalID TransactionKey");
                Console.WriteLine();
                Console.WriteLine("The TerminalID and TransactionKey are provided at registration:");
                Console.WriteLine();
                Console.WriteLine("        https://testwebmis.creditcall.com");
                Console.WriteLine();
                Console.WriteLine("Press Enter to continue . . .");
                Console.Read();
                return 1;
            }

            // Setup the request
            Request request = new Request();

            request.SoftwareName = "SoftwareName";
            request.SoftwareVersion = "SoftwareVersion";
            request.TerminalID = args[0];
            request.TransactionKey = args[1];

            // Setup the request detail
            request.RequestType = RequestType.Offline;
            request.Amount = "123";
            request.AddICCTag(new ICCTag("0x57", "5413330089000013D0712201087490938F"));
            request.AddICCTag(new ICCTag("0x5a", "5413330089000013"));
            request.AddICCTag(new ICCTag("0x5f24", "071231"));
            request.AddICCTag(new ICCTag("0x5f34", "00"));
            request.AddICCTag(new ICCTag("0x9f02", "000000000123"));
            request.AddICCTag(new ICCTag("0x9f03", "000000000000"));
            request.AddICCTag(new ICCTag("0x9f26", "095BDFF410C27326"));
            request.AddICCTag(new ICCTag("0x82", "1800"));
            request.AddICCTag(new ICCTag("0x9f36", "0003"));
            request.AddICCTag(new ICCTag("0x9f37", "28BB7FE5"));
            request.AddICCTag(new ICCTag("0x95", "800000E000"));
            request.AddICCTag(new ICCTag("0x9c", "00"));
            request.AddICCTag(new ICCTag("0x9f10", "020000000000"));
            request.AddICCTag(new ICCTag("0x9f06", "A0000000041010"));
            request.AddICCTag(new ICCTag("0x9f09", "0002"));
            request.AddICCTag(new ICCTag("0x9f27", "80"));
            request.AddICCTag(new ICCTag("0x9f34", "410302"));
            request.AddICCTag(new ICCTag("0x9f35", "24"));
            request.AddICCTag(new ICCTag("0x9b", "6800"));
            request.AddICCTag(new ICCTag("0x4f", "A0000000041010"));
            request.AddICCTag(new ICCTag("0x9f08", "0002"));
            request.AddICCTag(new ICCTag("0x9f07", "FF00"));
            request.AddICCTag(new ICCTag("0x5f28", "0056"));
            request.AddICCTag(new ICCTag("0x9f0d", "F040642000"));
            request.AddICCTag(new ICCTag("0x9f0e", "0010880000"));
            request.AddICCTag(new ICCTag("0x9f0f", "F0E064F800"));
            request.AddICCTag(new ICCTag("0x9f33", "6098C8"));
            request.AddICCTag(new ICCTag("0x9a", "060424"));
            request.AddICCTag(new ICCTag("0x5f20", ICCTagValueType.String, "REQ01 MC"));

            Console.WriteLine(request);

            // Setup the client
            Client client = new Client();
            client.AddServerURL("https://test.cardeasexml.com/generic.cex", 45000);
            client.Request = request;

            try
            {
                // Process the request
                client.ProcessRequest();
            }
            catch (CardEaseXMLCommunicationException e)
            {
                // There is something wrong with communication
                Console.WriteLine(e.Message + System.Environment.NewLine + e.StackTrace);
                return 1;
            }
            catch (CardEaseXMLRequestException e)
            {
                // There is something wrong with the request
                Console.WriteLine(e.Message + System.Environment.NewLine + e.StackTrace);
                return 1;
            }
            catch (CardEaseXMLResponseException e)
            {
                // There is something wrong with the response
                Console.WriteLine(e.Message + System.Environment.NewLine + e.StackTrace);
                return 1;
            }

            // Get the response
            Response response = client.Response;
            Console.WriteLine(response);

            return 0;
        }
    }
}
