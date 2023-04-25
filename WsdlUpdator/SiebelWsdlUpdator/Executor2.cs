//using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using System.Xml;
//using System.Data.OracleClient;
//using Microsoft.Data.Odbc;
using System.Data.Odbc;
using System.Data;
using System.Text.RegularExpressions;


namespace SiebelWsdlUpdator
{
    enum WfPropDirection {IN, OUT}
    enum MapperFields{WfName, PropName, Comments, DataType, WsPropName, IsRequired, Sequence, Error}
    static class Executor2
    {
        static Port[] _portsParametesCollection = null;
        
        #region publicProperies
        public static int PortsCount
        { get
            {
                if (_portsParametesCollection == null) return 0;
                return _portsParametesCollection.Length;
            }
        }

        public static string[] PortsNames
        {
            get
            {
                if (PortsCount == 0) return null;
                string[] portNames = new string[PortsCount];
                for (int i = 0; i < portNames.Length; i++)
                    portNames[i] = _portsParametesCollection[i].Name;
                return portNames;
            }

        }

        static private XmlDocument _wsdl;
        static public XmlDocument Wsdl
        {
            //static private XmlDocument _wsdl;
            get { return _wsdl; }
            set
            {
                _portsParametesCollection = CreatePortData(value);
                _wsdl = value; 
            }
        }
        #endregion

        private static DataTable _MappingTepmlate;
        static Executor2()
        {
            _MappingTepmlate = new DataTable();
            _MappingTepmlate.Columns.Add(MapperFields.WfName.ToString()/*"WfName"*/);
            _MappingTepmlate.Columns.Add(MapperFields.PropName.ToString()/*"PropName"*/);
            _MappingTepmlate.Columns.Add(MapperFields.Comments.ToString()/*"Comments"*/);
            _MappingTepmlate.Columns.Add(MapperFields.DataType.ToString()/*"DataType"*/);
            _MappingTepmlate.Columns.Add(MapperFields.WsPropName.ToString()/*"WsPropName"*/);
            _MappingTepmlate.Columns.Add(MapperFields.IsRequired.ToString()/*"IsRequired"*/, System.Type.GetType("System.Boolean"));
            _MappingTepmlate.Columns.Add(MapperFields.Sequence.ToString()/*"Sequence"*/, System.Type.GetType("System.Int32"));
            _MappingTepmlate.Columns.Add(MapperFields.Error.ToString()/*"Error"*/);
        }

        public static Dictionary<string, string> Execute(string ConnectionString)
        {
            //DbConnection.Open();

            OdbcParameter[] paramsCollection = new OdbcParameter[PortsCount];
            DataTable tblAllParams = new DataTable();
            string sql = @"
            select 
                tPort.NAME              as  PORT_NAME,
                --tWF.PROC_NAME         as  WF_NAME, /*Same as PORT_NAME*/
                --tWF.VERSION           as  WF_VERSION,
                --tWf.STATUS_CD         as  WF_STATUS,
                tProp.NAME              as  PROP_NAME,
                tProp.PROP_DEF_TYPE_CD  as  DIRECTION,
                tProp.DATA_TYPE_CD      as  DATA_TYPE,
                --DATE_VAL || NUM_VAL || CHAR_VAL as DEFAULT_VAL, 
                tProp.COMMENTS          as  COMMENTS
            from S_WS_PORT tPort
            join S_WS_PORT_TYPE tPortType on tPort.WS_PORT_TYPE_ID = tPortType.ROW_ID and tPortType.IMPL_TYPE_CD = 'WORKFLOW'
            join S_WFR_PROC tWf on tPortType.NAME = tWf.PROC_NAME and INACTIVE_FLG = 'N'--and tWf.STATUS_CD = 'COMPLETED'

            join S_WFA_DPLOY_DEF tWfDeployed on tWf.PROC_NAME = tWfDeployed.NAME and tWF.VERSION = tWfDeployed.REPOSITORY_VERSION and tWfDeployed.DEPLOY_STATUS_CD = 'ACTIVE'

            join S_REPOSITORY tRep on tWf.REPOSITORY_ID = tRep.ROW_ID and tRep.NAME = 'Siebel Repository'
            join S_WFR_PROC_PROP tProp on tProp.PROCESS_ID = tWf.ROW_ID and PROP_DEF_TYPE_CD in ('OUT','IN','INOUT')
            where tPort.NAME in (" + string.Join(",", (new string('?', _portsParametesCollection.Length)).ToCharArray()) /*string of ?,?,?,...?*/+ ")";


            using (OdbcConnection connection = new OdbcConnection(ConnectionString))
            {
                OdbcCommand cmd = new OdbcCommand(sql, connection);
                OdbcCommand command = new OdbcCommand(sql, connection);
                OdbcDataAdapter da = new OdbcDataAdapter(command);
                foreach (Port p in _portsParametesCollection)
                    command.Parameters.Add(new OdbcParameter(name: p.Name, value: p.WfName)); // name of port is not relevant
                try
                {
                    connection.Open();
                }
                catch (System.Exception ex)
                {
                    //System.Windows.Forms.MessageBox.Show(ex.Message);
                    Dictionary<string, string> res = new Dictionary<string, string>(1);
                    res.Add("Impossible to open DB connection", $"ConnectionString: {ConnectionString}\n{ex.Message}");
                    return res;
                }

                da.Fill(tblAllParams);
                connection.Close();

            }

            foreach (Port p in _portsParametesCollection)
            {
                p.MappingIn = InitTable(tblAllParams, p.WfName, WfPropDirection.IN/* "IN"*/);
                p.MappingOut = InitTable(tblAllParams, p.WfName, WfPropDirection.OUT/*"OUT"*/);
                if (p.MappingIn.Rows.Count + p.MappingOut.Rows.Count == 0)
                {
                    Dictionary<string, string> res = new Dictionary<string, string>(1);
                    res.Add("Is WF on server", $"Not found input and output params for port {p.Name}, based on WF {p.WfName}.\nIs published workflow version checked out?");
                    return res;
                }
                /////////////////////////////////////////

                /*Dictionary<string, string> res1 = new Dictionary<string, string>();
                res1.Add(p.Name + " IN", p.MappingIn == null ? "NULL" : p.MappingIn.Rows.Count.ToString());
                res1.Add(p.Name + " OUT", p.MappingOut == null ? "NULL" : p.MappingOut.Rows.Count.ToString());
                return res1;*/

            /////////////////////////////////////////
            }

            return Execute();
        }
        public static Dictionary<string, string> Execute(XmlDocument[] WfXmlFile/*, ref XmlDocument WsdlFile*/)
        {
            //return null;

            for (int i = 0; i < _portsParametesCollection.Length; i++)
            {
                _portsParametesCollection[i].MappingIn = InitTable(WfXmlFile[i], WfPropDirection.IN/* "IN"*/);
                _portsParametesCollection[i].MappingOut = InitTable(WfXmlFile[i], WfPropDirection.OUT/* "OUT"*/);
            }
            return Execute();
        }
        private static Dictionary<string, string> Execute()
        {
            foreach (Port p in _portsParametesCollection) 
            {
                Functions.UpdateDataMap(p.MappingIn);
                Functions.UpdateDataMap(p.MappingOut);
            }
            foreach (Port p in _portsParametesCollection)
            {
                //Functions.UpdateWsParams(p.InputWsParametersAttributes, p.MappingIn);
                //Functions.UpdateWsParams(p.OutputWsParametersAttributes, p.MappingOut);
                Functions.SetType(p.InputWsParametersAttributes, p.MappingIn);
                Functions.MarkAsOptional(p.InputWsParametersAttributes, p.MappingIn);

                Functions.SetType(p.OutputWsParametersAttributes, p.MappingOut);
                //Functions.MarkAsOptional(p.OutputWsParametersAttributes, p.MappingOut);
            }

            foreach (Port p in _portsParametesCollection)
            {

                // if no presents input parametrs without Ws_Sequence - order of input parameter is not relevant
                if (p.MappingIn.Select("[Sequence] is null and (DataType = 'VARCHAR' or DataType = 'NUMBER' or DataType = 'DATE' or DataType = 'ALIAS' or DataType = 'BINARY')").Length > 0)
                    Functions.MakeOrderIndependent(p.InputWsParametersAttributes);
                else
                    Functions.OrderWsParams(p.InputWsParametersAttributes, p.MappingIn);

                // order of scalar output params is not relevant
                Functions.MakeOrderIndependent(p.OutputWsParametersAttributes);
            }

            Dictionary<string, string> errors = new Dictionary<string, string>();
            string propName;
            foreach (Port p in _portsParametesCollection)
            {
                foreach (DataRow r in p.MappingIn.Rows)
                {
                    propName = (_portsParametesCollection.Length > 1 ? r[MapperFields.WfName.ToString()/*"WfName"*/].ToString() + ". " : "") + "Input. " + r[MapperFields.WsPropName.ToString()/*"WsPropName"*/].ToString();
                    if (!string.IsNullOrEmpty(r[MapperFields.Error.ToString()/*"Error"*/].ToString()))
                        errors.Add(propName, r[MapperFields.Error.ToString()/*"Error"*/].ToString());
                }
                foreach (DataRow r in p.MappingOut.Rows)
                {
                    propName = (_portsParametesCollection.Length > 1 ? r[MapperFields.WfName.ToString()/*"WfName"*/].ToString() + ". " : "") + "Output. " + r[MapperFields.WsPropName.ToString()/*"WsPropName"*/].ToString();
                    if (!string.IsNullOrEmpty(r[MapperFields.Error.ToString()/*"Error"*/].ToString()))
                        errors.Add(propName, r[MapperFields.Error.ToString()/*"Error"*/].ToString());
                }
            }

            if (errors.Count == 0) return null; else return errors;
        }

        /// <summary>
        /// From WF Xml Document create mapping for input or output and inOut WF props
        /// </summary>
        /// <param name="WfXml">Xml based on exported WF file</param>
        /// <param name="InOut">Direction of WF parameter</param>
        /// <returns>Table based on _MappingTepmlate</returns>
        private static DataTable InitTable(XmlDocument WfXml, /*string*/ WfPropDirection InOut/*IN, OUT*/)
        {
            string xPath = $"/SiebelMessage/ListOfRepositoryWorkflowProcess/RepositoryWorkflowProcess/ListOfRepositoryWfProcessProp/RepositoryWfProcessProp[InOut='{InOut}' or InOut = 'INOUT']/Name2";
            //string xPath = $"/SiebelMessage/ListOfRepositoryWorkflowProcess/RepositoryWorkflowProcess/ListOfRepositoryWfProcessProp/RepositoryWfProcessProp[InOut='{InOut}' or InOut = 'INOUT'][DataType = 'VARCHAR' or DataType = 'NUMBER' or DataType = 'DATE' or DataType = 'ALIAS' or DataType = 'BINARY']/Name2";
            DataTable t = _MappingTepmlate.Clone();
            DataRow r;
            //XmlNodeList wfProps = ReadWfXml(WfXml, InOut /*IN, OUT*/);
            XmlNodeList wfProps = WfXml.SelectNodes(xPath);

            string wfName = WfXml.SelectSingleNode(@"/SiebelMessage/ListOfRepositoryWorkflowProcess/RepositoryWorkflowProcess/Name").InnerText;
            foreach (XmlElement p in wfProps)
            {
                r = t.NewRow();
                r.SetField(MapperFields.WfName.ToString()/*"WfName"*/, wfName);
                r.SetField(MapperFields.PropName.ToString()/*"PropName"*/, p.InnerText);
                r.SetField(MapperFields.Comments.ToString()/*"Comments"*/, p.ParentNode.SelectSingleNode("Comments").InnerText);
                r.SetField(MapperFields.DataType.ToString()/*"DataType"*/, p.ParentNode.SelectSingleNode("DataType").InnerText);
                t.Rows.Add(r);
            }
            return t;
        }
        private static DataTable InitTable(DataTable AllParameters, string PortName, /*string*/ WfPropDirection InOut/*IN, OUT*/)
        {
            DataTable mappingTable = _MappingTepmlate.Clone();
            DataRow mappingRow;
            foreach (DataRow paramOfPort in AllParameters.Select($"[PORT_NAME] = '{PortName}' and ([DIRECTION] = '{InOut}' or [DIRECTION] = 'INOUT')"))
            {
                mappingRow = mappingTable.NewRow();
                mappingRow.SetField(MapperFields.WfName.ToString()/*"WfName"*/,   paramOfPort["PORT_NAME"].ToString());
                mappingRow.SetField(MapperFields.PropName.ToString()/*"PropName"*/, paramOfPort["PROP_NAME"].ToString());
                mappingRow.SetField(MapperFields.Comments.ToString()/*"Comments"*/, paramOfPort["COMMENTS"].ToString());
                mappingRow.SetField(MapperFields.DataType.ToString()/*"DataType"*/, paramOfPort["DATA_TYPE"].ToString());
                mappingTable.Rows.Add(mappingRow);
            }
            return mappingTable;
        }
        /// <summary>
        /// Return collections of Ports in WSDL. Each port contain list of XmlAttributes @name of each scalar parameter for input parametrs and for ouput parametrs
        /// </summary>
        /// <param name="Wsdl"></param>
        /// <returns></returns>
        private static Port[] CreatePortData(XmlDocument Wsdl)
        {
            Port[] ports = null;
            XmlNamespaceManager NmSpMngr = new XmlNamespaceManager(Wsdl.NameTable);
            NmSpMngr.AddNamespace("xsd", "http://www.w3.org/2001/XMLSchema");
            NmSpMngr.AddNamespace("defSpace", "http://schemas.xmlsoap.org/wsdl/");

            XmlNodeList portsNameAttr = Wsdl.DocumentElement.SelectNodes("/defSpace:definitions/defSpace:portType/@name", NmSpMngr);
            ports = new Port[portsNameAttr.Count];
            string xPath;
            string messageName;
            string portName;
            //foreach (XmlNode port in doc.DocumentElement.SelectNodes("/defSpace:definitions/defSpace:portType/@name", NmSpMngr))
            for (int i = 0; i < ports.Length; i++)
            {
                portName = portsNameAttr[i].Value;
                ports[i] = new Port(portName);

                xPath = $"/defSpace:definitions/defSpace:portType[@name='{portName}']/defSpace:operation/defSpace:input/@message";
                messageName = Wsdl.SelectSingleNode(xPath, NmSpMngr).Value;
                messageName = messageName.Substring(messageName.IndexOf(':') + 1);
                xPath = $"/defSpace:definitions/defSpace:types/xsd:schema/xsd:element[@name='{messageName}']/xsd:complexType/xsd:sequence/xsd:element/@name";
                ports[i].InputWsParametersAttributes = Wsdl.SelectNodes(xPath, NmSpMngr);

                xPath = $"/defSpace:definitions/defSpace:portType[@name='{portName}']/defSpace:operation/defSpace:output/@message";
                messageName = Wsdl.SelectSingleNode(xPath, NmSpMngr).Value;
                messageName = messageName.Substring(messageName.IndexOf(':') + 1);
                xPath = $"/defSpace:definitions/defSpace:types/xsd:schema/xsd:element[@name='{messageName}']/xsd:complexType/xsd:sequence/xsd:element/@name";
                ports[i].OutputWsParametersAttributes = Wsdl.SelectNodes(xPath, NmSpMngr);
            }
            return ports;
        }
    }
}

class Port
{
    public Port(string Name)
    {
        this.WfName = Name.Replace("_spc", " ");
        this.Name = Name;
    }
    public string Name { get; private set; }
    public string WfName { get; private set; }
    public XmlNodeList InputWsParametersAttributes { get; set; }
    public XmlNodeList OutputWsParametersAttributes { get; set; }
    public DataTable MappingIn { get; set; }
    public DataTable MappingOut { get; set; }
}
