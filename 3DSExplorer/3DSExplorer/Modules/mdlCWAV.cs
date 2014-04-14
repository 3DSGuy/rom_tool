using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace _3DSExplorer.Modules
{
    /*
     * Available encoders:
     * 
     * 16-bit PCM encoder
     * 8-bit PCM encoder
     * DSP ADPCM encoder
     * IMA ADPCM encoder
     * 
     */
    // Thanks Ris312 for investigating the file's structure

    //Uses DATABlobHeader from mdlBanner.cs

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CWAVChannelDataPointer
    {
        public uint Flags;
        public uint Offset;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CWAVChannelData
    {
        public uint Flags;
        public ulong Offset;
        public uint FFs;
        public uint Padding;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CWAVINFO
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x4)]
        public char[] Magic;
        public uint InfoDataLength;
        public uint Type;
        public uint SampleRate;
        public uint Unknown0;
        public uint NumberOfSamples;
        public uint Unknown2;
        public uint Channels;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CWAV
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x4)]
        public char[] Magic;

        public ushort Endianess;
        public ushort StructLength;
        public uint Unknown0;
        public uint FileSize;
        public uint NumOfChunks;
        public uint InfoChunkFlags;
        public uint InfoChunkOffset;
        public uint InfoChunkLength;
        public uint DataChunkFlags;
        public uint DataChunkOffset;
        public uint DataChunkLength;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x14)]
        public byte[] Reserved;
    }

    public class CWAVContext : IContext
    {
        public enum CWAVView
        {
            CWAV
        };

        private string errorMessage = string.Empty;
        public CWAV Wave;
        public byte BitsPerSample;
        public CWAVINFO InfoBlob;
        public CWAVChannelDataPointer[] ChannelDataPointers;
        public CWAVChannelData[] ChannelDatas;
        public DATABlobHeader DataBlob;
        public byte[][] ChannelsRawData;
        public byte[] MicrosoftWaveData;
        
        public bool Open(Stream fs)
        {
            var WavStartPos = fs.Position;
            Wave = MarshalUtil.ReadStruct<CWAV>(fs);
            fs.Seek(WavStartPos + Wave.InfoChunkOffset, SeekOrigin.Begin);
            InfoBlob = MarshalUtil.ReadStruct<CWAVINFO>(fs);
            BitsPerSample = (byte)(InfoBlob.Type == 1 ? 16 : 8);
            
            var ChannelsBaseOffset = fs.Position - 4;
            //read the channel data pointers based on the number of channels
            ChannelDataPointers = new CWAVChannelDataPointer[InfoBlob.Channels];
            for (var i = 0; i < InfoBlob.Channels;i++ ) 
                ChannelDataPointers[i] = MarshalUtil.ReadStruct<CWAVChannelDataPointer>(fs);
            //read the channel data fron the offsets
            ChannelDatas = new CWAVChannelData[InfoBlob.Channels];
            for (var i = 0; i < InfoBlob.Channels; i++)
            {
                fs.Seek(ChannelsBaseOffset + ChannelDataPointers[i].Offset, SeekOrigin.Begin);
                ChannelDatas[i] = MarshalUtil.ReadStruct<CWAVChannelData>(fs);
            }
            fs.Seek(WavStartPos + Wave.DataChunkOffset, SeekOrigin.Begin);
            DataBlob = MarshalUtil.ReadStruct<DATABlobHeader>(fs);
            var DataBaseOffset = fs.Position - 4;
            ChannelsRawData = new byte[InfoBlob.Channels][];
            for (var i = 0; i < InfoBlob.Channels; i++)
            {
                fs.Seek(DataBaseOffset + (long)ChannelDatas[i].Offset, SeekOrigin.Begin);
                ChannelsRawData[i] = new byte[InfoBlob.NumberOfSamples * (BitsPerSample / 8)];
                fs.Read(ChannelsRawData[i], 0, ChannelsRawData[i].Length);
            }
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
            f.ClearInformation();
            switch ((CWAVView)view)
            {
                case CWAVView.CWAV:
                    f.SetGroupHeaders("CWAV","INFO","Channels", "DATA");
                    f.AddListItem(0, 4, "Magic", Wave.Magic, 0);
                    f.AddListItem(4, 2, "Endianess", Wave.Endianess, 0);
                    f.AddListItem(6, 2, "Struct length", Wave.StructLength, 0);
                    f.AddListItem(8, 4, "Unknown0", Wave.Unknown0, 0);
                    f.AddListItem(0x0C, 4, "File Size", Wave.FileSize, 0);
                    f.AddListItem(0x10, 4, "Number of chunks", Wave.NumOfChunks, 0);
                    f.AddListItem(0x14, 4, "Info Chunk Flags", Wave.InfoChunkFlags, 0);
                    f.AddListItem(0x18, 4, "Info Chunk Offset", Wave.InfoChunkOffset, 0);
                    f.AddListItem(0x1C, 4, "Info Chunk Length", Wave.InfoChunkLength, 0);
                    f.AddListItem(0x20, 4, "Data Chunk Flags", Wave.DataChunkFlags, 0);
                    f.AddListItem(0x24, 4, "Data Chunk Offset", Wave.DataChunkOffset, 0);
                    f.AddListItem(0x28, 4, "Data Chunk Length", Wave.DataChunkLength, 0);
                    f.AddListItem(0x2C, 0x14, "Reserved", Wave.Reserved, 0);
                    
                    f.AddListItem(0, 4, "Magic", InfoBlob.Magic, 1);
                    f.AddListItem(4, 4, "Info Data Length", InfoBlob.InfoDataLength, 1);
                    f.AddListItem(8, 4, "Type", InfoBlob.Type, 1);
                    f.AddListItem(12, 4, "Samples per second", InfoBlob.SampleRate, 1);
                    f.AddListItem(16, 4, "Unknown 0", InfoBlob.Unknown0, 1);
                    f.AddListItem(20, 4, "NumberOfSamples", InfoBlob.NumberOfSamples, 1);
                    f.AddListItem(24, 4, "Unknown 2", InfoBlob.Unknown2, 1);
                    f.AddListItem(28, 4, "Channels", InfoBlob.Channels, 1);
                    for (var i = 0; i < ChannelDataPointers.Length; i++)
                    {
                        f.AddListItem(0, 4, "Channel " + i + " Ptr Flags", ChannelDataPointers[i].Flags, 2);
                        f.AddListItem(0, 4, "Channel " + i + " Ptr Offset", ChannelDataPointers[i].Offset, 2);
                        f.AddListItem(0, 4, "Channel " + i + " Data Flags", ChannelDatas[i].Flags, 2);
                        f.AddListItem(0, 8, "Channel " + i + " Data Offset", ChannelDatas[i].Offset, 2);
                        f.AddListItem(0, 4, "Channel " + i + " Data FFs", ChannelDatas[i].FFs, 2);
                        f.AddListItem(0, 4, "Channel " + i + " Data Padding", ChannelDatas[i].Padding, 2);
                    }
                    f.AddListItem(0, 4, "Magic", DataBlob.Magic, 3);
                    f.AddListItem(4, 4, "Length", DataBlob.Length, 3);
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
                    var saveFileDialog = new SaveFileDialog() { Filter = "WAV Files|*.wav", FileName = Path.GetFileName(filePath) + ".wav" };
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        if (MicrosoftWaveData == null)
                        {
                            var blockAlign = InfoBlob.Channels * (BitsPerSample / 8);
                            var wf = new WaveFormat
                            {
                                ByteRate = (ushort)(InfoBlob.SampleRate * blockAlign),
                                BlockAlign = (ushort)blockAlign,
                                Channels = (ushort)InfoBlob.Channels,
                                SampleRate = (ushort)InfoBlob.SampleRate,
                                BitsPerSample = BitsPerSample,
                                FormatTag = (ushort)WaveFormats.PCM
                            };
                            MicrosoftWaveData = WinMM.WriteWAVFile(wf, ChannelsRawData);
                        }
                        File.WriteAllBytes(saveFileDialog.FileName,MicrosoftWaveData);
                    }
                    break;
            }
        }

        public string GetFileFilter()
        {
            return "CTR Waves (*.b/cwav)|*.bcwav;*.cwav";
        }

        public TreeNode GetExplorerTopNode()
        {
            var topNode = new TreeNode("CWAV") { Tag = TreeViewContextTag.Create(this, (int)CWAVView.CWAV) };

            return topNode;
        }

        public TreeNode GetFileSystemTopNode()
        {
            var topNode = new TreeNode("CWAV", 1, 1);
            topNode.Nodes.Add(
                new TreeNode(TreeListView.TreeListViewControl.CreateMultiColumnNodeText("Wave.wav"))
                    {Tag = new[] {TreeViewContextTag.Create(this,0,"Save")}});
            return topNode;
        }
    }

}