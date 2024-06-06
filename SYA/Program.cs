using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SYA
{
    internal static class Program
    {
        private static HttpListener listener;
        private static Thread httpThread;
        private static CancellationTokenSource cts;

        [STAThread]
        static void Main()
        {
            // Initialize the cancellation token source
            cts = new CancellationTokenSource();

            // Start a new thread to handle HTTP requests
            httpThread = new Thread(() => HandleHttpRequests(cts.Token));
            httpThread.Start();
            Application.ApplicationExit += new EventHandler(OnApplicationExit);

            ApplicationConfiguration.Initialize();
            Application.Run(new main());
        }

        static void HandleHttpRequests(CancellationToken token)
        {
            listener = new HttpListener();
            try
            {
                listener.Prefixes.Add("http://localhost:5002/");
                listener.Start();
                Console.WriteLine("Listening for HTTP requests on port 5002...");

                // Handle incoming requests
                while (listener.IsListening && !token.IsCancellationRequested)
                {
                    var contextTask = listener.GetContextAsync();
                    contextTask.Wait(token); // Wait for a request, respecting the cancellation token

                    if (token.IsCancellationRequested)
                        break;

                    HttpListenerContext context = contextTask.Result;
                    Task.Run(() => ProcessRequest(context));
                }
            }
            catch (HttpListenerException ex)
            {
                Console.WriteLine($"HttpListenerException: {ex.Message}");
                MessageBox.Show($"Error: {ex.Message}");
            }
            catch (OperationCanceledException)
            {
                // Graceful shutdown
                Console.WriteLine("Operation was canceled.");
            }
            finally
            {
                listener?.Close();
            }
        }

        static void ProcessRequest(HttpListenerContext context)
        {
            try
            {
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
                                context.Response.StatusCode = (int)HttpStatusCode.OK;
                                string responseString = "Variables received and processed";
                                byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                                context.Response.ContentLength64 = buffer.Length;
                                context.Response.OutputStream.Write(buffer, 0, buffer.Length);
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
            catch (Exception ex)
            {
                // Log and handle the exception as needed
                Console.WriteLine($"Error processing request: {ex.Message}");
            }
        }

        private static void OnApplicationExit(object sender, EventArgs e)
        {
            // Signal the cancellation token
            cts.Cancel();

            // Ensure the listener is stopped when the application exits
            listener?.Stop();
            httpThread?.Join(); // Wait for the HTTP thread to finish
        }
    }
}
