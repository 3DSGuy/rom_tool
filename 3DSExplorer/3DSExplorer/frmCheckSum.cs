using System;
using System.IO;
using System.Windows.Forms;
using _3DSExplorer.Crypt;

namespace _3DSExplorer
{
    public partial class frmCheckSum : Form
    {
        private QuickCrc32 _quickCrc;
        private string _filePath;

        private frmCheckSum(string filePath)
        {
            InitializeComponent();
            _quickCrc= new QuickCrc32(CRCProgressChanged,CRCProcessFinished);
            txtResult.Text = "Press 'Start'";
            btnStartStop.Text = "Start";
            _filePath = filePath;
        }

        private void CRCProgressChanged(int value)
        {
            pBar.Value = value;
            txtResult.Text = "Processing... " + value + "%";
        }

        private void CRCProcessFinished(uint result)
        {
            txtResult.Text = result == 0 ? "Press 'Start'" : StringUtil.ToHexString(8,result);
            btnStartStop.Text = "Start";
            pBar.Value = 0;
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            if (btnStartStop.Text == "Start")
            {
                _quickCrc.Calculate(_filePath);
                btnStartStop.Text = "Stop";
            }
            else
            {
                _quickCrc.Cancel();
                btnStartStop.Text = "Start";
            }
        }

        public static void ShowDialog(string filePath)
        {
            var form = new frmCheckSum(filePath);
            form.ShowDialog();
        }
    }
}
