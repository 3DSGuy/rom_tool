using System;
using System.Windows.Forms;

namespace _3DSExplorer
{
    public static class StringUtil
    {
        public static string ByteArrayToString(byte[] array)
        {
            if (array == null) return "NULL";
            int i;
            var arraystring = string.Empty;
            for (i = 0; i < array.Length && i < 40; i++)
                arraystring += String.Format("{0:X2}", array[i]);
            if (i == 40) return arraystring + "..."; //ellipsis
            return arraystring;
        }
        
        /*
        public static string ByteArrayToStringSpaces(byte[] array)
        {
            int i;
            var arraystring = string.Empty;
            for (i = 0; i < array.Length && i < 33; i++)
                arraystring += String.Format("{0:X2}", array[i]) + (i < array.Length - 1 ? " " : "");
            if (i == 33) return arraystring + "..."; //ellipsis
            return arraystring;
        }*/

        public static string CharArrayToString(char[] array)
        {
            if (array == null) return string.Empty;
            int i;
            var arraystring = string.Empty;
            for (i = 0; i < array.Length; i++)
            {
                if (array[i] == 0) break;
                arraystring += array[i];
            }
            return arraystring + "";
        }

        public static string ToHexString(int digits, ulong number)
        {
            var formatString = "{0:X" + digits + "}";
            return "0x" + String.Format(formatString, number);
        }

        public static string ToHexString(int digits, float number)
        {
            var formatString = "{0:X" + digits + "}";
            var unumber = BitConverter.ToUInt32(BitConverter.GetBytes(number),0);
            return "0x" + String.Format(formatString, unumber);
        }

        public static byte[] ParseKeyStringToByteArray(string str)
        {
            if (str.Equals("")) return new byte[0];
            if ((str.Length % 2 > 0) || (str.Length != 32)) return null; //must be a mutliple of 2
            var retArray = new byte[str.Length / 2];
            try
            {
                for (var i = 0; i < str.Length; i += 2)
                {
                    retArray[i / 2] = Convert.ToByte(str.Substring(i, 2), 16);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Can't parse key string!\n" + ex.Message);
                return null;
            }
            return retArray;
        }

        public static byte[] ParseByteArray(string baString)
        {
            baString.Replace(" ", ""); //delete all spaces
            if (baString.Length % 2 != 0)
                return null;
            try
            {
                byte[] ret = new byte[(int)baString.Length / 2];
                for (int i = 0, j = 0; i < baString.Length; i += 2, j++)
                    ret[j] = Convert.ToByte(baString.Substring(i, 2), 16);
                return ret;
            }
            catch
            {
                return null;
            }
        }
    }
}
