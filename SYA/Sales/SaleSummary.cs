using System;
using System.IO;
using System.Net;
using System.Windows.Forms;
using RestSharp;
using System.Text.Json.Nodes;

namespace SYA.Stocks
{
    public partial class SaleSummary : Form
    {
        public SaleSummary()
        {
            InitializeComponent();
        }

        private void SaleSummary_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ConvertPdfToExcel("https://api.pspdfkit.com/build", "C:\\Users\\SYA\\Downloads\\report.pdf", "C:\\Users\\SYA\\Downloads\\result.xlsx");
        }

        public static void ConvertPdfToExcel(string apiUrl, string inputPdfPath, string outputExcelPath)
        {
            try
            {
                var client = new RestClient(apiUrl);

                var request = new RestRequest();
                request.Method = Method.Post;
                request.AddHeader("Authorization", "Bearer pdf_live_guovuWJqM7SrM93YySSqN5HMlfnkLSMqyaOBasRIOp1")
                       .AddFile("file", inputPdfPath)
                       .AddParameter("instructions", new JsonObject
                       {
                           ["parts"] = new JsonArray
                           {
                               new JsonObject
                               {
                                   ["file"] = "file"
                               }
                           },
                           ["output"] = new JsonObject
                           {
                               ["type"] = "xlsx"
                           }
                       }.ToString());

                var response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using var responseStream = new MemoryStream(response.RawBytes);
                    using var outputFileWriter = File.OpenWrite(outputExcelPath);
                    responseStream.CopyTo(outputFileWriter);
                    MessageBox.Show("File converted and saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    using var responseStreamReader = new StreamReader(new MemoryStream(response.RawBytes));
                    var responseContent = responseStreamReader.ReadToEnd();
                    MessageBox.Show("Error: " + responseContent, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (response.ErrorException != null)
                {
                    throw new ApplicationException("Error converting PDF to Excel", response.ErrorException);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception: " + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
