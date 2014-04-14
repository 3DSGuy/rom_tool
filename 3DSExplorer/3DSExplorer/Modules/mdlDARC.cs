using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace _3DSExplorer.Modules
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DARCHeader
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x4)]
        public char[] Magic;
        public ushort Endianness;
        public ushort HeaderLength;
        public uint Version;
        public uint FileLength;
        public uint FileTableOffset;
        public uint FileTableLength;
        public uint FilesDataOffset;
    }
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DARCFileTableEntry
    {
        public ushort NameOffset;
        public byte Parent;
        public byte Folder;
        public uint Offset;
        public uint Length;
    }

    public class DARCContext : IContext
    {
        private string errorMessage = string.Empty;

        public DARCHeader Header;
        public List<DARCFileTableEntry> Files = new List<DARCFileTableEntry>();
        public List<string> FileNames = new List<string>();

        public enum DARCView
        {
            Header,
            FilesTable
        };

        public enum DARCActivation
        {
            File
        };

        public bool Open(Stream fs)
        {
            fs.Seek(0, SeekOrigin.Begin);

            Header = MarshalUtil.ReadStruct<DARCHeader>(fs);

            
            fs.Seek(Header.FileTableOffset + 8, SeekOrigin.Begin);
            var fileCount = fs.ReadByte();

            fs.Seek(Header.FileTableOffset, SeekOrigin.Begin);
            
            while (fileCount > 0)
            {
                var entry = MarshalUtil.ReadStruct<DARCFileTableEntry>(fs);
                Files.Add(entry);
                fileCount--;
            }

            var nameTableOffset = fs.Position;
            for (var i = 0; i < Files.Count; i++)
            {
                fs.Seek(nameTableOffset + Files[i].NameOffset, SeekOrigin.Begin);
                var mems = new MemoryStream();
                int firstByte = 1, secondByte = 1;
                while (firstByte != 0 || secondByte != 0)
                {
                    firstByte = fs.ReadByte();
                    secondByte = fs.ReadByte();
                    mems.WriteByte((byte)firstByte);
                    mems.WriteByte((byte)secondByte);
                }
                FileNames.Add(Encoding.Unicode.GetString(mems.ToArray()));
                mems.Close();
            }
            return true;
        }

        public string GetErrorMessage()
        {
            return errorMessage;
        }

        public void Create(FileStream fs, FileStream src)
        {

        }

        public void View(frmExplorer f, int view, object[] values)
        {
            f.ClearInformation();
            switch ((DARCView)view)
            {
                case DARCView.Header:
                    f.SetGroupHeaders("DARC Header");
                    f.AddListItem(0x000, 4, "Magic", Header.Magic, 0);
                    f.AddListItem(0x004, 2, "Endianness", Header.Endianness, 0);
                    f.AddListItem(0x006, 2, "Header's Length", Header.HeaderLength, 0);
                    f.AddListItem(0x008, 4, "Version", Header.Version, 0);
                    f.AddListItem(0x00C, 4, "File Length",Header.FileLength , 0);
                    f.AddListItem(0x010, 4, "File Table Offset", Header.FileTableOffset, 0);
                    f.AddListItem(0x014, 4, "File Table Length", Header.FileTableLength, 0);
                    f.AddListItem(0x018, 4, "Files Data Offset", Header.FilesDataOffset, 0);
                    break;
                case DARCView.FilesTable:
                    f.SetGroupHeaders("Files");
                    for (var i = 0; i < Files.Count; i++ )
                    {
                        f.AddListItem(i, 1, " *** " + FileNames[i], 0, 0);
                        f.AddListItem(i, 2, "File's Name offset", Files[i].NameOffset, 0);
                        f.AddListItem(i, 1, "Parent", Files[i].Parent, 0);
                        f.AddListItem(i, 1, "Is folder", Files[i].Folder, 0);
                        f.AddListItem(i, 4, "File's offset", Files[i].Offset, 0);
                        f.AddListItem(i, 4, "File's length", Files[i].Length, 0);
                    }
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
            SaveFileDialog saveFileDialog;
            switch ((DARCActivation)type)
            {
                case DARCActivation.File:
                    var entry = (int)values[0];
                    

                    saveFileDialog = new SaveFileDialog { FileName = FileNames[entry] };
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        var fs = File.OpenRead(filePath);
                        fs.Seek(Files[entry].Offset, SeekOrigin.Begin);
                        var fileBuffer = new byte[Files[entry].Length];
                        fs.Read(fileBuffer, 0, fileBuffer.Length);
                        File.WriteAllBytes(saveFileDialog.FileName, fileBuffer);
                        fs.Close();
                    }
                    break;
            }
        }

        public string GetFileFilter()
        {
            return "DARC Files (darc/bcma)|*.darc;*.bcma";
        }

        public TreeNode GetExplorerTopNode()
        {
            var tNode = new TreeNode("DARC") { Tag = TreeViewContextTag.Create(this, (int)DARCView.Header) };
            var sNode = new TreeNode("Files") { Tag = TreeViewContextTag.Create(this, (int)DARCView.FilesTable, new object[] { 0 }) };
            tNode.Nodes.Add(sNode);
            return tNode;
        }

        public TreeNode GetFileSystemTopNode()
        {
            var tNode = new TreeNode("DARC", 1, 1);
            var folders = new List<TreeNode>();
            //add root folder
            folders.Add(tNode.Nodes.Add("root","ROOT",1,1));
            //add folders
            /*if (Folders.Length > 1)
                for (var i = 1; i < Folders.Length; i++)
                {
                    folders[i] = folders[Folders[i].ParentFolderIndex - 1].Nodes.Add(StringUtil.CharArrayToString(Folders[i].FolderName));
                    folders[i].ImageIndex = 1;
                    folders[i].SelectedImageIndex = 1;
                }
             */
            //add files
            if (Files.Count > 0)
            {
                var folder = 0;
                for (var i = 0; i < Files.Count; i++)
                {
                    
                    if (Files[i].Folder > 0)
                    {
                        
                        //folders.Add()
                        var node = folders[folder].Nodes.Add(FileNames[i]);
                        node.ImageIndex = 1;
                        node.SelectedImageIndex = 1;
                        //folder++;
                    }
                    else
                    {
                        var node = folders[folder].Nodes.Add(
                            TreeListView.TreeListViewControl.CreateMultiColumnNodeText(
                                FileNames[i],
                                Files[i].Length.ToString(),
                                StringUtil.ToHexString(6, Files[i].Offset)));
                        node.Tag = new[]
                                       {
                                           TreeViewContextTag.Create(this, (int) DARCActivation.File, "Save...",
                                                                     new object[] {i})
                                       };
                    }
                }
            }
            return tNode;
        }
    }

    
}
