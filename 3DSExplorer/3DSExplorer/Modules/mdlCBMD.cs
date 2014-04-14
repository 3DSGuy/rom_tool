using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace _3DSExplorer.Modules
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CBMD
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x4)]
        public char[] Magic;
        public uint Padding0;
        public uint CompressedCGFXOffset;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x78)]
        public byte[] Padding1;
        public uint CBMDLength;
    }

    public class CBMDContext : IContext
    {
        public enum CBMDView
        {
            CBMD
        };

        private enum CBMDActivation
        {
            SaveCGFX
        };


        private string errorMessage = string.Empty;
        public CBMD Header;
        public byte[] DecompressedCGFX;
        public CGFXContext CGFXContext;

        public bool Open(Stream fs)
        {
            Header = MarshalUtil.ReadStruct<CBMD>(fs); //read header

            //-- Graphics Reading --

            //Read ahead the size of the uncompressed file
            fs.Seek(Header.CompressedCGFXOffset + 1, SeekOrigin.Begin);
            var intBytes = new byte[4];
            fs.Read(intBytes, 0, 4);
            DecompressedCGFX = new byte[BitConverter.ToUInt32(intBytes, 0)];
            //Read again from the start
            fs.Seek(Header.CompressedCGFXOffset, SeekOrigin.Begin);
            var ms = new MemoryStream(DecompressedCGFX);
            try
            {
                var lz11 = new DSDecmp.Formats.Nitro.LZ11();
                lz11.Decompress(fs, Header.CBMDLength - fs.Position, ms);
            }
            catch
            { //might throw exception if size of compressed is bigger than it should be
            }
            ms.Seek(0, SeekOrigin.Begin);
            CGFXContext = new CGFXContext();
            CGFXContext.Open(ms);
            return true;
        }

        public string GetErrorMessage()
        {
            return errorMessage;
        }

        public void Create(FileStream fs, FileStream src)
        {
            CGFXContext.Create(fs, src);
        }

        public void View(frmExplorer f, int view, object[] values)
        {
            f.ClearInformation();
            switch ((CBMDView)view)
            {
                case CBMDView.CBMD:
                    var bmd = Header;
                    f.SetGroupHeaders("CBMD");
                    f.AddListItem(0, 4, "Magic", bmd.Magic, 0);
                    f.AddListItem(4, 4, "Padding 0", bmd.Padding0, 0);
                    f.AddListItem(8, 4, "Compressed CGFX Offset", bmd.CompressedCGFXOffset, 0);
                    f.AddListItem(0x10, 0x78, "Padding 1", bmd.Padding1, 0);
                    f.AddListItem(0x84, 4, "CBMD Length", bmd.CBMDLength, 0);
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
            switch ((CBMDActivation)type)
            {
                case CBMDActivation.SaveCGFX:
                    var saveFileDialog = new SaveFileDialog() { Filter = CGFXContext.GetFileFilter(), FileName = Path.GetFileName(filePath) + ".cgfx" };
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllBytes(saveFileDialog.FileName,DecompressedCGFX);
                    }
                    break;
            }
        }

        public string GetFileFilter()
        {
            return "CTR Banner Model Data (*.cbmd)|*.cbmd";
        }

        public TreeNode GetExplorerTopNode()
        {
            var topNode = new TreeNode("CBMD") { Tag = TreeViewContextTag.Create(this, (int)CBMDView.CBMD) };
            topNode.Nodes.Add(CGFXContext.GetExplorerTopNode());
            return topNode;
        }

        public TreeNode GetFileSystemTopNode()
        {
            var topNode = new TreeNode("CBMD", 1, 1);
            var cgfxNode = CGFXContext.GetFileSystemTopNode();
            cgfxNode.Tag = new[] { TreeViewContextTag.Create(this, (int)CBMDActivation.SaveCGFX, "Save CGFX...", null) };
            topNode.Nodes.Add(cgfxNode);
            return topNode;
        }
    }
}
