using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.ComponentModel;
using System.Net;
using _3DSExplorer.Modules;
using _3DSExplorer.Utils;

namespace _3DSExplorer
{
    public partial class frmExplorer : Form
    {
        private IContext _currentContext;
        private string _filePath, _remoteVer;
        private bool _checkNow;

        string file_path;

        public frmExplorer()
        {
            InitializeComponent();
            InitializeForm();
            if (Properties.Settings.Default.CheckForUpdatesOnStartup)
                bwCheckForUpdates.RunWorkerAsync();
        }

        public frmExplorer(string path) : this()
        {
            OpenFile(path);
        }

        private void InitializeForm()
        {
            Text = string.Format("{0} v.{1}",Application.ProductName,Application.ProductVersion);
            menuHelpCheckUpdates.Checked = Properties.Settings.Default.CheckForUpdatesOnStartup;
            frmExplorer_Resize(null, null);
        }

        #region Info ListView Functions

        public void SetGroupHeaders(params string[] groupHeader)
        {
            for (var i = 0; i < groupHeader.Length && i < lstInfo.Groups.Count; i++)
            {
                lstInfo.Groups[i].Header = groupHeader[i];
            }
        }

        public void ClearInformation()
        {
            lstInfo.Items.Clear();
        }

        public void AutoAlignColumns()
        {
            lstInfo.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }
        
        public void AddListItem(int offset, int size, string description, ulong value, int group)
        {
            var lvi = new ListViewItem("0x" + offset.ToString("X3"));
            lvi.SubItems.Add(size.ToString());
            lvi.SubItems.Add(description);
            lvi.SubItems.Add(value.ToString());
            lvi.SubItems.Add(StringUtil.ToHexString(size * 2,value));
            lvi.Group = lstInfo.Groups[group];
            lstInfo.Items.Add(lvi);
        }

        public void AddListItem(int offset, int size, string description, float value, int group)
        {
            var lvi = new ListViewItem("0x" + offset.ToString("X3"));
            lvi.SubItems.Add(size.ToString());
            lvi.SubItems.Add(description);
            lvi.SubItems.Add(value.ToString());
            lvi.SubItems.Add(StringUtil.ToHexString(size * 2, value));
            lvi.Group = lstInfo.Groups[group];
            lstInfo.Items.Add(lvi);
        }

        public void AddListItem(int offset, int size, string description, byte[] value, int group)
        {
            var lvi = new ListViewItem("0x" + offset.ToString("X3"));
            lvi.SubItems.Add(size.ToString());
            lvi.SubItems.Add(description);
            lvi.SubItems.Add("");
            lvi.SubItems.Add(StringUtil.ByteArrayToString(value));
            lvi.Group = lstInfo.Groups[group];
            lstInfo.Items.Add(lvi);
        }
        public void AddListItem(int offset, int size, string description, char[] value, int group)
        {
            var lvi = new ListViewItem("0x" + offset.ToString("X3"));
            lvi.SubItems.Add(size.ToString());
            lvi.SubItems.Add(description);
            lvi.SubItems.Add("");
            lvi.SubItems.Add(StringUtil.CharArrayToString(value));
            lvi.Group = lstInfo.Groups[group];
            lstInfo.Items.Add(lvi);
        }

        public void AddListItem(string offset, string size, string description, string value, string hexvalue, int group)
        {
            var lvi = new ListViewItem(offset);
            lvi.SubItems.Add(size);
            lvi.SubItems.Add(description);
            lvi.SubItems.Add(value);
            lvi.SubItems.Add(hexvalue);
            lvi.Group = lstInfo.Groups[group];
            lstInfo.Items.Add(lvi);
        }
                
        private void lstInfo_DoubleClick(object sender, EventArgs e)
        {
            if (lstInfo.SelectedIndices.Count <= 0) return;
            var toClip = lstInfo.SelectedItems[0].SubItems[3].Text == "" ? lstInfo.SelectedItems[0].SubItems[4].Text : lstInfo.SelectedItems[0].SubItems[3].Text;
            Clipboard.SetText(toClip);
            MessageBox.Show("Value copied to clipboard!");
        }
        
        #endregion

        private void OpenFile(string path)
        {
            menuToolsQuickCRC.Enabled = false;
            _filePath = path;
            var fs = new FileStream(_filePath, FileMode.Open, FileAccess.ReadWrite);//File.OpenWrite(_filePath));
            var type = ModuleHelper.GetModuleType(_filePath, fs);
            var tempContext = ModuleHelper.CreateByType(type);
            if (tempContext == null)
            {
                MessageBox.Show("This file is unsupported!");
                fs.Close();
                return;
            }
            fs.Seek(0, SeekOrigin.Begin);
            if (!tempContext.Open(fs))
            {
                MessageBox.Show(string.Format("Error: {0}",tempContext.GetErrorMessage()));
                fs.Close();
                return;
            }
            var fileSize = fs.Length;
            fs.Close();

            //Start the open process
            LoadText(_filePath);
            treeView.Nodes.Clear();
            var nodes = tempContext.GetExplorerTopNode();
            treeView.Nodes.Add(nodes);
            treeView.ExpandAll();
            lvFileTree.Nodes.Clear();
            nodes = tempContext.GetFileSystemTopNode();
            if (nodes != null)
                lvFileTree.Nodes.Add(nodes);
            lvFileTree.ExpandAll();

            _currentContext = tempContext;
            treeView.SelectedNode = treeView.Nodes[0];

            if (type == ModuleType.CCI)
            {
                file_path = path;
                var cciContext = (CCIContext)_currentContext;
                menuCci.Visible = false;
                if (cciContext.NcsdInfo.CCI_File_Status == 1 || cciContext.NcsdInfo.CCI_File_Status == 2) //Enable CCI Size Manipulation by 3DSGuy
                    handleCciMenu(cciContext);
            }
            menuFileSave.Enabled = _currentContext.CanCreate();
            menuToolsQuickCRC.Enabled = true;
        }

        private void handleCciMenu(CCIContext cciContext)
        {
            menuCci.Visible = true;
            var usedSize = cciContext.Header.UsedRomLength;
            if (cciContext.NcsdInfo.CCI_File_Status == 1) // Is Full size
            {
                menuCciTrim.Text = "Trim " + cciContext.NcsdInfo.ncsd_type;
            }
            else // Is Trimmed
            {
                menuCciTrim.Text = "Restore " + cciContext.NcsdInfo.ncsd_type;
            }
            if (cciContext.Header.CXIEntries[7].Length > 0) 
            {
                menuCciSuperTrim.Visible = true;
            }
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var tag = (TreeViewContextTag) e.Node.Tag;
            tag.Context.View(this, tag.Type, tag.Values);
            //BUG: lstInfo.SetListViewImage();
        }

        private void LoadText(string path)
        {
            lblTreeViewTitle.Text = Path.GetFileName(path);
        }

        #region Drag & Drop

        private void frmExplorer_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
                e.Effect = DragDropEffects.All;
        }

        private void frmExplorer_DragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            OpenFile(files[0]);
        }

        #endregion

        #region MENU File

        private void menuFileOpen_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = ModuleHelper.OpenString;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                OpenFile(openFileDialog.FileName);
        }

        private void menuFileSave_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = _currentContext.GetFileFilter() + "|All Files|*.*";
            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

            var outStream = File.OpenWrite(saveFileDialog.FileName);
            var inStream = File.OpenRead(_filePath);
            _currentContext.Create(outStream, inStream);
            inStream.Close();
            outStream.Close();
        }

        private void menuCciTrim_Click(object sender, EventArgs e)
        {
            var cciContext = (CCIContext)_currentContext;
            if (cciContext == null)
            {
                MessageBox.Show("How did you get here?");
                return;
            }

            if (cciContext.NcsdInfo.CCI_File_Status == 1)
            {
                if (MessageBox.Show("Are you sure you want to trim this " + cciContext.NcsdInfo.ncsd_type +"? (It might be a good idea to backup first)", "Trim " + cciContext.NcsdInfo.ncsd_type, MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
            }
            else
            {
                if (MessageBox.Show("Are you sure you want to restore this " + cciContext.NcsdInfo.ncsd_type + "? (It might be a good idea to backup first)", "Restore " + cciContext.NcsdInfo.ncsd_type, MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
            }
            
            try
            {
                var fs = File.OpenWrite(_filePath);
                cciContext.ToggleTrimmed(fs);
                switch (cciContext.NcsdInfo.CCI_File_Status) 
                {
                    case 1: menuCciTrim.Text = "&Trim..."; break;
                    case 2: menuCciTrim.Text = "&Restore..."; break; 
                }
                fs.Close();
                OpenFile(file_path); // Re-loading CCI
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            
        }

        private void menuFileSuperTrim_Click(object sender, EventArgs e)
        {
            var cciContext = (CCIContext)_currentContext;
            if (cciContext == null)
            {
                MessageBox.Show("How did you get here?");
                return;
            }
            if (MessageBox.Show("Are you sure you want to remove the Updata Data from this " + cciContext.NcsdInfo.ncsd_type + " ? (This is permanent, it might be a good idea to make a backup first)", "Remove Update Data", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            try
            {

                var fs = File.OpenWrite(_filePath);
                cciContext.SuperTrim(fs);
                fs.Close();
                OpenFile(file_path); // Re-loading CCI
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        private void menuFileExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        #region MENU Tools

        private void openForm<T>() where T : Form, new()
        {
            var form = (from Form f in Application.OpenForms where f.GetType().IsAssignableFrom(typeof (T)) select (T) f).FirstOrDefault() ??
                     new T();
            form.Show();
            form.BringToFront();
        }

        private void menuToolsXORTool_Click(object sender, EventArgs e)
        {
            openForm<frmXORTool>();
        }

        private void menuToolsHashTool_Click(object sender, EventArgs e)
        {
            openForm<frmHashTool>();
        }

        private void menuTools3DVideo_Click(object sender, EventArgs e)
        {
            openForm<frm3DVideo>();
        }

        private void menuToolsQuickCRC_Click(object sender, EventArgs e)
        {
            frmCheckSum.ShowDialog(_filePath);
        }

        private void menuToolsDSDecmpGUI_Click(object sender, EventArgs e)
        {
            openForm<frmDSDecmpGUI>();
        }

        private void menuToolsQRTool_Click(object sender, EventArgs e)
        {
            openForm<frmQRTool>();
        }

        private void menuToolsParentalControlUnlocker_Click(object sender, EventArgs e)
        {
            openForm<frmParentalControl>();
        }

        #endregion

        #region MENU Help

        private void menuHelpUpdateTitleDb_Click(object sender, EventArgs e)
        {
            bwUpdateTitleDb.RunWorkerAsync();
        }

        private void menuHelpCheckNow_Click(object sender, EventArgs e)
        {
            _checkNow = true;
            bwCheckForUpdates.RunWorkerAsync();
        }

        private void menuHelpCheckUpdates_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.CheckForUpdatesOnStartup = menuHelpCheckUpdates.Checked;
            Properties.Settings.Default.Save();
        }

        private void GoToUrl(string url)
        {
            try
            {
                System.Diagnostics.Process.Start(url);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("This system doesn't support clicking a link...\n\n{0}",ex.Message));
            }
        }

        private void menuHelpVisitGoogleCode_Click(object sender, EventArgs e)
        {
            GoToUrl("http://3dsexplorer.googlecode.com/");
        }

        private void menuHelpVisit3DBrew_Click(object sender, EventArgs e)
        {
            GoToUrl("http://www.3dbrew.org/");
        }

        private void menuHelpVisitNDev_Click(object sender, EventArgs e)
        {
            GoToUrl("http://www.n-dev.net");
        }

        #endregion
        
        private void menuLogo_Click(object sender, EventArgs e)
        {
            (new frmAbout()).ShowDialog();
        }

        #region CXTMENU FileContext

        private void cxtFileItemClick(object sender, EventArgs e)
        {
            var contextTag = (TreeViewContextTag)((ToolStripItem)sender).Tag;
            contextTag.Context.Activate(_filePath, contextTag.Type, contextTag.Values);
        }

        private void lvFileTree_TreeDoubleClicked(object sender, MouseEventArgs e)
        {
            var node = lvFileTree.NodeAt(e.Location);
            if (node == null) return;
            lvFileTree.TreeView.SelectedNode = node;
            switch (e.Button)
            {
                case MouseButtons.Left:

                    if (node.Tag != null)
                    {
                        var contextTag = ((TreeViewContextTag[])node.Tag)[0];
                        contextTag.Context.Activate(_filePath, contextTag.Type, contextTag.Values);
                    }
                    break;
            }
        }

        private void lvFileTree_TreeMouseClicked(object sender, MouseEventArgs e)
        {
            var node = lvFileTree.NodeAt(e.Location);
            if (node == null) return;
            lvFileTree.TreeView.SelectedNode = node;
            switch (e.Button)
            {
                case MouseButtons.Right:
                        
                    if (node.Tag != null)
                    {
                        var tags = (TreeViewContextTag[])node.Tag;
                        cxtFile.Items.Clear();
                        for (var i = 0; i < tags.Length; i++)
                        {
                            var toolItem = cxtFile.Items.Add(tags[i].ActivationString, null, cxtFileItemClick);
                            toolItem.Tag = tags[i];
                        }
                        cxtFile.Items[0].Font = new Font(cxtFile.Items[0].Font, FontStyle.Bold);
                        cxtFile.Show(lvFileTree.TreeView, e.Location);
                    }
                    break;
            }
        }
        #endregion

        #region Check for updates
        private void bwCheckForUpdates_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                _remoteVer = "<Error: Couldn't parse the version number>";
                var request = (HttpWebRequest) WebRequest.Create("http://3dsexplorer.googlecode.com/svn/trunk/3DSExplorer/Properties/AssemblyInfo.cs");
                var responseStream = request.GetResponse().GetResponseStream();
                if (responseStream == null) return;
                var reader = new StreamReader(responseStream);
                string line;
                while ((line = reader.ReadLine()) != null)
                    if (line.Contains("AssemblyFileVersion")) //Get the version between the quotation marks
                    {
                        var start = line.IndexOf('"') + 1;
                        var len = line.LastIndexOf('"') - start;
                        _remoteVer = line.Substring(start, len);
                        break;
                    }
            }
            catch
            {
                //No harm done...possibly no internet connection
            }
        }

        private bool IsNewerAvailable(string newerVersion)
        {
            var thisVersion = Version.Parse(Application.ProductVersion);
            var remoteVersion = Version.Parse(newerVersion);
            return remoteVersion.CompareTo(thisVersion) > 0;
        }

        private void bwCheckForUpdates_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (IsNewerAvailable(_remoteVer))
                MessageBox.Show(string.Format("This version is v{0}\nThe version on the server is v{1}\nYou might want to download a newer version.",Application.ProductVersion,_remoteVer));
            else if (_checkNow)
                MessageBox.Show(string.Format("v{0} is the latest version.", Application.ProductVersion));
        }
        #endregion

        private void bwUpdateTitleDb_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create("http://3dsexplorer.googlecode.com/files/title.db");
                var responseStream = request.GetResponse().GetResponseStream();
                if (responseStream == null)
                {
                    e.Result = false;
                    return;
                }
                var reader = responseStream;
                var readBytes = -1;
                var buffer = new byte[0x400];
                if (File.Exists(TitleDatabase.FilePath))
                    File.Delete(TitleDatabase.FilePath);
                var fs = File.OpenWrite(TitleDatabase.FilePath);
                while (readBytes != 0)
                {
                    readBytes = reader.Read(buffer, 0, buffer.Length);
                    fs.Write(buffer,0,readBytes);
                }
                fs.Close();
                e.Result = true;
            }
            catch (Exception ex)
            {
                e.Result = ex.Message;
                //No harm done...possibly no internet connection
            }
        }

        private void bwUpdateTitleDb_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result != null && e.Result is Boolean && (bool)e.Result)
                MessageBox.Show("Title.db file updated please restart application for changes to take effect.");
            else
                MessageBox.Show("There was an error while trying to update the title.db file.." + Environment.NewLine + e.Result);
        }

        private void frmExplorer_Resize(object sender, EventArgs e)
        {
            menuLogo.Margin = new Padding(Width - 280,0,0,0);
        }

    }

}
