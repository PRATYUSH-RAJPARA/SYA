using SYA.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace SYA
{
    public class ContactHelper
    {
       public DataTable getDataTable(string tableName)
        {
            DataTable dt = new DataTable();
            dt=helper.FetchDataTableFromSYAContactDataBase("select * from "+tableName);
            return dt;
        }
    }
}
