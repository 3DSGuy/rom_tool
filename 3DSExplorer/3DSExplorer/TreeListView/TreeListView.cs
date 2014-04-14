using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Drawing.Design;

namespace _3DSExplorer.TreeListView
{
    public partial class TreeListViewControl : UserControl, IColumnsProvider
    {
        public TreeListViewControl()
        {
            InitializeComponent();
            DoubleBuffered = true;
            treeView.ColumnProvider = this;
        }

        public const char ColumnSeperator = '©';

        public TreeNode SelectedNode { get { return treeView.SelectedNode; } set { treeView.SelectedNode = value; } }

        [MergableProperty(false)]
        [Editor("System.Windows.Forms.Design.ColumnHeaderCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Localizable(true)]
        public ListView.ColumnHeaderCollection Columns { get { return listView.Columns; } }
        public TreeView TreeView { get { return treeView; } }
        
        [Category("Behavior")]
        public ImageList ImageList { set { treeView.ImageList = value; }  get { return treeView.ImageList; } }
        public int TotalColWidth { get { return Columns.Cast<ColumnHeader>().Sum(col => col.Width); } }
        public TreeNodeCollection Nodes { get  { return treeView.Nodes; } }

        public static string CreateMultiColumnNodeText(params string[] texts)
        {
            return string.Concat(texts.Select(t => t.Replace(ColumnSeperator.ToString(), "") + ColumnSeperator.ToString()).ToArray());//.PadRight(400);
        }

        public void AddSubItem(TreeNode tn, string text)
        {
            tn.Text += ColumnSeperator + text;
        }

        public void ExpandAll()
        {
            treeView.ExpandAll();
        }

        public TreeNode NodeAt(Point point)
        {
            return treeView.HitTest(point).Node;
        }

        public string GetMainText(TreeNode tn)
        {
            return !tn.Text.Contains(ColumnSeperator) ? tn.Text : tn.Text.Substring(0,tn.Text.IndexOf(ColumnSeperator));
        }

        private void listView_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            treeView.Refresh();
        }

        private void listView_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            if (TotalColWidth > ClientSize.Width)
            {
                e.Cancel = true;
                e.NewWidth = ClientSize.Width - (TotalColWidth - Columns[e.ColumnIndex].Width);
            }
            treeView.Refresh();            
        }

        [Category("Action")]
        [Description("Fires when the treeview being double clicked.")]
        public event MouseEventHandler TreeDoubleClicked;

        [Category("Action")]
        [Description("Fires when the treeview being clicked.")]
        public event MouseEventHandler TreeMouseClicked;

        public List<TreeNode> TreeNodeListRecursive
        {
            get { return treeView.TreeNodeListRecursive; }
        }

        public List<TreeNode> ExposedTreeNodes
        {
            get { return treeView.ExposedNodes; }
        }

        private void TreeListViewControl_EnabledChanged(object sender, EventArgs e)
        {
            treeView.Enabled = Enabled;
            listView.Enabled = Enabled;
            listView.HeaderStyle = Enabled ? ColumnHeaderStyle.Clickable : ColumnHeaderStyle.Nonclickable;
        }

        private void treeView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            TreeDoubleClicked(sender, e);
        }

        private void treeView_MouseClick(object sender, MouseEventArgs e)
        {
            TreeMouseClicked(sender, e);
        }

    }
}
