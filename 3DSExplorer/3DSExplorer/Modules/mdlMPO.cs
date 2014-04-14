using System;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace _3DSExplorer.Modules
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IFD
    {
        public ushort Tag;
        public ushort Type;
        public uint CountValue;
        public uint ValueOffset;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NintendoNote
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x4)]
        public char[] Magic; //4

        public uint Type; //4
        public uint TimeStamp; //4
        public uint Padding0; //4
        public uint TitleIDLow; // 4
        public uint Flags; // 4
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x4)]
        public byte[] ConsoleID; //4
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0xC)]
        public byte[] Padding1; //12
        public float Parallax; //4
        public uint Padding2; //4
        public ushort Categories; //2
        public ushort Filters; //2
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0xC)]
        public byte[] Padding3; //12
    }

    public class MPOContext : IContext
    {
        /*
        private const int T_INT32 = 0; //4 byte
        private const int T_RAT64 = 1; //8 byte
        private const int T_STRING = 2; // string
        private const int T_INT16 = 3; // 2 byte
        private const int T_BYTEARR = 4; // byte array
        private const int T_DATA = 5; // byte array
        */
        public enum MPOView
        {
            MPO,
            Image,
            MPOExtensions,
            MakerNote
        };

        private string errorMessage = string.Empty;
        
        private byte[] LeftImageBytes, RightImageBytes;
        private Image LeftImage, RightImage, AnaglyphImage, RightLaxxedImage, LeftLaxxedImage;

        private IFD[] _ifds;
        private byte[] _firstIFDData;
        private NintendoNote _nintendoNote;

        private static int ByteArrToIntBE(byte[] byteArrayIn)
        {
            int ret = byteArrayIn[3];
            ret += byteArrayIn[2] << 8;
            ret += byteArrayIn[1] << 16;
            ret += byteArrayIn[0] << 24;
            return ret;
        }

        private static short ByteArrToShortBE(byte[] byteArrayIn)
        {
            short ret = byteArrayIn[1];
            ret += (short)(byteArrayIn[0] << 8);
            return ret;
        }

        private static Image ByteArrayToImage(byte[] byteArrayIn)
        {
            var ms = new MemoryStream(byteArrayIn);
            var returnImage = Image.FromStream(ms);
            return returnImage;
        }

        private static bool IsMPFMagic(byte[] fourBytes)
        {
            return (fourBytes[0] == 'M' && fourBytes[1] == 'P' && fourBytes[2] == 'F' && fourBytes[3] == 0x00);
        }

        private static int ExtractParallax(ushort parallax)
        {
            var b0 = (byte)(parallax >> 8);
            var b1 = (byte)(parallax & 0xff);
            var temp = (b1 - 0xC0) << 8;
            temp += b0;
            return -(int)Math.Round(Math.Pow(2, (double)temp / 128 + 2));
        }

        public bool Open(Stream fs)
        {
            try
            {
                var intSizeArr = new byte[4];
                var shortSizeArr = new byte[2];
                short shortSize;
                int intSize;
                long length;

                //Read JPG SOF (FF,D8)
                length = fs.Read(shortSizeArr, 0, 2);
                if (length != 2 || shortSizeArr[0] != 0xFF || shortSizeArr[1] != 0xD8)
                {    
                    errorMessage = "File has no 0xFFD8 on start!";
                    return false;
                }

                //READ First SOF 0:APP0:Jfif:0xFFE0 / 1:APP1:Exif:0xFFE1
                length = fs.Read(shortSizeArr, 0, 2);
                if (shortSizeArr[0] == 0xFF && shortSizeArr[1] == 0xE0)
                {   //If  SOF0 found then skip it
                    length = fs.Read(shortSizeArr, 0, 2);
                    shortSize = ByteArrToShortBE(shortSizeArr);
                    fs.Seek(shortSize - 2, SeekOrigin.Current);
                    length = fs.Read(shortSizeArr, 0, 2); //Read next SOF#
                }
                if (shortSizeArr[0] != 0xFF || shortSizeArr[1] != 0xE1) //suppose to be next
                {
                    errorMessage = "No Exif information (No APP1)";
                    return false;
                }
                //Skip over APP1
                length = fs.Read(shortSizeArr, 0, 2);
                shortSize = ByteArrToShortBE(shortSizeArr);
                fs.Seek(shortSize - 2, SeekOrigin.Current);

                //Read next SOF 2:APP2:MPE:0xFFE2
                length = fs.Read(shortSizeArr, 0, 2);
                if (length != 2 || shortSizeArr[0] != 0xFF || shortSizeArr[1] != 0xE2)
                {
                    errorMessage = "File has no 0xFFE2 on start!";
                    return false;
                }

                fs.Seek(2, SeekOrigin.Current); //Skip over APP2 length field
                //Read MP Magic
                length = fs.Read(intSizeArr, 0, 4);
                if (length != 4)
                {
                    errorMessage = "No MP Magic (MPF.)!";
                    return false;
                }
                if (!IsMPFMagic(intSizeArr))
                {
                    errorMessage = "Wrong MP Magic (MPF.)!";
                    return false;
                }

                var fromSOO = 0;
                //READ THE MP Header Endianess
                length = fs.Read(intSizeArr, 0, 4);
                if (length != 4)
                {
                    errorMessage = "No APP2 Endianess!";
                    return false;
                }
                fromSOO += 4;
                var endianess = 0;
                if (intSizeArr[0] == 0x49 && intSizeArr[1] == 0x49 && intSizeArr[2] == 0x2A && intSizeArr[3] == 0x00)
                    endianess = 1; // LE
                else if (intSizeArr[0] == 0x4D && intSizeArr[1] == 0x4D && intSizeArr[2] == 0x00 && intSizeArr[3] == 0x2A)
                    endianess = 2; // BE
                if (endianess == 0)
                {
                    errorMessage = "Wrong APP2 Endianess!";
                    return false;
                } 
                //Read First IFD Offset
                length = fs.Read(intSizeArr, 0, 4);
                if (length != 4)
                {
                    errorMessage = "No First IFD Offset!";
                    return false;
                }
                
                fromSOO += 4;
                intSize = (endianess == 1 ? BitConverter.ToInt32(intSizeArr, 0) : ByteArrToIntBE(intSizeArr));

                if (intSize - fromSOO > 0) //Goto Offset
                    fs.Seek(intSize - fromSOO, SeekOrigin.Current);
                // Skip MP Entry: Count[2], Version[12], No[12], No*16[12], Attr[4], Size[4]
                fs.Seek(46, SeekOrigin.Current);

                // Read 1st Image Size
                length = fs.Read(intSizeArr, 0, 4);
                if (length != 4)
                {
                    errorMessage = "No Image Size!";
                    return false;
                }

                intSize = (endianess == 1 ? BitConverter.ToInt32(intSizeArr, 0) : ByteArrToIntBE(intSizeArr));

                fs.Seek(0, SeekOrigin.Begin);

                LeftImageBytes = new byte[intSize];
                fs.Read(LeftImageBytes, 0, intSize);
                LeftImage = ByteArrayToImage(LeftImageBytes);

                // Go to the start of the next file (skip junk)
                //int space = 1;
                var found = false;
                while (!found)
                {
                    if (fs.ReadByte() == 0xFF)
                    {
                        if (fs.ReadByte() == 0xD8)
                        {
                            found = true;
                        }
                    }
                }

                fs.Seek(-2, SeekOrigin.Current);
                intSize = 0;
                RightImageBytes = new byte[fs.Length - intSize - 1];
                fs.Read(RightImageBytes, 0, (int)(fs.Length - intSize - 1));
                RightImage = ByteArrayToImage(RightImageBytes);

                fs.Close();
                fs = null;

                var propItems = LeftImage.PropertyItems;
                var nintendo = false;
                foreach (var propItem in propItems)
                {
                    if (propItem.Id == 0x010F) // Manufacturer
                        nintendo = Encoding.UTF8.GetString(propItem.Value).Equals("Nintendo\0");
                    else if ((propItem.Id == 0x927C) && nintendo) // MakerNote
                    {
                        var memStream = new MemoryStream(propItem.Value);
                        var ifdCount = (ushort)(memStream.ReadByte() << 8);
                        ifdCount += (byte)memStream.ReadByte();
                        _ifds = new IFD[ifdCount];
                        for (var i=0;i<_ifds.Length;i++)
                            _ifds[i] = MarshalUtil.ReadStructBE<IFD>(memStream) ;
                        //read the zero
                        //ifdCount = (ushort)(memStream.ReadByte() << 8);
                        //ifdCount += (byte)memStream.ReadByte();
                        //jump over the first ifd
                        memStream.Seek(4, SeekOrigin.Current);
                        _firstIFDData = new byte[_ifds[0].CountValue];
                        memStream.Read(_firstIFDData, 0, _firstIFDData.Length);
                        _nintendoNote = MarshalUtil.ReadStruct<NintendoNote>(memStream);
                        memStream.Close();
                        break;
                    }
                }
                var px = _nintendoNote.Parallax == 0 ? -10 : _nintendoNote.Parallax;

                //make images for animation
                LeftLaxxedImage = new Bitmap(LeftImage.Width + (int)px, LeftImage.Height);
                RightLaxxedImage = new Bitmap(RightImage.Width + (int)px, RightImage.Height);
                var g = Graphics.FromImage(LeftLaxxedImage);
                g.DrawImage(LeftImage, new Rectangle(0, 0, RightImage.Width + (int)px, RightImage.Height), new Rectangle(0, 0, LeftImage.Width + (int)px, LeftImage.Height), GraphicsUnit.Pixel);
                g.Dispose();
                g = Graphics.FromImage(RightLaxxedImage);
                g.DrawImage(RightImage, new Rectangle(0, 0, RightImage.Width + (int)px, RightImage.Height), new Rectangle(-(int)px, 0, RightImage.Width + (int)px, RightImage.Height), GraphicsUnit.Pixel);
                g.Dispose();

                //make anaglyph

                AnaglyphImage = Anaglyph.MakeAnaglyph(LeftImage, RightImage, (new Anaglyph()).HalfColorAnaglyph, (int)px);

                return true;
            }
            catch (Exception ex)
            {
                errorMessage = "Error in Opening file:" + Environment.NewLine + ex.Message;
                return false;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

        public string GetErrorMessage()
        {
            return errorMessage;
        }

        public void Create(FileStream fs, FileStream src)
        {
            throw new NotImplementedException();
        }

        public void View(frmExplorer f, int view, object[] values)
        {
            f.ClearInformation();
            switch ((MPOView)view)
            {
                case MPOView.MPO:
                    break;
                case MPOView.Image:
                    
                    var propItems = (int)values[0] == 0 ? LeftImage.PropertyItems : RightImage.PropertyItems;

                    string tempString;
                    foreach (var propItem in propItems)
                    {
                        switch (propItem.Id)
                        {
                            case 0x1000:
                                tempString = Encoding.UTF8.GetString(propItem.Value);
                                f.AddListItem(propItem.Id, tempString.Length, "InteroperabilityVersion",
                                              tempString.ToCharArray(), 0);
                                break;
                            case 0x010f:
                                tempString = Encoding.UTF8.GetString(propItem.Value);
                                f.AddListItem(propItem.Id, tempString.Length, "Manufacturer",
                                              tempString.ToCharArray(), 0);
                                break;
                            case 0x0110:
                                tempString = Encoding.UTF8.GetString(propItem.Value);
                                f.AddListItem(propItem.Id, tempString.Length, "Camera",
                                              tempString.ToCharArray(), 0);
                                break;
                            case 0x011a:
                                tempString = BitConverter.ToUInt32(propItem.Value, 0) + ":" + BitConverter.ToUInt32(propItem.Value, 4);
                                f.AddListItem(propItem.Id, 8, "XResolution",
                                              tempString.ToCharArray(), 0);
                                break;
                            case 0x011b:
                                tempString = BitConverter.ToUInt32(propItem.Value, 0) + ":" + BitConverter.ToUInt32(propItem.Value, 4);
                                f.AddListItem(propItem.Id, 8, "YResolution",
                                              tempString.ToCharArray(), 0);
                                break;
                            case 0x0128:
                                f.AddListItem(propItem.Id, 2, "ResolutionUnit", BitConverter.ToUInt16(propItem.Value, 0), 0);
                                break;
                            case 0x0131:
                                tempString = Encoding.UTF8.GetString(propItem.Value);
                                f.AddListItem(propItem.Id, tempString.Length, "Software",
                                              tempString.ToCharArray(), 0);
                                break;
                            case 0x0132:
                                tempString = Encoding.UTF8.GetString(propItem.Value);
                                f.AddListItem(propItem.Id, tempString.Length, "DateTime",
                                              tempString.ToCharArray(), 0);
                                break;
                            case 0x0201:
                                f.AddListItem(propItem.Id, 4, "JPEGInterchangeFormat", BitConverter.ToUInt32(propItem.Value, 0), 0);
                                break;
                            case 0x0202:
                                f.AddListItem(propItem.Id, 4, "JPEGInterchangeFormatLength", BitConverter.ToUInt32(propItem.Value, 0), 0);
                                break;
                            case 0x0213:
                                f.AddListItem(propItem.Id, 2, "YCbCrPositioning", BitConverter.ToUInt16(propItem.Value, 0), 0);
                                break;
                            case 0x501b:
                                f.AddListItem(propItem.Id, propItem.Value.Length, "ThumbnailData", propItem.Value, 0);
                                break;
                            case 0x5023:
                                f.AddListItem(propItem.Id, 2, "ThumbnailCompression", BitConverter.ToUInt16(propItem.Value, 0), 0);
                                break;
                            case 0x502d:
                                tempString = BitConverter.ToUInt32(propItem.Value, 0) + ":" + BitConverter.ToUInt32(propItem.Value, 4);
                                f.AddListItem(propItem.Id, 8, "ThumbnailXResolution",
                                              tempString.ToCharArray(), 0);
                                break;
                            case 0x502e:
                                tempString = BitConverter.ToUInt32(propItem.Value, 0) + ":" + BitConverter.ToUInt32(propItem.Value, 4);
                                f.AddListItem(propItem.Id, 8, "ThumbnailYResolution",
                                              tempString.ToCharArray(), 0);
                                break;
                            case 0x5030:
                                f.AddListItem(propItem.Id, 2, "ThumbnailTransferFunction", BitConverter.ToUInt16(propItem.Value, 0), 0);
                                break;
                            case 0x5041:
                                tempString = Encoding.UTF8.GetString(propItem.Value);
                                f.AddListItem(propItem.Id, tempString.Length, "InteroperabilityIndex",
                                              tempString.ToCharArray(), 0);
                                break;
                            case 0x5042:
                                tempString = Encoding.UTF8.GetString(propItem.Value);
                                f.AddListItem(propItem.Id, tempString.Length, "ExifInteroperabilityVersion",
                                              tempString.ToCharArray(), 0);
                                break;
                            case 0x5090:
                                f.AddListItem(propItem.Id, propItem.Value.Length, "LuminanceTable", propItem.Value, 0);
                                break;
                            case 0x5091:
                                f.AddListItem(propItem.Id, propItem.Value.Length, "ChrominanceTable", propItem.Value, 0);
                                break;
                            case 0x9000:
                                tempString = Encoding.UTF8.GetString(propItem.Value);
                                f.AddListItem(propItem.Id, tempString.Length, "ExifVersion",
                                              tempString.ToCharArray(), 0);
                                break;
                            case 0x9003:
                                tempString = Encoding.UTF8.GetString(propItem.Value);
                                f.AddListItem(propItem.Id, tempString.Length, "DateTimeOriginal",
                                              tempString.ToCharArray(), 0);
                                break;
                            case 0x9004:
                                tempString = Encoding.UTF8.GetString(propItem.Value);
                                f.AddListItem(propItem.Id, tempString.Length, "DateTimeDigitized",
                                              tempString.ToCharArray(), 0);
                                break;
                            case 0x9101:
                                f.AddListItem(propItem.Id, propItem.Value.Length, "ComponentsConfiguration", propItem.Value, 0);
                                break;
                            case 0x927c:
                                f.AddListItem(propItem.Id, propItem.Value.Length, "MakerNote", propItem.Value, 0);
                                break;
                            case 0xa000:
                                tempString = Encoding.UTF8.GetString(propItem.Value);
                                f.AddListItem(propItem.Id, tempString.Length, "FlashPixVersion",
                                              tempString.ToCharArray(), 0);
                                break;
                            case 0xa001:
                                f.AddListItem(propItem.Id, 2, "ColorSpace", BitConverter.ToUInt16(propItem.Value, 0), 0);
                                break;
                            case 0xa002:
                                f.AddListItem(propItem.Id, 4, "PixelXDimension", BitConverter.ToUInt32(propItem.Value, 0), 0);
                                break;
                            case 0xa003:
                                f.AddListItem(propItem.Id, 4, "PixelYDimension", BitConverter.ToUInt32(propItem.Value, 0), 0);
                                break;
                            default:
                                f.AddListItem(propItem.Id, propItem.Value.Length, "<Unknown>", propItem.Value, 0);
                                break;
                        }
                    }
                    break;
                case MPOView.MPOExtensions:
                    break;
                case MPOView.MakerNote:
                    f.SetGroupHeaders("IFD","First IFD","Nintendo MarkerNote");
                    if (_ifds != null)
                    {
                        f.AddListItem(0, 2, "IFD Count", (uint) _ifds.Length, 0);
                        for (var i = 0; i < _ifds.Length; i++)
                            f.AddListItem((2 + i*12).ToString(), "12", "IFD", "Tag = " + _ifds[i].Tag.ToString("X4"),
                                          string.Format("Type={0}, Count/Value={1}, Value/Offset={2}", _ifds[i].Type,
                                                        _ifds[i].CountValue, _ifds[i].ValueOffset), 0);

                        f.AddListItem(0, _firstIFDData.Length, "First IFD data", _firstIFDData, 1);
                    }
                    f.AddListItem(0, 4, "Magic", _nintendoNote.Magic, 2);
                    f.AddListItem(4, 4, "Type", _nintendoNote.Type, 2);
                    f.AddListItem(8, 4, "Time stamp", _nintendoNote.TimeStamp, 2);
                    f.AddListItem("","","","",(new DateTime(2000,1,1,0,0,0)).AddSeconds(_nintendoNote.TimeStamp).ToString(),2);
                    f.AddListItem(12, 4, "Padding 0", _nintendoNote.Padding0, 2);
                    f.AddListItem(16, 4, "Title ID Low", _nintendoNote.TitleIDLow, 2);
                    f.AddListItem(20, 4, "Flags", _nintendoNote.Flags, 2);
                    f.AddListItem(24, 4, "Console ID", _nintendoNote.ConsoleID, 2);
                    f.AddListItem(28, 12, "Padding 1", _nintendoNote.Padding1, 2);
                    f.AddListItem(40, 4, "Parallax", _nintendoNote.Parallax, 2);
                    f.AddListItem(44, 4, "Padding 2", _nintendoNote.Padding2, 2);
                    f.AddListItem(48, 2, "Categories", _nintendoNote.Categories, 2);
                    f.AddListItem(50, 2, "Filters", _nintendoNote.Filters, 2);
                    f.AddListItem(52, 12, "Padding 3", _nintendoNote.Padding3, 2);
                    break;
            }
            f.AutoAlignColumns();
        }

        public bool CanCreate()
        {
            return false;
        }

        public void Activate(string filePath, int type, object[] values)
        {
            switch (type)
            {
                case 0:
                    ImageBox.ShowDialog((Image)values[0]);
                    break;
                case 1:
                    MessageBox.Show("Not implemented yet.");//TODO: make switch images
                    break;
            }
        }

        public string GetFileFilter()
        {
            return "MPO (3D Images) Files (*.mpo)|*.mpo";
        }

        public TreeNode GetExplorerTopNode()
        {
            var topNode = new TreeNode("MPO") { Tag = TreeViewContextTag.Create(this, (int)MPOView.MPO) };
            var sub = new TreeNode("Left Image") {Tag = TreeViewContextTag.Create(this, (int) MPOView.Image, new object[] {0})};
            sub.Nodes.Add(new TreeNode("MPO Extensions") { Tag = TreeViewContextTag.Create(this, (int)MPOView.MPOExtensions, new object[] { 0 }) });
            sub.Nodes.Add(new TreeNode("Maker Note") { Tag = TreeViewContextTag.Create(this, (int)MPOView.MakerNote, new object[] { 0 }) });
            topNode.Nodes.Add(sub);
            sub = new TreeNode("Right Image") { Tag = TreeViewContextTag.Create(this, (int)MPOView.Image, new object[] { 1 }) };
            sub.Nodes.Add(new TreeNode("MPO Extensions") { Tag = TreeViewContextTag.Create(this, (int)MPOView.MPOExtensions, new object[] { 1 }) });
            sub.Nodes.Add(new TreeNode("Maker Note") { Tag = TreeViewContextTag.Create(this, (int)MPOView.MakerNote, new object[] { 1 }) });
            topNode.Nodes.Add(sub);

            return topNode;
        }

        public TreeNode GetFileSystemTopNode()
        {
            var topNode = new TreeNode("MPO", 1, 1);
            topNode.Nodes.Add(new TreeNode("Left") { Tag = new[] { TreeViewContextTag.Create(this, 0, "Show...", new object[] { LeftImage }), TreeViewContextTag.Create(this, 1, "Replace...", new object[] { LeftImage }) } });
            topNode.Nodes.Add(new TreeNode("Right") { Tag = new[] { TreeViewContextTag.Create(this, 0, "Show...", new object[] { RightImage }), TreeViewContextTag.Create(this, 1, "Replace...", new object[] { RightImage }) } });
            topNode.Nodes.Add(new TreeNode("Anaglyph") { Tag = new[] { TreeViewContextTag.Create(this, 0, "Show...", new object[] { AnaglyphImage }) } }); 
            return topNode;
        }
    }

}