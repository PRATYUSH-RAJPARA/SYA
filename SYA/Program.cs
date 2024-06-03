using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

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
            // Create an HttpListener to listen for requests on port 5002
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:5002/");
            listener.Start();
            Console.WriteLine("Listening for HTTP requests on port 5002...");

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
                        PrintRTGS objPrintRTGS = new PrintRTGS();
                        objPrintRTGS.PrintRTGS_API("27", "123");
                        MessageBox.Show("hi");
                        RichTextBox r = new RichTextBox();
                        Contact contact = new Contact();
                        contact.SortContactData(r, "all");
                        break;

                    case "/Reparing":
                        if (context.Request.HttpMethod == "POST")
                        {
                            // Read the incoming data
                            using (StreamReader reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
                            {
                                string requestBody = reader.ReadToEnd();
                                var data = System.Web.HttpUtility.ParseQueryString(requestBody);

                                // Create a list to store the values
                                List<string> reparingData = new List<string>();

                                // Define the expected keys
                                string[] keys = { "Name", "Number", "Weight", "Cost", "Comment" };

                                // Loop through each key and add the corresponding value to the list
                                foreach (var key in keys)
                                {
                                    string value = data[key] ?? "N/A"; // Default value if the key is not found
                                    reparingData.Add(value);
                                }

                                // Handle the variables as needed
                                Console.WriteLine($"Received variables: {string.Join(", ", reparingData)}");

                                // Display the values for debugging
                                MessageBox.Show(string.Join(", ", reparingData));

                                // Process the data
                                reparing reparing = new reparing();
                                reparing.printReparingTag(reparingData);

                                // Create a response
                                //context.Response.StatusCode = (int)HttpStatusCode.OK;
                                //string responseString = "Variables received and processed";
                                //byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                                //context.Response.ContentLength64 = buffer.Length;
                                //context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                            }
                        }
                        break;

                    default:
                        // Handle other endpoints if needed
                        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                }

                context.Response.OutputStream.Close();
            }
        }
    }
}
