using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            OnStart(args);
        }


        static void OnStart(string[] args)
        {
            // Initialize the service with the user configuration data

            InvoiceGenerator invGenerator = new InvoiceGenerator();

            invGenerator.Main();
        }


    }
}
