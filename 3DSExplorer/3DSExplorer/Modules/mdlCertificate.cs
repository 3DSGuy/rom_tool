using System;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace _3DSExplorer.Modules
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Certificate
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] Reserved0;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public char[] Issuer;
        public uint Tag;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public char[] Name;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x104)]
        public byte[] Key;
        public ushort Unknown1;
        public ushort Unknown2;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 52)]
        public byte[] Padding;
    }

    public class CertificateEntry
    {
        public Certificate Certificate;
        public SignatureType SignatureType;
        public byte[] Signature;
    }

    public class CertificatesContext : IContext
    {
        private string errorMessage = string.Empty;
        public ArrayList List; //of CertificateEntry

        public bool Open(Stream fs)
        {
            List = new ArrayList();
            try
            {
                var intBytes = new byte[4];
                while (fs.Position < fs.Length)
                {
                    var tcert = new CertificateEntry();
                    fs.Read(intBytes, 0, 4);
                    tcert.SignatureType = (SignatureType)BitConverter.ToInt32(intBytes, 0);
                    // RSA Type
                    switch (tcert.SignatureType)
                    {
                        case SignatureType.RSA_2048_SHA1:
                        case SignatureType.RSA_2048_SHA256:
                            tcert.Signature = new byte[256];
                            break;
                        case SignatureType.RSA_4096_SHA1:
                        case SignatureType.RSA_4096_SHA256:
                            tcert.Signature = new byte[512];
                            break;
                    }
                    if (tcert.Signature == null)
                        break; //this is the end of the certificates
                    fs.Read(tcert.Signature, 0, tcert.Signature.Length);
                    tcert.Certificate = MarshalUtil.ReadStructBE<Certificate>(fs);
                    List.Add(tcert);
                }
            }
            catch
            {

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
            var i = (int) values[0];
            f.ClearInformation();
            if (i < 0)
            {
                f.SetGroupHeaders("Certificates");
                f.AddListItem(0, 4, "Certificate Count", (ulong)List.Count, 0);
            }
            else
            {
                var entry = (CertificateEntry)List[i];
                var cert = entry.Certificate;
                f.SetGroupHeaders("Certificate");
                f.AddListItem(0, 4, "Signature Type", (ulong)entry.SignatureType, 0);
                int off = 4;
                if (entry.SignatureType == SignatureType.RSA_2048_SHA256 || entry.SignatureType == SignatureType.RSA_2048_SHA1)
                {
                    f.AddListItem(off, 0x100, "RSA-2048 signature of the content", entry.Signature, 0);
                    off += 0x100;
                }
                else
                {
                    f.AddListItem(off, 0x200, "RSA-4096 signature of the content", entry.Signature, 0);
                    off += 0x200;
                }
                f.AddListItem(off, 60, "Reserved0", cert.Reserved0, 0);
                f.AddListItem(off + 60, 64, "Issuer", cert.Issuer, 0);
                f.AddListItem(off + 124, 4, "Tag", cert.Tag, 0);
                f.AddListItem(off + 128, 64, "Name", cert.Name, 0);
                f.AddListItem(off + 292, 0x104, "Key", cert.Key, 0);
                f.AddListItem(off + 552, 2, "Unknown0", cert.Unknown1, 0);
                f.AddListItem(off + 554, 2, "Unknown1", cert.Unknown2, 0);
                f.AddListItem(off + 556, 52, "Padding", cert.Padding, 0);
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
            return "Certificate file (*.cert)|*.cert";
        }

        public TreeNode GetExplorerTopNode()
        {
            var tNode = new TreeNode("Certificates") { Tag = TreeViewContextTag.Create(this, 0, new object[] { -1 }) };
            for (var i = 0; i < List.Count; i++)
                tNode.Nodes.Add(new TreeNode("Certificate " + i) { Tag = TreeViewContextTag.Create(this, 0, new object[] { i }) });
            return tNode;
        }

        public TreeNode GetFileSystemTopNode()
        {
            return null;
        }
    }
}
