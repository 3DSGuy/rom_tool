using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace _3DSExplorer.Modules
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SaveFlashHeader
    {
        public uint Unknown1;
        public uint Unknown2;
    }
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SaveFlashHeaderEntry
    {
        public byte PhysicalSector; // when bit7 is set, block has checksums, otherwise checksums are all zero
        public byte AllocationCount;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]

        public byte[] CheckSums; // 8*0x200=0x1000, each byte hashes 0x200 block with ModbusCRC16 XORed to 1 byte
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SaveFlashSectorEntry
    {
        public byte VirtualSector;             // Mapped to sector
        public byte PreviousVirtualSector;     // Physical sector previously mapped to
        public byte PhysicalSector;            // Mapped from sector
        public byte PreviousPhysicalSector;    // Virtual sector previously mapped to
        public byte PhysSecReallocCount;       // Amount of times physical sector has been remapped
        public byte VirtSecReallocCount;       // Amount of times virtual sector has been remapped
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] CheckSums;
}
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SaveFlashLongSectorEntry
    {
        public SaveFlashSectorEntry Sector;
        public SaveFlashSectorEntry Dupe;
        public uint Magic; //constant through the journal
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SaveFlashBlockMapEntry
    {
        public uint StartBlock;
        public uint EndBlock;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FileSystemFolderEntry
    {
        public uint ParentFolderIndex;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x10)]
        public char[] FolderName;
        public uint Index;
        public uint Unknown1;
        public uint LastFileIndex;
        public uint Unknown2;
        public uint Unknown3;
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FileSystemFileEntry
    {
        public uint ParentFolderIndex;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x10)]
        public char[] Filename;
        public uint Index;
        public uint Magic;
        public uint BlockOffset;
        public ulong FileSize;
        public uint Unknown2; // flags and/or date?
        public uint Unknown3;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DISA
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x4)]
        public char[] Magic;
        public uint Unknown0;
        public ulong TableSize;
        public ulong PrimaryTableOffset;
        public ulong SecondaryTableOffset;
        public ulong TableLength;
        public ulong SAVEEntryOffset;
        public ulong SAVEEntryLength;
        public ulong DATAEntryOffset;
        public ulong DATAEntryLength;
        public ulong SAVEPartitionOffset;
        public ulong SAVEPartitionLength;
        public ulong DATAPartitionOffset;
        public ulong DATAPartitionLength;

        public uint ActiveTable;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x20)]
        public byte[] Hash;

        public uint ZeroPad0;
        public uint Flag0;
        public uint Unknown1;
        public uint ZeroPad1; 
        public uint Unknown2; //Magic
        public ulong DataFsLength; //Why??
        public ulong Unknown3;
        public uint Unknown4; 
        public uint Unknown5; 
        public uint Unknown6;
        public uint Unknown7;
        public uint Unknown8;
        public uint Flag1;
        public uint Flag2;
        public uint Flag3;
        public uint Flag4;
        public uint Unknown14;
        public uint Flag5;
        public uint Unknown16;
        public uint Magic17;
        public uint Unknown18;
        public uint Flag6;
        public uint Flag7;
        public uint Flag8;
        public uint Unknown21;
        public uint Unknown22;
        public uint Unknown23;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DIFI
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x4)]
        public char[] Magic;
        public uint MagicPadding;
        public ulong IVFCOffset;
        public ulong IVFCSize;
        public ulong DPFSOffset;
        public ulong DPFSSize;
        public ulong HashOffset;
        public ulong HashSize;
        public uint Flags;
        public ulong FileBase;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IVFC
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x4)]
        public char[] Magic;
        public uint MagicPadding;
        public ulong Unknown1;

        public ulong FirstHashOffset;
        public ulong FirstHashLength;
        public ulong FirstHashBlock;
        public ulong SecondHashOffset;
        public ulong SecondHashLength;
        public ulong SecondHashBlock;

        public ulong HashTableOffset;
        public ulong HashTableLength;
        public ulong HashTableBlock;
        public ulong FileSystemOffset;
        public ulong FileSystemLength;
        public ulong FileSystemBlock;
        public ulong Unknown3; //0x78
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DPFS
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x4)]
        public char[] Magic;
        public uint MagicPadding;

        public ulong FirstTableOffset;
        public ulong FirstTableLength;
        public ulong FirstTableBlock;
        public ulong SecondTableOffset;
        public ulong SecondTableLength;
        public ulong SecondTableBlock;
        public ulong OffsetToData;
        public ulong DataLength;
        public ulong DataBlock;
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SAVE
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x4)]
        public char[] Magic;

        public uint MagicPadding;
        public ulong Unknown1;
        public ulong PartitionSize;
        public uint PartitionMediaSize;
        public ulong Unknown3;
        public uint Unknown4;
        public ulong FolderMapOffset;
        public uint FolderMapSize;
        public uint FolderMapMediaSize; 
        public ulong FileMapOffset;
        public uint FileMapSize;
        public uint FileMapMediaSize;
        public ulong BlockMapOffset;
        public uint BlockMapSize;
        public uint BlockMapMediaSize;
        public ulong FileStoreOffset;
        public uint FileStoreLength;
        public uint FileStoreMedia;
        public uint FolderTableOffset;
        public uint FolderTableLength;
        public uint FolderTableUnknown;
        public uint FolderTableMedia; 
        public uint FSTOffset;
        public uint FSTLength;
        public uint FSTUnknown;
        public uint FSTMedia;
    }

    public class SaveFlashContext : IContext
    {
        private string errorMessage = string.Empty;
        public class Partition
        {
            public ulong OffsetInImage;

            public DIFI Difi;
            public IVFC Ivfc;
            public DPFS Dpfs;
            public byte[] Hash; //0x20 - SHA256

            public uint FirstFlag;
            public uint FirstFlagDupe;
            public uint SecondFlag;
            public uint SecondFlagDupe;
            /*
            public byte[] SecondFlagTable;
            public byte[] SecondFlagTableDupe;
            */
            public byte[][] HashTable;
        }

        public bool Encrypted;
        public bool FirstSave;

        //Wear-Level stuff

        public byte[] Key;

        public byte[] MemoryMap;
        public SaveFlashHeaderEntry[] Blockmap;
        public SaveFlashLongSectorEntry[] Journal;
        public uint JournalSize;
        public SaveFlashHeader FileHeader;
        public byte[] Image;

        //Image stuff

        public bool IsData;

        public byte[] ImageHash; //0x10 - ??
        public DISA Disa;

        public Partition[] Partitions;

        //SAVE Stuff
        public SAVE Save;
        public FileSystemFolderEntry[] Folders;
        public FileSystemFileEntry[] Files;
        public long FileBase;
        public uint[] FilesMap;
        public uint[] FoldersMap;
        public SaveFlashBlockMapEntry[] BlockMap;

        //private static int count;

        public enum SaveFlashView
        {
            Image,
            Partition,
            Tables
        };

        public enum SaveFlashActivation
        {
            Image,
            Key,
            File,
            ReplaceFile
        };

        public bool Open(Stream fs)
        {
            Encrypted = IsEncrypted(fs);
            
            var fileBuffer = new byte[fs.Length];
            fs.Read(fileBuffer, 0, fileBuffer.Length);

            var ms = new MemoryStream(fileBuffer);

            FileHeader = MarshalUtil.ReadStruct<SaveFlashHeader>(ms);

            //get the blockmap headers
            var bmSize = (int)(ms.Length >> 12) - 1;
            Blockmap = new SaveFlashHeaderEntry[bmSize];
            MemoryMap = new byte[bmSize];
            for (var i = 0; i < Blockmap.Length; i++)
            {
                Blockmap[i] = MarshalUtil.ReadStruct<SaveFlashHeaderEntry>(ms);
                MemoryMap[i] = Blockmap[i].PhysicalSector;
            }
            //Check crc16
            var crcBytes = new byte[2];
            ms.Read(crcBytes, 0, 2);
            var twoBytes = CRC16.GetCRC(fileBuffer, 0, ms.Position - 2);
            if (crcBytes[0] != twoBytes[0] || crcBytes[1] != twoBytes[1])
            {
                errorMessage = "CRC Error or Corrupt Save file.";
                ms.Close();
                return false;
            }
            //get journal updates
            var jSize = (int)(0x1000 - ms.Position) / Marshal.SizeOf(typeof(SaveFlashLongSectorEntry));
            Journal = new SaveFlashLongSectorEntry[jSize];
            JournalSize = 0;
            uint jc = 0;
            while (ms.Position < 0x1000) //assure stopping
            {
                Journal[jc] = MarshalUtil.ReadStruct<SaveFlashLongSectorEntry>(ms);
                if (IsFF(Journal[jc].Sector.CheckSums))
                    break;
                MemoryMap[Journal[jc].Sector.VirtualSector] = Journal[jc].Sector.PhysicalSector;
                //update the blockmap
                Blockmap[Journal[jc].Sector.VirtualSector].PhysicalSector = Journal[jc].Sector.PhysicalSector;
                Buffer.BlockCopy(Journal[jc].Sector.CheckSums, 0, Blockmap[Journal[jc].Sector.VirtualSector].CheckSums, 0, Journal[jc].Sector.CheckSums.Length);
                jc++;
            }
            JournalSize = jc;

            var errors = 0;
            //check the wearleveling checksums
            for (var i = 0; i < Blockmap.Length; i++)
            {
                var chks = Blockmap[i].CheckSums;
                if (!Is00(chks)) //not all zeros
                {
                    for (var j = 0; j < chks.Length; j++) //go over all the checksums
                    {
                        var crc = CRC16.Xor2(CRC16.GetCRC(fileBuffer, (Blockmap[i].PhysicalSector & 0x7F) * 0x1000 + j * 0x200, 0x200));
                        if (crc != chks[j])
                            errors++;
                    }
                }
            }
            if (errors > 0)
            {
                errorMessage = "CRC Error in the wearleveling. (" + errors + " errors)";
                ms.Close();
                return false;
            }
            
            //rearragne by virtual
            Image = new byte[fileBuffer.Length - 0x1000];
            for (var i = 0; i < MemoryMap.Length; i++)
                Buffer.BlockCopy(fileBuffer, (MemoryMap[i] & 0x7F)*0x1000, Image, i*0x1000, 0x1000);

            if (Encrypted)
            {
                var foundKey = FindKey(Image);

                /*
                File.WriteAllBytes("d:\\_img" + count + ".bin", Image);
                File.WriteAllBytes("d:\\_key" + count++ + ".bin", foundKey);
                */
                if (foundKey == null)
                {
                    ms.Close();
#if DEBUG
                    foundKey = MakeKey(Image);
                    File.WriteAllBytes("_img.bin", Image);
                    XorByteArray(Image, foundKey, 0);
                    File.WriteAllBytes("_dec.bin", Image);
                    File.WriteAllBytes("_key.bin", foundKey);
                    errorMessage = "Can't find key in binary file." + Environment.NewLine +
                                   "Tried to create a key and saved the binaries to " + Environment.NewLine +
                                   Environment.NewLine +
                                   "_img, _dec & _key";
#else
                    errorMessage = "Can't find key in binary file. (corrupted or for fw:2.2.0-4+)";
#endif
                    return false;
                }
                XorByteArray(Image, foundKey, 0);
                //XorExperimental(fileBuffer, key, 0x1000);
                Key = foundKey;
            }

            var ims = new MemoryStream(Image);
            ImageHash = ReadByteArray(ims, (int)Sizes.MD5);
            //Go to start of image
            ims.Seek(0x100, SeekOrigin.Begin);
            Disa = MarshalUtil.ReadStruct<DISA>(ims);
            IsData = Disa.TableSize > 1;
            if (!IsDisaMagic(Disa.Magic))
            {
                errorMessage = "Corrupt Save File!";
                ms.Close();
                ims.Close();
                return false;
            }
            //Which table to read
            if ((Disa.ActiveTable & 1) == 1) //second table
                ims.Seek((long)Disa.PrimaryTableOffset, SeekOrigin.Begin);
            else
                ims.Seek((long)Disa.SecondaryTableOffset, SeekOrigin.Begin);

            Partitions = new Partition[Disa.TableSize];
            for (var i = 0; i < Partitions.Length; i++)
            {
                //var startOfDifi = ims.Position;
                Partitions[i] = new Partition
                {
                    Difi = MarshalUtil.ReadStruct<DIFI>(ims),
                    Ivfc = MarshalUtil.ReadStruct<IVFC>(ims),
                    Dpfs = MarshalUtil.ReadStruct<DPFS>(ims),
                    Hash = ReadByteArray(ims, (int)Sizes.SHA256)
                };
                ims.Seek(4, SeekOrigin.Current); // skip garbage
            }

            for (var p = 0; p < Partitions.Length; p++)
            {
                if (p == 0)
                    ims.Seek((long)Disa.SAVEPartitionOffset, SeekOrigin.Begin);
                else
                    ims.Seek((long)Disa.DATAPartitionOffset, SeekOrigin.Begin);

                Partitions[p].OffsetInImage = (ulong)ims.Position;

                ims.Seek((long)Partitions[p].Dpfs.FirstTableOffset, SeekOrigin.Current);
                Partitions[p].FirstFlag = ReadUInt32(ims);
                Partitions[p].FirstFlagDupe = ReadUInt32(ims);
                Partitions[p].SecondFlag = ReadUInt32(ims);
                ims.Seek((long)Partitions[p].Dpfs.SecondTableLength - 4, SeekOrigin.Current);
                Partitions[p].SecondFlagDupe = ReadUInt32(ims);
                /*
                    Partitions[p].FirstFlagTableDupe = new byte[Partitions[p].Dpfs.FirstTableLength / 4];
                    for (int i = 0; i < Partitions[p].FirstFlagTableDupe.Length; i++)
                        Partitions[p].FirstFlagTableDupe[i] = ReadUInt32(ims);
                    Partitions[p].SecondFlagTable = new byte[Partitions[p].Dpfs.SecondTableLength / 4];
                    for (int i = 0; i < Partitions[p].SecondFlagTable.Length; i++)
                        Partitions[p].SecondFlagTable[i] = ReadUInt32(ims);
                    Partitions[p].SecondFlagTableDupe = new byte[Partitions[p].Dpfs.SecondTableLength / 4];
                    for (int i = 0; i < Partitions[p].SecondFlagTableDupe.Length; i++)
                        Partitions[p].SecondFlagTableDupe[i] = ReadUInt32(ims); 
                    */

                ims.Seek((long)(Partitions[p].OffsetInImage + Partitions[p].Dpfs.OffsetToData), SeekOrigin.Begin);

                //Get hashes table
                ims.Seek((long)Partitions[p].Ivfc.HashTableOffset, SeekOrigin.Current);
                Partitions[p].HashTable = new byte[Partitions[p].Ivfc.HashTableLength / 0x20][];
                for (int i = 0; i < Partitions[p].HashTable.Length; i++)
                    Partitions[p].HashTable[i] = ReadByteArray(ims, 0x20);

                ims.Seek((long)(Partitions[p].OffsetInImage + Partitions[p].Dpfs.OffsetToData), SeekOrigin.Begin);

                //jump to dupe if needed (SAVE partition is written twice)
                if ((Partitions[p].SecondFlag & 0x20000000) == 0) //*** EXPERIMENTAL ***
                    ims.Seek((long)Partitions[p].Dpfs.DataLength, SeekOrigin.Current);

                ims.Seek((long)Partitions[p].Ivfc.FileSystemOffset, SeekOrigin.Current);

                if (p == 0)
                {
                    var saveOffset = ims.Position;
                    Save = MarshalUtil.ReadStruct<SAVE>(ims);
                    //add SAVE information (if exists) (suppose to...)
                    if (IsSaveMagic(Save.Magic)) //read 
                    {
                        ims.Seek(saveOffset + (long)Save.FileMapOffset, SeekOrigin.Begin);
                        FilesMap = new uint[Save.FileMapSize];
                        for (int i = 0; i < FilesMap.Length; i++)
                            FilesMap[i] = ReadUInt32(ims);
                        ims.Seek(saveOffset + (long)Save.FolderMapOffset, SeekOrigin.Begin);
                        FoldersMap = new uint[Save.FolderMapSize];
                        for (int i = 0; i < FoldersMap.Length; i++)
                            FoldersMap[i] = ReadUInt32(ims);
                        ims.Seek(saveOffset + (long)Save.BlockMapOffset, SeekOrigin.Begin);
                        var first = MarshalUtil.ReadStruct<SaveFlashBlockMapEntry>(ims);
                        BlockMap = new SaveFlashBlockMapEntry[first.EndBlock + 2];
                        BlockMap[0] = first;
                        for (uint i = 1; i < BlockMap.Length; i++)
                            BlockMap[i] = MarshalUtil.ReadStruct<SaveFlashBlockMapEntry>(ims);

                        //-- Get folders -- (and set filebase 'while at it')
                        if (!IsData)
                        {
                            FileBase = saveOffset + (long)Save.FileStoreOffset;
                            ims.Seek(FileBase + Save.FolderTableOffset * 0x200, SeekOrigin.Begin);
                        }
                        else
                        {   //file base is remote
                            FileBase = (long)(Disa.DATAPartitionOffset + Partitions[1].Difi.FileBase);
                            ims.Seek(saveOffset + Save.FolderTableOffset, SeekOrigin.Begin);
                        }
                        var froot = MarshalUtil.ReadStruct<FileSystemFolderEntry>(ims);
                        Folders = new FileSystemFolderEntry[froot.ParentFolderIndex - 1];
                        if (froot.ParentFolderIndex > 1) //if has folders
                            for (int i = 0; i < Folders.Length; i++)
                                Folders[i] = MarshalUtil.ReadStruct<FileSystemFolderEntry>(ims);

                        //-- Get files --
                        //go to FST
                        if (!IsData)
                            ims.Seek(FileBase + Save.FSTOffset * 0x200, SeekOrigin.Begin);
                        else //file base is remote
                            ims.Seek(saveOffset + Save.FSTOffset, SeekOrigin.Begin);

                        var root = MarshalUtil.ReadStruct<FileSystemFileEntry>(ims);
                        Files = new FileSystemFileEntry[root.ParentFolderIndex - 1];
                        if ((root.ParentFolderIndex > 1) && (root.Magic == 0)) //if has files
                            for (int i = 0; i < Files.Length; i++)
                                Files[i] = MarshalUtil.ReadStruct<FileSystemFileEntry>(ims);
                    }
                    else
                    {   //Not a legal SAVE filesystem
                        Folders = new FileSystemFolderEntry[0];
                        Files = new FileSystemFileEntry[0];
                    }
                } // end if (p == 0)
            } //end foreach (partitions)
            ims.Close();
            ms.Close();
            return true;
        }

        public string GetErrorMessage()
        {
            return errorMessage;
        }

        public void Create(FileStream fs, FileStream src)
        {
            //Recompute the partitions' hash tables
            HashAlgorithm ha = SHA256.Create();
            int offset;
            foreach (Partition partition in Partitions)
            {
                // itrerate thorugh the hashes table
                var hashSize = (1 << (int)partition.Ivfc.FileSystemBlock);
                for (var j = 0; j < partition.HashTable.Length; j++)
                    if (!Is00(partition.HashTable[j])) //hash isn't zero
                    {
                        offset = (int)(partition.OffsetInImage + partition.Dpfs.OffsetToData + partition.Ivfc.FileSystemOffset);
                        offset += j * hashSize;
                        partition.HashTable[j] = ha.ComputeHash(Image, offset, hashSize);
                        //write it into the image
                        offset = (int)(partition.OffsetInImage + partition.Dpfs.OffsetToData + partition.Ivfc.HashTableOffset);
                        offset += j * (int)Sizes.SHA256;
                        Buffer.BlockCopy(partition.HashTable[j], 0, Image, offset, (int)Sizes.SHA256);
                    }
            }

            var sha256 = SHA256.Create();

            /*TODO: SaveFlashContext.Create make hashes
            
            [ ] (Unknwon) Make the 'Partition Hash Table Header'
            [ ] (Unknown) Correct The Partition table hashes (DIFIs).
            
            */

            offset = (int)((Disa.ActiveTable & 1) == 1 ? Disa.PrimaryTableOffset : Disa.SecondaryTableOffset);
            var newDisaHash = sha256.ComputeHash(Image, offset, (int)Disa.TableLength);
            Buffer.BlockCopy(newDisaHash, 0, Disa.Hash, 0, (int)Sizes.SHA256); //fix context
            Buffer.BlockCopy(newDisaHash, 0, Image, 0x16C, (int)Sizes.SHA256); //fix image

            /*TODO: SaveFlashContext.Create make hashes2
            
            [ ] (Unknown) Correct The Image 128bit hash.
            
            */


            var ms = new MemoryStream();

            var blockmapEntrySize = Marshal.SizeOf(typeof(SaveFlashHeaderEntry));
            var sfHeaderSize = Marshal.SizeOf(typeof(SaveFlashHeader));
            var crcBlock = new byte[blockmapEntrySize * Blockmap.Length + sfHeaderSize];

            //Prepare the file header
            var temp = MarshalUtil.StructureToByteArray(FileHeader);
            Buffer.BlockCopy(temp, 0, crcBlock, 0, temp.Length);

            //Update & Prepare the blockmap (straight, no journal)
            for (byte i = 0; i < MemoryMap.Length; i++)
                MemoryMap[i] = i;
            Journal = new SaveFlashLongSectorEntry[0];
            JournalSize = 0;

            XorByteArray(Image, Key, 0); //Encrypt for blockmap building

            for (byte i = 0; i < Blockmap.Length; i++)
            {
                Blockmap[i].AllocationCount = 0;
                Blockmap[i].PhysicalSector = (byte)(i + 0x81); //checksum flag + 1 (offset from file header)
                for (var j = 0; j < Blockmap[i].CheckSums.Length; j++)
                    Blockmap[i].CheckSums[j] = CRC16.Xor2(CRC16.GetCRC(Image, 0x1000 * i + 0x200 * j, 0x200));
                temp = MarshalUtil.StructureToByteArray(Blockmap[i]);
                Buffer.BlockCopy(temp, 0, crcBlock, sfHeaderSize + i * blockmapEntrySize, blockmapEntrySize);
            }
            XorByteArray(Image, Key, 0); //Decrypt image back

            //Write the header and the blockmap
            ms.Write(crcBlock, 0, crcBlock.Length);

            //Write the CRC16
            ms.Write(CRC16.GetCRC(crcBlock), 0, 2);

            //Write an empty journal
            while (ms.Position < 0x1000)
                ms.WriteByte(0xFF);

            //Write the image
            ms.Write(Image, 0, Image.Length);
            var buffer = ms.ToArray();

            //XOR with the key
            XorByteArray(buffer, Key, 0x1000);

            fs.Write(buffer,0,buffer.Length);
        }

        public void View(frmExplorer f, int view, object[] values)
        {
            f.ClearInformation();
            switch ((SaveFlashView)view)
            {
                case SaveFlashView.Image:
                    f.SetGroupHeaders("SaveFlash", "Image");
                    f.AddListItem(0x000, 4, "Unknown 1", FileHeader.Unknown1, 0);
                    f.AddListItem(0x004, 4, "Unknown 2", FileHeader.Unknown2, 0);
                    f.AddListItem(0, 4, "** Blockmap length", (ulong)Blockmap.Length, 0);
                    f.AddListItem(0, 4, "** Journal size", JournalSize, 0);
                    f.AddListItem(0, 0x10, "** Image Hash", ImageHash, 1);
                    f.AddListItem(0x000, 4, "Magic DISA", Disa.Magic, 1);
                    f.AddListItem(0x004, 4, "Magic Number", Disa.Unknown0, 1);
                    f.AddListItem(0x008, 8, "Table Size", Disa.TableSize, 1);
                    f.AddListItem(0x010, 8, "Primary Table offset", Disa.PrimaryTableOffset, 1);
                    f.AddListItem(0x018, 8, "Secondary Table offset", Disa.SecondaryTableOffset, 1);
                    f.AddListItem(0x020, 8, "Table Length", Disa.TableLength, 1);
                    f.AddListItem(0x028, 8, "SAVE Entry Table offset", Disa.SAVEEntryOffset, 1);
                    f.AddListItem(0x030, 8, "SAVE Entry Table length", Disa.SAVEEntryLength, 1);
                    f.AddListItem(0x038, 8, "DATA Entry Table offset", Disa.DATAEntryOffset, 1);
                    f.AddListItem(0x040, 8, "DATA Entry Table length", Disa.DATAEntryLength, 1);
                    f.AddListItem(0x048, 8, "SAVE Partition Offset", Disa.SAVEPartitionOffset, 1);
                    f.AddListItem(0x050, 8, "SAVE Partition Length", Disa.SAVEPartitionLength, 1);
                    f.AddListItem(0x058, 8, "DATA Partition Offset", Disa.DATAPartitionOffset, 1);
                    f.AddListItem(0x060, 8, "DATA Partition Length", Disa.DATAPartitionLength, 1);
                    f.AddListItem(0x068, 4, "Active Table is " + ((Disa.ActiveTable & 1) == 1 ? "Primary" : "Secondary"), Disa.ActiveTable, 1);
                    f.AddListItem(0x06C, 0x20, "Hash", Disa.Hash, 1);
                    f.AddListItem(0x08C, 4, "Zero Padding 0(to 8 bytes)", Disa.ZeroPad0, 1);
                    f.AddListItem(0x090, 4, "Flag 0 ?", Disa.Flag0, 1);
                    f.AddListItem(0x094, 4, "Zero Padding 1(to 8 bytes)", Disa.ZeroPad1, 1);
                    f.AddListItem(0x098, 4, "Unknown 1", Disa.Unknown1, 1);
                    f.AddListItem(0x09C, 4, "Unknown 2 (Magic?)", Disa.Unknown2, 1);
                    f.AddListItem(0x0A0, 8, "Data FS Length", Disa.DataFsLength, 1);
                    f.AddListItem(0x0A8, 8, "Unknown 3", Disa.Unknown3, 1);
                    f.AddListItem(0x0B0, 4, "Unknown 4", Disa.Unknown4, 1);
                    f.AddListItem(0x0B4, 4, "Unknown 5", Disa.Unknown5, 1);
                    f.AddListItem(0x0B8, 4, "Unknown 6", Disa.Unknown6, 1);
                    f.AddListItem(0x0BC, 4, "Unknown 7", Disa.Unknown7, 1);
                    f.AddListItem(0x0C0, 4, "Unknown 8", Disa.Unknown8, 1);
                    f.AddListItem(0x0C4, 4, "Flag 1 ?", Disa.Flag1, 1);
                    f.AddListItem(0x0C8, 4, "Flag 2 ?", Disa.Flag2, 1);
                    f.AddListItem(0x0CC, 4, "Flag 3 ?", Disa.Flag3, 1);
                    f.AddListItem(0x0D0, 4, "Flag 4 ?", Disa.Flag4, 1);
                    f.AddListItem(0x0D4, 4, "Unknown 14", Disa.Unknown14, 1);
                    f.AddListItem(0x0D8, 4, "Flag 5 ?", Disa.Flag5, 1);
                    f.AddListItem(0x0DC, 4, "Unknown 16", Disa.Unknown16, 1);
                    f.AddListItem(0x0E0, 4, "Magic 17", Disa.Magic17, 1);
                    f.AddListItem(0x0E4, 4, "Unknown 18", Disa.Unknown18, 1);
                    f.AddListItem(0x0E8, 4, "Flag 6 ?", Disa.Flag6, 1);
                    f.AddListItem(0x0EC, 4, "Flag 7 ?", Disa.Flag7, 1);
                    f.AddListItem(0x0F0, 4, "Flag 8 ?", Disa.Flag8, 1);
                    f.AddListItem(0x0F4, 4, "Unknown 21", Disa.Unknown21, 1);
                    f.AddListItem(0x0F8, 4, "Unknown 22", Disa.Unknown22, 1);
                    f.AddListItem(0x0FC, 4, "Unknown 23", Disa.Unknown23, 1);
                    break;
                case SaveFlashView.Partition:
                    var j = (int) values[0];
                    var difi = Partitions[j].Difi;
                    var ivfc = Partitions[j].Ivfc;
                    var dpfs = Partitions[j].Dpfs;
                    var save = Save;

                    f.SetGroupHeaders("DIFI", "IVFC", "DPFS", "Hash", "SAVE", "Folders", "Files");
                    f.AddListItem(0x000, 4, "Magic DIFI", difi.Magic, 0);
                    f.AddListItem(0x004, 4, "Magic Number", difi.MagicPadding, 0);
                    f.AddListItem(0x008, 8, "IVFC Offset", difi.IVFCOffset, 0);
                    f.AddListItem(0x010, 8, "IVFC Size", difi.IVFCSize, 0);
                    f.AddListItem(0x018, 8, "DPFS Offset", difi.DPFSOffset, 0);
                    f.AddListItem(0x020, 8, "DPFS Size", difi.DPFSSize, 0);
                    f.AddListItem(0x028, 8, "Hash Offset", difi.HashOffset, 0);
                    f.AddListItem(0x030, 8, "Hash Size", difi.HashSize, 0);
                    f.AddListItem(0x038, 4, "Flags", difi.Flags, 0);
                    f.AddListItem(0x03C, 8, "File Base (for DATA partitions)", difi.FileBase, 0);

                    f.AddListItem(0x000, 4, "Magic IVFC", ivfc.Magic, 1);
                    f.AddListItem(0x004, 4, "Magic Number", ivfc.MagicPadding, 1);
                    f.AddListItem(0x008, 8, "Unknown 1", ivfc.Unknown1, 1);
                    f.AddListItem(0x010, 8, "FirstHash Offset", ivfc.FirstHashOffset, 1);
                    f.AddListItem(0x018, 8, "FirstHash Length", ivfc.FirstHashLength, 1);
                    f.AddListItem(0x020, 8, "FirstHash Block" + " (=" + (1 << (int)ivfc.FirstHashBlock) + ")", ivfc.FirstHashBlock, 1);
                    f.AddListItem(0x028, 8, "SecondHash Offset", ivfc.SecondHashOffset, 1);
                    f.AddListItem(0x030, 8, "SecondHash Length", ivfc.SecondHashLength, 1);
                    f.AddListItem(0x038, 8, "SecondHash Block" + " (=" + (1 << (int)ivfc.SecondHashBlock) + ")", ivfc.SecondHashBlock, 1);
                    f.AddListItem(0x040, 8, "HashTable Offset", ivfc.HashTableOffset, 1);
                    f.AddListItem(0x048, 8, "HashTable Length", ivfc.HashTableLength, 1);
                    f.AddListItem(0x050, 8, "HashTable Block" + " (=" + (1 << (int)ivfc.HashTableBlock) + ")", ivfc.HashTableBlock, 1);
                    f.AddListItem(0x058, 8, "FileSystem Offset", ivfc.FileSystemOffset, 1);
                    f.AddListItem(0x060, 8, "FileSystem Length", ivfc.FileSystemLength, 1);
                    f.AddListItem(0x068, 8, "FileSystem Block" + " (=" + (1 << (int)ivfc.FileSystemBlock) + ")", ivfc.FileSystemBlock, 1);
                    f.AddListItem(0x070, 8, "Unknown 3 (?=0x78)", ivfc.Unknown3, 1);

                    f.AddListItem(0x000, 4, "Magic DPFS", dpfs.Magic, 2);
                    f.AddListItem(0x004, 4, "Magic Number", dpfs.MagicPadding, 2);
                    f.AddListItem(0x008, 8, "First Table Offset", dpfs.FirstTableOffset, 2);
                    f.AddListItem(0x010, 8, "First Table Length", dpfs.FirstTableLength, 2);
                    f.AddListItem(0x018, 8, "First Table Block", dpfs.FirstTableBlock, 2);
                    f.AddListItem(0x020, 8, "Second Table Offset", dpfs.SecondTableOffset, 2);
                    f.AddListItem(0x028, 8, "Second Table Length", dpfs.SecondTableLength, 2);
                    f.AddListItem(0x030, 8, "Second Table Block", dpfs.SecondTableBlock, 2);
                    f.AddListItem(0x038, 8, "Offset to Data", dpfs.OffsetToData, 2);
                    f.AddListItem(0x040, 8, "Data Length", dpfs.DataLength, 2);
                    f.AddListItem(0x048, 8, "Data Block", dpfs.DataBlock, 2);

#if DEBUG
            f.AddListItem(0x000, 4, "* First Flag", Partitions[j].FirstFlag, 2);
            f.AddListItem(0x000, 4, "* First Flag Dupe", Partitions[j].FirstFlagDupe,2);
            f.AddListItem(0x000, 4, "* Second Flag", Partitions[j].SecondFlag, 2);
            f.AddListItem(0x000, 4, "* Second Flag Dupe", Partitions[j].SecondFlagDupe, 2);
#endif

                    f.AddListItem(0x000, 0x20, "Hash", Partitions[j].Hash, 3);

                    if (j == 0)
                    {
                        f.AddListItem(0x000, 4, "Magic SAVE", save.Magic, 4);
                        f.AddListItem(0x004, 4, "Magic Number", save.MagicPadding, 4);
                        f.AddListItem(0x008, 8, "Unknown 1 (?=0x020)", save.Unknown1, 4);
                        f.AddListItem(0x010, 8, "Size of data Partition [media units]", save.PartitionSize, 4);
                        f.AddListItem(0x018, 4, "Partition Media Size", save.PartitionMediaSize, 4);
                        f.AddListItem(0x01C, 8, "Unknown 3 (?=0x000)", save.Unknown3, 4);
                        f.AddListItem(0x024, 4, "Unknown 4 (?=0x200)", save.Unknown4, 4);
                        f.AddListItem(0x028, 8, "File Map Offset", save.FileMapOffset, 4);
                        f.AddListItem(0x030, 4, "File Map Size", save.FileMapSize, 4);
                        f.AddListItem(0x034, 4, "File Map MediaSize", save.FileMapMediaSize, 4);
                        f.AddListItem(0x038, 8, "Folder Map Offset", save.FolderMapOffset, 4);
                        f.AddListItem(0x040, 4, "Folder Map Size", save.FolderMapSize, 4);
                        f.AddListItem(0x044, 4, "Folder Map Media Size", save.FolderMapMediaSize, 4);
                        f.AddListItem(0x048, 8, "Block Map Offset", save.BlockMapOffset, 4);
                        f.AddListItem(0x050, 4, "Block Map Size", save.BlockMapSize, 4);
                        f.AddListItem(0x054, 4, "Block Map Media Size", save.BlockMapMediaSize, 4);
                        f.AddListItem(0x058, 8, "Filestore Offset (from SAVE)", save.FileStoreOffset, 4);
                        f.AddListItem(0x060, 4, "Filestore Length (medias)", save.FileStoreLength, 4);
                        f.AddListItem(0x064, 4, "Filestore Media", save.FileStoreMedia, 4);
                        f.AddListItem(0x068, 4, "Folders Table offset (medias/exact)", save.FolderTableOffset, 4);
                        f.AddListItem(0x06C, 4, "Folders Table Length (medias)", save.FolderTableLength, 4);
                        f.AddListItem(0x070, 4, "Folders Table Unknown", save.FolderTableUnknown, 4);
                        f.AddListItem(0x074, 4, "Folders Table Media Size", save.FolderTableMedia, 4);
                        f.AddListItem(0x078, 4, "Files Table Offset (medias/exact)", save.FSTOffset, 4);
                        f.AddListItem(0x07C, 4, "Files Table Length", save.FSTLength, 4);
                        f.AddListItem(0x080, 4, "Files Table Unknown", save.FSTUnknown, 4);
                        f.AddListItem(0x084, 4, "Files Table Media Size", save.FSTMedia, 4);

                        if (IsSaveMagic(save.Magic))
                        {
                            var i = 1;
                            foreach (var fse in Folders)
                                f.AddListItem(i++.ToString(),
                                            fse.Index.ToString(),
                                            StringUtil.CharArrayToString(fse.FolderName),
                                            fse.ParentFolderIndex.ToString(),
                                            StringUtil.ToHexString(8, fse.LastFileIndex),
                                            5);
                            i = 1;
                            foreach (var fse in Files)
                                f.AddListItem(i++.ToString(),
                                            fse.BlockOffset.ToString(),
                                            "[" + fse.Index + "] " + StringUtil.CharArrayToString(fse.Filename) + ", (" + fse.FileSize + "b)",
                                            fse.ParentFolderIndex.ToString(),
                                            StringUtil.ToHexString(8, fse.Unknown2) + " " + StringUtil.ToHexString(8, fse.Magic),
                                            6);
                        }
                    }
                    break;
                case SaveFlashView.Tables:
                    f.SetGroupHeaders("Files", "Folders", "Unknown");
                    if (IsSaveMagic(Save.Magic))
                    {
                        for (var i = 0; i < FilesMap.Length; i++)
                            f.AddListItem(i, 4, "UInt32", FilesMap[i], 0);
                        for (var i = 0; i < FoldersMap.Length; i++)
                            f.AddListItem(i, 4, "UInt32", FoldersMap[i], 1);

                        f.AddListItem("", "", "Start", "Start:" + (BlockMap[0].StartBlock & 0xff) + ", End: " + (BlockMap[0].EndBlock & 0xff), "Start:" + BlockMap[0].StartBlock.ToString("X8") + ", End: " + BlockMap[0].EndBlock.ToString("X8"), 2);
                        for (var i = 1; i < BlockMap.Length - 1; i++)
                            f.AddListItem("", (i - 1).ToString(), "Block " + i + (BlockMap[i].EndBlock == 0x80000000 && BlockMap[i].StartBlock == 0x80000000 ? " (Start of data)" : ""), "Start:" + (BlockMap[i].StartBlock & 0xff) + ", End: " + (BlockMap[i].EndBlock & 0xff), "Start:" + BlockMap[i].StartBlock.ToString("X8") + ", End: " + BlockMap[i].EndBlock.ToString("X8"), 2);
                        f.AddListItem("", "", "End", "", "Start:" + (BlockMap[BlockMap.Length - 1].StartBlock & 0xff) + ", End: " + (BlockMap[BlockMap.Length - 1].EndBlock & 0xff), 2);
                    }
                    break;
            }
            f.AutoAlignColumns();
        }

        public bool CanCreate()
        {
            return true;
        }

        public void Activate(string filePath, int type, object[] values)
        {
            SaveFileDialog saveFileDialog;
            switch ((SaveFlashActivation)type)
            {
                case SaveFlashActivation.Image:
                    saveFileDialog = new SaveFileDialog {Filter = @"Image Files (*.bin)|*.bin"};
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        File.WriteAllBytes(saveFileDialog.FileName, Image);
                    break;
                case SaveFlashActivation.Key:
                    saveFileDialog = new SaveFileDialog {Filter = @"Key Xorpad Files (*.bin)|*.bin"};
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        File.WriteAllBytes(saveFileDialog.FileName, Key);
                    break;
                case SaveFlashActivation.File:
                    var entry = (FileSystemFileEntry)values[0];
                    saveFileDialog = new SaveFileDialog { FileName = StringUtil.CharArrayToString(entry.Filename) };
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        var fileBuffer = new byte[entry.FileSize];
                        Buffer.BlockCopy(Image, (int)(FileBase + entry.BlockOffset * 0x200), fileBuffer, 0, fileBuffer.Length);
                        File.WriteAllBytes(saveFileDialog.FileName, fileBuffer);
                    }
                    break;
                case SaveFlashActivation.ReplaceFile:
                    var openFileDialog = new OpenFileDialog() { Filter = @"All Files|*.*" };
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        var originalFile = (FileSystemFileEntry)values[0];
                        var newFile = File.OpenRead(openFileDialog.FileName);
                        var newFileSize = (ulong)newFile.Length;
                        newFile.Close();
                        if (originalFile.FileSize != newFileSize)
                        {
                            MessageBox.Show(@"File's size doesn't match the target file. \nIt must be the same size as the one to replace.");
                            return;
                        }
                        var offSetInImage = FileBase + originalFile.BlockOffset * 0x200;
                        Buffer.BlockCopy(File.ReadAllBytes(openFileDialog.FileName), 0, Image, (int)offSetInImage, (int)newFileSize);
                        MessageBox.Show(@"File replaced.");
                    }
                    break;
            }
        }

        public string GetFileFilter()
        {
            return "Save Flash Files (*.sav/bin)|*.bin;*.sav";
        }

        public TreeNode GetExplorerTopNode()
        {
            var tNode = new TreeNode("Save Flash " + (Encrypted ? "(Encrypted)" : "")) { Tag = TreeViewContextTag.Create(this, (int)SaveFlashView.Image) };
            var sNode = new TreeNode("SAVE Partition") { Tag = TreeViewContextTag.Create(this, (int)SaveFlashView.Partition, new object[] { 0 }) };
            sNode.Nodes.Add("Maps").Tag = TreeViewContextTag.Create(this,(int)SaveFlashView.Tables);
            tNode.Nodes.Add(sNode);
            if (IsData)
                tNode.Nodes.Add("DATA Partition").Tag = TreeViewContextTag.Create(this, (int)SaveFlashView.Partition, new object[] { 1 });
            return tNode;
        }

        public TreeNode GetFileSystemTopNode()
        {
            var tNode = new TreeNode("SaveFlash", 1, 1);
            var folders = new TreeNode[Folders.Length];

            tNode.Nodes.Add(new TreeNode("Image.bin") {Tag = new[] {TreeViewContextTag.Create(this,(int)SaveFlashActivation.Image, "Save image...")}});
            if (Encrypted)
                tNode.Nodes.Add(new TreeNode("Key.bin") { Tag = new[] {TreeViewContextTag.Create(this, (int)SaveFlashActivation.Key, "Save key...") }});
            //add root folder
            folders[0] = tNode.Nodes.Add("root","ROOT",1,1);
            //add folders
            if (Folders.Length > 1)
                for (var i = 1; i < Folders.Length; i++)
                {
                    folders[i] = folders[Folders[i].ParentFolderIndex - 1].Nodes.Add(StringUtil.CharArrayToString(Folders[i].FolderName));
                    folders[i].ImageIndex = 1;
                    folders[i].SelectedImageIndex = 1;
                }
            //add files
            if (Files.Length > 0)
            {
                for (var i = 0; i < Files.Length; i++)
                {
                    var node = folders[Files[i].ParentFolderIndex - 1].Nodes.Add(
                        TreeListView.TreeListViewControl.CreateMultiColumnNodeText(
                            StringUtil.CharArrayToString(Files[i].Filename),
                            Files[i].FileSize.ToString(),
                            StringUtil.ToHexString(6, (ulong) FileBase + 0x200*Files[i].BlockOffset)));
                    node.Tag = new[]
                                   {
                                       TreeViewContextTag.Create(this, (int) SaveFlashActivation.File, "Save...", new object[] {Files[i]}),
                                       TreeViewContextTag.Create(this, (int) SaveFlashActivation.ReplaceFile, "Replace...", new object[] {Files[i]})
                                   };
                }
            }
            return tNode;
        }

        #region Public Methods

        public static bool IsEncrypted(Stream fs)
        {
            var lastPos = fs.Position;
            //check if encrypted
            var magic = new byte[4];
            fs.Seek(0x1000, SeekOrigin.Begin); //Start of information
            while ((fs.Length - fs.Position > 0x200) & !IsSaveMagic(magic))
            {
                fs.Read(magic, 0, 4);
                fs.Seek(0x200 - 4, SeekOrigin.Current);
            }
            var result = fs.Length - fs.Position;
            fs.Seek(lastPos, SeekOrigin.Begin);
            return (result <= 0x200);
        }

        #endregion

        #region Private Methods
        private struct HashEntry
        {
            public byte[] Hash;
            public int BlockIndex;
            public int Count;
        }

        private static bool IsJournalMagic(byte[] buf, int offset)
        {
            return (buf[offset] == 0xE0 && buf[offset + 1] == 0x6C && buf[offset + 2] == 0x0D && buf[offset + 3] == 0x08);
        }

        private static bool IsDifiMagic(char[] buf)
        {
            return (buf[0] == 'D' && buf[1] == 'I' && buf[2] == 'F' && buf[3] == 'I');
        }

        private static bool IsDisaMagic(IList<char> buf)
        {
            return (buf[0] == 'D' && buf[1] == 'I' && buf[2] == 'S' && buf[3] == 'A');
        }

        private static bool IsSaveMagic(IList<char> buf)
        {
            return (buf[0] == 'S' && buf[1] == 'A' && buf[2] == 'V' && buf[3] == 'E');
        }

        private static bool IsSaveMagic(byte[] buf)
        {
            return (buf[0] == 'S' && buf[1] == 'A' && buf[2] == 'V' && buf[3] == 'E');
        }

        private static bool IsFF(IEnumerable<byte> buf)
        {
            return buf.All(t => t == 0xFF);
        }

        private static bool Is00(IEnumerable<byte> buf)
        {
            return buf.All(t => t == 0x00);
        }

        private static void XorByteArray(byte[] array, byte[] mask, int start)
        {
            for (var i = start; i < array.Length; i++)
                array[i] ^= mask[i % mask.Length];
        }

        private static void XorExperimental(byte[] array, byte[] mask, int start)
        {
            for (var j = start; j < array.Length; j += 0x200)
            {
                var lastNonFF = j + 0x200 - 1 < array.Length ? j + 0x200 - 1 : array.Length - 1;
                //find to what point need to be xored
                while (array[lastNonFF] == 0xFF)
                    lastNonFF--;
                //xor it
                for (var i = j; i <= lastNonFF; i++)
                    array[i] ^= mask[i % mask.Length];
            }
        }

        private static bool BufferSame(byte[] array1, byte[] array2, int length)
        {
            for (var i = 0; i < length; i++)
                if (array1[i] != array2[i])
                    return false;
            return true;
        }

        private static byte[] FindKey2(byte[] input)
        {
            var ms = new MemoryStream(input);
            var disa = new byte[4];
            ms.Seek(0x100, SeekOrigin.Begin);
            ms.Read(disa, 0, disa.Length);
            ms.Seek(0x200, SeekOrigin.Current);
            var check = new byte[4];
            while (ms.Position < ms.Length)
            {
                ms.Read(check, 0, check.Length);
                XorByteArray(check, disa, 0);
                if (check[0] == 'D' && check[1] == 'I' && check[2] == 'S' && check[3] == 'A')
                {
                    ms.Seek(-0x104, SeekOrigin.Current);
                    var key = new byte[0x200];
                    ms.Read(key, 0, key.Length);
                    return key;
                }
                ms.Seek(0x200, SeekOrigin.Current);
            }
            return null; //key not found
        }

        private static byte[] MakeKey(byte[] input)
        {
            var keyArray = new byte[0x200];

            //copy from 0x0010
            for (var i = 0x10; i < 0x100; i++)
                keyArray[i] = (byte)(input[i] ^ 0);
            //copy from 0x0100
            var x00100 = new byte[] { 0x44, 0x49, 0x53, 0x41, 00, 00, 04, 00 };
            for (var i = 0; i < 8; i++)
                keyArray[0x100 + i] = (byte)(input[0x100 + i] ^ x00100[i]);
            //copy from 0x1000
            var x01000 = new byte[] { 0, 0, 0, 8 };
            for (var i = 0; i < 4; i++)
                keyArray[i] = (byte)(input[0x1000 + i] ^ x01000[i]);
            //find where SAVE is
            var saveOffset = 0x2400;
            if ((input[saveOffset] ^ keyArray[0]) != 'S' ||
                (input[saveOffset + 1] ^ keyArray[1]) != 'A' ||
                (input[saveOffset + 2] ^ keyArray[2]) != 'S' ||
                (input[saveOffset + 3] ^ keyArray[3]) != 'S')
                saveOffset += 0xC00;
            //copy from SAVE
            var xSave = new byte[] { 00, 00, 04, 00, 0x20, 00, 00, 00, 00, 00, 00, 00 };
            for (var i = 0x04; i < 0x10; i++)
                keyArray[i] = (byte)(input[saveOffset + i] ^ xSave[i - 0x04]);

            return keyArray;
        }

        private static byte[] FindKey(byte[] input)
        {
            var md5 = new MD5CryptoServiceProvider();
            var count = 0;
            var found = false;

            var ffHash = new byte[] { 0xde, 0x03, 0xfe, 0x65, 0xa6, 0x76, 0x5c, 0xaa, 0x8c, 0x91, 0x34, 0x3a, 0xcc, 0x62, 0xcf, 0xfc };

            var hashList = new HashEntry[(input.Length / 0x200) + 1];

            for (var i = 0; i < (input.Length / 0x200); i++, found = false)
            {
                var hash = md5.ComputeHash(input, i * 0x200, 0x200);

                if (BufferSame(hash, ffHash, 16)) //skip ff blocks...
                    continue;

                // see if we already came up with that hash
                for (var j = 0; j < count; j++)
                    if (BufferSame(hashList[j].Hash, hash, 16))
                    {
                        hashList[j].Count++;
                        found = true;
                        break;
                    }

                // push new hashlist entry
                if (found) continue;
                hashList[count] = new HashEntry { Hash = new byte[hash.Length] };
                Buffer.BlockCopy(hash, 0, hashList[count].Hash, 0, hash.Length);
                hashList[count].Count = 1;
                hashList[count].BlockIndex = i;
                count++;
            }
            //Sort by count, Decending
            Array.Sort(hashList, (entry1, entry2) => entry2.Count - entry1.Count);
            
            //Check the keys one by one
            var foundOffset = 0;
            for (var i=0 ; i<hashList.Count() && !found; i++)
            {
                foundOffset = hashList[i].BlockIndex * 0x200;
                if (((input[0x100] ^ input[foundOffset + 0x100]) == 'D') &&
                    ((input[0x101] ^ input[foundOffset + 0x101]) == 'I') &&
                    ((input[0x102] ^ input[foundOffset + 0x102]) == 'S') &&
                    ((input[0x103] ^ input[foundOffset + 0x103]) == 'A'))
                     found = true;
            }
            if (!found)    
                return null; //That's not it

            var outbuf = new byte[0x200];
            Buffer.BlockCopy(input, foundOffset, outbuf, 0, 0x200);

            return outbuf;
        }

        private static byte[] ReadByteArray(Stream fs, int size)
        {
            var buffer = new byte[size];
            fs.Read(buffer, 0, size);
            return buffer;
        }

        private static uint ReadUInt32(Stream fs)
        {
            var buffer = new byte[4];
            fs.Read(buffer, 0, 4);
            return BitConverter.ToUInt32(buffer, 0);
        }

        private static ulong ReadUInt64(Stream fs)
        {
            var buffer = new byte[8];
            fs.Read(buffer, 0, 8);
            return BitConverter.ToUInt64(buffer, 0);
        }
        #endregion
    }

    
}
