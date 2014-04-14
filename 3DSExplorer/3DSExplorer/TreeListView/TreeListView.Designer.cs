namespace _3DSExplorer.TreeListView
{
    partial class TreeListViewControl
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
            this.listView = new System.Windows.Forms.ListView();
            this.treeView = new TreeListView.MultiColumnTreeView();
            this.SuspendLayout();
            // 
            // listView
            // 
            this.listView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listView.BackColor = System.Drawing.SystemColors.Menu;
            this.listView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listView.Location = new System.Drawing.Point(0, 0);
            this.listView.Name = "listView";
            this.listView.Scrollable = false;
            this.listView.Size = new System.Drawing.Size(100, 26);
            this.listView.TabIndex = 0;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            this.listView.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(this.listView_ColumnWidthChanged);
            this.listView.ColumnWidthChanging += new System.Windows.Forms.ColumnWidthChangingEventHandler(this.listView_ColumnWidthChanging);
            // 
            // treeView
            // 
            this.treeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.treeView.Location = new System.Drawing.Point(0, 26);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(100, 74);
            this.treeView.TabIndex = 1;
            this.treeView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.treeView_MouseClick);
            this.treeView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.treeView_MouseDoubleClick);
            // 
            // TreeListViewControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listView);
            this.Controls.Add(this.treeView);
            this.Name = "TreeListViewControl";
            this.Size = new System.Drawing.Size(100, 100);
            this.EnabledChanged += new System.EventHandler(this.TreeListViewControl_EnabledChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView;
        private TreeListView.MultiColumnTreeView treeView;
    }
}
