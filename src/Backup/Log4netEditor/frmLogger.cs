using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using System.Xml;

namespace Log4netEditor
{
	public class frmLogger : Form
	{
		#region WinForm Controls

        private Button btnCancel;
		private Button btnSave;
		private IContainer components;
        private ComboBox ddlAdditivity;
        private ComboBox ddlLevel;
		private GroupBox gbAppenderList;
		private Label lblAdditivity;
		private Label lblLevel;
		private Label lblLogger;
		private ToolTip toolTip;
		private TextBox txtLoggerName;
		#endregion

		private XmlDocument moXmlDoc;
        private DataGridView dgAppenders;
		private string ms_tmpRquid = null;
        private DataGridViewComboBoxColumn appenderName;
        private DataGridViewTextBoxColumn appenderType;
        private static readonly Dictionary<string, string> _appenderClassDtnry = new Dictionary<string,string>();

		// Methods
		public frmLogger(XmlDocument log4net_config_XmlDoc)
		{
			InitializeComponent();
			moXmlDoc = log4net_config_XmlDoc;
			InitAppenderList();
		}

		public void EditExistedLogger(string sloggerName, string rquid)
		{
			ms_tmpRquid = rquid;
			XmlNode oLogger = null;
			DataTable dtAppenders = new DataTable("Appenders");
			dtAppenders.Columns.Add("Appender Name", typeof(string));
			dtAppenders.Columns.Add("Appender Type", typeof(string));
			if ("root" == sloggerName)
			{
				foreach (XmlNode node2 in moXmlDoc.SelectNodes("//root"))
				{
					if (CompareLogger(out oLogger, node2)) 
					{
						txtLoggerName.Text = "root";
						break;
					}
				}
			}
			else
			{
				foreach (XmlNode node2 in moXmlDoc.SelectNodes("//logger"))
				{
					if (node2.Attributes["name"].Value == sloggerName)
					{
						if (CompareLogger(out oLogger, node2)) 
						{
							txtLoggerName.Text = oLogger.Attributes["name"].Value;
							break;
						}
					}
				}
			}
			if (oLogger == null)
			{
				MessageBox.Show(this, "Logger(" + sloggerName + ") is not existed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			else
			{
				ddlAdditivity.Text = (oLogger.Attributes["additivity"] == null) ? "true" : oLogger.Attributes["additivity"].Value;
				ddlLevel.Text = oLogger.SelectSingleNode("level").Attributes["value"].Value;
				foreach (XmlNode node3 in oLogger.SelectNodes("appender-ref"))
				{
					DataRow drAppender = dtAppenders.NewRow();
					drAppender[dtAppenders.Columns[0].ColumnName] = node3.Attributes["ref"].Value;
					foreach (XmlNode node4 in moXmlDoc.SelectNodes("//appender"))
					{
						if (node4.Attributes["name"].Value == node3.Attributes["ref"].Value)
						{
							drAppender[dtAppenders.Columns[1].ColumnName] = node4.Attributes["type"].Value;
							break;
						}
					}
					dtAppenders.Rows.Add(drAppender);
				}
				dgAppenders.DataSource = dtAppenders;
                dtAppenders.ColumnChanged += new DataColumnChangeEventHandler(dtAppenders_ColumnChanged);
			}
		}

        void dtAppenders_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            if ("Appender Name" == e.Column.ColumnName)
            {
                e.Row["Appender Type"] = _appenderClassDtnry[(string)e.Row["Appender Name"]];
            }
            else
            {
                dgAppenders.Refresh();
            }
        }

		public void EditExistedLogger(string sloggerName)
		{
			EditExistedLogger(sloggerName, null);
		}

		public new DialogResult ShowDialog(IWin32Window owner)
		{
			if (dgAppenders.DataSource == null)
			{
				DataTable table1 = new DataTable("AppenderList");
				table1.Columns.Add("Name", typeof(string));
				table1.Columns.Add("Type", typeof(string));
				dgAppenders.DataSource = table1;
			}
			return base.ShowDialog(owner);
		}

		public XmlDocument Current_Log4net_config_XmlDoc
		{
			get { return moXmlDoc; }
		}

		#region Code generated by VS2003
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.txtLoggerName = new System.Windows.Forms.TextBox();
            this.lblLogger = new System.Windows.Forms.Label();
            this.lblLevel = new System.Windows.Forms.Label();
            this.ddlLevel = new System.Windows.Forms.ComboBox();
            this.lblAdditivity = new System.Windows.Forms.Label();
            this.ddlAdditivity = new System.Windows.Forms.ComboBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.gbAppenderList = new System.Windows.Forms.GroupBox();
            this.dgAppenders = new System.Windows.Forms.DataGridView();
            this.appenderName = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.appenderType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbAppenderList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgAppenders)).BeginInit();
            this.SuspendLayout();
            // 
            // txtLoggerName
            // 
            this.txtLoggerName.Location = new System.Drawing.Point(40, 80);
            this.txtLoggerName.Name = "txtLoggerName";
            this.txtLoggerName.Size = new System.Drawing.Size(368, 21);
            this.txtLoggerName.TabIndex = 5;
            this.toolTip.SetToolTip(this.txtLoggerName, "Pick a logger name.");
            // 
            // lblLogger
            // 
            this.lblLogger.AutoSize = true;
            this.lblLogger.Location = new System.Drawing.Point(8, 48);
            this.lblLogger.Name = "lblLogger";
            this.lblLogger.Size = new System.Drawing.Size(89, 15);
            this.lblLogger.TabIndex = 4;
            this.lblLogger.Text = "Logger Name :";
            // 
            // lblLevel
            // 
            this.lblLevel.AutoSize = true;
            this.lblLevel.Location = new System.Drawing.Point(8, 112);
            this.lblLevel.Name = "lblLevel";
            this.lblLevel.Size = new System.Drawing.Size(113, 15);
            this.lblLevel.TabIndex = 6;
            this.lblLevel.Text = "Choose Log Level :";
            // 
            // ddlLevel
            // 
            this.ddlLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlLevel.Items.AddRange(new object[] {
            "OFF",
            "FATAL",
            "ERROR",
            "WARN",
            "INFO",
            "DEBUG",
            "ALL"});
            this.ddlLevel.Location = new System.Drawing.Point(40, 136);
            this.ddlLevel.Name = "ddlLevel";
            this.ddlLevel.Size = new System.Drawing.Size(368, 23);
            this.ddlLevel.TabIndex = 7;
            this.toolTip.SetToolTip(this.ddlLevel, "Choose a level to decide what kind of info you want to write into log.");
            // 
            // lblAdditivity
            // 
            this.lblAdditivity.AutoSize = true;
            this.lblAdditivity.Location = new System.Drawing.Point(8, 168);
            this.lblAdditivity.Name = "lblAdditivity";
            this.lblAdditivity.Size = new System.Drawing.Size(59, 15);
            this.lblAdditivity.TabIndex = 8;
            this.lblAdditivity.Text = "Additivity :";
            // 
            // ddlAdditivity
            // 
            this.ddlAdditivity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlAdditivity.Items.AddRange(new object[] {
            "true",
            "false"});
            this.ddlAdditivity.Location = new System.Drawing.Point(40, 192);
            this.ddlAdditivity.Name = "ddlAdditivity";
            this.ddlAdditivity.Size = new System.Drawing.Size(368, 23);
            this.ddlAdditivity.TabIndex = 9;
            this.toolTip.SetToolTip(this.ddlAdditivity, "Enable this logger or not.");
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(312, 432);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(392, 432);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 12;
            this.btnSave.Text = "&Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // gbAppenderList
            // 
            this.gbAppenderList.Controls.Add(this.dgAppenders);
            this.gbAppenderList.Location = new System.Drawing.Point(8, 224);
            this.gbAppenderList.Name = "gbAppenderList";
            this.gbAppenderList.Size = new System.Drawing.Size(456, 200);
            this.gbAppenderList.TabIndex = 14;
            this.gbAppenderList.TabStop = false;
            this.gbAppenderList.Text = "Appender List";
            // 
            // dgAppenders
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgAppenders.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgAppenders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgAppenders.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.appenderName,
            this.appenderType});
            this.dgAppenders.Location = new System.Drawing.Point(7, 21);
            this.dgAppenders.Name = "dgAppenders";
            this.dgAppenders.RowTemplate.Height = 24;
            this.dgAppenders.Size = new System.Drawing.Size(441, 173);
            this.dgAppenders.TabIndex = 18;
            // 
            // appenderName
            // 
            this.appenderName.DataPropertyName = "Appender Name";
            this.appenderName.HeaderText = "Appender Name";
            this.appenderName.Name = "appenderName";
            this.appenderName.Width = 200;
            // 
            // appenderType
            // 
            this.appenderType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.appenderType.DataPropertyName = "Appender Type";
            this.appenderType.HeaderText = "Appender Type";
            this.appenderType.Name = "appenderType";
            this.appenderType.ReadOnly = true;
            this.appenderType.Width = 108;
            // 
            // frmLogger
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(472, 464);
            this.Controls.Add(this.gbAppenderList);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblAdditivity);
            this.Controls.Add(this.lblLevel);
            this.Controls.Add(this.txtLoggerName);
            this.Controls.Add(this.lblLogger);
            this.Controls.Add(this.ddlAdditivity);
            this.Controls.Add(this.ddlLevel);
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLogger";
            this.Text = "Log4net Logger Editor";
            this.gbAppenderList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgAppenders)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#endregion

		private void btnCancel_Click(object sender, EventArgs e)
		{
			base.Close();
			base.Dispose();
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			XmlNode oLogger = null;
			string sLoggerName = txtLoggerName.Text;
			bool IsNewLogger = false;
            
			#region Search this logger node
			if ("root" == sLoggerName)
			{
				foreach (XmlNode node2 in moXmlDoc.SelectNodes("//root"))
				{
					if (CompareLogger(out oLogger, node2)) break;
				}
			}
			else
			{
				foreach (XmlNode node2 in moXmlDoc.SelectNodes("//logger"))
				{
					if (node2.Attributes["name"].Value == sLoggerName)
					{
						if (CompareLogger(out oLogger, node2)) break;
					}
				}
			}
			#endregion

			try
			{
				if (oLogger == null)
				{
					#region Create new logger node
					IsNewLogger = true;
					if ("root" == sLoggerName)
					{
						oLogger = moXmlDoc.CreateElement("root");
					}
					else
					{
						oLogger = moXmlDoc.CreateElement("logger");
					}

					XmlAttribute tmpAttri = moXmlDoc.CreateAttribute("name");
					tmpAttri.Value = sLoggerName;
					oLogger.Attributes.Append(tmpAttri);

					tmpAttri = moXmlDoc.CreateAttribute("additivity");
					tmpAttri.Value = ddlAdditivity.Text;
					oLogger.Attributes.Append(tmpAttri);

					tmpAttri = moXmlDoc.CreateAttribute("rquid");
					tmpAttri.Value = Guid.NewGuid().ToString();
					oLogger.Attributes.Append(tmpAttri);
					XmlElement element1 = moXmlDoc.CreateElement("level");
					XmlAttribute attribute2 = moXmlDoc.CreateAttribute("value");
					attribute2.Value = ddlLevel.Text;
					element1.Attributes.Append(attribute2);
					oLogger.AppendChild(element1);
					#endregion
				}
				else
				{
					string tmpRquid = null;
					if (null != oLogger.Attributes["rquid"]) tmpRquid = oLogger.Attributes["rquid"].Value;
					IsNewLogger = false;
					oLogger.RemoveAll();
					XmlAttribute tmpAttri = moXmlDoc.CreateAttribute("name");
					tmpAttri.Value = sLoggerName;
					oLogger.Attributes.Append(tmpAttri);

					tmpAttri = moXmlDoc.CreateAttribute("additivity");
					tmpAttri.Value = ddlAdditivity.Text;
					oLogger.Attributes.Append(tmpAttri);

					tmpAttri = moXmlDoc.CreateAttribute("rquid");
					tmpAttri.Value = ((null == tmpRquid) ? Guid.NewGuid().ToString() : tmpRquid);
					oLogger.Attributes.Append(tmpAttri);

					XmlNode node3 = oLogger.SelectSingleNode("level");
					if (node3 != null)
					{
						node3.Attributes["value"].Value = ddlLevel.Text;
					}
					else
					{
						node3 = moXmlDoc.CreateElement("level");
						XmlAttribute attribute4 = moXmlDoc.CreateAttribute("value");
						attribute4.Value = ddlLevel.Text;
						node3.Attributes.Append(attribute4);
						oLogger.AppendChild(node3);
					}
				}
				foreach (DataRow row1 in ((DataTable) dgAppenders.DataSource).Rows)
				{
					XmlNode node4 = moXmlDoc.CreateElement("appender-ref");
					XmlAttribute attribute5 = moXmlDoc.CreateAttribute("ref");
					attribute5.Value = (string) row1[0];
					node4.Attributes.Append(attribute5);
					oLogger.AppendChild(node4);
				}
				if (IsNewLogger)
				{
					moXmlDoc.DocumentElement.InsertAfter(oLogger, moXmlDoc.DocumentElement.LastChild);
				}
			}
			catch (Exception oEX)
			{
				MessageBox.Show(this, oEX.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			DialogResult = DialogResult.OK;
			base.Close();
		}

		private void InitAppenderList()
		{
            _appenderClassDtnry.Clear();
			XmlNodeList list1 = moXmlDoc.SelectNodes("//appender");
			foreach (XmlNode node1 in list1)
			{
                appenderName.Items.Add(node1.Attributes["name"].Value);
                _appenderClassDtnry.Add(node1.Attributes["name"].Value, node1.Attributes["type"].Value);
			}
		}

		private bool CompareLogger(out XmlNode targetLogger, XmlNode logger)
		{
			targetLogger = null;
			if (null != ms_tmpRquid && string.Empty != ms_tmpRquid)
			{
				#region Compare Rquid
				if (null != logger.Attributes["rquid"]) 
				{
					if (ms_tmpRquid == logger.Attributes["rquid"].Value) 
					{
						targetLogger = logger;
					}
				}
				#endregion
			} 
			else 
			{
				if (null == logger.Attributes["rquid"] || string.Empty == logger.Attributes["rquid"].Value)	targetLogger = logger;
			}
			return !(null == targetLogger);
		}
	}
}
