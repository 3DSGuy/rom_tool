using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace _3DSExplorer
{
    public partial class frmXORTool : Form
    {
        bool _first, _second;

        // returns the bigger array xored with the smaller cyclicly
        // dst suppose to be the size of the bigger array
        private static void XorBlock(byte[] dst, byte[] first, byte[] second)
        {
            byte[] big = first, small = second;
            if (first.Length < second.Length)
            {
                big = second;
                small = first;
            }
            for (var i = 0; i < dst.Length; i++)
                dst[i] = (byte)(big[i] ^ small[i % small.Length]);
        }

        public frmXORTool()
        {
            InitializeComponent();
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() != DialogResult.OK) return;
            txtFirst.Text = openFileDialog.FileName;
            _first = true;
            btnSave.Enabled = _first && _second;
        }

        private void btnSecond_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() != DialogResult.OK) return;
            txtSecond.Text = openFileDialog.FileName;
            _second = true;
            btnSave.Enabled = _first && _second;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFirst.Text) || string.IsNullOrEmpty(txtSecond.Text) ||
                saveFileDialog.ShowDialog() != DialogResult.OK) return;
            var firstByteArray = File.ReadAllBytes(txtFirst.Text);
            var secondByteArray = File.ReadAllBytes(txtSecond.Text);
            var xored = new byte[Math.Max(firstByteArray.Length, secondByteArray.Length)];
            XorBlock(xored, firstByteArray, secondByteArray);
            File.WriteAllBytes(saveFileDialog.FileName, xored);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private string byteArrayToString(byte[] array)
        {
            int i;
            string arraystring = "";
            for (i = 0; i < array.Length; i++)
                arraystring += String.Format("{0:X2}", array[i]);
            return arraystring;
        }

        private void btnXorArrays_Click(object sender, EventArgs e)
        {
            var one = StringUtil.ParseByteArray(txtBox1.Text);
            var two = StringUtil.ParseByteArray(txtBox2.Text);

            if (one != null && two != null && one.Length == two.Length)
            {
                var dst = new byte[one.Length];
                for (var i = 0; i < dst.Length; i++)
                    dst[i] = (byte)(one[i] ^ two[i]);
                txtBoxResult.Text = byteArrayToString(dst);
            }
            else
                MessageBox.Show("Error with length (must be a multiple of 2 or same size)");
        }

        private void btnAesGo_Click(object sender, EventArgs e)
        {
            var key = StringUtil.ParseByteArray(txtKey.Text);
            var iv = StringUtil.ParseByteArray(txtIV.Text);
            var data = StringUtil.ParseByteArray(txtEncData.Text);

            if (key != null && iv != null && data != null && key.Length == 16 && iv.Length == 16)
            {
                var aes = new Aes128Ctr(key, iv);
                aes.TransformBlock(data);
                txtDecData.Text = byteArrayToString(data);
            }
            else
                MessageBox.Show("Error with length (must be a multiple of 2 or key & iv must be 16 bytes)");
        }

        #region Drag & Drop

        private void FileDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
                e.Effect = DragDropEffects.All;
        }

        private void FileDragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            ((TextBox)sender).Text = files[0];
        }

        #endregion
    }
}
