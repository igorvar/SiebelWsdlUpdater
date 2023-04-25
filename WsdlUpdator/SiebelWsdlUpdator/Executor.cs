using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
//using System.Data.OracleClient;
//using Microsoft.Data.Odbc;
using System.Data.Odbc;
using System.Data.OracleClient;
using System.Data;
using System.Text.RegularExpressions;
using System.IO;

namespace SiebelWsdlUpdator
{
    /*
select  tProp.NAME Name, 
        tProp.DATA_TYPE_CD DataType, 
        DATE_VAL || NUM_VAL || CHAR_VAL Default_Val, 
        tProp.COMMENTS COMMENTS
    from S_WS_PORT tPort
    join S_WS_PORT_TYPE tPortType on tPort.WS_PORT_TYPE_ID = tPortType.ROW_ID and tPortType.IMPL_TYPE_CD = 'WORKFLOW'
    join S_WFR_PROC tWf on tPortType.NAME = tWf.PROC_NAME and INACTIVE_FLG = 'N' and tWf.STATUS_CD = 'COMPLETED'
    join S_REPOSITORY tRep on tWf.REPOSITORY_ID = tRep.ROW_ID and tRep.NAME = 'Siebel Repository'
    join S_WFR_PROC_PROP tProp on tProp.PROCESS_ID = tWf.ROW_ID and PROP_DEF_TYPE_CD in ('OUT','IN','INOUT')
    where tPort.NAME = 'MNR Update SR Process WS' 
    
        */


    static class Executor
    {
        public static Dictionary<string, string> Execute(XmlDocument WfXmlFile, ref XmlDocument WsdlFile  )
        {

            
            //string newWsdl = Regex.Replace(WsdlFile, "(.wsdl)$", ".Updated$1", RegexOptions.IgnoreCase); //WsdlFile.Replace()


            DataTable mapping = InitTable(WfXmlFile);
            Functions.UpdateDataMap(mapping);
            XmlNodeList wsParams = ReadWsdl(WsdlFile);
            Functions.UpdateWsParams(wsParams, mapping);
 
            Functions.MakeOrderIndependent(wsParams);

/*            XmlWriter wrtr;
            wrtr = XmlWriter.Create(newWsdl);
            wsParams[0].OwnerDocument.WriteTo(wrtr);
            wrtr.Close();
            */

            Dictionary<string, string> errors = new Dictionary<string, string>();
            foreach (DataRow r in mapping.Rows)
                if ( ! string.IsNullOrEmpty(r["Comments"].ToString()))
                    errors.Add(r["WsPropName"].ToString(), r["Comments"].ToString());
            if (errors.Count == 0) return null; else return errors;
        }

        //removed to Functions 
        ///// <summary>
        ///// Remove element 'sequence', parent of XmlNodeList. properties and copy all properties to new element 'all'
        ///// </summary>
        ///// <param name="wsParamsNames"></param>
        //public static void MakeOrderIndependent(XmlNodeList wsParamsNames)
        //{
        //    XmlElement prop;
        //    XmlElement sequence = null;
        //    XmlElement all = null;
        //    foreach (XmlNode paramNameAtt in wsParamsNames)
        //    {
        //        //if (paramNameAtt.Value != r["WsPropName"].ToString()) continue;
        //        prop = ((XmlAttribute)paramNameAtt).OwnerElement;
        //        if (prop.ParentNode.LocalName != "sequence") continue;
        //        sequence = (XmlElement)prop.ParentNode;
        //        if (sequence.ParentNode == null) continue; // all nodes under sequence was moved to all and sequence was removed
        //        all = prop.OwnerDocument.CreateElement(sequence.Prefix, "all", sequence.NamespaceURI);
        //        foreach (XmlNode n in sequence.ChildNodes)
        //            all.AppendChild(n.CloneNode(true));
        //        sequence.ParentNode.InsertAfter(all, sequence);
        //        sequence.ParentNode.RemoveChild(sequence);
        //    }
        //}

            //Removed to Functions
        //public static void OrderWsParams(XmlNodeList wsParamsNames, DataTable mapping)
        //{
        //    //DataRow[] orderedRows = mapping.Select("Sequence is not null", "Sequence desc");
        //    //string[] propsNamesByOrder = new string[orderedRows.Length];
        //    //int i = 0;
        //    XmlElement wsPropertyNew = null;
        //    XmlElement wsPropertyOld = null;
        //    XmlElement sequence = null;
        //    foreach (DataRow r in mapping.Select("Sequence is not null", "Sequence asc"))
        //    {
        //        foreach (XmlNode paramNameAtt in wsParamsNames)
        //        {
        //            if (paramNameAtt.Value != r["WsPropName"].ToString()) continue;
        //            wsPropertyOld = ((XmlAttribute)paramNameAtt).OwnerElement;
        //            if (sequence == null) sequence = (XmlElement)wsPropertyOld.ParentNode;
        //            wsPropertyNew = (XmlElement)wsPropertyOld.CloneNode(true);
        //            /*wsPropertyOld.ParentNode*/sequence.AppendChild(wsPropertyNew);
        //            /*wsPropertyOld.ParentNode*/sequence.RemoveChild(wsPropertyOld);
        //        }
        //    }
        //    wsParamsNames = sequence.SelectNodes(@"*/@name");
        //}
        ///// <summary>
        ///// Update each parameter in wsdl: Set type for Date, number, define it as option based on mapping table
        ///// </summary>
        ///// <param name="wsParamNames">collection of xmlAttribues. Each attribute - name of input or output WS prop</param>
        ///// <param name="mapping">Table based on WF definition</param>
        //public static void UpdateWsParams(XmlNodeList wsParamNames, DataTable mapping)
        //{
        //    DataRow[] selected;
        //    DataRow r;
        //    foreach (XmlAttribute att in wsParamNames)
        //    {
                
        //        selected = mapping.Select($"WsPropName = '{att.Value}'");
        //        if (selected.Length == 0) continue;
        //        r = selected[0];

        //        SetType(att, r["DataType"].ToString());

        //        switch (r["IsRequired"].ToString())
        //        {
        //            case "False":
        //                MarkAsOptional(att);
        //                break;
        //            case "True":
        //                //MarkAsRequired(att, r["DataType"].ToString());
        //                //nothing to do
        //                break;
        //            default:
        //                r["Error"] += string.IsNullOrEmpty(r["Error"].ToString()) ? "" : ". " + $"Required or optional definition of Parameter '{att.Value}' not changed, because not found definition Ws_Required in WF"; 
        //                break;
        //        }
        //    }
        //}

        //removed to Functions
        //private static void MarkAsOptional(XmlAttribute prop)
        //{
        //    XmlAttribute minOcc = prop.OwnerDocument.CreateAttribute("minOccurs");
        //    minOcc.Value = "0";
        //    prop.OwnerElement.Attributes.Append(minOcc);
        //}

        //Removed to Functions
        //private static void SetType(XmlAttribute WsParamName, string DataType /*VARCHAR, NUMBER, DATE*/)
        //{
        //    XmlElement owner = WsParamName.OwnerElement;
        //    XmlComment comment = null;  
        //    XmlElement simpleType = null;
        //    XmlElement simpleType_restriction = null;
        //    XmlElement simpleType_restriction_pattern1 = null;
        //    XmlElement simpleType_restriction_pattern2 = null;
        //    XmlAttribute restrictionBase = null;
        //    XmlAttribute simpleType_restriction_pattern1Value = null;
        //    switch (DataType)
        //    {
        //        //VARCHAR,NUMBER, DATE, ALIAS and BINARY
        //        case "VARCHAR":
        //        case "ALIAS":
        //        case "BINARY":
        //            /*nothing to do. In siebel it always string*/
        //            break;
        //        case "NUMBER":
        //            owner.Attributes["type"].Value = "xsd:decimal";
        //            break;
        //        case "DATE":
        //            /*
        //            <xsd:element name="Date" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
        //                <xsd:simpleType>
        //                    <xsd:restriction base="xsd:string" >
        //                        <xsd:pattern value = "(0[1-9])|10|11|12\/(([0-2]\d)|30|31)\/\d\d\d\d(\s[0-2]\d:[0-5]\d:[0-5]\d)?"/>
        //                    </xsd:restriction>
        //                </xsd:simpleType>
        //            </xsd:element>*/
        //            owner.RemoveAttribute("type");
        //            simpleType = WsParamName.OwnerDocument.CreateElement(owner.Prefix, "simpleType", owner.NamespaceURI);
        //            {
        //                simpleType_restriction = WsParamName.OwnerDocument.CreateElement(owner.Prefix, "restriction", owner.NamespaceURI);
        //                restrictionBase = WsParamName.OwnerDocument.CreateAttribute("base");
        //                restrictionBase.Value = "xsd:string";
        //                simpleType_restriction.Attributes.Append(restrictionBase);
        //            }
        //            {
        //                simpleType_restriction_pattern1 = WsParamName.OwnerDocument.CreateElement(owner.Prefix, "pattern", owner.NamespaceURI);
        //                simpleType_restriction_pattern1Value = WsParamName.OwnerDocument.CreateAttribute("value");
        //                simpleType_restriction_pattern1Value.Value = @"((0[1-9])|10|11|12)\/(([0-2]\d)|30|31)\/\d\d\d\d(\s[0-2]\d:[0-5]\d:[0-5]\d)?";
        //                simpleType_restriction_pattern1.Attributes.Append(simpleType_restriction_pattern1Value);
        //            }
        //            simpleType_restriction_pattern2 = (XmlElement)simpleType_restriction_pattern1.Clone();
        //            simpleType_restriction_pattern2.Attributes[0].Value = "";

        //            comment = WsParamName.OwnerDocument.CreateComment("Date in format MM/DD/YYYY or DateTime in format MM/DD/YYYY hh:mm:ss");
        //            owner.AppendChild(comment);
        //            simpleType_restriction.AppendChild(simpleType_restriction_pattern1);
        //            simpleType_restriction.AppendChild(simpleType_restriction_pattern2);
        //            simpleType.AppendChild(simpleType_restriction);
        //            owner.AppendChild(simpleType);
        //            break;

        //    }
        //}
        /// <summary>
        /// Deprecated. Not in use
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="DataType"></param>
        private static void MarkAsRequired(XmlAttribute prop, string DataType /*VARCHAR, NUMBER, DATE*/)
        {
          
            XmlElement owner = prop.OwnerElement;
            //owner.RemoveAttribute(owner.Attributes["type"]);
            owner.RemoveAttribute("type");
            XmlElement simpleType = prop.OwnerDocument.CreateElement(owner.Prefix, "simpleType", owner.NamespaceURI);
            XmlElement restriction = prop.OwnerDocument.CreateElement(owner.Prefix, "restriction", owner.NamespaceURI);
            XmlComment ownerComment = prop.OwnerDocument.CreateComment("");
            XmlAttribute minLength_value = null;
            XmlAttribute restriction_base = null;
            XmlElement restriction_pattern = null;
            XmlAttribute restrictionPatert_value = null;

            XmlElement rest_minLength = null;// prop.OwnerDocument.CreateElement(owner.Prefix, "minLength", owner.NamespaceURI);

            switch (DataType)
            {
                case "VARCHAR":
                    /*
                    <xsd:element name="Text" >
                      <xsd:simpleType>
                        <xsd:restriction base="xsd:string"  >
                          <xsd:minLength value="1"/>
                        </xsd:restriction>
                      </xsd:simpleType>
                    </xsd:element>
                    */
                    rest_minLength = prop.OwnerDocument.CreateElement(owner.Prefix, "minLength", owner.NamespaceURI);
                    restriction_base = prop.OwnerDocument.CreateAttribute("base"); restriction_base.Value = "xsd:string";
                    minLength_value = prop.OwnerDocument.CreateAttribute("value"); minLength_value.Value = "1";
                    rest_minLength.Attributes.Append(minLength_value);
                    ownerComment.Value = "No empty string";
                    break;
                case "NUMBER":
                    /*
                    <xsd:element name="number" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
                      <xsd:simpleType>
                        <xsd:restriction base="xsd:decimal" />
                      </xsd:simpleType>
                    </xsd:element>
                    */
                    restriction_base = prop.OwnerDocument.CreateAttribute("base"); restriction_base.Value = "xsd:decimal";
                    ownerComment.Value = "Numbers only";
                    break;
                case "DATE":
                    /*
                    <xsd:element name="ContactIdTypeD" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
                        <xsd:simpleType>
                        <xsd:restriction base="xsd:string" >
                            <xsd:pattern value = "(0[1-9])|10|11|12\/(([0-2]\d)|30|31)\/\d\d\d\d(\s[0-2]\d:[0-5]\d:[0-5]\d)?"/>
                        </xsd:restriction>
                        </xsd:simpleType>
                    </xsd:element>
                    */
                    restriction_base = prop.OwnerDocument.CreateAttribute("base"); restriction_base.Value = "xsd:string";
                    restriction_pattern = prop.OwnerDocument.CreateElement(owner.Prefix, "pattern", owner.NamespaceURI);
                    restrictionPatert_value = prop.OwnerDocument.CreateAttribute("value"); restrictionPatert_value.Value = @"((0[1-9])|10|11|12)\/(([0-2]\d)|30|31)\/\d\d\d\d(\s[0-2]\d:[0-5]\d:[0-5]\d)?";
                    restriction_pattern.Attributes.Append(restrictionPatert_value);
                    ownerComment.Value = "Date in format MM/DD/YYYY or DateTime in format MM/DD/YYYY hh:mm:ss";
                    break;
            }

            owner.AppendChild(ownerComment);
            owner.AppendChild(simpleType);
            simpleType.AppendChild(restriction);
            if (minLength_value != null)
                restriction.AppendChild(rest_minLength);
            restriction.Attributes.Append(restriction_base);
            if (restriction_pattern != null)
                restriction.AppendChild(restriction_pattern);


        }
        /// <summary>
        /// removed to Functions
        /// </summary>
        /// <param name="t"></param>
        //public static void UpdateTable(DataTable t)
        //{
        //    string cmnt;
        //    Regex propRequired = new Regex(@"(Ws_Req(?:uired)?)\s*:\s*(.*?)\s*(?:\;|$)", RegexOptions.IgnoreCase);
        //    Regex propSequence = new Regex(@"(Ws_Seq(?:uence)?)\s*:\s*(.*?)\s*(?:\;|$)", RegexOptions.IgnoreCase);
        //    Match mtch;
        //    int sequence;
        //    foreach (DataRow r in t.Rows)
        //    {
        //        if (r["PropName"].ToString().IndexOf(' ') > 0)
        //            r["WsPropName"] = r["PropName"].ToString().Replace(" ", "_spc");
        //        else
        //            r["WsPropName"] = r["PropName"];

        //        cmnt = r["Comments"].ToString();
        //        if (string.IsNullOrEmpty(cmnt)) continue;

        //        if (propRequired.IsMatch(cmnt))
        //        {
        //            mtch = propRequired.Match(cmnt);
        //            switch (mtch.Groups[2].Value.ToLower())
        //            {
        //                case "true":
        //                case "y":
        //                    r["IsRequired"] = true; break;
        //                case "false":
        //                case "n":
        //                    r["IsRequired"] = false; break;
        //                default:
        //                    //r["Error"] = "WS param defenition " + mtch.Groups[1].Value + " with Invalid value: '" + mtch.Groups[2].Value + "'. Expected true, false, Y or N";
        //                    r["Error"] = $"WS param defenition {mtch.Groups[1].Value} with Invalid value: '{mtch.Groups[2].Value}'. Expected true, false, Y or N";
        //                    break;
        //            }
        //        }

        //        if (propSequence.IsMatch(cmnt))
        //        {
        //            mtch = propSequence.Match(cmnt);
        //            if (int.TryParse(mtch.Groups[2].Value, out sequence))
        //                r["Sequence"] = sequence;
        //            else
        //                r["Error"] += string.IsNullOrEmpty(r["Error"].ToString()) ? "" : ". " + $"WS param defenition {mtch.Groups[1].Value} with invalid value: '{mtch.Groups[2].Value}'. Expected integer number";
        //        }
        //    }
        //}
        public static DataTable InitTable(XmlDocument WfXml/*string WfXml*/)
        {
            DataTable t = new DataTable();
            DataRow r;
            t.Columns.Add("PropName");
            t.Columns.Add("Comments");
            t.Columns.Add("DataType");
            t.Columns.Add("WsPropName");
            t.Columns.Add("IsRequired", System.Type.GetType("System.Boolean"));
            t.Columns.Add("Sequence", System.Type.GetType("System.Int32"));
            t.Columns.Add("Error");

            XmlNodeList wfProps = ReadWfXml(WfXml);
            foreach (XmlElement p in wfProps)
            {
                r = t.NewRow();
                r.SetField("PropName", p.InnerText);
                r.SetField("Comments", p.ParentNode.SelectSingleNode("Comments").InnerText);
                r.SetField("DataType", p.ParentNode.SelectSingleNode("DataType").InnerText);
                t.Rows.Add(r);
            }
            return t;
        }
        private static XmlNodeList ReadWfXml(XmlDocument doc/*string File*/)
        {
            //string file = @"C:\Siebel\15.0.0.0.0\Tools\OBJECTS\MNR Set Contact Status HW IWS.XML";
            //XmlDocument doc = new XmlDocument();
            //doc.Load(File);
            XmlNodeList props = doc.SelectNodes("/SiebelMessage/ListOfRepositoryWorkflowProcess/RepositoryWorkflowProcess/ListOfRepositoryWfProcessProp/RepositoryWfProcessProp[InOut!='NONE']/Name2");
            return props;
        }
        private static XmlNodeList ReadWsdl(XmlDocument doc)
        {
            //string file = @"C:\Users\igorva\Downloads\http___siebel.com_CustomUI_SBL_SetContactStatusHW.WSDL";
            //XmlDocument doc = new XmlDocument();
            //doc.Load(File);
            XmlNamespaceManager NmSpMngr = new XmlNamespaceManager(doc.NameTable);
            NmSpMngr.AddNamespace("xsd", "http://www.w3.org/2001/XMLSchema");
            NmSpMngr.AddNamespace("defSpace", "http://schemas.xmlsoap.org/wsdl/");
            XmlNodeList propNamesAtts = doc.DocumentElement.SelectNodes(
                "/defSpace:definitions/defSpace:types/xsd:schema/xsd:element/xsd:complexType/xsd:sequence/xsd:element/@name", 
                NmSpMngr);
            return propNamesAtts;
        }


        private static void ReadDb(string PortName)
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = "Server=SIEBELOUI_TST";
            conn.Open();


            //Based on https://docs.oracle.com/cd/E17952_01/connector-odbc-en/connector-odbc-examples-programming-net-csharp.html
            string connectionString = "(DESCRIPTION=    (ADDRESS=      (PROTOCOL=TCP)      (HOST=dbcc1-scan.il1.ocm.s1747013.oraclecloudatcustomer.com )      (PORT=1521)    )    (CONNECT_DATA=      (SERVICE_NAME=SIEBELQASRV.il1.ocm.s1747013.oraclecloudatcustomer.com)        (FAILOVER_MODE =        (TYPE = select)        (METHOD = basic)      )    )  )";
            //connectionString = "Driver={Microsoft ODBC for Oracle}; Server=SIEBELOUI_TST";
            /*@"(DESCRIPTION=
    (ADDRESS=
    (PROTOCOL=TCP)
    (HOST=dbcc1-scan.il1.ocm.s1747013.oraclecloudatcustomer.com )
    (PORT=1521)
    )
    (CONNECT_DATA=
    (SERVICE_NAME=SIEBELQASRV.il1.ocm.s1747013.oraclecloudatcustomer.com)
    (FAILOVER_MODE =
    (TYPE = select)
    (METHOD = basic)
    )
    )
    )";*/
            string cmdString = @"select  tProp.NAME Name, 
        tProp.DATA_TYPE_CD DataType,
        DATE_VAL || NUM_VAL || CHAR_VAL Default_Val, 
        tProp.COMMENTS COMMENTS
    from S_WS_PORT tPort
    join S_WS_PORT_TYPE tPortType on tPort.WS_PORT_TYPE_ID = tPortType.ROW_ID and tPortType.IMPL_TYPE_CD = 'WORKFLOW'
    join S_WFR_PROC tWf on tPortType.NAME = tWf.PROC_NAME and INACTIVE_FLG = 'N' and tWf.STATUS_CD = 'COMPLETED'
    join S_REPOSITORY tRep on tWf.REPOSITORY_ID = tRep.ROW_ID and tRep.NAME = 'Siebel Repository'
    join S_WFR_PROC_PROP tProp on tProp.PROCESS_ID = tWf.ROW_ID and PROP_DEF_TYPE_CD in ('OUT','IN','INOUT')
    where tPort.NAME = 'MNR Update SR Process WS' ";

            connectionString = "Dsn=MsOdbcForSiebelTest;uid=SIEBEL;pwd=SIEBEL";
            connectionString = "DRIVER={Siebel Oracle90}; SERVER=dbcc1-scan.il1.ocm.s1747013.oraclecloudatcustomer.com;DATABASE=SIEBELQASRV";
            //connectionString = "DRIVER={MySQL ODBC 3.51 Driver}; SERVICE_NAME=SIEBELQASRV.il1.ocm.s1747013.oraclecloudatcustomer.com;UID=Siebel;PASSWORD=sadmin";
            //connectionString = "DRIVER={MySQL ODBC 3.51 Driver};";
            OdbcConnection con = new OdbcConnection(connectionString);

            con.Open();
            OdbcParameter wsPortNameParam = new OdbcParameter("PortName", PortName);
            OdbcCommand cmd = new OdbcCommand(cmdString, con);
            cmd.Parameters.Add(wsPortNameParam);
            cmd.ExecuteNonQuery();
            OdbcDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            { };



        }




    }


}




