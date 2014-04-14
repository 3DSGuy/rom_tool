using System;
using System.IO;
using System.Windows.Forms;
using DSDecmp;
using DSDecmp.Formats.Nitro;

namespace _3DSExplorer
{
    public partial class frmDSDecmpGUI : Form
    {
        private CompositeCTRFormat _compressionFormat;

        private long _currentFileSize = 0;

        public frmDSDecmpGUI()
        {
            InitializeComponent();
            
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.Cancel)
                return;
            btnCompress.Enabled = true;
            btnDecompress.Enabled = true;

            txtFilePath.Text = ofd.FileName;
            var fi= new FileInfo(ofd.FileName);
            _currentFileSize = fi.Length;
            numLength.Value = _currentFileSize;
        }

        private void btnDecompress_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == DialogResult.Cancel)
                return;

            var offset = numOffset.Value;
            var length = numLength.Value;

            if (_compressionFormat == null)
                _compressionFormat = new CompositeCTRFormat();

            var fs = File.OpenRead(txtFilePath.Text);
            fs.Seek((long) offset, SeekOrigin.Begin);

            if (_compressionFormat.Supports(fs, (long)length))
            {
                var os = File.Create(sfd.FileName);
                _compressionFormat.Decompress(fs, (long)length, os);
                os.Close();
            }
            else
                MessageBox.Show(@"This file is not is a supported format.");
                
            fs.Close();

        }

        private void btnFileSize_Click(object sender, EventArgs e)
        {
            numLength.Value = _currentFileSize;
        }

        private void frmDSDecmpGUI_Load(object sender, EventArgs e)
        {
            cmbAlgorithm.SelectedIndex = 2; /* LZ-10 */
        }

        private void btnCompress_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == DialogResult.Cancel)
                return;

            var offset = numOffset.Value;
            var length = numLength.Value;

            if (_compressionFormat == null)
                _compressionFormat = new CompositeCTRFormat();

            var compression = _compressionFormat.GetCompression(cmbAlgorithm.SelectedIndex);
            
            var fs = File.OpenRead(txtFilePath.Text);
            fs.Seek((long) offset, SeekOrigin.Begin);

            var os = File.Create(sfd.FileName);
            compression.Compress(fs, (long)length, os);
            os.Close();

            fs.Close();
        }

        #region Drag & Drop

        private void FileDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
                e.Effect = DragDropEffects.All;
        }

        private void FileDragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            
            btnCompress.Enabled = true;
            btnDecompress.Enabled = true;

            txtFilePath.Text = files[0];
            var fi = new FileInfo(txtFilePath.Text);
            _currentFileSize = fi.Length;
            numLength.Value = _currentFileSize;
        }

        #endregion
    }
}
