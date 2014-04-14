using System;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;
using System.Windows.Forms;
using _3DSExplorer.Crypt;

namespace _3DSExplorer
{
    public partial class frmHashTool : Form
    {
        private class ValueObject
        {
            public ValueObject(int value)
            {
                Value = value;
            }
            public readonly int Value;
        }

        [DllImport("msvcrt.dll")]
        static extern int memcmp(byte[] b1, byte[] b2, long count);

        private byte[] _searchKey;
        private HashAlgorithm _ha;

        private string _filePath;

        public frmHashTool()
        {
            InitializeComponent();
            cbAlgo.SelectedIndex = 0;
            cbOption.SelectedIndex = 0;
            if (Clipboard.GetText().Length == 64)
            {
                txtSearch.Text = Clipboard.GetText();
            }
        }

        private static string byteArrayToString(byte[] array)
        {
            var arraystring = string.Empty;
            for (var i = 0; i < array.Length ; i++)
                arraystring += array[i].ToString("X2");
            return arraystring;
        }
        
        // ReSharper disable AccessToStaticMemberViaDerivedType
        private void setHashAlgorithm()
        {
                switch (cbAlgo.SelectedIndex)
                {
                    case 0:
                        switch (cbOption.SelectedIndex)
                        {
                            case 0: _ha = SHA256.Create();
                                break;
                            case 1: _ha = SHA256Cng.Create();
                                break;
                            case 2: _ha = HMACSHA256.Create();
                                break;

                        }
                        break;
                    case 1:
                        switch (cbOption.SelectedIndex)
                        {
                            case 0: _ha = SHA512.Create();
                                break;
                            case 1: _ha = SHA512Cng.Create();
                                break;
                            case 2: _ha = HMACSHA512.Create();
                                break;
                        }
                        break;
                    case 2:
                        switch (cbOption.SelectedIndex)
                        {
                            case 0: _ha = SHA1.Create();
                                break;
                            case 1: _ha = SHA1Cng.Create();
                                break;
                            case 2: _ha = HMACSHA1.Create();
                                break;
                        }
                        break;
                    case 3:
                        switch (cbOption.SelectedIndex)
                        {
                            case 0: _ha = MD5.Create();
                                break;
                            case 1: _ha = MD5Cng.Create();
                                break;
                            case 2: _ha = HMACMD5.Create();
                                break;
                        }
                        break;
                    case 4:
                        //stays null for Modbus-CRC16
                        break;
                    case 5:
                        _ha = new Crc32();
                        break;
                }
        }
        // ReSharper restore AccessToStaticMemberViaDerivedType

        private void btnCompute_Click(object sender, EventArgs e)
        {
            try
            {
                var fs = File.OpenRead(_filePath);
                
                var blockSize = chkEntireFile.Checked ? fs.Length : Int32.Parse(cbComputeBlockSize.Text);
                var blocks = chkEntireFile.Checked ? 1 : Int32.Parse(txtBlocks.Text);

                var block = new byte[blockSize];
                setHashAlgorithm();

                progressBar.Maximum = (blocks > 0 ? blocks : (int)(fs.Length / blockSize));
                progressBar.Value = 0;
                var sb = new StringBuilder();
                fs.Seek(Int32.Parse(txtOffset.Text), SeekOrigin.Begin);
                int readBytes;
                do
                {
                    var pos = fs.Position;
                    readBytes = fs.Read(block, 0, (int)blockSize);
                    var hash = _ha != null ? _ha.ComputeHash(block) : CRC16.GetCRC(block);
                    sb.Append("@" + pos.ToString("X7") + ": " + byteArrayToString(hash) + Environment.NewLine);
                    blocks--;
                    progressBar.PerformStep();
                } while (readBytes == blockSize && blocks != 0);                    
                // Show results
                txtList.Text = sb.ToString();
                
                fs.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() != DialogResult.OK) return;
            btnCompute.Enabled = true;
            btnBrute.Enabled = true;
            btnSuperBrute.Enabled = true;
            _filePath = openFileDialog.FileName;
            lblFilename.Text = _filePath;
        }

        private byte[] parseByteArray(string baString)
        {
            if (baString.Length % 2 != 0)
                return null;
            try
            {
                var ret = new byte[baString.Length / 2];
                for (int i = 0, j = 0; i < baString.Length; i += 2, j++)
                    ret[j] = Convert.ToByte(baString.Substring(i, 2), 16);
                return ret;
            }
            catch
            {
                return null;
            }
        }

        private void btnBrute_Click(object sender, EventArgs e)
        {
            var key = parseByteArray(txtSearch.Text);
            if (key == null)
                MessageBox.Show(@"Error with search string!");
            else
            {
                try
                {
                    var fs = File.OpenRead(_filePath);
                    var blockSize = Int32.Parse(txtSize.Text);
                    var blocks = (int)fs.Length / blockSize;

                    var block = new byte[blockSize];
                    setHashAlgorithm();

                    progressBar.Maximum = blocks * blockSize;
                    progressBar.Value = 0;
                    var sb = new StringBuilder();
                    for (var i = 0; i < blockSize; i++) // Each iteration the starting offset is different
                    {
                        fs.Seek(i, SeekOrigin.Begin);
                        var readBytes = 0;
                        var blockCount = blocks;
                        do
                        {
                            var pos = fs.Position;
                            readBytes = fs.Read(block, 0, blockSize);
                            var hash = _ha != null ? _ha.ComputeHash(block) : CRC16.GetCRC(block);
                            if (memcmp(key,hash,key.Length) == 0) //are equal
                                sb.Append("@" + pos.ToString("X7") + Environment.NewLine);
                            blockCount--;
                            progressBar.PerformStep();
                        } while (readBytes == blockSize && blockCount != 0);
                    }
                    // Show results
                    txtList.Text = sb.Length == 0 ? @"Search Key not found!" : sb.ToString();
                    fs.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnSuperBrute_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(@"Are you sure you want to do a Super Brute-Force search for this key?", @"Super Brute-Force", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _searchKey = parseByteArray(txtSearch.Text);
                if (_searchKey == null)
                    MessageBox.Show(@"Error with search string!");
                else if (!superBruteForce.IsBusy)
                {
                    setHashAlgorithm();
                    if (_searchKey.Length != _ha.HashSize / 8)
                    {
                        MessageBox.Show(@"Wrong key length.. suppose to be " + _ha.HashSize / 8 + @" bytes");
                        return;
                    }
                    btnSuperBrute.Enabled = false;
                    btnCancel.Visible = true;
                    superBruteForce.RunWorkerAsync();
                }
            }
        }

        private void superBruteForce_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker)sender;
            try
            {
                var fileBuffer = File.ReadAllBytes(_filePath);

                worker.ReportProgress(0, new ValueObject(fileBuffer.Length));
                for (var blockSize = 64; blockSize <= fileBuffer.Length; blockSize += 4)
                {
                    worker.ReportProgress(1, new ValueObject(fileBuffer.Length - blockSize));
                    for (var offset = 0; offset < fileBuffer.Length - blockSize; offset+= 4)
                    {
                        if (worker.CancellationPending)
                        {
                            e.Cancel = true;
                            return;
                        }

                        var hash = _ha != null ? _ha.ComputeHash(fileBuffer, offset, blockSize) : CRC16.GetCRC(fileBuffer, offset, blockSize);
                        if (_searchKey[0] == hash[0]) // 1:256 probability
                            if (memcmp(_searchKey, hash, _searchKey.Length) == 0) //key found!!!
                            {
                                e.Result = "@" + offset.ToString("X7") + " of " + blockSize + " : " + byteArrayToString(hash);
                                return;
                            }
                        if (!chkHighCPU.Checked && (offset % 64) == 0)
                            System.Threading.Thread.Sleep(1); //let the cpu cool off
                        worker.ReportProgress(11, new ValueObject(offset));
                    }
                    worker.ReportProgress(10, new ValueObject(blockSize));
                }               
                e.Result = "Search key not found.";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void superBruteForce_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var val = (ValueObject)e.UserState;
            switch (e.ProgressPercentage)
            {
                case 0: //set max for progress
                    progressBar.Minimum = 0;
                    progressBar.Maximum = val.Value;
                    break;
                case 1: //set max for sub-progress
                    subProgressBar.Minimum = 0;
                    subProgressBar.Maximum = val.Value;
                    break;
                case 10: //report progress
                    progressBar.Value = val.Value;
                    break;
                case 11: //report sub-progress
                    subProgressBar.Value = val.Value;
                    break;
            }
        }

        private void superBruteForce_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
                txtList.Text = @"Canceled!";
            else if (e.Error != null)
                txtList.Text = (@"Error: " + e.Error.Message);
            else
                txtList.Text = @"Done!" + Environment.NewLine + e.Result;
            btnCancel.Visible = false;
            btnSuperBrute.Enabled = true;
            progressBar.Value = 0;
            subProgressBar.Value = 0;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            superBruteForce.CancelAsync();
        }

        private void picTool_Click(object sender, EventArgs e)
        {
            txtList.Text = @"Super Brute-Force checks every block size starting from" + Environment.NewLine +
                @"64 bytes to the size of the file increamented by 4 every iteration." + Environment.NewLine + 
                @"That block is hashed at every offset starting from 0 to the last" + Environment.NewLine +
                @"possible offset in the file. The operation is very slow..." + Environment.NewLine +
                @"You could speed it up by checking the High CPU usage but be aware" + Environment.NewLine +
                @"that your CPU might heat up because of the intense processing." + Environment.NewLine + 
                @"Good luck!...";
        }

        private void chkEntireFile_CheckedChanged(object sender, EventArgs e)
        {
            txtBlocks.Enabled = !chkEntireFile.Checked;
            txtOffset.Enabled = !chkEntireFile.Checked;
            cbComputeBlockSize.Enabled = !chkEntireFile.Checked;
        }
    }
}
