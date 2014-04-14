using System;
using System.Windows.Forms;

namespace _3DSExplorer
{
    public partial class InputBox : Form
    {
        private string _input;

        private InputBox()
        {
            InitializeComponent();
            Text = Application.ProductName;
        }

        private void SetLabels(string text)
        {
            lblMessage.Text = text;
        }

        public static string ShowDialog(string messageText)
        {
            var inBox = new InputBox();
            inBox.SetLabels(messageText);
            return inBox.ShowDialog() == DialogResult.OK ? inBox._input : null;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            _input = txtInput.Text;
        }
    }
}
