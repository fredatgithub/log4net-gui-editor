using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace Log4netEditor
{
  public class ParameterGrid : UserControl
  {
    // Fields
    private Container components = null;
    private DataGridTextBoxColumn conversionPattern;
    private DataGridTextBoxColumn dbType;
    private DataGrid dgParameter;
    private DataGridTextBoxColumn layout;
    private DataGridTextBoxColumn parameterName;
    private DataGridTextBoxColumn size;
    private DataGridTableStyle tsParameters;

    // Methods
    public ParameterGrid()
    {
      InitializeComponent();
      dsADOParameters.ParametersDataTable table = new dsADOParameters.ParametersDataTable();
      dgParameter.DataSource = table;
    }

    private XmlNode CreateParamNode(string NodeName, string ValueName, string Value, XmlDocument oDoc)
    {
      XmlNode node = oDoc.CreateNode(XmlNodeType.Element, NodeName, string.Empty);
      XmlAttribute attribute = oDoc.CreateAttribute(ValueName);
      attribute.Value = Value;
      node.Attributes.Append(attribute);
      return node;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }

      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      dgParameter = new DataGrid();
      tsParameters = new DataGridTableStyle();
      parameterName = new DataGridTextBoxColumn();
      dbType = new DataGridTextBoxColumn();
      layout = new DataGridTextBoxColumn();
      conversionPattern = new DataGridTextBoxColumn();
      size = new DataGridTextBoxColumn();
      dgParameter.BeginInit();
      SuspendLayout();
      dgParameter.CaptionText = "Parameters";
      dgParameter.DataMember = "";
      dgParameter.HeaderForeColor = SystemColors.ControlText;
      dgParameter.Location = new Point(0, 0);
      dgParameter.Name = "dgParameter";
      dgParameter.Size = new Size(0x200, 0x108);
      dgParameter.TabIndex = 0;
      dgParameter.TableStyles.AddRange(new DataGridTableStyle[] { tsParameters });
      tsParameters.DataGrid = dgParameter;
      tsParameters.GridColumnStyles.AddRange(new DataGridColumnStyle[] { parameterName, dbType, layout, conversionPattern, size });
      tsParameters.HeaderForeColor = SystemColors.ControlText;
      tsParameters.MappingName = "Parameters";
      parameterName.Format = "";
      parameterName.FormatInfo = null;
      parameterName.HeaderText = "Parameter Name";
      parameterName.MappingName = "parameterName";
      parameterName.Width = 0x4b;
      dbType.Format = "";
      dbType.FormatInfo = null;
      dbType.HeaderText = "DBType";
      dbType.MappingName = "dbType";
      dbType.Width = 0x4b;
      layout.Format = "";
      layout.FormatInfo = null;
      layout.HeaderText = "Layout";
      layout.MappingName = "layout";
      layout.Width = 0x4b;
      conversionPattern.Format = "";
      conversionPattern.FormatInfo = null;
      conversionPattern.HeaderText = "Conversion Pattern";
      conversionPattern.MappingName = "conversionPattern";
      conversionPattern.Width = 0x4b;
      size.Format = "";
      size.FormatInfo = null;
      size.HeaderText = "size";
      size.MappingName = "size";
      size.Width = 0x4b;
      Controls.Add(dgParameter);
      Font = new Font("Arial", 9f, FontStyle.Regular, GraphicsUnit.Point, 0x88);
      Name = "ParameterGrid";
      Size = new Size(520, 0x110);
      Resize += new EventHandler(ParameterGrid_Resize);
      dgParameter.EndInit();
      ResumeLayout(false);
    }

    private void ParameterGrid_Resize(object sender, EventArgs e)
    {
      dgParameter.Width = Width;
      dgParameter.Height = Height;
    }

    // Properties
    public XmlNodeList ParameterXmlNodes
    {
      get
      {
        XmlDocument oDoc = new XmlDocument();
        XmlNode node = oDoc.CreateNode(XmlNodeType.Element, "parameters", string.Empty);
        IEnumerator enumerator = ((dsADOParameters.ParametersDataTable)dgParameter.DataSource).GetEnumerator();
        try
        {
          while (enumerator.MoveNext())
          {
            dsADOParameters.ParametersRow row = (dsADOParameters.ParametersRow)enumerator.Current;
            XmlNode newChild = oDoc.CreateNode(XmlNodeType.Element, "parameter", string.Empty);
            node.AppendChild(newChild);
            newChild.AppendChild(CreateParamNode("parameterName", "value", row.parameterName, oDoc));
            newChild.AppendChild(CreateParamNode("dbType", "value", row.dbType, oDoc));
            if (row.size != 0)
            {
              newChild.AppendChild(CreateParamNode("size", "value", ((int)row.size).ToString(), oDoc));
            }
            XmlNode node2 = CreateParamNode("layout", "type", row.layout, oDoc);
            newChild.AppendChild(node2);
            if ((row.conversionPattern != null) && (string.Empty != row.conversionPattern))
            {
              node2.AppendChild(CreateParamNode("conversionPattern", "value", row.conversionPattern, oDoc));
            }
          }
        }
        finally
        {
          IDisposable disposable = enumerator as IDisposable;
          if (disposable != null)
          {
            disposable.Dispose();
          }
        }

        return node.SelectNodes("//parameter");
      }

      set
      {
        if (value != null)
        {
          dsADOParameters.ParametersDataTable table = (dsADOParameters.ParametersDataTable)dgParameter.DataSource;
          table.Clear();
          try
          {
            IEnumerator enumerator = value.GetEnumerator();
            try
            {
              while (enumerator.MoveNext())
              {
                XmlNode node = (XmlNode)enumerator.Current;
                dsADOParameters.ParametersRow row = table.NewParametersRow();
                row.parameterName = node.SelectSingleNode("parameterName").Attributes["value"].Value;
                row.dbType = node.SelectSingleNode("dbType").Attributes["value"].Value;
                row.layout = node.SelectSingleNode("layout").Attributes["type"].Value;
                XmlNode node2 = node.SelectSingleNode("size");
                if (node2 != null)
                {
                  row.size = int.Parse(node2.Attributes["value"].Value);
                }
                node2 = node.SelectSingleNode("layout/conversionPattern");
                if (node2 != null)
                {
                  row.conversionPattern = node2.Attributes["value"].Value;
                }
                table.AddParametersRow(row);
              }
            }
            finally
            {
              IDisposable disposable = enumerator as IDisposable;
              if (disposable != null)
              {
                disposable.Dispose();
              }
            }
          }
          catch (Exception)
          {
          }
        }
      }
    }
  }
}
