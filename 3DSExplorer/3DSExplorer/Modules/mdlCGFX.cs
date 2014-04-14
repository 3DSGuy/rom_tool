using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;
using _3DSExplorer.Utils;

namespace _3DSExplorer.Modules
{
    //Uses DATABlobHeader from mdlBanner.cs

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CGFX
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x4)]
        public char[] Magic;
        public ushort Endianess;
        public ushort DataOffset;
        public uint Unknown0;
        public uint FileSize;
        public uint Unknown1;
        public DATABlobHeader DataBlob;
        /*
         * 
            0x094 DICT (the first one of many) { uint32 length; and data[length-8]}

		    0x0F0 CMDL

			    0x83C SOBJ (first one of several)
			    0x8BC MTOB (MaterialsObject?)
			    0xBE0 TXOB (TextureObject??)
			    0xC20 SHDR (Shader?)
	
		    0x1068 COMMON
         * 
         */
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CGFXIMAG
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x4)]
        public char[] Magic;
        public ushort Length;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x82)]
        public byte[] Unknown;
    }

    public class CGFXContext : IContext
    {
        public enum CGFXView
        {
            CGFX
        };

        private string errorMessage = string.Empty;
        public CGFX Graphics;
        public byte[] GraphicsData;
        public CGFXIMAG GraphicsImage;
        public Bitmap BannerImage;      

        public bool Open(Stream fs)
        {
            Graphics = MarshalUtil.ReadStruct<CGFX>(fs);
            GraphicsData = new byte[Graphics.DataBlob.Length - Marshal.SizeOf(Graphics.DataBlob)];
            fs.Read(GraphicsData, 0, GraphicsData.Length);
            var imagPos = fs.Position;
            GraphicsImage = MarshalUtil.ReadStruct<CGFXIMAG>(fs);
            fs.Seek(imagPos + GraphicsImage.Length, SeekOrigin.Begin);
            
            BannerImage = ImageUtil.ReadImageFromStream(fs, 256, 128, ImageUtil.PixelFormat.RGBA4);
            FileStream tmp1 = new FileStream("base.bin", FileMode.Create);
            ImageUtil.WriteImageToStream(BannerImage, tmp1, ImageUtil.PixelFormat.RGBA4);
            return true;
        }

        public string GetErrorMessage()
        {
            return errorMessage;
        }

        public void Create(FileStream fs, FileStream src)
        {
            ImageUtil.WriteImageToStream(BannerImage, fs, ImageUtil.PixelFormat.RGBA4);
        }

        public void View(frmExplorer f, int view, object[] values)
        {
            f.ClearInformation();
            switch ((CGFXView)view)
            {
                case CGFXView.CGFX:
                    f.SetGroupHeaders("CGFX","DATA");
                    f.AddListItem(0x00, 4, "Magic", Graphics.Magic, 0);
                    f.AddListItem(0x04, 2, "Endianess", Graphics.Endianess, 0);
                    f.AddListItem(0x06, 2, "DATA Offset", Graphics.DataOffset, 0);                            
                    f.AddListItem(0x08, 4, "Unknown 0", Graphics.Unknown0, 0);
                    f.AddListItem(0x0C, 4, "File Size", Graphics.FileSize, 0);
                    f.AddListItem(0x10, 4, "Unknown 1", Graphics.Unknown1, 0);
                    f.AddListItem(0x14, 4, "Magic", Graphics.DataBlob.Magic, 1);
                    f.AddListItem(0x18, 4, "Length", Graphics.DataBlob.Length, 1);
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
            switch (type)
            {
                case 0:
                    ImageBox.ShowDialog((Image)values[0]);
                    
                    break;
                case 1:
                    var openFileDialog = new OpenFileDialog() { Filter = @"All Files|*.*" };
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        var iconImage = (Image)values[0];
                        var graphics = System.Drawing.Graphics.FromImage(iconImage);
                        try
                        {
                            var newImage = Image.FromFile(openFileDialog.FileName);
                            graphics.DrawImage(newImage, 0, 0, iconImage.Width, iconImage.Height);
                            MessageBox.Show(@"File replaced.");
                            newImage.Dispose();
                        }
                        catch
                        {
                            MessageBox.Show(@"The file selected is not a valid image!");
                        }
                    }
                    FileStream tmp2 = new FileStream("new.bin", FileMode.Create);
                    ImageUtil.WriteImageToStream(BannerImage, tmp2, ImageUtil.PixelFormat.RGBA4);
                    break;
            }
        }

        public string GetFileFilter()
        {
            return "CTR Graphics (*.cgfx)|*.cgfx";
        }

        public TreeNode GetExplorerTopNode()
        {
            var topNode = new TreeNode("CGFX") { Tag = TreeViewContextTag.Create(this, (int)CGFXView.CGFX) };

            return topNode;
        }

        public TreeNode GetFileSystemTopNode()
        {
            var topNode = new TreeNode("CGFX",1,1);
            topNode.Nodes.Add(new TreeNode(TreeListView.TreeListViewControl.CreateMultiColumnNodeText("Banner Image", "256x128")) { Tag = new[] { TreeViewContextTag.Create(this, 0, "Show...", new object[] { BannerImage }), TreeViewContextTag.Create(this, 1, "Replace...", new object[] { BannerImage }) } });
            return topNode;
        }
    }

}