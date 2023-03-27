using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml;
using Log4netConfigConsulter;

namespace Log4netEditor
{
  /// <summary>
  /// frmAppender ªººK­n´y­z¡C
  /// </summary>
  public class frmAppender : Form
  {
    private bool bSkipScrollChangeEvent;
    private const string msCONST_DefaultAppender = "OutputDebugStringAppender";
    private const int mnCONST_SpacingInControls = 5;
    private AppenderConsulter moConsulter;
    private XmlDocument moXmlDoc;
    private string msAliasName;

    #region Form Controls
    private Button btnCancel;
    private Button btnSave;
    private IContainer components;
    private ComboBox ddlAppenderClasses;
    private GroupBox gpArgContainer;
    private Label lblAlias;
    private Label lblAppenderClass;
    private Label lblDesc;
    private Panel pnlArguments;
    private ToolTip toolTip;
    private TextBox txtAlias;
    private TextBox txtDesc;
    private GroupBox gbLayout;
    private Label lblLayoutType;
    private ComboBox ddlLayoutType;
    private TextBox txtConversionPattern;
    private Label lblCnsnPtn;
    private Label lblPtnPreview;
    private Label lblPreviewResult;
    private Label lblDemoString;
    private TextBox txtDemoString;
    private Button btnPatternHelp;
    #endregion

    public XmlDocument Current_Log4net_config_XmlDoc
    {
      get { return moXmlDoc; }
    }

    public frmAppender(XmlDocument log4net_config_XmlDoc)
    {
      bSkipScrollChangeEvent = true;
      moXmlDoc = log4net_config_XmlDoc;
      InitializeComponent();
      InitAppenderDropDownList();
    }

    private void ArrangeControls()
    {
      int nTop = 0;
      int nLeftSpace = (int)(pnlArguments.Width * 0.1);
      if (pnlArguments.Controls.Count > 0)
      {
        foreach (Control ArgControl in pnlArguments.Controls)
        {
          if (gbLayout != ArgControl)
          {
            ArgControl.Visible = true;
            ArgControl.Width = (int)(pnlArguments.Width * 0.8);
            ArgControl.Left = nLeftSpace;
          }

          ArgControl.Top = nTop;
          nTop += ArgControl.Height + 5;
        }
      }
    }

    private void BtnCancel_Click(object sender, EventArgs e)
    {
      Close();
      Dispose();
    }

    private void BtnSave_Click(object sender, EventArgs e)
    {
      XmlNode oCurrentAppender = null;
      msAliasName = txtAlias.Text;

      #region Update arguments
      foreach (Control control1 in pnlArguments.Controls)
      {
        if (!(control1 is Label))
        {
          if (control1 is TextBox)
          {
            moConsulter.SearchUpdateArg((string)control1.Tag, ((TextBox)control1).Text);
          }
          else if (control1 is ComboBox)
          {
            moConsulter.SearchUpdateArg((string)control1.Tag, ((ComboBox)control1).Text);
          }
          else if (control1 is ParameterGrid)
          {
            moConsulter.SearchUpdateArg((string)control1.Tag, string.Empty).ParameterXml = ((ParameterGrid)control1).ParameterXmlNodes;
          }
        }
      }
      #endregion

      #region Update Layout & pattern
      moConsulter.SearchUpdateArg("layout", ddlLayoutType.Text);
      moConsulter.SearchUpdateArg("conversionpattern", txtConversionPattern.Text);
      #endregion

      #region Update & Save log4net.config for this appender
      foreach (XmlNode oAppender in moXmlDoc.SelectNodes("//appender"))
      {
        if (oAppender.Attributes["name"].Value == msAliasName)
        {
          oCurrentAppender = oAppender;
          break;
        }
      }
      try
      {
        if (oCurrentAppender == null)
        {
          oCurrentAppender = moXmlDoc.CreateElement("appender");
          XmlAttribute oAttri = moXmlDoc.CreateAttribute("name");
          oAttri.Value = msAliasName;
          oCurrentAppender.Attributes.Append(oAttri);
          oAttri = moXmlDoc.CreateAttribute("type");
          oAttri.Value = "log4net.Appender." + ddlAppenderClasses.Text;
          oCurrentAppender.Attributes.Append(oAttri);
          oCurrentAppender.InnerXml = moConsulter.GetConfigXML();
          moXmlDoc.DocumentElement.InsertBefore(oCurrentAppender, moXmlDoc.DocumentElement.FirstChild);
        }
        else
        {
          oCurrentAppender.Attributes["type"].Value = "log4net.Appender." + ddlAppenderClasses.Text;
          oCurrentAppender.InnerXml = moConsulter.GetConfigXML();
        }
      }
      catch (Exception oEX)
      {
        MessageBox.Show(this, oEX.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      #endregion
      DialogResult = DialogResult.OK;

      Close();
    }

    private void DdlAppenderClasses_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (bSkipScrollChangeEvent)
      {
        bSkipScrollChangeEvent = false;
      }
      else
      {
        moConsulter = null;
        InitGroupArgument();
      }
    }

    public void EditExistedAppender(string sAlias)
    {
      XmlNode node1 = null;
      msAliasName = sAlias;
      foreach (XmlNode node2 in moXmlDoc.SelectNodes("//appender"))
      {
        if (node2.Attributes["name"].Value == msAliasName)
        {
          node1 = node2;
          break;
        }
      }
      if (node1 == null)
      {
        throw new ApplicationException("This Appender named '" + msAliasName + "' is not existed in this log4net config file.");
      }

      txtAlias.Text = msAliasName;
      txtAlias.ReadOnly = true;
      try
      {
        string[] textArray1 = node1.Attributes["type"].Value.Split(new char[] { '.' });
        bSkipScrollChangeEvent = true;
        ddlAppenderClasses.Text = textArray1[textArray1.Length - 1];
        moConsulter = AppenderConsulter.GetAppender(textArray1[textArray1.Length - 1]);
        if (null == moConsulter)
        {
          moConsulter = AppenderConsulter.GetAppender(msCONST_DefaultAppender);
        }

        moConsulter.RestoreArgsFromXml(node1);
      }
      catch (InvalidCastException)
      {
        moConsulter = null;
      }

      InitGroupArgument();
    }

    private void InitAppenderDropDownList()
    {
      ddlAppenderClasses.DataSource = Helper.GetAppenders();
    }

    private void InitGroupArgument()
    {
      if (moConsulter == null)
      {
        try
        {
          moConsulter = AppenderConsulter.GetAppender(ddlAppenderClasses.Text);
        }
        catch (InvalidCastException)
        {
          moConsulter = null;
        }
      }

      pnlArguments.Controls.Clear();
      if (moConsulter == null)
      {
        txtDesc.Text = "Default basic consulter can't work.";
        btnSave.Enabled = false;
      }
      else
      {
        txtDesc.Text = moConsulter.GetAppenderDesc();
      }

      btnSave.Enabled = true;
      ArgumentStruct[] structArray1 = moConsulter.Arguments;
      if (structArray1 != null)
      {
        foreach (ArgumentStruct struct1 in structArray1)
        {
          RecursiveGenerateControls(struct1);
        }
      }

      if (Constants.msCONST_NOLAYOUT_APPENDER.ToLower() != ddlAppenderClasses.Text.ToLower())
      {
        pnlArguments.Controls.Add(gbLayout);
      }

      ArrangeControls();
    }


    #region Code generated by Visual Studio 2003
    private void InitializeComponent()
    {
      components = new Container();
      lblAlias = new Label();
      lblAppenderClass = new Label();
      ddlAppenderClasses = new ComboBox();
      txtAlias = new TextBox();
      btnSave = new Button();
      btnCancel = new Button();
      lblDesc = new Label();
      txtDesc = new TextBox();
      toolTip = new ToolTip(components);
      btnPatternHelp = new Button();
      gpArgContainer = new GroupBox();
      pnlArguments = new Panel();
      gbLayout = new GroupBox();
      txtDemoString = new TextBox();
      lblDemoString = new Label();
      lblPreviewResult = new Label();
      lblPtnPreview = new Label();
      lblCnsnPtn = new Label();
      txtConversionPattern = new TextBox();
      ddlLayoutType = new ComboBox();
      lblLayoutType = new Label();
      gpArgContainer.SuspendLayout();
      pnlArguments.SuspendLayout();
      gbLayout.SuspendLayout();
      SuspendLayout();
      // 
      // lblAlias
      // 
      lblAlias.AutoSize = true;
      lblAlias.Location = new System.Drawing.Point(8, 48);
      lblAlias.Name = "lblAlias";
      lblAlias.Size = new System.Drawing.Size(47, 15);
      lblAlias.TabIndex = 0;
      lblAlias.Text = "Name :";
      // 
      // lblAppenderClass
      // 
      lblAppenderClass.AutoSize = true;
      lblAppenderClass.Location = new System.Drawing.Point(8, 104);
      lblAppenderClass.Name = "lblAppenderClass";
      lblAppenderClass.Size = new System.Drawing.Size(95, 15);
      lblAppenderClass.TabIndex = 1;
      lblAppenderClass.Text = "Appender Type :";
      // 
      // ddlAppenderClasses
      // 
      ddlAppenderClasses.DropDownStyle = ComboBoxStyle.DropDownList;
      ddlAppenderClasses.Location = new System.Drawing.Point(40, 128);
      ddlAppenderClasses.Name = "ddlAppenderClasses";
      ddlAppenderClasses.Size = new System.Drawing.Size(368, 23);
      ddlAppenderClasses.TabIndex = 2;
      ddlAppenderClasses.SelectedIndexChanged += new EventHandler(DdlAppenderClasses_SelectedIndexChanged);
      // 
      // txtAlias
      // 
      txtAlias.Location = new System.Drawing.Point(40, 80);
      txtAlias.Name = "txtAlias";
      txtAlias.Size = new System.Drawing.Size(368, 21);
      txtAlias.TabIndex = 1;
      // 
      // btnSave
      // 
      btnSave.Location = new System.Drawing.Point(392, 552);
      btnSave.Name = "btnSave";
      btnSave.Size = new System.Drawing.Size(75, 23);
      btnSave.TabIndex = 5;
      btnSave.Text = "&Save";
      btnSave.Click += new EventHandler(BtnSave_Click);
      // 
      // btnCancel
      // 
      btnCancel.DialogResult = DialogResult.Cancel;
      btnCancel.Location = new System.Drawing.Point(312, 552);
      btnCancel.Name = "btnCancel";
      btnCancel.Size = new System.Drawing.Size(75, 23);
      btnCancel.TabIndex = 6;
      btnCancel.Text = "&Cancel";
      btnCancel.Click += new EventHandler(BtnCancel_Click);
      // 
      // lblDesc
      // 
      lblDesc.AutoSize = true;
      lblDesc.Location = new System.Drawing.Point(8, 160);
      lblDesc.Name = "lblDesc";
      lblDesc.Size = new System.Drawing.Size(76, 15);
      lblDesc.TabIndex = 0;
      lblDesc.Text = "Description :";
      // 
      // txtDesc
      // 
      txtDesc.Location = new System.Drawing.Point(40, 184);
      txtDesc.Multiline = true;
      txtDesc.Name = "txtDesc";
      txtDesc.ReadOnly = true;
      txtDesc.ScrollBars = ScrollBars.Vertical;
      txtDesc.Size = new System.Drawing.Size(424, 56);
      txtDesc.TabIndex = 7;
      // 
      // btnPatternHelp
      // 
      btnPatternHelp.FlatStyle = FlatStyle.Popup;
      btnPatternHelp.Location = new System.Drawing.Point(384, 8);
      btnPatternHelp.Name = "btnPatternHelp";
      btnPatternHelp.Size = new System.Drawing.Size(32, 23);
      btnPatternHelp.TabIndex = 8;
      btnPatternHelp.Text = "?";
      toolTip.SetToolTip(btnPatternHelp, "How to use?");
      btnPatternHelp.Click += new EventHandler(btnPatternHelp_Click);
      // 
      // gpArgContainer
      // 
      gpArgContainer.Controls.Add(pnlArguments);
      gpArgContainer.Location = new System.Drawing.Point(8, 248);
      gpArgContainer.Name = "gpArgContainer";
      gpArgContainer.Size = new System.Drawing.Size(456, 296);
      gpArgContainer.TabIndex = 8;
      gpArgContainer.TabStop = false;
      gpArgContainer.Text = "Arguments";
      // 
      // pnlArguments
      // 
      pnlArguments.AutoScroll = true;
      pnlArguments.Controls.Add(gbLayout);
      pnlArguments.Location = new System.Drawing.Point(8, 17);
      pnlArguments.Name = "pnlArguments";
      pnlArguments.Size = new System.Drawing.Size(442, 271);
      pnlArguments.TabIndex = 0;
      // 
      // gbLayout
      // 
      gbLayout.Controls.Add(btnPatternHelp);
      gbLayout.Controls.Add(txtDemoString);
      gbLayout.Controls.Add(lblDemoString);
      gbLayout.Controls.Add(lblPreviewResult);
      gbLayout.Controls.Add(lblPtnPreview);
      gbLayout.Controls.Add(lblCnsnPtn);
      gbLayout.Controls.Add(txtConversionPattern);
      gbLayout.Controls.Add(ddlLayoutType);
      gbLayout.Controls.Add(lblLayoutType);
      gbLayout.Location = new System.Drawing.Point(0, 0);
      gbLayout.Name = "gbLayout";
      gbLayout.Size = new System.Drawing.Size(424, 272);
      gbLayout.TabIndex = 1;
      gbLayout.TabStop = false;
      gbLayout.Text = "Layout";
      // 
      // txtDemoString
      // 
      txtDemoString.Location = new System.Drawing.Point(32, 168);
      txtDemoString.Name = "txtDemoString";
      txtDemoString.Size = new System.Drawing.Size(376, 21);
      txtDemoString.TabIndex = 7;
      txtDemoString.Text = "Demo info(You may change it)";
      txtDemoString.TextChanged += new EventHandler(txtDemoString_TextChanged);
      // 
      // lblDemoString
      // 
      lblDemoString.AutoSize = true;
      lblDemoString.Location = new System.Drawing.Point(24, 144);
      lblDemoString.Name = "lblDemoString";
      lblDemoString.Size = new System.Drawing.Size(82, 15);
      lblDemoString.TabIndex = 6;
      lblDemoString.Text = "Demo String :";
      // 
      // lblPreviewResult
      // 
      lblPreviewResult.BorderStyle = BorderStyle.Fixed3D;
      lblPreviewResult.Location = new System.Drawing.Point(32, 224);
      lblPreviewResult.Name = "lblPreviewResult";
      lblPreviewResult.Size = new System.Drawing.Size(376, 40);
      lblPreviewResult.TabIndex = 5;
      lblPreviewResult.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // lblPtnPreview
      // 
      lblPtnPreview.AutoSize = true;
      lblPtnPreview.Location = new System.Drawing.Point(24, 200);
      lblPtnPreview.Name = "lblPtnPreview";
      lblPtnPreview.Size = new System.Drawing.Size(98, 15);
      lblPtnPreview.TabIndex = 4;
      lblPtnPreview.Text = "Pattern Preview :";
      // 
      // lblCnsnPtn
      // 
      lblCnsnPtn.AutoSize = true;
      lblCnsnPtn.Location = new System.Drawing.Point(24, 88);
      lblCnsnPtn.Name = "lblCnsnPtn";
      lblCnsnPtn.Size = new System.Drawing.Size(118, 15);
      lblCnsnPtn.TabIndex = 3;
      lblCnsnPtn.Text = "Conversion Pattern :";
      // 
      // txtConversionPattern
      // 
      txtConversionPattern.Location = new System.Drawing.Point(32, 112);
      txtConversionPattern.Name = "txtConversionPattern";
      txtConversionPattern.Size = new System.Drawing.Size(376, 21);
      txtConversionPattern.TabIndex = 2;
      txtConversionPattern.TextChanged += new EventHandler(txtConversionPattern_TextChanged);
      // 
      // ddlLayoutType
      // 
      ddlLayoutType.DropDownStyle = ComboBoxStyle.DropDownList;
      ddlLayoutType.Location = new System.Drawing.Point(32, 48);
      ddlLayoutType.Name = "ddlLayoutType";
      ddlLayoutType.Size = new System.Drawing.Size(376, 23);
      ddlLayoutType.TabIndex = 1;
      // 
      // lblLayoutType
      // 
      lblLayoutType.AutoSize = true;
      lblLayoutType.Location = new System.Drawing.Point(24, 24);
      lblLayoutType.Name = "lblLayoutType";
      lblLayoutType.Size = new System.Drawing.Size(39, 15);
      lblLayoutType.TabIndex = 0;
      lblLayoutType.Text = "Type :";
      // 
      // frmAppender
      // 
      AutoScaleBaseSize = new System.Drawing.Size(6, 14);
      ClientSize = new System.Drawing.Size(472, 584);
      Controls.Add(txtDesc);
      Controls.Add(txtAlias);
      Controls.Add(lblAppenderClass);
      Controls.Add(lblAlias);
      Controls.Add(lblDesc);
      Controls.Add(btnCancel);
      Controls.Add(btnSave);
      Controls.Add(ddlAppenderClasses);
      Controls.Add(gpArgContainer);
      Font = new System.Drawing.Font("Arial", 9F);
      FormBorderStyle = FormBorderStyle.FixedSingle;
      MaximizeBox = false;
      MinimizeBox = false;
      Name = "frmAppender";
      Text = "Log4net Appender Editor";
      gpArgContainer.ResumeLayout(false);
      pnlArguments.ResumeLayout(false);
      gbLayout.ResumeLayout(false);
      gbLayout.PerformLayout();
      ResumeLayout(false);
      PerformLayout();
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

    private void RecursiveGenerateControls(ArgumentStruct oArg)
    {
      Control control1;
      switch (oArg.Name)
      {
        case "layout":
          foreach (string sEnumValue in oArg.EnumValues)
          {
            ddlLayoutType.Items.Add(sEnumValue);
          }

          ddlLayoutType.Text = oArg.Value;
          break;
        case "conversionPattern":
          txtConversionPattern.Text = oArg.Value;
          break;
        default:
          Label label1 = new Label();
          label1.Text = oArg.Name + " : ";
          label1.AutoSize = true;
          switch (oArg.UIType)
          {
            case UIControlType.MultiLineTextBox:
              control1 = new TextBox();
              ((TextBox)control1).Multiline = true;
              ((TextBox)control1).ScrollBars = ScrollBars.Both;
              ((TextBox)control1).Text = oArg.Value;
              control1.Height *= 5;
              break;

            case UIControlType.DropDownList:
              control1 = new ComboBox();
              ((ComboBox)control1).DropDownStyle = ComboBoxStyle.DropDownList;
              foreach (string text2 in oArg.EnumValues)
              {
                ((ComboBox)control1).Items.Add(text2);
              }
              ((ComboBox)control1).Text = oArg.Value;
              break;

            case UIControlType.ListBox:
              control1 = new ListBox();
              foreach (string text1 in oArg.EnumValues)
              {
                ((ListBox)control1).Items.Add(text1);
              }
              ((ListBox)control1).Text = oArg.Value;
              break;

            case UIControlType.ParameterGrid:
              control1 = new ParameterGrid();
              ((ParameterGrid)control1).ParameterXmlNodes = oArg.ParameterXml;
              break;

            case UIControlType.None:
              control1 = new Label();
              break;

            default:
              control1 = new TextBox();
              ((TextBox)control1).Text = oArg.Value;
              break;
          }

          control1.Tag = oArg.Name;
          toolTip.SetToolTip(control1, oArg.Description);
          pnlArguments.Controls.Add(label1);
          pnlArguments.Controls.Add(control1);
          break;
      }

      if (oArg.ChildArguments != null)
      {
        foreach (ArgumentStruct struct1 in oArg.ChildArguments)
        {
          RecursiveGenerateControls(struct1);
        }
      }
    }

    public new DialogResult ShowDialog()
    {
      return ShowDialog(null);
    }

    public new DialogResult ShowDialog(IWin32Window owner)
    {
      if (moConsulter == null)
      {
        InitGroupArgument();
      }

      return base.ShowDialog(owner);
    }

    public string CurrentAliasName
    {
      get
      {
        return msAliasName;
      }
    }

    private void txtConversionPattern_TextChanged(object sender, EventArgs e)
    {
      RenderPatternDemo();
    }

    private void txtDemoString_TextChanged(object sender, EventArgs e)
    {
      RenderPatternDemo();
    }

    private void RenderPatternDemo()
    {
      try
      {
        lblPreviewResult.Text = Helper.PreviewPattern(txtDemoString.Text, ddlLayoutType.Text, txtConversionPattern.Text, this).Replace("\t", "    ");
      }
      catch (Exception exception)
      {
        MessageBox.Show(exception.ToString(), exception.Message);
      }
    }

    private void btnPatternHelp_Click(object sender, EventArgs e)
    {
      // old url: http://logging.apache.org/log4net/release/sdk/log4net.Layout.PatternLayout.html
      // new url: https://logging.apache.org/log4net/log4net-1.2.13/release/sdk/log4net.Layout.PatternLayout.html
      Process.Start("IExplore.exe", "https://logging.apache.org/log4net/log4net-1.2.13/release/sdk/log4net.Layout.PatternLayout.html");
    }
  }
}
