using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using com.google.zxing;
using com.google.zxing.common;
using com.google.zxing.qrcode;
using com.google.zxing.qrcode.decoder;

namespace _3DSExplorer
{
    public partial class frmQRTool : Form
    {
        public frmQRTool()
        {
            InitializeComponent();
        }

        private void DoQrToBin(string filename)
        {
            try
            {
                var fileStream = File.OpenRead(filename);
                var img = Image.FromStream(fileStream);
                var bmp = new Bitmap(img);
                fileStream.Close();
                var binary = new BinaryBitmap(new HybridBinarizer(new RGBLuminanceSource(bmp, bmp.Width, bmp.Height)));
                var reader = new QRCodeReader();
                var result = reader.decode(binary);
                var resultList = (ArrayList)result.ResultMetadata[ResultMetadataType.BYTE_SEGMENTS];
                if (resultList == null)
                {
                    File.WriteAllBytes(filename + ".txt", Encoding.UTF8.GetBytes(result.Text));
                    System.Diagnostics.Process.Start(filename + ".txt");
                    txtQRText.Text = result.Text;
                }
                else
                    File.WriteAllBytes(filename + ".bin", (byte[]) resultList[0]);
            }
            catch (ReaderException ex)
            {
                MessageBox.Show(@"Error Loading:" + Environment.NewLine + ex.Message);
            }
        }

        private void DoBinToQr(string filename)
        {
            try
            {
                var byteArray = File.ReadAllBytes(filename);
                var writer = new QRCodeWriter();
                const string encoding = "ISO-8859-1";
                var str = Encoding.GetEncoding(encoding).GetString(byteArray);
                var hints = new Hashtable { { EncodeHintType.CHARACTER_SET, encoding } };
                var matrix = writer.encode(str, BarcodeFormat.QR_CODE, 100, 100, hints);
                var img = new Bitmap(200, 200);
                var g = Graphics.FromImage(img);
                g.Clear(Color.White);
                for (var y = 0; y < matrix.Height; ++y)
                    for (var x = 0; x < matrix.Width; ++x)
                        if (matrix.get_Renamed(x, y) != -1)
                            g.FillRectangle(Brushes.Black, x * 2, y * 2, 2, 2);
                ImageBox.ShowDialog(img);
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Error Loading:" + Environment.NewLine + ex.Message);
            }
        }

        private void DoTextToQr(string text)
        {
            try
            {
                var writer = new QRCodeWriter();
                const string encoding = "UTF-8";// "ISO-8859-1";
                var hints = new Hashtable { { EncodeHintType.CHARACTER_SET, encoding }, { EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.L } };
                var matrix = writer.encode(text, BarcodeFormat.QR_CODE, 100, 100, hints);
                var img = new Bitmap(200, 200);
                var g = Graphics.FromImage(img);
                g.Clear(Color.White);
                for (var y = 0; y < matrix.Height; ++y)
                    for (var x = 0; x < matrix.Width; ++x)
                        if (matrix.get_Renamed(x, y) != -1)
                            g.FillRectangle(Brushes.Black, x * 2, y * 2, 2, 2);
                ImageBox.ShowDialog(img);
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Error Loading:" + Environment.NewLine + ex.Message);
            }
        }

        private void QrToBin(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog { Filter = @"All Supported|*.png;*.jpg;*.bmp;*.gif|PNG Files|*.png|Jpeg Files|*.jpg|Bitmap Files|*.bmp|GIF Files|*.gif" };
            if (ofd.ShowDialog() != DialogResult.OK) return;
            DoQrToBin(ofd.FileName);
        }

        private void BinToQr(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog { Filter = @"Binary Files|*.bin" };
            if (ofd.ShowDialog() != DialogResult.OK) return;
            DoBinToQr(ofd.FileName);
        }

        #region Drag & Drop

        private new void DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
                e.Effect = DragDropEffects.All;
        }

        private new void DragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (sender == lblDragBinToQr)
                DoBinToQr(files[0]);
            else
                DoQrToBin(files[0]);
        }

        #endregion

        private void ButtonFromTextClick(object sender, EventArgs e)
        {
            DoTextToQr(txtQRText.Text);
        }
    }
}
