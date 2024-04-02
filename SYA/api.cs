using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web.Http;

namespace SYA
{
    public class api : ApiController
    {
        [HttpGet]
        [Route("execute_function")]
        public IHttpActionResult ExecuteFunction()
        {
            try
            {
                // Call the function you want to execute
               // Contact.showMsg();
                PrintRTGS objPrintRTGS = new PrintRTGS();
                objPrintRTGS.PrintRTGS_API("27", "123");
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
