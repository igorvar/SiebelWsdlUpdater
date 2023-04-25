using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using SiebelWsdlUpdator;
using System.Data.SqlClient;
using System.Data;

namespace TestDbConnection
{
    class Program
    {

        //Console.WriteLine(
        //string.Join(",", (new string('?', _portsParametesCollection.Length)).ToCharArray());
        //);
        static void Main(string[] args)
        {
        
            string connectionString = "Dsn=SEAW enDEV;Uid=Siebel; Password=Siebel";
            //string connectionString = "Dsn=SEAW enDEV;Driver={Microsoft ODBC Driver for Oracle};Uid=siebel;Pwd=siebel";
            //string connectionString = "Dsn=SIEBELOUI_TST;Uid=Siebel; Password=Siebel";
            
            string sql = @"
select 
tPort.NAME,
tWF.VERSION,
tWf.STATUS_CD,
        tProp.NAME Name,
        tProp.DATA_TYPE_CD DataType,
        DATE_VAL || NUM_VAL || CHAR_VAL Default_Val, 
        tProp.COMMENTS COMMENTS
    from S_WS_PORT tPort
    join S_WS_PORT_TYPE tPortType on tPort.WS_PORT_TYPE_ID = tPortType.ROW_ID and tPortType.IMPL_TYPE_CD = 'WORKFLOW'
    join S_WFR_PROC tWf on tPortType.NAME = tWf.PROC_NAME and INACTIVE_FLG = 'N'--and tWf.STATUS_CD = 'COMPLETED'


    join S_WFA_DPLOY_DEF tWfDeployed on tWf.PROC_NAME = tWfDeployed.NAME and tWF.VERSION = tWfDeployed.REPOSITORY_VERSION and tWfDeployed.DEPLOY_STATUS_CD = 'ACTIVE'


    join S_REPOSITORY tRep on tWf.REPOSITORY_ID = tRep.ROW_ID and tRep.NAME = 'Siebel Repository'
    join S_WFR_PROC_PROP tProp on tProp.PROCESS_ID = tWf.ROW_ID and PROP_DEF_TYPE_CD in ('OUT','IN','INOUT')
    where tPort.NAME in (?)";

            //sql = @"select * from S_WS_PORT where NAME = 'MNR TestWs IWS'";

            
            using (OdbcConnection con = new OdbcConnection())
            {
                con.ConnectionString = connectionString;
                Console.WriteLine($"Connection status: {con.State}"); Console.ReadLine();
                //SqlCommand cmd = new SqlCommand(sql, con);
                OdbcCommand cmd = new OdbcCommand(sql, con);
                //Console.WriteLine(cmd.CommandText);
                //OdbcDataReader rdr = cmd.ExecuteReader();

                OdbcCommand c = new OdbcCommand();
                c.CommandText = sql;
                c.Parameters.Add("PortName", OdbcType.VarChar);
                c.Parameters.Add("PortName", OdbcType.VarChar);
                c.Parameters[0].Value = "MNR TestWs IWS";
                c.Parameters[1].Value = "FinancialAssetService";
                /*OdbcParameter p = new OdbcParameter();
                p.Direction = ParameterDirection.Input;
                p.ParameterName = ":1";
                p.Value = "MNR TestWs IWS";
                c.Parameters.Add(p);*/
                Console.WriteLine($"Parameter Added {c.Parameters[0]}");
                
                c.Connection = con;

                OdbcDataAdapter da = new OdbcDataAdapter(c);
                DataTable tbl = new System.Data.DataTable();
                
                con.Open();
                da.Fill(tbl);
                Console.WriteLine("DA filled");
                foreach (DataRow r in tbl.Rows)
                {
                    foreach (DataColumn f in tbl.Columns)
                    {
                        //if (f.ColumnName == "NAME")
                            Console.WriteLine($"{f.ColumnName} = {r[f]}");
                    }
                    
                }
                Console.ReadLine();


                /*if (!rdr.HasRows)
                {
                    Console.WriteLine("No records found");
                    return;
                }
                object[] values = new object[rdr.FieldCount];
                while (rdr.Read())
                {
                    
                    rdr.GetValues(values);
                    foreach (object item in values)
                        Console.Write($"{item}\t");
                    Console.WriteLine();
                }*/
            }
            Console.ReadLine();
        }
        static void Main2(string[] args)
        {
            System.Data.DataSet ds = new DataSet1();
            DataSet1 d = new DataSet1();
            //d.Load()
            DataSet1TableAdapters.DataTable1TableAdapter dta = new DataSet1TableAdapters.DataTable1TableAdapter();
            var s = dta.GetData();
            
            //Console.WriteLine("DSN");
            //string conStr = Console.ReadLine();
            ////OdbcConnector32 c = new OdbcConnector32(conStr, "siebel", "siebel");
            //Console.WriteLine(c.Connection.State);
            //Console.ReadLine();
        }
        static void Main1(string[] args)
        {
            string ConnectionString = "Dsn=SIEBELOUI_TST;Uid=Siebel; Password=Siebel";
            //ConnectionString = "Dsn=SIEBELOUI_TST;Uid=5555Siebel;password=444";
            //ConnectionString = "Server = SIEBELOUI_TST";
            //https://progi.pro/kak-podklyuchitsya-k-baze-dannih-oracle-8748776
            OdbcConnectionStringBuilder b = new OdbcConnectionStringBuilder();
            b.Dsn = "SIEBELOUI_TST";
            Console.WriteLine("DSN: ");
            string DsnName = Console.ReadLine();
            b.Dsn = DsnName;

            //b.Add("Driver", "$SIEBEL_HOME/lib/SEor825.so");
            //b.Add("Driver", "seor825.dll version 06.11.0086 (B0094, U0049)");
            //b.Add("Driver", "seor825.dll");
            //b.Add("Driver", "SEor825.so");
            //b.Add("Driver", "Oracle in OraClient11g_home1");
            //b.Add("Driver", "msdaora");
            b.Add("Driver", "Microsoft ODBC Driver for Oracle");
            //b.Add("Dbq", @"(DESCRIPTION=((ADDRESS=(PROTOCOL=TCP)(HOST=dbcc1-scan.il1.ocm.s1747013.oraclecloudatcustomer.com)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=SIEBELQASRV.il1.ocm.s1747013.oraclecloudatcustomer.com)(FAILOVER_MODE =(TYPEselect)(METHOD = basic))))");
            //b.Add("Dbq", @"(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=dbcc1-scan.il1.ocm.s1747013.oraclecloudatcustomer.com)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=SIEBELQASRV.il1.ocm.s1747013.oraclecloudatcustomer.com)(FAILOVER_MODE=(TYPE=select)(METHOD = basic))))");
            b.Add("Uid", "siebel");
            b.Add("Pwd", "siebel");
            //b.Add("DataSource", "SIEBELOUI_TST");
            ConnectionString = b.ConnectionString;
            OdbcConnection OdbcConn = new OdbcConnection(ConnectionString);
            //ConnectionString = "Driver={Microsoft ODBC Driver for Oracle};ConnectString=HOST=dbcc1-scan.il1.ocm.s1747013.oraclecloudatcustomer.com;Uid=SIEBEL;Pwd=SIEBEL;";
            try
            {

                OdbcConn.Open();
                Console.WriteLine(OdbcConn.State);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
            /*
https://docs.microsoft.com/ru-ru/dotnet/api/system.data.odbc.odbcconnection.connectionstring?view=dotnet-plat-ext-5.0
connection-string ::= empty-string[;] | attribute[;] | attribute; connection-string  
empty-string ::=  
attribute ::= attribute-keyword=attribute-value | DRIVER=[{]attribute-value[}]  
attribute-keyword ::= DSN | UID | PWD  
 | driver-defined-attribute-keyword  
attribute-value ::= character-string  
driver-defined-attribute-keyword ::= identifier 
            */
        }
    }
}
