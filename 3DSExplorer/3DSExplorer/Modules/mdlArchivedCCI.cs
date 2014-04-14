using System;
using System.IO;
using System.Windows.Forms;
using SharpCompress.Archive;

namespace _3DSExplorer.Modules
{
    public class ArchivedCCIContext : IContext
    {
        public enum ArchivedCCIView
        {
            Archive
        };

        private string errorMessage = string.Empty;
        private string _cciName = string.Empty;
        private uint _crc;
        public CCIContext CCIContext;

        public bool Open(Stream fs)
        {
            var reader = ArchiveFactory.Open(fs);

            foreach (var entry in reader.Entries)
                if (entry.FilePath.EndsWith(".3ds") || entry.FilePath.EndsWith(".cci") || entry.FilePath.EndsWith(".csu"))
                {
                    _cciName = entry.FilePath;
                    _crc = entry.Crc;
                    CCIContext = new CCIContext();
                    var entryStream = entry.OpenEntryStream();
                    var tempStream = new MemoryStream();
                    //read only the first 0x5000 bytes
                    var buffer = new byte[0x5000];
                    entryStream.Read(buffer, 0, buffer.Length);
                    tempStream.Write(buffer,0,buffer.Length);
                    tempStream.Seek(0, SeekOrigin.Begin);
                    CCIContext.Open(tempStream);
                    break;
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
            switch ((ArchivedCCIView)view)
            {
                case ArchivedCCIView.Archive:
                    f.SetGroupHeaders("CRC32");
                    f.AddListItem(0x000, 0x000, _cciName, _crc , 0);
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
            return "Archive files (zip/7z)|*.zip;*.7z";
        }

        public TreeNode GetExplorerTopNode()
        {
            var topNode = new TreeNode("Archive") { Tag = TreeViewContextTag.Create(this, (int)ArchivedCCIView.Archive) };
            topNode.Nodes.Add(CCIContext.GetExplorerTopNode());
            return topNode;
        }

        public TreeNode GetFileSystemTopNode()
        {
            var topNode = new TreeNode("Archive", 1, 1);
            //TODO: topNode.Nodes.Add(CCIContext.GetFileSystemTopNode());
            return topNode;
        }
    }
}
