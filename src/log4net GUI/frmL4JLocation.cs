using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Log4netEditor;

namespace l4nEditor
{
  public class frmL4JLocation : Form
  {
    private GroupBox gpbl4nPath;
    private TextBox txtl4nPath;
    private Button btnBrowe;
    private TabControl tabLOG4NET;
    private TabPage tabAppenders;
    private TabPage tabLoggers;
    private DataGrid dgAppenders;
    private DataGrid dgLoggers;
    private Button btnSave;
    private Button btnReload;
    private OpenFileDialog openFile;
    private SaveFileDialog saveFile;
    private Button btnRemoveAppender;
    private Button btnAddAppender;
    private Button btnRemoveLogger;
    private Button btnAddLogger;
    private XmlDocument moDoc = new XmlDocument();

    private System.ComponentModel.Container components = null;

    public frmL4JLocation()
    {
      //
      // Windows Form 設計工具支援的必要項
      //
      InitializeComponent();

      //
      // TODO: 在 InitializeComponent 呼叫之後加入任何建構函式程式碼
      //
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (components != null)
        {
          components.Dispose();
        }
      }

      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      gpbl4nPath = new GroupBox();
      btnBrowe = new Button();
      txtl4nPath = new TextBox();
      tabLOG4NET = new TabControl();
      tabAppenders = new TabPage();
      btnAddAppender = new Button();
      btnRemoveAppender = new Button();
      dgAppenders = new DataGrid();
      tabLoggers = new TabPage();
      btnAddLogger = new Button();
      btnRemoveLogger = new Button();
      dgLoggers = new DataGrid();
      btnSave = new Button();
      btnReload = new Button();
      openFile = new OpenFileDialog();
      saveFile = new SaveFileDialog();
      gpbl4nPath.SuspendLayout();
      tabLOG4NET.SuspendLayout();
      tabAppenders.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(dgAppenders)).BeginInit();
      tabLoggers.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(dgLoggers)).BeginInit();
      SuspendLayout();
      // 
      // gpbl4nPath
      // 
      gpbl4nPath.Controls.Add(btnBrowe);
      gpbl4nPath.Controls.Add(txtl4nPath);
      gpbl4nPath.Location = new System.Drawing.Point(8, 8);
      gpbl4nPath.Name = "gpbl4nPath";
      gpbl4nPath.Size = new System.Drawing.Size(392, 56);
      gpbl4nPath.TabIndex = 0;
      gpbl4nPath.TabStop = false;
      gpbl4nPath.Text = "log4net config file path:";
      // 
      // btnBrowe
      // 
      btnBrowe.Location = new System.Drawing.Point(336, 24);
      btnBrowe.Name = "btnBrowe";
      btnBrowe.Size = new System.Drawing.Size(48, 23);
      btnBrowe.TabIndex = 1;
      btnBrowe.Text = "...";
      btnBrowe.Click += new EventHandler(btnBrowe_Click);
      // 
      // txtl4nPath
      // 
      txtl4nPath.BackColor = System.Drawing.Color.White;
      txtl4nPath.Location = new System.Drawing.Point(8, 24);
      txtl4nPath.Name = "txtl4nPath";
      txtl4nPath.ReadOnly = true;
      txtl4nPath.Size = new System.Drawing.Size(328, 21);
      txtl4nPath.TabIndex = 0;
      txtl4nPath.Text = "";
      // 
      // tabLOG4NET
      // 
      tabLOG4NET.Controls.Add(tabAppenders);
      tabLOG4NET.Controls.Add(tabLoggers);
      tabLOG4NET.Location = new System.Drawing.Point(8, 104);
      tabLOG4NET.Name = "tabLOG4NET";
      tabLOG4NET.SelectedIndex = 0;
      tabLOG4NET.Size = new System.Drawing.Size(392, 216);
      tabLOG4NET.TabIndex = 1;
      // 
      // tabAppenders
      // 
      tabAppenders.Controls.Add(btnAddAppender);
      tabAppenders.Controls.Add(btnRemoveAppender);
      tabAppenders.Controls.Add(dgAppenders);
      tabAppenders.Location = new System.Drawing.Point(4, 21);
      tabAppenders.Name = "tabAppenders";
      tabAppenders.Size = new System.Drawing.Size(384, 191);
      tabAppenders.TabIndex = 0;
      tabAppenders.Text = "Appenders";
      // 
      // btnAddAppender
      // 
      btnAddAppender.Enabled = false;
      btnAddAppender.Location = new System.Drawing.Point(224, 160);
      btnAddAppender.Name = "btnAddAppender";
      btnAddAppender.TabIndex = 2;
      btnAddAppender.Text = "Add";
      btnAddAppender.Click += new EventHandler(btnAddAppender_Click);
      // 
      // btnRemoveAppender
      // 
      btnRemoveAppender.Enabled = false;
      btnRemoveAppender.Location = new System.Drawing.Point(304, 160);
      btnRemoveAppender.Name = "btnRemoveAppender";
      btnRemoveAppender.TabIndex = 1;
      btnRemoveAppender.Text = "Remove";
      btnRemoveAppender.Click += new EventHandler(btnRemoveAppender_Click);
      // 
      // dgAppenders
      // 
      dgAppenders.DataMember = "";
      dgAppenders.HeaderForeColor = System.Drawing.SystemColors.ControlText;
      dgAppenders.Location = new System.Drawing.Point(8, 8);
      dgAppenders.Name = "dgAppenders";
      dgAppenders.ReadOnly = true;
      dgAppenders.Size = new System.Drawing.Size(368, 144);
      dgAppenders.TabIndex = 0;
      dgAppenders.CurrentCellChanged += new EventHandler(dgAppenders_CurrentCellChanged);
      // 
      // tabLoggers
      // 
      tabLoggers.Controls.Add(btnAddLogger);
      tabLoggers.Controls.Add(btnRemoveLogger);
      tabLoggers.Controls.Add(dgLoggers);
      tabLoggers.Location = new System.Drawing.Point(4, 21);
      tabLoggers.Name = "tabLoggers";
      tabLoggers.Size = new System.Drawing.Size(384, 191);
      tabLoggers.TabIndex = 1;
      tabLoggers.Text = "Loggers";
      // 
      // btnAddLogger
      // 
      btnAddLogger.Enabled = false;
      btnAddLogger.Location = new System.Drawing.Point(224, 160);
      btnAddLogger.Name = "btnAddLogger";
      btnAddLogger.TabIndex = 2;
      btnAddLogger.Text = "Add";
      btnAddLogger.Click += new EventHandler(btnAddLogger_Click);
      // 
      // btnRemoveLogger
      // 
      btnRemoveLogger.Enabled = false;
      btnRemoveLogger.Location = new System.Drawing.Point(304, 160);
      btnRemoveLogger.Name = "btnRemoveLogger";
      btnRemoveLogger.TabIndex = 1;
      btnRemoveLogger.Text = "Remove";
      btnRemoveLogger.Click += new EventHandler(btnRemoveLogger_Click);
      // 
      // dgLoggers
      // 
      dgLoggers.DataMember = "";
      dgLoggers.HeaderForeColor = System.Drawing.SystemColors.ControlText;
      dgLoggers.Location = new System.Drawing.Point(8, 8);
      dgLoggers.Name = "dgLoggers";
      dgLoggers.ReadOnly = true;
      dgLoggers.Size = new System.Drawing.Size(368, 144);
      dgLoggers.TabIndex = 0;
      dgLoggers.CurrentCellChanged += new EventHandler(dgLoggers_CurrentCellChanged);
      // 
      // btnSave
      // 
      btnSave.Enabled = false;
      btnSave.Location = new System.Drawing.Point(328, 72);
      btnSave.Name = "btnSave";
      btnSave.TabIndex = 2;
      btnSave.Text = "Save";
      btnSave.Click += new EventHandler(btnSave_Click);
      // 
      // btnReload
      // 
      btnReload.Enabled = false;
      btnReload.Location = new System.Drawing.Point(248, 72);
      btnReload.Name = "btnReload";
      btnReload.TabIndex = 3;
      btnReload.Text = "Reload";
      btnReload.Click += new EventHandler(btnReload_Click);
      // 
      // openFile
      // 
      openFile.DefaultExt = "config";
      openFile.Filter = "log4net configuration|*.config|All Files|*.*";
      // 
      // frmL4JLocation
      // 
      AutoScaleBaseSize = new System.Drawing.Size(6, 14);
      ClientSize = new System.Drawing.Size(410, 328);
      Controls.Add(btnReload);
      Controls.Add(btnSave);
      Controls.Add(tabLOG4NET);
      Controls.Add(gpbl4nPath);
      Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(136)));
      FormBorderStyle = FormBorderStyle.FixedSingle;
      MaximizeBox = false;
      MinimizeBox = false;
      Name = "frmL4JLocation";
      Text = "Log4net Location";
      gpbl4nPath.ResumeLayout(false);
      tabLOG4NET.ResumeLayout(false);
      tabAppenders.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(dgAppenders)).EndInit();
      tabLoggers.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(dgLoggers)).EndInit();
      ResumeLayout(false);

    }

    private void btnBrowe_Click(object sender, EventArgs e)
    {
      if (DialogResult.OK == openFile.ShowDialog(this))
      {
        txtl4nPath.Text = openFile.FileName;
        btnReload.Enabled = true;
        btnSave.Enabled = true;
        btnReload_Click(sender, e);
      }
    }

    private void BindAppenderGrid()
    {
      dgAppenders.DataSource = PrepareAppenderDT(moDoc);
    }

    private void BindLoggerGrid()
    {
      dgLoggers.DataSource = PrepareLoggerDT(moDoc);
    }

    private void btnReload_Click(object sender, EventArgs e)
    {
      if (string.Empty != txtl4nPath.Text)
      {
        try
        {
          moDoc.Load(txtl4nPath.Text);
          BindAppenderGrid();
          BindLoggerGrid();
          btnAddAppender.Enabled = true;
          btnRemoveAppender.Enabled = true;
          btnAddLogger.Enabled = true;
          btnRemoveLogger.Enabled = true;
        }
        catch (Exception oEX)
        {
          btnReload.Enabled = false;
          MessageBox.Show(this, oEX.Message, "OPEN ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (DialogResult.OK == saveFile.ShowDialog(this))
      {
        txtl4nPath.Text = saveFile.FileName;
        SaveXmlDocument(moDoc, txtl4nPath.Text);
      }
    }

    private DataTable PrepareAppenderDT(XmlDocument oDoc)
    {
      DataTable table1 = new DataTable("Appenders");
      table1.Columns.Add("Alias Name", typeof(string));
      table1.Columns.Add("Appender Class", typeof(string));
      XmlNodeList list1 = oDoc.SelectNodes("//appender");
      foreach (XmlNode node1 in list1)
      {
        DataRow row1 = table1.NewRow();
        row1[table1.Columns[0]] = node1.Attributes["name"].Value;
        row1[table1.Columns[1]] = node1.Attributes["type"].Value;
        table1.Rows.Add(row1);
      }
      return table1;
    }

    private DataTable PrepareLoggerDT(XmlDocument oDoc)
    {
      DataRow row1;
      DataTable table1 = new DataTable("Loggers");
      table1.Columns.Add("Logger Name", typeof(string));
      table1.Columns.Add("Additivity", typeof(bool));
      XmlNode node1 = oDoc.SelectSingleNode("//root");
      if (node1 != null)
      {
        row1 = table1.NewRow();
        row1[table1.Columns[0]] = "root";
        row1[table1.Columns[1]] = (node1.Attributes["additivity"] == null) || bool.Parse(node1.Attributes["additivity"].Value);
        table1.Rows.Add(row1);
      }
      XmlNodeList list1 = oDoc.SelectNodes("//logger");
      foreach (XmlNode node2 in list1)
      {
        row1 = table1.NewRow();
        row1[table1.Columns[0]] = node2.Attributes["name"].Value;
        row1[table1.Columns[1]] = (node2.Attributes["additivity"] == null) || bool.Parse(node2.Attributes["additivity"].Value);
        table1.Rows.Add(row1);
      }
      return table1;
    }

    private void dgAppenders_CurrentCellChanged(object sender, EventArgs e)
    {
      DataGridCell cell1 = dgAppenders.CurrentCell;
      if (cell1.ColumnNumber == 0)
      {
        frmAppender oAppenderEditor = new frmAppender(moDoc);
        oAppenderEditor.EditExistedAppender((string)dgAppenders[cell1]);
        oAppenderEditor.ShowDialog(this);
      }
    }

    private void dgLoggers_CurrentCellChanged(object sender, EventArgs e)
    {
      DataGridCell cell1 = dgLoggers.CurrentCell;
      if (cell1.ColumnNumber == 0)
      {
        frmLogger oLoggerEditor = new frmLogger(moDoc);
        oLoggerEditor.EditExistedLogger((string)dgLoggers[cell1]);
        oLoggerEditor.ShowDialog(this);
      }
    }

    private void btnAddAppender_Click(object sender, EventArgs e)
    {
      frmAppender oAppenderEditor = new frmAppender(moDoc);
      if (DialogResult.OK == oAppenderEditor.ShowDialog(this))
      {
        BindAppenderGrid();
      }
    }

    private void btnRemoveAppender_Click(object sender, EventArgs e)
    {
      string sAliasName = (string)dgAppenders[dgAppenders.CurrentRowIndex, 0];
      foreach (XmlNode oApder in moDoc.SelectNodes("//appender"))
      {
        if (oApder.Attributes["name"].Value == sAliasName)
        {
          moDoc.DocumentElement.RemoveChild(oApder);
          break;
        }
      }
    }

    private void btnAddLogger_Click(object sender, EventArgs e)
    {
      frmLogger oLoggerEditor = new frmLogger(moDoc);
      if (DialogResult.OK == oLoggerEditor.ShowDialog(this))
      {
        BindLoggerGrid();
      }
    }

    private void btnRemoveLogger_Click(object sender, EventArgs e)
    {
      string sLoggerName = (string)dgLoggers[dgLoggers.CurrentRowIndex, 0];
      foreach (XmlNode node1 in moDoc.SelectNodes("//logger"))
      {
        if (node1.Attributes["name"].Value == sLoggerName)
        {
          moDoc.DocumentElement.RemoveChild(node1);
          break;
        }
      }
    }

    private static void SaveXmlDocument(XmlDocument oDoc, string sXmlFilePath)
    {
      XmlTextWriter writer1 = new XmlTextWriter(sXmlFilePath, Encoding.UTF8);
      try
      {
        writer1.Formatting = Formatting.Indented;
        writer1.Indentation = 4;
        oDoc.WriteTo(writer1);
        writer1.Flush();
      }
      catch (Exception exception1)
      {
        MessageBox.Show(exception1.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return;
      }
      finally
      {
        if (writer1 != null)
        {
          writer1.Close();
        }
      }
    }
  }
}
