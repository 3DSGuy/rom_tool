using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace _3DSExplorer.Modules
{
    public enum SignatureType
    {
        RSA_2048_SHA256 = 0x04000100,
        RSA_4096_SHA256 = 0x03000100,
        RSA_2048_SHA1 = 0x01000100,
        RSA_4096_SHA1 = 0x00000100
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TMDHeader
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] Reserved0;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public char[] Issuer;
        public byte Version;
        public byte CarCrlVersion;
        public byte SignerVersion;
        public byte Reserved1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] SystemVersion;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] TitleID;
        public uint TitleType;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public char[] GroupID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 62)]
        public byte[] Reserved2;
        public uint AccessRights;
        public ushort TitleVersion;
        public ushort ContentCount;
        public ushort BootContent;
        public ushort Padding0;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] ContentInfoRecordsHash;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TMDContentInfoRecord
    {
        public ushort ContentIndexOffset;
        public ushort ContentCommandCount; //K
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] NextContentHash; //SHA-256 hash of the next k content records that have not been hashed yet
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TMDContentChunkRecord
    {
        public uint ContentID;
        public ushort ContentIndex;
        public ushort ContentType;
        public ulong ContentSize;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] ContentHash; //SHA-256
    }

    public class TMDContext : IContext
    {
        private string errorMessage = string.Empty;
        public TMDHeader Head;
        public SignatureType SignatureType;
        public TMDContentInfoRecord[] ContentInfoRecords;
        public TMDContentChunkRecord[] Chunks;
        public byte[] Hash;
        public CertificatesContext CertificatesContext;

        public enum TMDView
        {
            TMD,
            ContentInfoRecord,
            ContentChunkRecord
        };

        public bool Open(Stream fs)
        {
            try
            {
                var intBytes = new byte[4];
                fs.Read(intBytes, 0, 4);
                SignatureType = (SignatureType)BitConverter.ToInt32(intBytes, 0);
                // Read the TMD RSA Type
                if (SignatureType == SignatureType.RSA_2048_SHA256)
                    Hash = new byte[256];
                else if (SignatureType == SignatureType.RSA_4096_SHA256)
                    Hash = new byte[512];
                else
                {
                    errorMessage = "This kind of TMD is unsupported.";
                    return false;
                }
                fs.Read(Hash, 0, Hash.Length);
                //Continue reading header
                Head = MarshalUtil.ReadStructBE<TMDHeader>(fs); //read header
                ContentInfoRecords = new TMDContentInfoRecord[64];
                for (var i = 0; i < ContentInfoRecords.Length; i++)
                    ContentInfoRecords[i] = MarshalUtil.ReadStructBE<TMDContentInfoRecord>(fs);
                Chunks = new TMDContentChunkRecord[Head.ContentCount]; // new ArrayList();
                for (var i = 0; i < Head.ContentCount; i++)
                    Chunks[i] = MarshalUtil.ReadStructBE<TMDContentChunkRecord>(fs);
                //Check if certificates are next
                fs.Read(intBytes, 0, 4);
                switch ((SignatureType)BitConverter.ToInt32(intBytes, 0))
                {
                    case SignatureType.RSA_2048_SHA1:
                    case SignatureType.RSA_2048_SHA256:
                    case SignatureType.RSA_4096_SHA1:
                    case SignatureType.RSA_4096_SHA256:
                        fs.Seek(-4, SeekOrigin.Current); //go back
                        CertificatesContext = new CertificatesContext();
                        if (!CertificatesContext.Open(fs))
                        {
                            errorMessage = CertificatesContext.GetErrorMessage();
                            return false;
                        }
                        break;
                }
                return true;
            }
            catch
            {
                errorMessage = "Error opening the TMD file. may be corrupt.";
                return false;
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

        private static string TypeToString(ushort type)
        {
            string ret = "";
            if ((type & 1) != 0)
                ret += "[encrypted]";
            if ((type & 2) != 0)
                ret += "[disc]";
            if ((type & 4) != 0)
                ret += "[cfm]";
            if ((type & 0x4000) != 0)
                ret += "[optional]";
            if ((type & 0x8000) != 0)
                ret += "[shared]";
            return ret;
        }

        public void View(frmExplorer f, int view, object[] values)
        {
            f.ClearInformation();
            switch ((TMDView)view)
            {
                case TMDView.TMD:
                    f.SetGroupHeaders("TMD");
                    f.AddListItem(0, 4, "Signature Type", (ulong)SignatureType, 0);
                    int off = 4;
                    if (SignatureType == SignatureType.RSA_2048_SHA256 || SignatureType == SignatureType.RSA_2048_SHA1)
                    {
                        f.AddListItem(off, 0x100, "RSA-2048 signature of the TMD", Hash, 0);
                        off += 0x100;
                    }
                    else
                    {
                        f.AddListItem(off, 0x200, "RSA-4096 signature of the TMD", Hash, 0);
                        off += 0x200;
                    }
                    f.AddListItem(off, 60, "Reserved0", Head.Reserved0, 0);
                    f.AddListItem(off + 60, 64, "Issuer", Head.Issuer, 0);
                    f.AddListItem(off + 124, 4, "Version", Head.Version, 0);
                    f.AddListItem(off + 128, 1, "Car Crl Version", Head.CarCrlVersion, 0);
                    f.AddListItem(off + 129, 1, "Signer Version", Head.SignerVersion, 0);
                    f.AddListItem(off + 130, 1, "Reserved1", Head.Reserved1, 0);
                    f.AddListItem(off + 131, 8, "System Version", Head.SystemVersion, 0);
                    f.AddListItem(off + 139, 8, "Title ID", Head.TitleID, 0);
                    f.AddListItem(off + 147, 4, "Title Type", Head.TitleType, 0);
                    f.AddListItem(off + 151, 2, "Group ID", Head.GroupID, 0);
                    f.AddListItem(off + 153, 62, "Reserved2", Head.Reserved2, 0);
                    f.AddListItem(off + 215, 4, "Access Rights", Head.AccessRights, 0);
                    f.AddListItem(off + 219, 2, "Title Version", Head.TitleVersion, 0);
                    f.AddListItem(off + 221, 2, "Content Count", Head.ContentCount, 0);
                    f.AddListItem(off + 223, 2, "Boot Content", Head.BootContent, 0);
                    f.AddListItem(off + 225, 2, "Padding", Head.Padding0, 0);
                    f.AddListItem(off + 227, 32, "Content Info Records Hash", Head.ContentInfoRecordsHash, 0);
                    break;
                case TMDView.ContentInfoRecord:
                    f.SetGroupHeaders("TMD Content Records");
                    for (var i = 0; i < 64; i++)
                    {
                        f.AddListItem(i * 36, 2, "Content Command Count", ContentInfoRecords[i].ContentCommandCount, 0);
                        f.AddListItem(i * 36 + 2, 2, "Content Index Offset", ContentInfoRecords[i].ContentIndexOffset, 0);
                        f.AddListItem(i * 36 + 4, 32, "Next Content Hash", ContentInfoRecords[i].NextContentHash, 0);
                    }
                    break;
                case TMDView.ContentChunkRecord:
                    f.SetGroupHeaders("TMD Content Chunks");
                    for (var i = 0; i < Chunks.Length; i++)
                    {
                        f.AddListItem(i, 4, "Content ID", Chunks[i].ContentID, 0);
                        f.AddListItem(0, 2, "Content Index", Chunks[i].ContentIndex, 0);
                        f.AddListItem(0, 2, "Content Type (=" + TypeToString(Chunks[i].ContentType) + ")", Chunks[i].ContentType, 0);
                        f.AddListItem(0, 8, "Content Size", Chunks[i].ContentSize, 0);
                        f.AddListItem(0, 32, "Content Hash", Chunks[i].ContentHash, 0);
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
            throw new NotImplementedException();
        }

        public string GetFileFilter()
        {
            return "Title Metadata (*.tmd)|*.tmd";
        }

        public TreeNode GetExplorerTopNode()
        {
            var tNode = new TreeNode("TMD") { Tag = TreeViewContextTag.Create(this, (int)TMDView.TMD) };
            tNode.Nodes.Add("Content Info Records").Tag = TreeViewContextTag.Create(this, (int)TMDView.ContentInfoRecord);
            tNode.Nodes.Add("Content Chunk Records").Tag = TreeViewContextTag.Create(this, (int)TMDView.ContentChunkRecord);
            if (CertificatesContext != null)
                tNode.Nodes.Add(CertificatesContext.GetExplorerTopNode());
            return tNode;
        }

        public TreeNode GetFileSystemTopNode()
        {
            return null;
        }
    }
}
