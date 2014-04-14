using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace _3DSExplorer.Modules
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)] //, Size = 0x330
    public struct CCICXIEntry
    {
        public uint Offset;
        public uint Length;
    }

    public struct TitleID
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] ID;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)] //, Size = 0x330
    public struct CCIHeader
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x100)]
        public byte[] NCSDHeaderSignature;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public char[] Magic;
        public uint CCILength;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] MainTitleID;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] PartitionFStype;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] PartitionCrypttype;
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public CCICXIEntry[] CXIEntries;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x20)]
        public byte[] ExHeaderHash;

        public uint AdditionalHeaderSize;
        public uint SectorZeroOffset;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] Flags;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public TitleID[] NCCHTitleIDs;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x30)]
        public byte[] Reserved_0;

        // Card Info Header
        public uint WritableRegionAddress;

        public uint CardInfoBitmask;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0xF8)]
        public byte[] Reserved_1;
        
        // Production "makerom" notes start 
        public uint UsedRomLength; //in bytes

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x1C)]
        public byte[] Reserved_2;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] CVerTitleId;
        public UInt16 CVerTitleVersion;
        // Production "makerom" notes end
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0xCD6)]
        public byte[] Reserved_3;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x8)]
        public byte[] MediaID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x8)]
        public byte[] Reserved_4;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x30)]
        public byte[] InitialData;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0xC0)]
        public byte[] Reserved_5;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x100)]
        public byte[] cxi_Header;

        // Dev Card Info Header
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x200)]
        public byte[] CardDeviceReserved1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x10)]
        public byte[] TitleKey;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0xF0)]
        public byte[] CardDeviceReserved2;
    }

    public struct NCSDHDetails
    {
        public string ncsd_type;

        public string CardDevice;
        public string MediaType;
        public string SaveCrypto;
        public string Min3DSFirm;

        // Media Unit Size, used for basicly everything
        public UInt32 media_unit;

        // CARD2 Writable Region
        public bool HasWritableRegion;
        public UInt64 WritableRegionAddress;
        public UInt64 WritableRegionSize;

        // Various Sizes
        public UInt64 Media_Size;
        public UInt64 Media_Size_Gbit;
        public string Media_Size_Str;
        public UInt64 CCI_Data_Size;
        public string CCI_Data_Size_Str;
        public UInt64 CCI_S_Trimmed_Size;

        // CCI File Condition
        public UInt64 CCI_File_Size;
        public string CCI_File_Size_Str;
        public int CCI_File_Status;
        public string CCI_File_Status_Str;
    }


    public class CCIContext : IContext
    {
        private string _errorMessage = string.Empty;
        public CCIHeader Header;
        public NCSDHDetails NcsdInfo;
        private CXIContext[] CXIContexts;

        private enum CCIView
        {
            NCSD
        };

        private enum CCIActivation
        {
            SaveNCCH,
            SaveWritableRegion
        };

        private enum SizeUnits
        {
            KB = 1024,
            MB = 1048576,
            GB = 1073741824
        };

        private enum MediaType
        {
            InternalDevice = 0,
            Card1 = 1,
            Card2 = 2,
            ExternalDevice = 3
        };

        private enum CardDevice
        {
            CARD_DEVICE_NOR_FLASH = 1,
            CARD_DEVICE_NONE = 2,
            CARD_DEVICE_BT = 3
        };

        private enum CCI_File_Status
        {
            CCI_Malformed = 0,
            CCI_FullSize = 1,
            CCI_DataSize = 2,
            CCI_UpdateDataRemoved = 3
        };

        private enum NCSD_Flags
        {
            MEDIA_6X_SAVE_CRYPTO = 1,
            MEDIA_CARD_DEVICE = 3,
            MEDIA_PLATFORM_INDEX = 4,
            MEDIA_TYPE_INDEX = 5,
            MEDIA_UNIT_SIZE = 6,
            MEDIA_CARD_DEVICE_OLD = 7
        };
        public bool Open(Stream fs)
        {
            Header = MarshalUtil.ReadStruct<CCIHeader>(fs);
            GetNCSDDetails(fs);
            if (Header.Flags[(int)NCSD_Flags.MEDIA_TYPE_INDEX] < 1 || Header.Flags[(int)NCSD_Flags.MEDIA_TYPE_INDEX] > 2) //Checking if CCI/CSU
            {
                _errorMessage = "NCSD Image is not CCI/CSU";
                return false;
            }
            CXIContexts = new CXIContext[8];
            // Read the CXIs
            for (var i = 0; i < CXIContexts.Length;i++ )
                if (Header.CXIEntries[i].Length > 0)
                {
                    CXIContexts[i] = new CXIContext();
                    var offset = Header.CXIEntries[i].Offset * NcsdInfo.media_unit; 
                    if (!(fs is MemoryStream) ||  offset < fs.Length) //fix for reading from memory streams (archived)
                        fs.Seek(offset, SeekOrigin.Begin);
                    else
                        fs.Seek(0, SeekOrigin.End);
                    CXIContexts[i].Open(fs);
                }
            GetNCSDType();
            return true;
        }

        private static void Fill(Stream stream, byte value, UInt32 media_unit, UInt32 len)
        {
            // Creating Dummy Byte Buffer
            byte[] buffer = new byte[media_unit];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = value;
            }

            //
            for (UInt32 i = 0; i < len; i++) 
            {
                stream.Write(buffer, 0, (int)media_unit);
            }
        }

        public void ToggleTrimmed(Stream fs)
        {
            if (NcsdInfo.CCI_File_Status == (int)CCI_File_Status.CCI_DataSize) // CCI is trimmed, so this will un trim
            {
                fs.SetLength((long)NcsdInfo.Media_Size); // Setting to full size
                fs.Seek((long)NcsdInfo.CCI_Data_Size, SeekOrigin.Begin);
                Fill(fs, 0xFF, NcsdInfo.media_unit, (UInt32)(NcsdInfo.Media_Size / NcsdInfo.media_unit) - (UInt32)(NcsdInfo.CCI_Data_Size / NcsdInfo.media_unit));
            }
            else // CCI is full size, so this will trim
            {
                fs.SetLength((long)NcsdInfo.CCI_Data_Size); // Setting to CCI Data size
            }
        }

        public void SuperTrim(Stream fs)
        {
            fs.SetLength((long)NcsdInfo.CCI_S_Trimmed_Size); // The Offset of the update partition
        }

        public string GetErrorMessage()
        {
            return _errorMessage;
        }

        public void Create(FileStream fs, FileStream src)
        {
            throw new NotImplementedException();
        }

        public void View(frmExplorer f, int view, object[] values)
        {
            //var i = values != null ? (int)values[0] : -1;
            f.ClearInformation();
            switch ((CCIView)view)
            {
                case CCIView.NCSD:
                    f.SetGroupHeaders(NcsdInfo.ncsd_type + " Info","Signature", "NCSD Header", "Card Info Header", "Dev Card Info Header");

                    // CCI Details
                    f.AddListItem(string.Empty, string.Empty, "Media Type", NcsdInfo.MediaType,string.Empty, 0);
                    /*if (NcsdInfo.HasWritableRegion)
                    {
                        f.AddListItem(string.Empty, string.Empty, " > Writable Region Offset:", "0x" + NcsdInfo.WritableRegionAddress.ToString("X"), string.Empty, 0);
                        f.AddListItem(string.Empty, string.Empty, " > Writable Region Size:", "0x" + NcsdInfo.WritableRegionSize.ToString("X"), string.Empty, 0);
                    }*/
                    f.AddListItem(string.Empty, string.Empty, "Media Size", NcsdInfo.Media_Size_Str, NcsdInfo.Media_Size_Gbit.ToString("D") + " Gbit", 0);
                    f.AddListItem(string.Empty, string.Empty, "CCI Data Size", NcsdInfo.CCI_Data_Size_Str, "0x" + NcsdInfo.CCI_Data_Size.ToString("X") + " Bytes", 0);
                    f.AddListItem(string.Empty, string.Empty, "CCI File", string.Empty, string.Empty, 0);
                    f.AddListItem(string.Empty, string.Empty, " > Size:", NcsdInfo.CCI_File_Size_Str, string.Empty, 0);
                    f.AddListItem(string.Empty, string.Empty, " > Status:", NcsdInfo.CCI_File_Status_Str, string.Empty, 0);
                    f.AddListItem(string.Empty, string.Empty, "Card Device", NcsdInfo.CardDevice, string.Empty, 0);
                    f.AddListItem(string.Empty, string.Empty, "Save Crypto", NcsdInfo.SaveCrypto, string.Empty, 0);
                    if (Header.CVerTitleVersion > 0)
                    {
                        f.AddListItem(string.Empty, string.Empty, "Minimum 3DS Firmware", NcsdInfo.Min3DSFirm, string.Empty, 0);
                    }

                    // Signature
                    f.AddListItem(0x000, 0x100, "RSA-2048 signature of the NCSD header [SHA-256]", Header.NCSDHeaderSignature, 1);

                    // NCSD Header
                    f.AddListItem(0x100, 4, "Magic (='NCSD')", Header.Magic, 2);
                    f.AddListItem(0x104, 4, "NCSD length [media units]", Header.CCILength, 2);
                    f.AddListItem(0x108, 8, "Main Title ID", Header.MainTitleID, 2);
                    f.AddListItem(0x110, 8, "Partitions FS Type", Header.PartitionFStype, 2);
                    f.AddListItem(0x118, 8, "Partitions Crypt Type", Header.PartitionCrypttype, 2);
                    for (var i = 0; i < 8; i++)
                    {
                        f.AddListItem(0x120 + i * 8, 4, "NCCH " + i + " offset [media units]", Header.CXIEntries[i].Offset, 2);
                        f.AddListItem(0x124 + i * 8, 4, "NCCH " + i + " length [media units]", Header.CXIEntries[i].Length, 2);
                    }
                    f.AddListItem(0x160, 0x20, "ExHeader Hash [SHA-256]", Header.ExHeaderHash, 2);
                    f.AddListItem(0x180, 4, "Additional Header Size [media units]", Header.AdditionalHeaderSize, 2);
                    f.AddListItem(0x184, 4, "Sector Zero Offset [media units]", Header.SectorZeroOffset, 2);
                    f.AddListItem(0x188, 8, NcsdInfo.ncsd_type + " Flags", Header.Flags, 2);
                    for (var i = 0; i < 8; i++)
                    {
                        f.AddListItem(0x190 + i * 8, 8, "NCCH " + i + " Title ID", Header.NCCHTitleIDs[i].ID, 2);
                    }
                    f.AddListItem(0x1D0, 0x30, "Reserved", Header.Reserved_0, 2);
                   

                    // Card Info Header
                    f.AddListItem(0x200, 4, "Writable Address [media units]", Header.WritableRegionAddress, 3);
                    f.AddListItem(0x204, 4, "Card Info Bitmask", Header.CardInfoBitmask, 3);
                    f.AddListItem(0x208, 248, "Reserved", Header.Reserved_1, 3);
                    f.AddListItem(0x300, 4, "Used ROM length [bytes]", Header.UsedRomLength, 3);
                    f.AddListItem(0x304, 28, "Reserved", Header.Reserved_2, 3);
                    f.AddListItem(0x320, 8, "CVer Title ID", Header.CVerTitleId, 3);
                    f.AddListItem(0x328, 2, "CVer Title Version", Header.CVerTitleVersion, 3);
                    f.AddListItem(0x32B, 28, "Reserved", Header.Reserved_3, 3);
                    f.AddListItem(0x1000, 8, "Media ID", Header.MediaID, 3);
                    f.AddListItem(0x1008, 8, "Reserved", Header.Reserved_4, 3);
                    f.AddListItem(0x1010, 0x30, "Initial Data", Header.InitialData, 3);
                    f.AddListItem(0x1040, 0xC0, "Reserved", Header.Reserved_5, 3);
                    // NCCH Header Duplicate of NCCH 0
                    /*
                    f.AddListItem(0x1100, 4, "Magic (='NCCH')", Header.cxi_Header.Magic, 3);
                    f.AddListItem(0x1104, 4, "NCCH length [media units]", Header.cxi_Header.CXILength, 3);
                    f.AddListItem(0x1108, 8, "Title ID", Header.cxi_Header.TitleID, 3);
                    f.AddListItem(0x1110, 2, "Maker Code", Header.cxi_Header.MakerCode, 3);
                    f.AddListItem(0x1112, 2, "NCCH Version", Header.cxi_Header.Version, 3);
                    f.AddListItem(0x1118, 8, "Program ID", Header.cxi_Header.ProgramID, 3);
                    f.AddListItem(0x1120, 0x10, "Reserved", Header.cxi_Header.Unknown0_0, 3);
                    f.AddListItem(0x1130, 0x20, "Logo Region Hash", Header.cxi_Header.LogoRegionHash, 3);
                    f.AddListItem(0x1150, 0x10, "Product Code", Header.cxi_Header.ProductCode, 3);
                    f.AddListItem(0x1160, 0x20, "Extended Header Hash", Header.cxi_Header.ExtendedHeaderHash, 3);
                    f.AddListItem(0x1180, 4, "Extended header size", Header.cxi_Header.ExtendedHeaderSize, 3);
                    f.AddListItem(0x1184, 4, "Reserved", Header.cxi_Header.Unknown1, 3);
                    f.AddListItem(0x1188, 8, "Flags", Header.cxi_Header.Flags, 3);
                    f.AddListItem(0x1190, 4, "Plain region offset [media units]", Header.cxi_Header.PlainRegionOffset, 3);
                    f.AddListItem(0x1194, 4, "Plain region length [media units]", Header.cxi_Header.PlainRegionSize, 3);
                    f.AddListItem(0x1198, 4, "Logo region offset [media units]", Header.cxi_Header.LogoRegionOffset, 3);
                    f.AddListItem(0x119C, 4, "Logo region length [media units]", Header.cxi_Header.LogoRegionLength, 3);
                    f.AddListItem(0x11A0, 4, "ExeFS offset [media units]", Header.cxi_Header.ExeFSOffset, 3);
                    f.AddListItem(0x11A4, 4, "ExeFS length [media units]", Header.cxi_Header.ExeFSLength, 3);
                    f.AddListItem(0x11A8, 4, "ExeFS hash region length [media units]", Header.cxi_Header.ExeFSHashRegionSize, 3);
                    f.AddListItem(0x11AC, 4, "Reserved", Header.cxi_Header.Unknown5, 3);
                    f.AddListItem(0x11B0, 4, "RomFS offset [media units]", Header.cxi_Header.RomFSOffset, 3);
                    f.AddListItem(0x11B4, 4, "RomFS length [media units]", Header.cxi_Header.RomFSLength, 3);
                    f.AddListItem(0x11B8, 4, "RomFS hash region length [media units]", Header.cxi_Header.RomFSHashRegionSize, 3);
                    f.AddListItem(0x11BC, 4, "Reserved", Header.cxi_Header.Unknown6, 3);
                    f.AddListItem(0x11C0, 0x20, "ExeFS SuperBlock hash [SHA-256]", Header.cxi_Header.ExeFSSuperBlockhash, 3);
                    f.AddListItem(0x11E0, 0x20, "RomFS SuperBlock hash [SHA-256]", Header.cxi_Header.RomFSSuperBlockhash, 3);
                    */
                    // Dev Card Info Header
                    if (Header.CardDeviceReserved1[0] != 0xff && Header.CardDeviceReserved1[10] != 0xff)
                    {
                        f.AddListItem(0x1200, 0x200, "CardDeviceReserved1", Header.CardDeviceReserved1, 4);
                        f.AddListItem(0x1400, 0x10, "TitleKey", Header.TitleKey, 4);
                        f.AddListItem(0x1410, 0xf0, "CardDeviceReserved2", Header.CardDeviceReserved2, 4);
                    }
                    break;
            }
            f.AutoAlignColumns();
        }

        public bool CanCreate()
        {
            return false;
        }

        public void GetNCSDDetails(Stream fs)
        {
            // Important Value, used in most CCI offset/len calculations
            NcsdInfo.media_unit = (UInt32)(0x200 * Math.Pow(2, Header.Flags[(int)NCSD_Flags.MEDIA_UNIT_SIZE]));
            
            NcsdInfo.CCI_File_Size = (UInt64)fs.Length; // Size of CCI file
            NcsdInfo.Media_Size = (ulong)Header.CCILength * (ulong)NcsdInfo.media_unit; // Size of Media
            GetCCI_DataSize(); // Size used up by actual CCI data
            
            // Checking Status of CCI file
            CheckCCI_FileSize();
            
            // Getting Formatted Strings relating to Important Size data
            GetSizeStr();
            
            // Interpreting NCSD Flags, preparing formatted strings
            InterpreteNCSD_Flags();

            // Calculate Min FW from CVer Version
            GetMinFW();
            
            // Swap Order of TitleID byte arrays
            EndianSwap_TitleIDs();

            // Determine Save Crypto
            GetSaveCrypto();
        }

        public void GetSaveCrypto()
        {
            if (Header.Flags[(int)NCSD_Flags.MEDIA_6X_SAVE_CRYPTO] == 0 && Header.Flags[(int)NCSD_Flags.MEDIA_CARD_DEVICE] == 0 && Header.Flags[(int)NCSD_Flags.MEDIA_CARD_DEVICE_OLD] == 0)
            {
                NcsdInfo.SaveCrypto = "Repeating CTR Fail";
            }
            else if (Header.Flags[(int)NCSD_Flags.MEDIA_6X_SAVE_CRYPTO] == 0 && (Header.Flags[(int)NCSD_Flags.MEDIA_CARD_DEVICE] != 0 || Header.Flags[(int)NCSD_Flags.MEDIA_CARD_DEVICE_OLD] != 0))
            {
                NcsdInfo.SaveCrypto = "2.2.0-4 KeyY Method";
            }
            else if (Header.Flags[(int)NCSD_Flags.MEDIA_6X_SAVE_CRYPTO] == 1 && Header.Flags[(int)NCSD_Flags.MEDIA_CARD_DEVICE_OLD] == 0)
            {
                //if(header->partition_flags[MEDIA_CARD_DEVICE] == 2){
                //	printf(" Save Crypto:           2.2.0-4 KeyY Method\n");
                //}
                /*else*/
                if (Header.Flags[(int)NCSD_Flags.MEDIA_CARD_DEVICE] != 0/* == 3*/)
                {
                    NcsdInfo.SaveCrypto = "6.0.0-11 KeyY Method";
                }
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

        public void EndianSwap_TitleIDs()
        {
            byte[] tmp = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                EndianSwapByteArray(Header.NCCHTitleIDs[i].ID);
            }
            EndianSwapByteArray(Header.MainTitleID);
            EndianSwapByteArray(Header.CVerTitleId);
            EndianSwapByteArray(Header.MediaID);
        }

        public void InterpreteNCSD_Flags()
        {
            CardDevice CardDevice_flag = (CardDevice)(Header.Flags[3] | Header.Flags[7]);
            MediaType MediaType_flag = (MediaType)Header.Flags[5];
            switch (MediaType_flag)
            {
                case (MediaType.InternalDevice): NcsdInfo.MediaType = "Internal Device"; break;
                case (MediaType.Card1): NcsdInfo.MediaType = "Card1"; break;
                case (MediaType.Card2): NcsdInfo.MediaType = "Card2"; break;
                case (MediaType.ExternalDevice): NcsdInfo.MediaType = "External Device"; break;
            }
            if (MediaType_flag == MediaType.Card2)
            {
                NcsdInfo.HasWritableRegion = true;
                NcsdInfo.WritableRegionAddress = Header.WritableRegionAddress * NcsdInfo.media_unit;
                NcsdInfo.WritableRegionSize = NcsdInfo.Media_Size - NcsdInfo.WritableRegionAddress;
            }
            else
            {
                NcsdInfo.HasWritableRegion = false;
            }
            switch (CardDevice_flag)
            {
                case (CardDevice.CARD_DEVICE_BT): NcsdInfo.CardDevice = "BT"; break;
                case (CardDevice.CARD_DEVICE_NONE): NcsdInfo.CardDevice = "None"; break;
                case (CardDevice.CARD_DEVICE_NOR_FLASH): NcsdInfo.CardDevice = "EEPROM"; break;
            }
        }

        public void GetCCI_DataSize()
        {
            NcsdInfo.CCI_Data_Size = (ulong)Header.CXIEntries[0].Offset * (ulong)NcsdInfo.media_unit;
            for (int i = 0; i < 8; i++)
            {
                NcsdInfo.CCI_Data_Size += (ulong)Header.CXIEntries[i].Length * (ulong)NcsdInfo.media_unit;
                if (i == 6)
                {
                    NcsdInfo.CCI_S_Trimmed_Size = NcsdInfo.CCI_Data_Size;
                }
            }
        }

        public void CheckCCI_FileSize()
        {
            if (NcsdInfo.CCI_File_Size == NcsdInfo.Media_Size)
            {
                NcsdInfo.CCI_File_Status = (int)CCI_File_Status.CCI_FullSize;
                NcsdInfo.CCI_File_Status_Str = "Full Size";
            }
            else if (NcsdInfo.CCI_File_Size == NcsdInfo.CCI_Data_Size)
            {
                NcsdInfo.CCI_File_Status = (int)CCI_File_Status.CCI_DataSize;
                NcsdInfo.CCI_File_Status_Str = "Trimmed";
            }
            else if (NcsdInfo.CCI_File_Size == NcsdInfo.CCI_S_Trimmed_Size)
            {
                NcsdInfo.CCI_File_Status = (int)CCI_File_Status.CCI_UpdateDataRemoved;
                NcsdInfo.CCI_File_Status_Str = "Update Partition Removed (Not Reversible)";
            }
            else
            {
                NcsdInfo.CCI_File_Status = (int)CCI_File_Status.CCI_Malformed;
                NcsdInfo.CCI_File_Status_Str = "Malformed";
            }
        }

        public void GetSizeStr()
        {
            // Media Size
            if (NcsdInfo.Media_Size < (long)SizeUnits.MB)
            {
                UInt64 size = NcsdInfo.Media_Size / (UInt64)SizeUnits.KB;
                NcsdInfo.Media_Size_Str = size.ToString("D") + " KB";
            }
            else if (NcsdInfo.Media_Size < (long)SizeUnits.GB)
            {
                UInt64 size = NcsdInfo.Media_Size / (UInt64)SizeUnits.MB;
                NcsdInfo.Media_Size_Str = size.ToString("D") + " MB";
            }
            else
            {
                UInt64 size = NcsdInfo.Media_Size / (UInt64)SizeUnits.GB;
                NcsdInfo.Media_Size_Str = size.ToString("D") + " GB";
            }

            // CCI Data Size
            if (NcsdInfo.CCI_Data_Size < (long)SizeUnits.MB)
            {
                UInt64 size = NcsdInfo.CCI_Data_Size / (UInt64)SizeUnits.KB;
                NcsdInfo.CCI_Data_Size_Str = size.ToString("D") + " KB";
            }
            else
            {
                UInt64 size = NcsdInfo.CCI_Data_Size / (long)SizeUnits.MB;
                NcsdInfo.CCI_Data_Size_Str = size.ToString("D") + " MB";
            }
            NcsdInfo.Media_Size_Gbit = (NcsdInfo.Media_Size * 8) / (UInt64)SizeUnits.GB;

            // CCI File Size
            if (NcsdInfo.CCI_File_Size < (long)SizeUnits.MB)
            {
                UInt64 size = NcsdInfo.CCI_File_Size / (long)SizeUnits.KB;
                NcsdInfo.CCI_File_Size_Str = size.ToString("D") + " KB";
            }
            else
            {
                UInt64 size = NcsdInfo.CCI_File_Size / (long)SizeUnits.MB;
                NcsdInfo.CCI_File_Size_Str = size.ToString("D") + " MB";
            }
        }

        public void GetMinFW()
        {
            string Region = "X";
            int MAJOR = 0;
            int MINOR = 0;
            int BUILD = 0;
            switch (Header.CVerTitleId[1]) 
            {
                case (0x71): Region = "E"; break;
                case (0x72): Region = "J"; break;
                case (0x73): Region = "U"; break;
                case (0x74): Region = "C"; break;
                case (0x75): Region = "K"; break;
                case (0x76): Region = "T"; break;
            }
            switch (Header.CVerTitleVersion)
            {
                case (3088): MAJOR = 3; MINOR = 0; BUILD = 0; break;
                default:
                    MAJOR = (Header.CVerTitleVersion & 0x7C00) >> 10;
                    MINOR = (Header.CVerTitleVersion & 0x3e0) >> 5;
                    BUILD = Header.CVerTitleVersion & 0x1F; 
                    break;
            }

            NcsdInfo.Min3DSFirm = MAJOR.ToString("D") + "." + MINOR.ToString("D") + "." + BUILD.ToString("D") + "-X" + Region;
        }

        public void GetNCSDType() 
        {
            NcsdInfo.ncsd_type = "CCI";
            if (CXIContexts[0].Header.PlainRegionSize == 0) // This seems to be the case
            {
                NcsdInfo.ncsd_type = "CSU";
            }
        }

        public void Activate(string filePath, int type, object[] values)
        {
            switch ((CCIActivation)type)
            {
                case CCIActivation.SaveNCCH:
                    var cxiIndex = (int)values[0];
                    var saveFileDialog = new SaveFileDialog() { Filter = CXIContexts[cxiIndex].GetFileFilter(), FileName = CXIContexts[cxiIndex].TitleInfo.ProductCode };
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        var infs = File.OpenRead(filePath);
                        infs.Seek(Header.CXIEntries[cxiIndex].Offset * NcsdInfo.media_unit, SeekOrigin.Begin);
                        SaverProcess.Run("Saving " + CXIContexts[cxiIndex].NcchInfo.ncch_type,infs,saveFileDialog.FileName,Header.CXIEntries[cxiIndex].Length*NcsdInfo.media_unit);
                    }
                    break;
                case CCIActivation.SaveWritableRegion:
                    saveFileDialog = new SaveFileDialog() { Filter = "Binary files (*.bin)|*.bin" };
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        var infs = File.OpenRead(filePath);
                        infs.Seek((long)NcsdInfo.WritableRegionAddress, SeekOrigin.Begin); //right after the header
                        var buffer = new byte[(long)NcsdInfo.WritableRegionSize];
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
            return "CTR Cartridge Images (*.cci/3ds/csu)|*.3ds;*.cci;*.csu";
        }

        public TreeNode GetExplorerTopNode()
        {
            var tNode = new TreeNode(NcsdInfo.ncsd_type) { Tag = TreeViewContextTag.Create(this, (int)CCIView.NCSD) };
            for (var i = 0; i < CXIContexts.Length; i++)
                if (CXIContexts[i] != null && Header.CXIEntries[i].Offset*NcsdInfo.media_unit < NcsdInfo.CCI_File_Size) // Only shows NCCH files which actually exist in CCI
                {
                    var cxiNode = CXIContexts[i].GetExplorerTopNode();
                    cxiNode.Text = cxiNode.Text + @" " + i;
                    tNode.Nodes.Add(cxiNode);
                }
            return tNode;
        }

        public TreeNode GetFileSystemTopNode()
        {
            var tNode = new TreeNode(NcsdInfo.ncsd_type, 1, 1);
            for (var i = 0; i < CXIContexts.Length; i++)
                if (CXIContexts[i] != null && Header.CXIEntries[i].Offset * NcsdInfo.media_unit < NcsdInfo.CCI_File_Size) // Only shows NCCH files which actually exist in CCI
                {
                    var cxiNode = CXIContexts[i].GetFileSystemTopNode();
                    cxiNode.Text = cxiNode.Text + @" " + i;
                    cxiNode.Tag = new[] { TreeViewContextTag.Create(this, (int)CCIActivation.SaveNCCH, "Save " + CXIContexts[i].NcchInfo.ncch_type + "...", new object[] { i }) };
                    tNode.Nodes.Add(cxiNode);
                }
            if (NcsdInfo.HasWritableRegion && NcsdInfo.CCI_File_Status == (int)CCI_File_Status.CCI_FullSize) // Only give option to save writable region, if it exists and the CCI dump is still large enough to hold it
            {
                tNode.Nodes.Add(new TreeNode(
                            TreeListView.TreeListViewControl.CreateMultiColumnNodeText(
                                "WritableRegion.bin",
                                (NcsdInfo.WritableRegionSize).ToString(),
                                StringUtil.ToHexString(6, NcsdInfo.WritableRegionAddress)
                                )) { Tag = new[] { TreeViewContextTag.Create(this, (int)CCIActivation.SaveWritableRegion, "Save...") } });
            }
            return tNode;
        }
    }

}
