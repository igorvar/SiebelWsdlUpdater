using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;

namespace SiebelWsdlUpdator
{

    public class OdbcConnector32
    {
        public OdbcConnector32()
        {
            
        }
        public void OdbcConnector3211(string DsnName, string UserName, string Password)
        {
            OdbcConnectionStringBuilder b = new OdbcConnectionStringBuilder();
            //b.Dsn = DsnName;
            b.Add("Uid", UserName);
            b.Add("Pwd", Password);
            b.Add("Driver" , "System.Data.OracleClient");
            b.Add("Data Source", "SIEBELOUI_TST");
            
            bool isStop = false;
            for (int i = 32000 * 2; i < 64000 +1 && !isStop; i++)
            {
                try
                {
                    //b.Add("PORT", i);
                    //{Driver={System.Data.OracleClient};User ID=siebel;Password=siebel;Unicode=False}
                    this.Connection = new OdbcConnection(b.ConnectionString);
                    //this.Connection = new OdbcConnection("Data Source=SIEBELOUI_TST;User ID=siebel;Password=siebel;Unicode=False");
                    //this.Connection = new OdbcConnection("Driver={System.Data.OracleClient};Data Source=SIEBELOUI_TST;User ID=siebel;Password=siebel;Unicode=False");

                    Connection.Open();
                    isStop = true;
                    Console.WriteLine($"=========================================== port {i} is ok =====================================================");
                }
                catch (OdbcException ex)
                {

                    bool isEr12545 = false;
                    foreach(OdbcError er in ex.Errors)
                    {
                        if (er.NativeError == 12545)
                        {
                            Console.WriteLine($"{DateTime.Now.ToString("HH:mm:ss")}. {i} - error {er.Message}");
                            isEr12545 = true;
                            break;
                        }
                    }
                    if (!isEr12545)
                    {
                        Console.WriteLine(ex.Message);
                        isStop = true;
                    }
                    
                }

            }
        }
        public OdbcConnection Connection { get; private set; }
       
    }
}
