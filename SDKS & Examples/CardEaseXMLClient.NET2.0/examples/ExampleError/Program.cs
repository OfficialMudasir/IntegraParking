using System;
#if NET11
using System.Collections;
#else
using System.Collections.Generic;
using System.Collections.ObjectModel;
#endif
using CardEaseXML;

namespace ExampleError
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
            request.RequestType = RequestType.Auth;
            request.Amount = "123";
            request.Track2 = ";this is a bad track2;";

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

#if NET11
			IList errors = response.Errors;
#else
			Collection<Error> errors = response.Errors;
#endif
            if (errors != null)
            {
                foreach (Error error in errors)
                {
                    Console.WriteLine(error);
                }
            }

            return 0;
        }
    }
}
