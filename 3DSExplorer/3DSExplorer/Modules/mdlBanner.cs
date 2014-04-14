using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace _3DSExplorer.Modules
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DATABlobHeader
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x4)]
        public char[] Magic;
        public uint Length;
    }

    public class BannerContext : IContext
    {
        public enum BannerView
        {
            Banner
        };

        private string errorMessage = string.Empty;
        public CBMDContext CBMDContext;
        public CWAVContext CWAVContext;        

        public bool Open(Stream fs)
        {
            CBMDContext = new CBMDContext();
            var ret = CBMDContext.Open(fs);
            if (!ret)
            {
                errorMessage = CBMDContext.GetErrorMessage();
                return false;
            }
            if (fs.Position % 0x20 != 0)
                fs.Seek(0x20 - (fs.Position%0x20), SeekOrigin.Current); //complete alignment to the next 0x20
            CWAVContext = new CWAVContext();
            ret = CWAVContext.Open(fs);
            if (!ret)
            {
                errorMessage = CWAVContext.GetErrorMessage();
                return false;
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
            switch ((BannerView)view)
            {
                case BannerView.Banner:
                    f.SetGroupHeaders("Banner");
                    var len = CBMDContext.Header.CBMDLength;
                    f.AddListItem(0, 4, "CBMD Offset", 0, 0);
                    f.AddListItem(0, 4, "CBMD Length", len, 0);
                    f.AddListItem(0, 4, "CWAV Offset", len + ((len % 0x20) == 0 ? 0 : 0x20 - (len % 0x20)), 0);
                    f.AddListItem(0, 4, "CWAV Length", CWAVContext.Wave.DataChunkLength - 8, 0);
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
            throw new NotImplementedException();
        }

        public string GetFileFilter()
        {
            return "CTR Banners (*.bnr)|*.bnr";
        }

        public TreeNode GetExplorerTopNode()
        {
            var topNode = new TreeNode("Banner") { Tag = TreeViewContextTag.Create(this, (int)BannerView.Banner) };
            topNode.Nodes.Add(CBMDContext.GetExplorerTopNode());
            topNode.Nodes.Add(CWAVContext.GetExplorerTopNode());
            return topNode;
        }

        public TreeNode GetFileSystemTopNode()
        {
            var topNode = new TreeNode("Banner", 1, 1);
            topNode.Nodes.Add(CBMDContext.GetFileSystemTopNode());
            topNode.Nodes.Add(CWAVContext.GetFileSystemTopNode());
            return topNode;
        }
    }
}