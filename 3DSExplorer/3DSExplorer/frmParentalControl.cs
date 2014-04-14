using System;
using System.Text;
using System.Windows.Forms;
using _3DSExplorer.Crypt;

namespace _3DSExplorer
{
    public partial class frmParentalControl : Form
    {
        public frmParentalControl()
        {
            InitializeComponent();

            comboAlgorithm.SelectedIndex = 0;
        }

        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            // Parse input
            var servicecode = (uint) numericEnquiryNumber.Value;
            var uday = (uint) dateTimeDevice.Value.Day;
            var umonth = (uint) dateTimeDevice.Value.Month;
            servicecode %= 10000;
            uday %= 100;
            umonth %= 100;
            var str = string.Format("{0:D02}{1:D02}{2:D04}", umonth, uday, servicecode);
            var array = Encoding.ASCII.GetBytes(str);

            var poly = comboAlgorithm.SelectedIndex == 0 ? 0xEDBA6320 : 0xEDB88320;
            var crc = Crc32.Compute(poly, 0xFFFFFFFF, 0x0000AAAA, array);
            crc += (uint) (comboAlgorithm.SelectedIndex == 0 ? 0x00001657 : 0x000014C1);
            if (comboAlgorithm.SelectedIndex == 0)
            {
                var crclong = (crc + 1)*(ulong)0xA7C5AC47;
                var crchi = (uint)(crclong >> 48); //shr 6 bytes
                crchi *= 0xFFFFF3CB;
                crc += (crchi << 5);
            }
            crc = crc % 100000;

            textCode.Text = string.Format("{0:D05}",crc);
        }
    }
}
