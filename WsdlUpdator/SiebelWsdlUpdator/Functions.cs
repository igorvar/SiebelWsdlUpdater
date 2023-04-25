using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace SiebelWsdlUpdator
{
    internal static class Functions
    {
        public static void UpdateDataMap(DataTable t)
        {
            string cmnt;
            string wsPropName;
            Regex propRequired = new Regex(@"(Ws_Req(?:uired)?)\s*:\s*(.*?)\s*(?:\;|$)", RegexOptions.IgnoreCase);
            Regex propSequence = new Regex(@"(Ws_Seq(?:uence)?)\s*:\s*(.*?)\s*(?:\;|$)", RegexOptions.IgnoreCase);
            Match mtch;
            int sequence;
            foreach (DataRow r in t.Rows)
            {
                /*if (r["PropName"].ToString().IndexOf(' ') > 0)
                    r["WsPropName"] = r["PropName"].ToString().Replace(" ", "_spc").Replace("+", "_002B");
                else
                    r["WsPropName"] = r["PropName"];*/
                wsPropName = r[MapperFields.PropName.ToString()/*"PropName"*/].ToString();
                if (wsPropName.ToLower() == "xml") wsPropName = "_" + wsPropName;

                if (wsPropName[0] == '-' || wsPropName[0] == '.') wsPropName = "_sblesc" + wsPropName;

                if (wsPropName[0] == '\'' || wsPropName[0] == '!' || wsPropName[0] == '#' ||
                    wsPropName[0] == '$' || wsPropName[0] == '%' || wsPropName[0] == '&' ||
                    wsPropName[0] == '(' || wsPropName[0] == ')' || wsPropName[0] == '*' ||
                    wsPropName[0] == '/' || wsPropName[0] == '?' || wsPropName[0] == '@' || 
                    wsPropName[0] == '[' || wsPropName[0] == '\\' || wsPropName[0] == ']' ||
                    wsPropName[0] == '^' || wsPropName[0] == '{' || wsPropName[0] == '|' ||
                    wsPropName[0] == '}' || wsPropName[0] == '<' || wsPropName[0] == '>' ||
                    wsPropName[0] == '"' || wsPropName[0] == '+' || wsPropName[0] == ',')
                    wsPropName = "_sblesc" + wsPropName;

                wsPropName = wsPropName
                                        .Replace("'", "_sqt")
                                        .Replace("!", "_0021")
                                        .Replace("#", "_pnd")
                                        .Replace("$", "_0024")
                                        .Replace("%", "_0025")
                                        .Replace("&", "_amp")
                                        .Replace("(", "_lpr")
                                        .Replace(")", "_rpr")
                                        .Replace("*", "_002A")
                                        .Replace("/", "_slh")
                                        .Replace("?", "_qst")
                                        .Replace("@", "_0040")
                                        .Replace("[", "_005B")
                                        .Replace("\\", "_005C")
                                        .Replace("]", "_005D")
                                        .Replace("^", "_005E")
                                        .Replace("{", "_007B")
                                        .Replace("|", "_007C")
                                        .Replace("}", "_007D")
                                        .Replace("<", "_lst")
                                        .Replace(">", "_grt")
                                        .Replace(" ", "_spc")
                                        .Replace("\"", "_dqt")
                                        .Replace("+", "_002B")
                                        .Replace(",", "_cma");

                r[MapperFields.WsPropName.ToString()/*"WsPropName"*/] = wsPropName;
                cmnt = r[MapperFields.Comments.ToString()/*"Comments"*/].ToString();
                if (string.IsNullOrEmpty(cmnt)) continue;

                if (propRequired.IsMatch(cmnt))
                {
                    mtch = propRequired.Match(cmnt);
                    switch (mtch.Groups[2].Value.ToLower())
                    {
                        case "true":
                        case "y":
                            r[MapperFields.IsRequired.ToString()/*"IsRequired"*/] = true; break;
                        case "false":
                        case "n":
                            r[MapperFields.IsRequired.ToString()/*"IsRequired"*/] = false; break;
                        default:
                            //r["Error"] = "WS param defenition " + mtch.Groups[1].Value + " with Invalid value: '" + mtch.Groups[2].Value + "'. Expected true, false, Y or N";
                            r[MapperFields.Error.ToString()/*"Error"*/] = $"WS param defenition {mtch.Groups[1].Value} with Invalid value: '{mtch.Groups[2].Value}'. Expected true, false, Y or N";
                            break;
                    }
                }

                if (propSequence.IsMatch(cmnt))
                {
                    mtch = propSequence.Match(cmnt);
                    if (int.TryParse(mtch.Groups[2].Value, out sequence))
                        r[MapperFields.Sequence.ToString()/*"Sequence"*/] = sequence;
                    else
                        r[MapperFields.Error.ToString()/*"Error"*/] += (string.IsNullOrEmpty(r[MapperFields.Error.ToString()/*"Error"*/].ToString()) ? "" : ". ") + $"WS param defenition {mtch.Groups[1].Value} with invalid value: '{mtch.Groups[2].Value}'. Expected integer";
                }
            }
        }

        /// <summary>
        /// Update each parameter in wsdl: Set type for Date, number, define it as option based on mapping table
        /// </summary>
        /// <param name="wsParamNames">collection of xmlAttribues. Each attribute - name of input or output WS prop</param>
        /// <param name="mapping">Table based on WF definition</param>
        public static void UpdateWsParams(XmlNodeList wsParamNames, DataTable mapping)
        {
            DataRow[] selected;
            DataRow r;
            foreach (XmlAttribute att in wsParamNames)
            {

                selected = mapping.Select($"WsPropName = '{att.Value}'");
                if (selected.Length == 0) continue;
                r = selected[0];

                SetType(att, r["DataType"].ToString());

                switch (r[MapperFields.IsRequired.ToString()/*"IsRequired"*/].ToString().ToLower())
                {
                    case "false":
                        MarkAsOptional(att);
                        break;
                    case "true":
                        //MarkAsRequired(att, r["DataType"].ToString());
                        //nothing to do
                        break;
                    default:
                        r[MapperFields.Error.ToString()/*"Error"*/] += (string.IsNullOrEmpty(r[MapperFields.Error.ToString()/*"Error"*/].ToString()) ? "" : ". ") + $"Required or optional definition of Parameter '{att.Value}' not changed, because not found definition Ws_Required in WF";
                        break;
                }
            }
        }
        public static void SetType(XmlNodeList wsParamNames, DataTable mapping)
        {
            DataRow[] selected;
            DataRow r;
            foreach (XmlAttribute att in wsParamNames)
            {
                selected = mapping.Select($"WsPropName = '{att.Value}'");
                if (selected.Length == 0) continue;
                r = selected[0];
                SetType(att, r["DataType"].ToString());
            }
        }

        public static void MarkAsOptional(XmlNodeList wsParamNames, DataTable mapping)
        {
            DataRow[] selected;
            DataRow r;
            foreach (XmlAttribute att in wsParamNames)
            {
                selected = mapping.Select($"WsPropName = '{att.Value}'");
                if (selected.Length == 0) continue;
                r = selected[0];
                switch (r[MapperFields.IsRequired.ToString()/*"IsRequired"*/].ToString())
                {
                    case "False":
                        MarkAsOptional(att);
                        break;
                    case "True":
                        // by default all attribs are required
                        break;
                    default:
                        r[MapperFields.Error.ToString()/*"Error"*/] += (string.IsNullOrEmpty(r[MapperFields.Error.ToString()/*"Error"*/].ToString()) ? "" : ". ") + $"Required or optional definition of Parameter '{att.Value}' not changed, because not found definition Ws_Required in WF";
                        break;
                }
            }
        }
        private static void MarkAsOptional(XmlAttribute prop)
        {
            XmlAttribute minOcc = prop.OwnerDocument.CreateAttribute("minOccurs");
            minOcc.Value = "0";
            prop.OwnerElement.Attributes.Append(minOcc);
        }

        public static void OrderWsParams(XmlNodeList wsParamsNames, DataTable mapping)
        {
            //DataRow[] orderedRows = mapping.Select("Sequence is not null", "Sequence desc");
            //string[] propsNamesByOrder = new string[orderedRows.Length];
            //int i = 0;
            XmlElement wsPropertyNew = null;
            XmlElement wsPropertyOld = null;
            XmlElement sequence = null;
            //foreach (DataRow r in mapping.Select("Sequence is not null", "Sequence asc"))
            foreach (DataRow r in mapping.Select($"{MapperFields.Sequence} is not null", $"{MapperFields.Sequence} asc"))
            {
                foreach (XmlNode paramNameAtt in wsParamsNames)
                {
                    if (paramNameAtt.Value != r["WsPropName"].ToString()) continue;
                    wsPropertyOld = ((XmlAttribute)paramNameAtt).OwnerElement;
                    if (sequence == null) sequence = (XmlElement)wsPropertyOld.ParentNode;
                    wsPropertyNew = (XmlElement)wsPropertyOld.CloneNode(true);
                    /*wsPropertyOld.ParentNode*/
                    sequence.AppendChild(wsPropertyNew);
                    /*wsPropertyOld.ParentNode*/
                    sequence.RemoveChild(wsPropertyOld);
                }
            }
            wsParamsNames = sequence.SelectNodes(@"*/@name");
        }

        /// <summary>
        /// Remove element 'sequence', parent of XmlNodeList. properties and copy all properties to new element 'all'
        /// </summary>
        /// <param name="wsParamsNames"></param>
        public static void MakeOrderIndependent(XmlNodeList wsParamsNames)
        {
            XmlElement prop;
            XmlElement sequence = null;
            XmlElement all = null;
            foreach (XmlNode paramNameAtt in wsParamsNames)
            {
                //if (paramNameAtt.Value != r["WsPropName"].ToString()) continue;
                prop = ((XmlAttribute)paramNameAtt).OwnerElement;
                if (prop.ParentNode.LocalName != "sequence") continue;
                sequence = (XmlElement)prop.ParentNode;
                if (sequence.ParentNode == null) continue; // all nodes under sequence was moved to all and sequence was removed
                all = prop.OwnerDocument.CreateElement(sequence.Prefix, "all", sequence.NamespaceURI);
                foreach (XmlNode n in sequence.ChildNodes)
                    all.AppendChild(n.CloneNode(true));
                sequence.ParentNode.InsertAfter(all, sequence);
                sequence.ParentNode.RemoveChild(sequence);
            }
        }

        private static void SetType(XmlAttribute WsParamName, string DataType /*VARCHAR, NUMBER, DATE*/)
        {
            XmlElement owner = WsParamName.OwnerElement;
            XmlComment comment = null;
            XmlElement simpleType = null;
            XmlElement simpleType_restriction = null;
            XmlElement simpleType_restriction_pattern1 = null;
            XmlElement simpleType_restriction_pattern2 = null;
            XmlAttribute restrictionBase = null;
            XmlAttribute simpleType_restriction_pattern1Value = null;
            switch (DataType)
            {
                //VARCHAR,NUMBER, DATE, ALIAS and BINARY
                case "VARCHAR":
                case "ALIAS":
                case "BINARY":
                    /*nothing to do. In siebel it always string*/
                    break;
                case "NUMBER":
                    owner.Attributes["type"].Value = "xsd:decimal";
                    break;
                case "DATE":
                    /*
                    <xsd:element name="Date" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
                        <xsd:simpleType>
                            <xsd:restriction base="xsd:string" >
                                <xsd:pattern value = "(0[1-9])|10|11|12\/(([0-2]\d)|30|31)\/\d\d\d\d(\s[0-2]\d:[0-5]\d:[0-5]\d)?"/>
                            </xsd:restriction>
                        </xsd:simpleType>
                    </xsd:element>*/
                    owner.RemoveAttribute("type");
                    simpleType = WsParamName.OwnerDocument.CreateElement(owner.Prefix, "simpleType", owner.NamespaceURI);
                    {
                        simpleType_restriction = WsParamName.OwnerDocument.CreateElement(owner.Prefix, "restriction", owner.NamespaceURI);
                        restrictionBase = WsParamName.OwnerDocument.CreateAttribute("base");
                        restrictionBase.Value = "xsd:string";
                        simpleType_restriction.Attributes.Append(restrictionBase);
                    }
                    {
                        simpleType_restriction_pattern1 = WsParamName.OwnerDocument.CreateElement(owner.Prefix, "pattern", owner.NamespaceURI);
                        simpleType_restriction_pattern1Value = WsParamName.OwnerDocument.CreateAttribute("value");
                        simpleType_restriction_pattern1Value.Value = @"((0[1-9])|10|11|12)\/(([0-2]\d)|30|31)\/\d\d\d\d(\s[0-2]\d:[0-5]\d:[0-5]\d)?";
                        simpleType_restriction_pattern1.Attributes.Append(simpleType_restriction_pattern1Value);
                    }
                    simpleType_restriction_pattern2 = (XmlElement)simpleType_restriction_pattern1.Clone();
                    simpleType_restriction_pattern2.Attributes[0].Value = "";

                    comment = WsParamName.OwnerDocument.CreateComment("Date in format MM/DD/YYYY or DateTime in format MM/DD/YYYY hh:mm:ss");
                    owner.AppendChild(comment);
                    simpleType_restriction.AppendChild(simpleType_restriction_pattern1);
                    simpleType_restriction.AppendChild(simpleType_restriction_pattern2);
                    simpleType.AppendChild(simpleType_restriction);
                    owner.AppendChild(simpleType);
                    break;

            }
        }
    }
}
