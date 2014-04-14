namespace _3DSExplorer.TreeListView
{
    partial class MultiColumnTreeView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // MultiColumnTreeView
            // 
            this.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.LineColor = System.Drawing.Color.Black;
            this.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterCollapse);
            this.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.treeView_DrawNode);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeView_MouseDown);
            this.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterExpand);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
