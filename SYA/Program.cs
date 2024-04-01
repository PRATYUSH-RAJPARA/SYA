using System.Net;

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
                // Handle the request here by calling Contact.showMsg() or any other function
                PrintRTGS objPrintRTGS = new PrintRTGS();
                objPrintRTGS.ContactAPI("27", "123");

                Contact.showMsg(); // Call the function you want to execute
            }
        }
    }
}
