using System;
using System.Windows.Forms;

namespace _3DSExplorer.Modules
{
    public partial class ProgressDialog : Form
    {
        public int Value
        {
            get { return progressBar.Value; }
            set { progressBar.Value = value; }
        }

        public int Maximum
        {
            get { return progressBar.Maximum; }
            set { progressBar.Maximum = value; }
        }

        public int Step
        {
            get { return progressBar.Step; }
            set { progressBar.Step = value; }
        }

        public string Message
        {
            get { return lblMessage.Text; }
            set { lblMessage.Text = value; }
        }

        public event EventHandler CancelClicked
        {
            add { btnCancel.Click += value; }
            remove { btnCancel.Click -= value; }
        }

        public ProgressDialog(string title, int max)
        {
            InitializeComponent();
            Text = title;
            Maximum = max;
        }

        public void PerformStep()
        {
            progressBar.PerformStep();
        }

        private void ProgressDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            btnCancel.PerformClick();
        }
    }
}
