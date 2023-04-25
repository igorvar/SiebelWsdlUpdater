using System;
using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
using System.Drawing;
using System.IO;
//using System.Linq;
//using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

using System.Configuration;
using System.Globalization;

namespace SiebelWsdlUpdator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //helpProvider1.SetShowHelp(txtWfFile, true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            XmlDocument[] wfs;
            int i = 0;
            Dictionary<string, string> errors;
            XmlWriter wrtr;
            string newWsdl;
            XmlDocument wsdl = new XmlDocument();
            if (Executor2.PortsCount == 0)
            {
                if (string.IsNullOrEmpty( txtWsdlFile.Text)) MessageBox.Show("Select file WSDL of Siebel inbound WS", "The file is not selected yet", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                else MessageBox.Show($"Invalid WSDL {txtWsdlFile.Text}", "Invalid WSDL", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                btnSelectWsdl.Select();
                return;
            }
            wsdl.Load(txtWsdlFile.Text);
            Executor2.Wsdl = wsdl;

            if (tabsSources.SelectedTab == srcDB)
            //if (string.IsNullOrEmpty(txtWfFile.Text) && dataGridView.RowCount == 0)

                //errors = Executor2.Execute(ConfigurationManager.AppSettings["OdbcConnectionString"]);
                errors = Executor2.Execute(ConfigurationManager.ConnectionStrings["OdbcConnectionString"].ConnectionString);

            //errors = Executor2.Execute(ConfigurationManager.
            else
            {

                wfs = new XmlDocument[Executor2.PortsCount];
                if (Executor2.PortsCount == 1)
                {
                    wfs[0] = new XmlDocument();
                    try { wfs[0].Load(txtWfFile.Text); }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error on loading WF definition XML from file {txtWfFile.Text}: {ex.Message}", "Invalid XML", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        btnSelectWfXmd.Select();
                        return;
                    }
                }
                else
                    foreach (string port in Executor2.PortsNames)
                        foreach (DataGridViewRow r in dataGridView.Rows)
                        {
                            if ((string)r.Cells["clmnPort"].Value != port) continue;
                            if (string.IsNullOrWhiteSpace((string)r.Cells["clmnWfFile"].Value))
                            {
                                MessageBox.Show($"Select WF definition XML file for port {port}", "The file is not selected yet", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                return;
                            }
                            if (!File.Exists((string)r.Cells["clmnWfFile"].Value))
                            {
                                MessageBox.Show($"Not present file described WF it used by port {port}. Select other file", "Invalid file selected", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                return;
                            }

                            wfs[i] = new XmlDocument();
                            try { wfs[i++].Load((string)r.Cells["clmnWfFile"].Value); }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error on loading XML for port {port} from file {(string)r.Cells["clmnWfFile"].Value}: {ex.Message}", "Invalid XML", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                                return;
                            }
                        }

                errors = Executor2.Execute(wfs);
            }
            newWsdl = Regex.Replace(txtWsdlFile.Text, "(.wsdl)$", "_Updated$1", RegexOptions.IgnoreCase);
            wrtr = XmlWriter.Create(newWsdl);
            Executor2.Wsdl.WriteTo(wrtr);
            wrtr.Close();
            txtWsdlFile2.Text = newWsdl;
            txtError.Text = "";
            if (errors == null) txtError.Text = "No errors";
            else
                foreach (var er in errors)
                    txtError.Text += $"{er.Key}\r\n{er.Value}\r\n\r\n";

        }
        //private void button1_Click1(object sender, EventArgs e)
        //{
        //    XmlDocument[] wfs;
        //    Dictionary<string, string> errors;
        //    if (string.IsNullOrWhiteSpace(txtWfFile.Text))
        //    {
        //        MessageBox.Show("Enter file xml", "The field is not filled", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        //        return;
        //    }
        //    if (!File.Exists(txtWfFile.Text))
        //    {
        //        MessageBox.Show("Not present file with WF definition", "Invalid file", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        //        return;
        //    }
        //    wfs = new XmlDocument[Executor2.PortsCount];
        //    wfs[0] = new XmlDocument();
        //    wfs[0].Load(txtWfFile.Text);
        //    errors = Executor2.Execute(wfs);
        //    string newWsdl = Regex.Replace(txtWsdlFile.Text, "(.wsdl)$", "_Updated$1", RegexOptions.IgnoreCase);
        //    XmlWriter wrtr;
        //    wrtr = XmlWriter.Create(newWsdl);
            
        //    Executor2.Wsdl.WriteTo(wrtr);
        //    wrtr.Close();
        //    txtWsdlFile2.Text = newWsdl;

        //}

        private void btnSelectWsdl_Click(object sender, EventArgs e)
        {
            
            XmlDocument wsdl = new XmlDocument();
            //dlgOpenWsdl.InitialDirectory = System.Environment.ExpandEnvironmentVariables(@"%userprofile%\downloads\");
                                             
            
            if (DialogResult.OK != dlgOpenWsdl.ShowDialog())
                return;
            txtWsdlFile.Text = dlgOpenWsdl.FileName;
            txtWfFile.Text = "";
            dataGridView.Rows.Clear();
            
            wsdl.Load(txtWsdlFile.Text);
            Executor2.Wsdl = wsdl;
            switch (Executor2.PortsCount)
            {
                case 0:
                    MessageBox.Show("Not presents WS ports in selected file", "Invalid file", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    break;
                case 1:
                    tabControl.SelectTab(0);
                    break;
                default:
                    tabControl.SelectTab(1);
                    foreach (string port in Executor2.PortsNames)
                        dataGridView.Rows.Add(port);
                    break;
            }
        
    }

        private void btnSelectWfXmd_Click(object sender, EventArgs e)
        {
            dlgOpenWfXml.InitialDirectory = @"C:\Siebel\15.0.0.0.0\Tools\OBJECTS";
            if (DialogResult.OK == dlgOpenWfXml.ShowDialog())
                txtWfFile.Text = dlgOpenWfXml.FileName;
        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 2 || e.RowIndex < 0 ) return;
            if (DialogResult.OK == dlgOpenWfXml.ShowDialog())
                dataGridView[1, e.RowIndex].Value = dlgOpenWfXml.FileName;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tabControl.ItemSize = new Size(0, 1);
            try
            {
                txtDbInfo.Text = $"Connection to {ConfigurationManager.ConnectionStrings["OdbcConnectionString"].ProviderName}.\r\nDefinitions in file {Application.ExecutablePath}.config";
                dlgOpenWfXml.InitialDirectory = ConfigurationManager.AppSettings["DefaultFolderWsXml"];
                if (ConfigurationManager.AppSettings["DefaultFolderWsdl"] != null)
                    dlgOpenWsdl.InitialDirectory = Environment.ExpandEnvironmentVariables(
                        ConfigurationManager.AppSettings["DefaultFolderWsdl"]
                    );
            }
            catch (NullReferenceException)
            {
                string cnf = $"{Application.ExecutablePath}.config";
                if (!File.Exists(cnf))
                    txtDbInfo.Text = $"Not found file {cnf}. Impossible open DB";
                else
                    txtDbInfo.Text = $"Not found definition 'OdbcConnectionString' in configuration/connectionStrings in file {cnf} ";
                //srcDisk.Select();
                tabsSources.SelectedTab = srcDisk;
            }
            catch (ConfigurationErrorsException ex)
            {
                txtDbInfo.Text = $"{ex.Message}\r\nIs file {Application.ExecutablePath}.config valid?";
            }
            
        }

        

       
    }
}
