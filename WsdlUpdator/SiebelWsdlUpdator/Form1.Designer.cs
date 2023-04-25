namespace SiebelWsdlUpdator
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnExecute = new System.Windows.Forms.Button();
            this.txtWsdlFile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSelectWsdl = new System.Windows.Forms.Button();
            this.txtWfFile = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSelectWfXmd = new System.Windows.Forms.Button();
            this.dlgOpenWsdl = new System.Windows.Forms.OpenFileDialog();
            this.dlgOpenWfXml = new System.Windows.Forms.OpenFileDialog();
            this.txtWsdlFile2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.clmnPort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmnWfFile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmnSelectFile = new System.Windows.Forms.DataGridViewButtonColumn();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txtError = new System.Windows.Forms.RichTextBox();
            this.tabsSources = new System.Windows.Forms.TabControl();
            this.srcDB = new System.Windows.Forms.TabPage();
            this.txtDbInfo = new System.Windows.Forms.TextBox();
            this.srcDisk = new System.Windows.Forms.TabPage();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabsSources.SuspendLayout();
            this.srcDB.SuspendLayout();
            this.srcDisk.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnExecute
            // 
            this.btnExecute.Location = new System.Drawing.Point(9, 206);
            this.btnExecute.Margin = new System.Windows.Forms.Padding(2);
            this.btnExecute.Name = "btnExecute";
            this.helpProvider1.SetShowHelp(this.btnExecute, true);
            this.btnExecute.Size = new System.Drawing.Size(65, 40);
            this.btnExecute.TabIndex = 0;
            this.btnExecute.Text = "Go";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtWsdlFile
            // 
            this.txtWsdlFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWsdlFile.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.txtWsdlFile.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.helpProvider1.SetHelpString(this.txtWsdlFile, "Path to wsdl file exported from Siebel");
            this.txtWsdlFile.Location = new System.Drawing.Point(95, 15);
            this.txtWsdlFile.Name = "txtWsdlFile";
            this.txtWsdlFile.ReadOnly = true;
            this.helpProvider1.SetShowHelp(this.txtWsdlFile, true);
            this.txtWsdlFile.Size = new System.Drawing.Size(578, 20);
            this.txtWsdlFile.TabIndex = 1;
            this.txtWsdlFile.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Current WSDL";
            // 
            // btnSelectWsdl
            // 
            this.btnSelectWsdl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectWsdl.Location = new System.Drawing.Point(673, 14);
            this.btnSelectWsdl.Name = "btnSelectWsdl";
            this.btnSelectWsdl.Size = new System.Drawing.Size(24, 22);
            this.btnSelectWsdl.TabIndex = 3;
            this.btnSelectWsdl.Text = "...";
            this.btnSelectWsdl.UseVisualStyleBackColor = true;
            this.btnSelectWsdl.Click += new System.EventHandler(this.btnSelectWsdl_Click);
            // 
            // txtWfFile
            // 
            this.txtWfFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWfFile.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.txtWfFile.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.txtWfFile.Location = new System.Drawing.Point(96, 23);
            this.txtWfFile.Name = "txtWfFile";
            this.txtWfFile.ReadOnly = true;
            this.txtWfFile.Size = new System.Drawing.Size(561, 20);
            this.txtWfFile.TabIndex = 1;
            this.txtWfFile.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Wf Definition File";
            // 
            // btnSelectWfXmd
            // 
            this.btnSelectWfXmd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectWfXmd.Location = new System.Drawing.Point(657, 22);
            this.btnSelectWfXmd.Name = "btnSelectWfXmd";
            this.btnSelectWfXmd.Size = new System.Drawing.Size(24, 22);
            this.btnSelectWfXmd.TabIndex = 3;
            this.btnSelectWfXmd.Text = "...";
            this.btnSelectWfXmd.UseVisualStyleBackColor = true;
            this.btnSelectWfXmd.Click += new System.EventHandler(this.btnSelectWfXmd_Click);
            // 
            // dlgOpenWsdl
            // 
            this.dlgOpenWsdl.Filter = "Siebel Inbound WS definition | *.wsdl";
            this.dlgOpenWsdl.Title = "Siebel Inbound WS WSDL";
            // 
            // dlgOpenWfXml
            // 
            this.dlgOpenWfXml.Filter = "xml | *.xml";
            this.dlgOpenWfXml.Title = "WF definition XML";
            // 
            // txtWsdlFile2
            // 
            this.txtWsdlFile2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWsdlFile2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.txtWsdlFile2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.helpProvider1.SetHelpString(this.txtWsdlFile2, "HHHH");
            this.txtWsdlFile2.Location = new System.Drawing.Point(96, 249);
            this.txtWsdlFile2.Name = "txtWsdlFile2";
            this.txtWsdlFile2.ReadOnly = true;
            this.helpProvider1.SetShowHelp(this.txtWsdlFile2, true);
            this.txtWsdlFile2.Size = new System.Drawing.Size(577, 20);
            this.txtWsdlFile2.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 252);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Updated WSDL";
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmnPort,
            this.clmnWfFile,
            this.clmnSelectFile});
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.dataGridView.Location = new System.Drawing.Point(3, 3);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.Size = new System.Drawing.Size(686, 100);
            this.dataGridView.TabIndex = 6;
            this.dataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellClick);
            // 
            // clmnPort
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.clmnPort.DefaultCellStyle = dataGridViewCellStyle1;
            this.clmnPort.DividerWidth = 1;
            this.clmnPort.HeaderText = "Port";
            this.clmnPort.Name = "clmnPort";
            this.clmnPort.ReadOnly = true;
            this.clmnPort.Width = 250;
            // 
            // clmnWfFile
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.clmnWfFile.DefaultCellStyle = dataGridViewCellStyle2;
            this.clmnWfFile.DividerWidth = 1;
            this.clmnWfFile.HeaderText = "Wf Definition File";
            this.clmnWfFile.Name = "clmnWfFile";
            this.clmnWfFile.ReadOnly = true;
            this.clmnWfFile.Width = 400;
            // 
            // clmnSelectFile
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.ControlLight;
            this.clmnSelectFile.DefaultCellStyle = dataGridViewCellStyle3;
            this.clmnSelectFile.DividerWidth = 1;
            this.clmnSelectFile.HeaderText = "";
            this.clmnSelectFile.Name = "clmnSelectFile";
            this.clmnSelectFile.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.clmnSelectFile.Text = "...";
            this.clmnSelectFile.UseColumnTextForButtonValue = true;
            this.clmnSelectFile.Width = 20;
            // 
            // tabControl
            // 
            this.tabControl.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.ItemSize = new System.Drawing.Size(10, 10);
            this.tabControl.Location = new System.Drawing.Point(3, 3);
            this.tabControl.Multiline = true;
            this.tabControl.Name = "tabControl";
            this.tabControl.Padding = new System.Drawing.Point(0, 0);
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(700, 124);
            this.tabControl.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl.TabIndex = 7;
            this.tabControl.TabStop = false;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.txtWfFile);
            this.tabPage1.Controls.Add(this.btnSelectWfXmd);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Location = new System.Drawing.Point(4, 14);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(692, 106);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dataGridView);
            this.tabPage2.Location = new System.Drawing.Point(4, 14);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(692, 106);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // txtError
            // 
            this.txtError.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtError.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtError.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.txtError.Location = new System.Drawing.Point(5, 275);
            this.txtError.Name = "txtError";
            this.txtError.ReadOnly = true;
            this.txtError.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtError.Size = new System.Drawing.Size(714, 159);
            this.txtError.TabIndex = 8;
            this.txtError.TabStop = false;
            this.txtError.Text = "";
            // 
            // tabsSources
            // 
            this.tabsSources.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabsSources.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabsSources.Controls.Add(this.srcDB);
            this.tabsSources.Controls.Add(this.srcDisk);
            this.helpProvider1.SetHelpString(this.tabsSources, "Data about WF from Repository or from file");
            this.tabsSources.Location = new System.Drawing.Point(5, 42);
            this.tabsSources.Name = "tabsSources";
            this.tabsSources.SelectedIndex = 0;
            this.helpProvider1.SetShowHelp(this.tabsSources, true);
            this.tabsSources.Size = new System.Drawing.Size(714, 159);
            this.tabsSources.TabIndex = 9;
            // 
            // srcDB
            // 
            this.srcDB.BackColor = System.Drawing.SystemColors.Control;
            this.srcDB.Controls.Add(this.txtDbInfo);
            this.srcDB.Location = new System.Drawing.Point(4, 25);
            this.srcDB.Name = "srcDB";
            this.srcDB.Padding = new System.Windows.Forms.Padding(3);
            this.srcDB.Size = new System.Drawing.Size(706, 130);
            this.srcDB.TabIndex = 0;
            this.srcDB.Text = "Update on base DB info about WF";
            // 
            // txtDbInfo
            // 
            this.txtDbInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDbInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDbInfo.Location = new System.Drawing.Point(3, 3);
            this.txtDbInfo.Multiline = true;
            this.txtDbInfo.Name = "txtDbInfo";
            this.txtDbInfo.ReadOnly = true;
            this.txtDbInfo.Size = new System.Drawing.Size(700, 124);
            this.txtDbInfo.TabIndex = 0;
            // 
            // srcDisk
            // 
            this.srcDisk.BackColor = System.Drawing.SystemColors.Control;
            this.srcDisk.Controls.Add(this.tabControl);
            this.srcDisk.Location = new System.Drawing.Point(4, 25);
            this.srcDisk.Name = "srcDisk";
            this.srcDisk.Padding = new System.Windows.Forms.Padding(3);
            this.srcDisk.Size = new System.Drawing.Size(706, 130);
            this.srcDisk.TabIndex = 1;
            this.srcDisk.Text = "WF defenition exported to XML";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(724, 436);
            this.Controls.Add(this.tabsSources);
            this.Controls.Add(this.txtError);
            this.Controls.Add(this.btnSelectWsdl);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtWsdlFile2);
            this.Controls.Add(this.txtWsdlFile);
            this.Controls.Add(this.btnExecute);
            this.HelpButton = true;
            this.helpProvider1.SetHelpString(this, @"Modifies WSDL received from Siebel CRM in accordance with WF properties definitions.
    In Comments of WF propertiy need be defined properties WS_Requred(WS_Req) and/or  WS_Sequence(WS_Seq).Defenition of property must by finished by ';'.WS_Requred may have value true(y) or false(n).WS_Sequence may have integer value.
    For example:
    WS_Requred: Y; WS_Sequence: 15
    WS_SEQUENCE: true
    WS_Seq: 0; WS_Requred: false.
    If for some property present WS_Requred as false, then the minOccurs = '0' attribute corresponding to the input of the parameter in output WSDL.
    If for properties present WS_Sequence, then all input parameters are placed in <xsd:sequence> in order defined by number. If no found property with WS_Sequence, all input parameters placed in <xsd: all>;
    If Datatype of WF property is defined as Number for the property, type = 'xsd:Decimal' is set for the parameter
    If the Datatype of property defined as Date for the property, the reconstruction defined for input parameter");

            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(180, 284);
            this.Name = "Form1";
            this.helpProvider1.SetShowHelp(this, true);
            this.Text = "Siebel IWS Wsdl Pliers";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabsSources.ResumeLayout(false);
            this.srcDB.ResumeLayout(false);
            this.srcDB.PerformLayout();
            this.srcDisk.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.TextBox txtWsdlFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSelectWsdl;
        private System.Windows.Forms.TextBox txtWfFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSelectWfXmd;
        private System.Windows.Forms.OpenFileDialog dlgOpenWsdl;
        private System.Windows.Forms.OpenFileDialog dlgOpenWfXml;
        private System.Windows.Forms.TextBox txtWsdlFile2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridViewButtonColumn clmnSelectFile;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmnWfFile;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmnPort;
        private System.Windows.Forms.RichTextBox txtError;
        private System.Windows.Forms.TabControl tabsSources;
        private System.Windows.Forms.TabPage srcDB;
        private System.Windows.Forms.TabPage srcDisk;
        private System.Windows.Forms.TextBox txtDbInfo;
        private System.Windows.Forms.HelpProvider helpProvider1;
    }
}

