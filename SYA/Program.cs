using System;
using System.Net;
using System.Threading;
namespace SYA
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Start a new thread to handle HTTP requests
            Thread thread = new Thread(HandleHttpRequests);
            thread.Start();
            ApplicationConfiguration.Initialize();
            Application.Run(new main());
        }
        static void HandleHttpRequests()
        {
            // Create an HttpListener to listen for requests on port 5001
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:5001/");
            listener.Start();
            Console.WriteLine("Listening for HTTP requests on port 5001...");
            // Handle incoming requests
            while (true)
            {
                // Accept an incoming request
                HttpListenerContext context = listener.GetContext();
                // Handle the request based on the URL path
                string urlPath = context.Request.Url.AbsolutePath;
                switch (urlPath)
                {
                    case "/SortContacts":
                        //PrintRTGS objPrintRTGS = new PrintRTGS();
                        //objPrintRTGS.PrintRTGS_API("27", "123");
                        //Contact.showMsg();
                        MessageBox.Show("hi");
                        RichTextBox r = new RichTextBox();
                        Contact contact = new Contact();
                        contact.SortContactData(r, "all");
                        break;
                    default:
                        // Handle other endpoints if needed
                        break;
                }
            }
        }
    }
}
