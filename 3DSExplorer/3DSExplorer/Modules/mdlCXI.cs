using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using _3DSExplorer.Utils;

namespace _3DSExplorer.Modules
{

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct String8
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public char[] text;
    }

    public class CXIPlaingRegion
    {
        public string[] PlainRegionStrings;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CXIHeader
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] NCCHHeaderSignature;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public char[] Magic;
        public uint CXILength;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] TitleID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public char[] MakerCode;
        public ushort Version;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Reserved0;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] ProgramID;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] Unknown0_0;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] LogoRegionHash;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public char[] ProductCode;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] ExtendedHeaderHash;
        public uint ExtendedHeaderSize;

        public uint Unknown1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] Flags;

        public uint PlainRegionOffset;
        public uint PlainRegionSize;

        public uint LogoRegionOffset;
        public uint LogoRegionLength;

        public uint ExeFSOffset;
        public uint ExeFSLength;
        public uint ExeFSHashRegionSize;

        public uint Unknown5;

        public uint RomFSOffset;
        public uint RomFSLength;
        public uint RomFSHashRegionSize;

        public uint Unknown6;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x20)]
        public byte[] ExeFSSuperBlockhash;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x20)]
        public byte[] RomFSSuperBlockhash;

        public static CXIPlaingRegion getPlainRegionStringsFrom(byte[] buffer)
        {
            CXIPlaingRegion temp = new CXIPlaingRegion();
            string bigstring = System.Text.ASCIIEncoding.ASCII.GetString(buffer);
            string[] splited = bigstring.Split(new char[] { '\0' }, StringSplitOptions.RemoveEmptyEntries);
            temp.PlainRegionStrings = splited;
            return temp;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CXIExtendedHeader
    {
        public CXIExtendedHeaderCodeSetInfo CodeSetInfo;
        public CXIExtendedHeaderDependencyList DependencyList;
        public CXIExtendedHeaderSystemInfo SystemInfo;
        public CXIExtendedHeaderArm11SystemLocalCaps Arm11SystemLocalCaps;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CXIExtendedHeaderCodeSetInfo
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public char[] Name;
        public CXIExtendedHeaderSystemInfoFlags Flags;
        public CXIExtendedHeaderCodeSegmentInfo Text;
        public uint StackSize;
        public CXIExtendedHeaderCodeSegmentInfo Ro;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Reserved;
        public CXIExtendedHeaderCodeSegmentInfo Data;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] BssSize;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CXIExtendedHeaderSystemInfoFlags
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public byte[] Reserved;
        public byte Flag;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] RemasterVersion;

    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CXIExtendedHeaderCodeSegmentInfo
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Address;
        public uint NumMaxPages;
        public uint CodeSize;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CXIExtendedHeaderDependencyList
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x30)]
        public String8[] ProgramID;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CXIExtendedHeaderSystemInfo
    {
        public uint SaveDataSize;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Reserved;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] JumpID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x30)]
        public byte[] Reserved2;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CXIExtendedHeaderStorageInfo
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] ExtSaveDataID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] SystemSaveDataID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] Reserved;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public byte[] AccessInfo;
        public byte OtherAttributes;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ResourceLimit
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] Data;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CXIExtendedHeaderArm11SystemLocalCaps
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] ProgramID;
        public ulong Flags;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x10)]
        public ResourceLimit[] ResourceLimitDescriptor;
        public CXIExtendedHeaderStorageInfo StorageInfo;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x20)]
        public String8[] ServiceAccessControl;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x1f)]
        public byte[] Reserved;
        public byte ResourceLimitCategory;
    }

    public struct NCCHDetails
    {
        public int crypto_type;
        public ulong ncch_size;

        public string encryption;
        public string ncch_type;
        public string ncch_size_str;
        public string content_type_str;
        public string sdk_build;
        public string sdk_version;
        public string req_kernel;

        public bool IsCfa;
        public UInt32 media_unit;

        public UInt32 UniqueID;

    }



    public class CXIContext : IContext
    {
        public CXIHeader Header;
        public NCCHDetails NcchInfo;
        private string errorMessage = string.Empty;
        public CXIPlaingRegion PlainRegion;
        public TitleInfo TitleInfo;
        //public CXIExtendedHeader ExtendedHeader;

        public long OffsetInNCSD;

        public enum CXIView
        {
            NCCH,
            NCCHPlainRegion
        };

        private enum SizeUnits
        {
            KB = 1024,
            MB = 1048576,
            GB = 1073741824
        };

        public enum NCCH_Crypto
        {
            None,
            Zeros,
            SystemFixed,
            Secure
        };

        public enum CXIActivation
        {
            Logo,
            RomFS,
            ExeFS,
            ExHeader,
            ReplaceRomFS,
            ReplaceExeFS
        }

        public bool Open(Stream fs)
        {
            PlainRegion = new CXIPlaingRegion();
            byte[] plainRegionBuffer;
            OffsetInNCSD = fs.Position;
            Header = MarshalUtil.ReadStruct<CXIHeader>(fs);
            // get Plaing Region
            fs.Seek(OffsetInNCSD + Header.PlainRegionOffset * 0x200, SeekOrigin.Begin);
            plainRegionBuffer = new byte[Header.PlainRegionSize * 0x200];
            fs.Read(plainRegionBuffer, 0, plainRegionBuffer.Length);
            PlainRegion = CXIHeader.getPlainRegionStringsFrom(plainRegionBuffer);
            GetNCCHDetails();
            // byte[] exhBytes = new byte[2048];
            // fs.Read(exhBytes, 0, exhBytes.Length); //TODO: read extended header
            // Array.Reverse(exh);
            TitleInfo = TitleInfo.Resolve(Header.ProductCode, Header.MakerCode);
            return true;
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
            //var i = values != null ? values[0] : -1;
            f.ClearInformation();
            switch ((CXIView)view)
            {
                case CXIView.NCCH:
                    int mod = 0;
                    if (!NcchInfo.IsCfa)
                    {
                        f.SetGroupHeaders("Title", NcchInfo.ncch_type + " Details", "Signature", "NCCH Header");
                        f.AddListItem(string.Empty, string.Empty, "Full Title (Name & Region)", string.Empty, TitleInfo.Title + " - " + TitleInfo.Region, 0);
                        f.AddListItem(string.Empty, string.Empty, "Title Type", string.Empty, TitleInfo.Type, 0);
                        f.AddListItem(string.Empty, string.Empty, "Publisher", string.Empty, TitleInfo.Developer, 0);
                    }
                    else 
                    {
                        f.SetGroupHeaders(NcchInfo.ncch_type + " Details", "Signature", "NCCH Header");
                        mod = 1;
                    }

                    
                    f.AddListItem(string.Empty, string.Empty, "Content Type", string.Empty, NcchInfo.content_type_str, 1-mod);
                    f.AddListItem(string.Empty, string.Empty, "Unique ID", string.Empty, "0x" + NcchInfo.UniqueID.ToString("X"), 1 - mod);
                    f.AddListItem(string.Empty, string.Empty, "Crypto Key", string.Empty, NcchInfo.encryption, 1 - mod);
                    f.AddListItem(string.Empty, string.Empty, NcchInfo.ncch_type + " Size", string.Empty, NcchInfo.ncch_size_str, 1 - mod);
                    if (!NcchInfo.IsCfa)
                    {
                        if (NcchInfo.sdk_build != null)
                        {
                            f.AddListItem(string.Empty, string.Empty, "Build Type", string.Empty, NcchInfo.sdk_build, 1);
                        }
                        if (NcchInfo.sdk_version != null)
                        {
                            f.AddListItem(string.Empty, string.Empty, "SDK Version", string.Empty, NcchInfo.sdk_version, 1);
                        }
                        if (NcchInfo.req_kernel != null)
                        {
                            f.AddListItem(string.Empty, string.Empty, "Required Kernel Version", string.Empty, NcchInfo.req_kernel, 1);
                        }
                    }
                    

                    f.AddListItem(0x000, 0x100, "RSA-2048 signature of the NCCH header [SHA-256]", Header.NCCHHeaderSignature, 2-mod);

                    f.AddListItem(0x100, 4, "Magic (='NCCH')", Header.Magic, 3-mod);
                    f.AddListItem(0x104, 4, NcchInfo.ncch_type + " length [media units]", Header.CXILength, 3-mod);
                    f.AddListItem(0x108, 8, "Title ID", Header.TitleID, 3-mod);
                    f.AddListItem(0x110, 2, "Maker Code", Header.MakerCode, 3-mod);
                    f.AddListItem(0x112, 2, NcchInfo.ncch_type + " Version", Header.Version, 3-mod);
                    f.AddListItem(0x118, 8, "Program ID", Header.ProgramID, 3-mod);
                    f.AddListItem(0x120, 0x10, "Reserved", Header.Unknown0_0, 3-mod);
                    f.AddListItem(0x130, 0x20, "Logo Region Hash", Header.LogoRegionHash, 3-mod);
                    f.AddListItem(0x150, 0x10, "Product Code", Header.ProductCode, 3-mod);
                    f.AddListItem(0x160, 0x20, "Extended Header Hash", Header.ExtendedHeaderHash, 3-mod);
                    f.AddListItem(0x180, 4, "Extended header size", Header.ExtendedHeaderSize, 3-mod);
                    f.AddListItem(0x184, 4, "Reserved", Header.Unknown1, 3-mod);
                    f.AddListItem(0x188, 8, "Flags", Header.Flags, 3-mod);
                    f.AddListItem(0x190, 4, "Plain region offset [media units]", Header.PlainRegionOffset, 3-mod);
                    f.AddListItem(0x194, 4, "Plain region length [media units]", Header.PlainRegionSize, 3-mod);
                    f.AddListItem(0x198, 4, "Logo region offset [media units]", Header.LogoRegionOffset, 3-mod);
                    f.AddListItem(0x19C, 4, "Logo region length [media units]", Header.LogoRegionLength, 3-mod);
                    f.AddListItem(0x1A0, 4, "ExeFS offset [media units]", Header.ExeFSOffset, 3-mod);
                    f.AddListItem(0x1A4, 4, "ExeFS length [media units]", Header.ExeFSLength, 3-mod);
                    f.AddListItem(0x1A8, 4, "ExeFS hash region length [media units]", Header.ExeFSHashRegionSize, 3-mod);
                    f.AddListItem(0x1AC, 4, "Reserved", Header.Unknown5, 3-mod);
                    f.AddListItem(0x1B0, 4, "RomFS offset [media units]", Header.RomFSOffset, 3-mod);
                    f.AddListItem(0x1B4, 4, "RomFS length [media units]", Header.RomFSLength, 3-mod);
                    f.AddListItem(0x1B8, 4, "RomFS hash region length [media units]", Header.RomFSHashRegionSize, 3-mod);
                    f.AddListItem(0x1BC, 4, "Reserved", Header.Unknown6, 3-mod);
                    f.AddListItem(0x1C0, 0x20, "ExeFS SuperBlock hash [SHA-256]", Header.ExeFSSuperBlockhash, 3 - mod);
                    f.AddListItem(0x1E0, 0x20, "RomFS Superblock hash [SHA-256]", Header.RomFSSuperBlockhash, 3 - mod);
                    break;
                case CXIView.NCCHPlainRegion:
                    f.SetGroupHeaders("Plain Regions");
                    for (var j = 0; j < PlainRegion.PlainRegionStrings.Length; j++)
                        f.AddListItem(0, 4, PlainRegion.PlainRegionStrings[j], (ulong)PlainRegion.PlainRegionStrings[j].Length, 0);
                    break;
            }
            f.AutoAlignColumns();
        }

        public bool CanCreate()
        {
            return false;
        }

        public void GetNCCHDetails()
        {
            // Reading Flags
            NcchInfo.encryption = null;
            NcchInfo.ncch_type = null;
           
            // Getting Media Unit
            NcchInfo.media_unit = (UInt32)(0x200 * Math.Pow(2, Header.Flags[6]));
            NcchInfo.ncch_size = (ulong)Header.CXILength * (ulong)NcchInfo.media_unit;
            DetermineContentType();
            DetermineCrypto();
            EndianSwap_IDs();
            GetNCCHSizeString();
            if (Header.PlainRegionSize > 0) 
            {
                ExtractPlainRegionDetails();
            }
            
        }

        public void EndianSwapByteArray(byte[] array)
        {
            byte[] tmp = new byte[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                tmp[i] = array[array.Length - 1 - i];
            }
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = tmp[i];
            }
        }

        public void ExtractPlainRegionDetails()
        {
            NcchInfo.sdk_build = "Release";
           
            bool found_sdk_ver = false;
            for(int i = 0; i < PlainRegion.PlainRegionStrings.Length; i++)
            {
                if (strncmp(PlainRegion.PlainRegionStrings[i],"[SDK+NINTENDO:DEBUG]",18))
                {
                    NcchInfo.sdk_build = "Debug or Development";
                }
                else if (strncmp(PlainRegion.PlainRegionStrings[i],"[SDK+NINTENDO:CTR_SDK-",21) && !found_sdk_ver)
                {
                    found_sdk_ver = true;
                    int j = 22;
                    int k = 0;
                    char[] major = new char[4];
                    char[] minor = new char[4];
                    char[] micro = new char[4];
                    char[] rev = new char[4];
                    char[] patch = new char[50];
                    string strcheck = PlainRegion.PlainRegionStrings[i];
                    while (strcheck.Length != j)
                    {
                        if (strcheck[j] == '_' || strcheck[j] == ']')
                        {
                            k = 0;
                            j++;
                            break;
                        }
                        major[k] = strcheck[j];
                        j++;
                        k++;
                    }
                    while (strcheck.Length != j)
                    {
                        if (strcheck[j] == '_' || strcheck[j] == ']')
                        {
                            k = 0;
                            j++;
                            break;
                        }
                        minor[k] = strcheck[j];
                        j++;
                        k++;
                    }
                    while (strcheck.Length != j)
                    {
                        if (strcheck[j] == '_' || strcheck[j] == ']')
                        {
                            k = 0;
                            j++;
                            break;
                        }
                        micro[k] = strcheck[j];
                        j++;
                        k++;
                    }
                    while (strcheck.Length != j)
                    {
                        if (strcheck[j] == '_' || strcheck[j] == ']')
                        {
                            k = 0;
                            j++;
                            break;
                        }
                        rev[k] = strcheck[j];
                        j++;
                        k++;
                    }
                    while (strcheck.Length != j)
                    {
                        if (strcheck[j] == '_' || strcheck[j] == ']')
                        {
                            k = 0;
                            j++;
                            break;
                        }
                        patch[k] = strcheck[j];
                        j++;
                        k++;
                    }
                    int major_int = 0;
                    if (major[0] != '\0')
                    {
                        major_int = Convert.ToInt32(new string(major));
                    }
                    int minor_int = 0;
                    if (minor[0] != '\0')
                    {
                        minor_int = Convert.ToInt32(new string(minor));
                    }
                    int micro_int = 0;
                    if (micro[0] != '\0')
                    {
                        micro_int = Convert.ToInt32(new string(micro));
                    }
                    string patch_str = new string(patch);
                    if (strncmp(patch_str,"none",4))
                        patch_str = "Release";
                    NcchInfo.sdk_version = major_int.ToString("D") + "." + minor_int.ToString("D") + "." + micro_int.ToString("D") + " " + patch_str;
                }
                else if (strncmp(PlainRegion.PlainRegionStrings[i],"[SDK+NINTENDO:Firmware-",22))
                {
                    int j = 23;
                    int k = 0;
                    char[] major = new char[4];
                    char[] minor = new char[4];
                    char[] build = new char[4];
                    string strcheck = PlainRegion.PlainRegionStrings[i];
                    while (strcheck.Length != j)
                    {
                        if (strcheck[j] == '_' || strcheck[j] == ']')
                        {
                            //major[k] = '\0';
                            k = 0;
                            j++;
                            break;
                        }
                        major[k] = strcheck[j];
                        j++;
                        k++;
                    }
                    while (strcheck.Length != j)
                    {
                        if (strcheck[j] == '_' || strcheck[j] == ']')
                        {
                            //minor[k] = '\0';
                            k = 0;
                            j++;
                            break;
                        }
                        minor[k] = strcheck[j];
                        j++;
                        k++;
                    }
                    while (strcheck.Length != j)
                    {
                        if (strcheck[j] == ']')
                        {
                            //build[k] = '\0';
                            k = 0;
                            j++;
                            break;
                        }
                        build[k] = strcheck[j];
                        j++;
                        k++;
                    }
                    int major_int = 0;
                    if (major[0] != '\0') 
                    {
                        major_int = Convert.ToInt32(new string(major));
                    }
                    int minor_int = 0;
                    if (minor[0] != '\0')
                    {
                        minor_int = Convert.ToInt32(new string(minor));
                    }
                    int build_int = 0;
                    if (build[0] != '\0')
                    {
                        build_int = Convert.ToInt32(new string(build));
                    }
                    NcchInfo.req_kernel = major_int.ToString("D") + "." + minor_int.ToString("D") + "-" + build_int.ToString("D");
                }
            }
            if (!found_sdk_ver)
            {
                if (Header.LogoRegionLength > 0)
                {
                    NcchInfo.sdk_version = "5.0.0 Release";
                }
                if(Header.Flags[3] == 1)
                {
                    NcchInfo.sdk_version = "7.0.0 Release";
                }
            }
           
        }

        public void FillCharArray(char[] array, char a)
        {
            for(int i = 0; i < array.Length ; i++)
            {
                array[i] = a;
            }
        }

        public bool strncmp(string a, string b, int len)
        {
            for (int i = 0; i < len; i++)
            {
                if (a.Length == i && b.Length == i)
                {
                    return true;
                }
                if (a.Length == i || b.Length == i)
                {
                    return false;
                }
                
                if (a[i] != b[i])
                {
                    return false;
                }
            }
            return true;
        }

        public void DetermineContentType()
        {
            // Getting NCCH Type
            int content_type = Header.Flags[5];
            if ((content_type & 0x1) == 0x1 && (content_type & 0x2) != 0x2)
            {
                NcchInfo.IsCfa = true;
                NcchInfo.ncch_type = "CFA";
                if ((content_type & 0x8) == 0x8 && (content_type & 0x4) != 0x4)
                    NcchInfo.content_type_str = "E-Manual";
                else if ((content_type & (0x4 | 0x8)) == (0x4 | 0x8))
                    NcchInfo.content_type_str = "Child";
                else if ((content_type & 0x4) == 0x4 && (content_type & 0x8) != 0x8)
                    NcchInfo.content_type_str = "Update Data";
                else {
                    NcchInfo.content_type_str = "File Archive";
                }
            }
            else if ((content_type & 0x2) == 0x2)
            {
                NcchInfo.IsCfa = false;
                NcchInfo.ncch_type = "CXI";
                if ((content_type & 0x1) == 0x1)
                {
                    NcchInfo.content_type_str = "Executable Content";
                }
                else
                {
                    NcchInfo.content_type_str = "Executable Content without RomFS";
                }
            }
        }

        public void DetermineCrypto()
        {
            int crypto_type = Header.Flags[7];

            // Getting Crypto Type
            if ((crypto_type & 0x1) == 0x1)
            {
                if ((crypto_type & 0x4) == 0x4)
                {
                    NcchInfo.encryption = "Not Encrypted";
                    NcchInfo.crypto_type = (int)NCCH_Crypto.None;
                }
                else if ((Header.ProgramID[4] & 0x10) == 0x10)
                {
                    NcchInfo.encryption = "System Fixed Key";
                    NcchInfo.crypto_type = (int)NCCH_Crypto.SystemFixed;
                }
                else
                {
                    NcchInfo.encryption = "Zeros Key";
                    NcchInfo.crypto_type = (int)NCCH_Crypto.Zeros;
                }
            }
            else
            {
                if (Header.Flags[3] == 1)
                    NcchInfo.encryption = "Secure Key (Type 2)";
                else
                    NcchInfo.encryption = "Secure Key";
                NcchInfo.crypto_type = (int)NCCH_Crypto.Secure;
            }
        }

        public void EndianSwap_IDs()
        {
            NcchInfo.UniqueID = BitConverter.ToUInt32(Header.ProgramID, 1);
            NcchInfo.UniqueID = NcchInfo.UniqueID & 0xffffff;
            EndianSwapByteArray(Header.TitleID);
            EndianSwapByteArray(Header.ProgramID);
        }

        public void GetNCCHSizeString()
        {
            if (NcchInfo.ncch_size < (long)SizeUnits.MB)
            {
                UInt64 size = NcchInfo.ncch_size / (UInt64)SizeUnits.KB;
                NcchInfo.ncch_size_str = size.ToString("D") + " KB";
            }
            else
            {
                UInt64 size = NcchInfo.ncch_size / (long)SizeUnits.MB;
                NcchInfo.ncch_size_str = size.ToString("D") + " MB";
            }
        }

        public void Activate(string filePath, int type, object[] values)
        {
            switch ((CXIActivation)type)
            {
                case CXIActivation.RomFS:
                case CXIActivation.ExeFS:
                    var isRom = (CXIActivation)type == CXIActivation.RomFS;    
                    var saveFileDialog = new SaveFileDialog() { Filter = "Binary files (*.bin)|*.bin"};
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string strKey = "";
                        
                        if(NcchInfo.crypto_type == (int)NCCH_Crypto.Zeros)
                            strKey = "00000000000000000000000000000000";
                        else if(NcchInfo.crypto_type != (int)NCCH_Crypto.None)
                            strKey = InputBox.ShowDialog("Please Enter Key:\nPress OK with empty key to save encrypted");
                        if (strKey != null) //Cancel wasn't pressed
                        {
                            // returns (null if error, byte[0] on Empty, byte[16] on valid)
                            var key = StringUtil.ParseKeyStringToByteArray(strKey);

                            if (key == null)
                                MessageBox.Show(@"Error parsing key string (must be a multiple of 2 and made of hex letters).");
                            else
                            {
                                var infs = File.OpenRead(filePath);
                                long offset = OffsetInNCSD + ((long)Header.ExeFSOffset * (long)NcchInfo.media_unit);
                                int size = (int)Header.ExeFSLength * (int)NcchInfo.media_unit;
                                if (isRom)
                                {
                                    offset = OffsetInNCSD + ((long)Header.RomFSOffset * (long)NcchInfo.media_unit);
                                    size = (int)Header.RomFSLength * (int)NcchInfo.media_unit;
                                }
                                infs.Seek(offset, SeekOrigin.Begin);
                                var buffer = new byte[size];
                                infs.Read(buffer, 0, buffer.Length);
                                infs.Close();
                                if (key.Length > 0)
                                {
                                    var iv = new byte[0x10];
                                    for (var i = 0; i < 8; i++)
                                        iv[i] = 0;
                                    Buffer.BlockCopy(Header.ProgramID, 0, iv, 8, 8); //TODO: change to TitleID

                                    var aes = new Aes128Ctr(key,iv);
                                    aes.TransformBlock(buffer);
                                }
                                var outpath = saveFileDialog.FileName;
                                var outfs = File.OpenWrite(outpath);
                                outfs.Write(buffer, 0, buffer.Length);
                                outfs.Close();
                            }
                        }
                    }
                    break;
                case CXIActivation.ExHeader:
                    saveFileDialog = new SaveFileDialog() { Filter = "Binary files (*.bin)|*.bin" };
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string strKey = "";
                        if (NcchInfo.crypto_type != (int)NCCH_Crypto.None)
                            strKey = InputBox.ShowDialog("Please Enter Key:\nPress OK with empty key to save encrypted");
                        if (strKey != null) //Cancel wasn't pressed
                        {
                            // returns (null if error, byte[0] on Empty, byte[16] on valid)
                            var key = StringUtil.ParseKeyStringToByteArray(strKey);

                            if (key == null)
                                MessageBox.Show(@"Error parsing key string (must be a multiple of 2 and made of hex letters).");
                            else
                            {
                                var infs = File.OpenRead(filePath);
                                infs.Seek(OffsetInNCSD + Marshal.SizeOf(Header), SeekOrigin.Begin); //right after the header
                                var buffer = new byte[Header.ExtendedHeaderSize];
                                infs.Read(buffer, 0, buffer.Length);
                                infs.Close();
                                if (key.Length > 0)
                                {
                                    var iv = new byte[0x10];
                                    for (var i = 0; i < 8; i++)
                                        iv[i] = 0;
                                    Buffer.BlockCopy(Header.ProgramID, 0, iv, 8, 8); //TODO: change to TitleID

                                    var aes = new Aes128Ctr(key, iv);
                                    aes.TransformBlock(buffer);
                                }
                                var outpath = saveFileDialog.FileName;
                                var outfs = File.OpenWrite(outpath);
                                outfs.Write(buffer, 0, buffer.Length);
                                outfs.Close();
                            }
                        }
                    }
                    break;
                case CXIActivation.Logo:
                    saveFileDialog = new SaveFileDialog() { Filter = "Binary files (*.bin)|*.bin" };
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        var infs = File.OpenRead(filePath);
                        infs.Seek(OffsetInNCSD + (Header.LogoRegionOffset * NcchInfo.media_unit), SeekOrigin.Begin); //right after the header
                        var buffer = new byte[Header.LogoRegionLength * NcchInfo.media_unit];
                        infs.Read(buffer, 0, buffer.Length);
                        var outpath = saveFileDialog.FileName;
                        var outfs = File.OpenWrite(outpath);
                        outfs.Write(buffer, 0, buffer.Length);
                        outfs.Close();
                           
                    }
                    break;
            }
        }

        public string GetFileFilter()
        {
            if (NcchInfo.IsCfa)
                return "CTR File Archive (*.cfa)|*.cfa";
            else 
                return "CTR Executable Image (*.cxi)|*.cxi";
        }

        public TreeNode GetExplorerTopNode()
        {
            var tNode = new TreeNode(string.Format(NcchInfo.ncch_type + " ({0})", StringUtil.CharArrayToString(Header.ProductCode))) { Tag = TreeViewContextTag.Create(this, (int)CXIView.NCCH) };
            if (Header.PlainRegionSize > 0) //Add PlainRegions
                tNode.Nodes.Add("PlainRegion").Tag = TreeViewContextTag.Create(this, (int)CXIView.NCCHPlainRegion);
            return tNode;
        }

        public TreeNode GetFileSystemTopNode()
        {
            var tNode = new TreeNode(string.Format(NcchInfo.ncch_type + " ({0})", StringUtil.CharArrayToString(Header.ProductCode)), 1, 1);
                // Thanks to Ris312 for that fix! ExtendedHeaderSize
            if (Header.ExtendedHeaderSize > 0)
                tNode.Nodes.Add(new TreeNode(
                    TreeListView.TreeListViewControl.CreateMultiColumnNodeText(
                        "ExHeader.bin",
                        (Header.ExtendedHeaderSize).ToString(),
                        StringUtil.ToHexString(6, (ulong)(OffsetInNCSD + Marshal.SizeOf(Header)))
                        )) { Tag = new[] { TreeViewContextTag.Create(this, (int)CXIActivation.ExHeader, "Save...") } });
            if (Header.LogoRegionLength > 0)
                tNode.Nodes.Add(new TreeNode(
                    TreeListView.TreeListViewControl.CreateMultiColumnNodeText(
                        "Logo.bin",
                        ((ulong)Header.LogoRegionLength * (ulong)NcchInfo.media_unit).ToString(),
                        StringUtil.ToHexString(6, (ulong)OffsetInNCSD + (ulong)Header.LogoRegionOffset * (ulong)NcchInfo.media_unit)
                        )) { Tag = new[] { TreeViewContextTag.Create(this, (int)CXIActivation.Logo, "Save...") } }); 

                if (Header.ExeFSLength > 0)
                    tNode.Nodes.Add(new TreeNode(
                        TreeListView.TreeListViewControl.CreateMultiColumnNodeText(
                            "ExeFS.bin",
                            ((ulong)Header.ExeFSLength * (ulong)NcchInfo.media_unit).ToString(),
                            StringUtil.ToHexString(6, (ulong)OffsetInNCSD + (ulong)Header.ExeFSOffset * (ulong)NcchInfo.media_unit)
                            )) 
                            { Tag = new[] {TreeViewContextTag.Create(this, (int)CXIActivation.ExeFS,"Save...")} });

                if (Header.RomFSLength > 0)
                    tNode.Nodes.Add(new TreeNode(
                            TreeListView.TreeListViewControl.CreateMultiColumnNodeText(
                                "RomFS.bin",
                                ((ulong)Header.RomFSLength * (ulong)NcchInfo.media_unit).ToString(),
                                StringUtil.ToHexString(6, (ulong)OffsetInNCSD + (ulong)Header.RomFSOffset * (ulong)NcchInfo.media_unit)
                                )) { Tag = new[] { TreeViewContextTag.Create(this, (int)CXIActivation.RomFS, "Save...") } });
            return tNode;
        }
    }

}
