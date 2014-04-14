using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _3DSExplorer.TreeListView
{
    internal partial class MultiColumnTreeView : TreeView
    {
        public MultiColumnTreeView()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            CreateHandle();                        
        }

        internal IColumnsProvider ColumnProvider { get; set; }

        internal List<TreeNode> ExposedNodes
        {
            get
            {
                List<TreeNode> nodes = new List<TreeNode>();
                foreach (TreeNode tn in TreeNodeListRecursive)
                {
                    Func<TreeNode, IList<TreeNode>, IEnumerable<TreeNode>> findParentsFunc =
                        (treenode, alreadyFound) =>
                        {
                            return new TreeNode[] { treenode.Parent }.Where(parent => parent != null);
                        };
                    var parents = GetRecursive(tn, new List<TreeNode>(), findParentsFunc).ToList();
                    if (parents.Count == 0 || parents.All(t => t.IsExpanded))
                        nodes.Add(tn);
                }
                return nodes;
            }
        }

        internal List<TreeNode> TreeNodeListRecursive
        {
            get
            {
                List<TreeNode> nodes = new List<TreeNode>(Nodes.Cast<TreeNode>());
                foreach (TreeNode tn in Nodes)
                {
                    nodes.AddRange(GetRecursive(tn, new List<TreeNode>(), (treenode, alreadyFoundList) => treenode.Nodes.Cast<TreeNode>()));
                }
                return nodes;
            }
        }

        protected override void WndProc(ref Message messg)
        {
            // turn the erase background message and the horizontal scroll message into a null message
            if ((int)0x0014 == messg.Msg || //if message is erase background
                (int)0x114 == messg.Msg)    //or horicontal scroll
            {
                messg.Msg = (int)0x0000;    //reset message to null
            }
            if ((int)0x110F == messg.Msg)   //Scheint die "Neuer Node"-Nachricht zu sein
            {
                OnHideOrShowScrollBar();
            }
            base.WndProc(ref messg);
        }

        private void treeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if (!e.Node.IsVisible) return;

            Brush bg, fg;
            if (e.Node.IsSelected ||
                e.State == TreeNodeStates.Focused ||
                e.State == TreeNodeStates.Hot ||
                e.State == TreeNodeStates.Marked ||
                e.State == TreeNodeStates.Selected ||
                e.State == TreeNodeStates.Checked ||
                e.State == TreeNodeStates.Indeterminate ||
                e.State == TreeNodeStates.ShowKeyboardCues)
            {
                bg = SystemBrushes.Highlight;
                fg = Brushes.White;
            }
            else
            {
                bg = Brushes.White;
                fg = Brushes.Black;
            }
            var newBounds = e.Bounds;
            newBounds.Width = e.Node.TreeView.ClientRectangle.Width;
            e.Graphics.FillRectangle(bg, newBounds);

            var texts = e.Node.Text.Split(TreeListViewControl.ColumnSeperator);
            int column = 0;
            float colX = 0;
            float colWidth = 0;
            foreach (var text in texts)
            {
                try
                {
                    float extraSpacing = ((column == 0) ? (float)(e.Bounds.X - e.Node.TreeView.Location.X) : 0f);
                    colWidth = (column == texts.Length - 1 ? e.Node.TreeView.Width : ColumnProvider.Columns[column].Width);
                    e.Graphics.DrawString(text, e.Node.TreeView.Font, fg, new RectangleF(colX + extraSpacing, e.Bounds.Y + 1, colWidth - extraSpacing, e.Bounds.Height - 2));
                    colX += ColumnProvider.Columns[column].Width;
                    column++;
                }
                catch { }
            }
        }
        
        private void treeView_MouseDown(object sender, MouseEventArgs e)
        {
            TreeNode node = null;
            for (int x = 0; x < Width; x += 20)
            {
                node = GetNodeAt(x, e.Y);
                if (node != null)
                    break;
            }
            if (node != null)
                SelectedNode = node;
        }
        
        private void treeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            OnHideOrShowScrollBar();
        }

        private void treeView_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            OnHideOrShowScrollBar();
        }

        internal class HideScrollBarEventArgs : EventArgs { public bool Hide { get; set; } }
        public event EventHandler<HideScrollBarEventArgs> HideOrShowScrollBar;

        public void OnHideOrShowScrollBar()
        {
            bool hideScrollBar = (Height - ClientSize.Height > 5);

            if (HideOrShowScrollBar != null)
                HideOrShowScrollBar.Invoke(this, new HideScrollBarEventArgs() { Hide = hideScrollBar });
        }               
        
        private static IEnumerable<T> GetRecursive<T>(T rootElement, IList<T> alreadyFoundElements, Func<T, IList<T>, IEnumerable<T>> findSubElementsFunction)
        {
            var subElements = findSubElementsFunction(rootElement, alreadyFoundElements).ToList();
            if (subElements.Count == 0 || subElements == null) return new T[] { };
            alreadyFoundElements = alreadyFoundElements.Union(subElements).ToList();
            foreach (T subE in subElements)
                alreadyFoundElements = alreadyFoundElements.Union(GetRecursive(subE, alreadyFoundElements, findSubElementsFunction)).ToList();
            return alreadyFoundElements;           
        }
    }
}
