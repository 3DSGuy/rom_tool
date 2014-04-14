using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace _3DSExplorer.Modules
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TimeLimitEntry
    {
        public uint EnableTimeLimit;
        public uint TimeLimitSeconds;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Ticket
    {
        public SignatureType SignatureType;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x100)]
        public byte[] Signature;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x3C)]
        public byte[] Padding0;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x40)]
        public char[] Issuer;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x3C)]
        public byte[] ECDSA;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x3)]
        public byte[] Padding1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x10)]
        public byte[] EncryptedTitleKey;
        public byte Unknown0;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] TicketID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] ConsoleID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] TitleID;
        public ushort SystemAccess;
        public ushort TicketVersion;
        public uint PermittedTitlesMask;
        public uint PermitMask;
        public byte TitleExport;
        public byte CommonKeyIndex;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x30)]
        public byte[] Unknown1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x40)]
        public byte[] ContentPermissions;
        public ushort Padding2;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public TimeLimitEntry[] TimeLimitEntries;
    }

    public class TicketContext : IContext
    {
        private string errorMessage = string.Empty;
        public Ticket Ticket;

        public bool Open(Stream fs)
        {
            try
            {
                Ticket = MarshalUtil.ReadStructBE<Ticket>(fs);
                return true;
            }
            catch
            {
                errorMessage = "Error opening ticket.";
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

        public void View(frmExplorer f, int view, object[] values)
        {
            f.ClearInformation();
            f.SetGroupHeaders("Ticket", "Ticket Time Limits");
            f.AddListItem(0x000, 0x004, "Signature Type", (ulong)Ticket.SignatureType, 0);
            f.AddListItem(0x004, 0x100, "RSA-2048 signature of the Ticket", Ticket.Signature, 0);
            f.AddListItem(0x104, 0x03C, "Padding 0", Ticket.Padding0, 0);
            f.AddListItem(0x140, 0x040, "Issuer", Ticket.Issuer, 0);
            f.AddListItem(0x180, 0x03C, "ECDSA", Ticket.ECDSA, 0);
            f.AddListItem(0x1BC, 0x003, "Padding 1", Ticket.Padding1, 0);
            f.AddListItem(0x1BF, 0x010, "Encrypted Title Key", Ticket.EncryptedTitleKey, 0);
            f.AddListItem(0x1CF, 0x001, "Unknown 0", Ticket.Unknown0, 0);
            f.AddListItem(0x1D0, 0x008, "Ticket ID", Ticket.TicketID, 0);
            f.AddListItem(0x1D8, 0x004, "Console ID", Ticket.ConsoleID, 0);
            f.AddListItem(0x1DC, 0x008, "Title ID", Ticket.TitleID, 0);
            f.AddListItem(0x1E4, 0x002, "System Access", Ticket.SystemAccess, 0);
            f.AddListItem(0x1E6, 0x002, "Ticket Version", Ticket.TicketVersion, 0);
            f.AddListItem(0x1E8, 0x004, "Permitted Titles Mask", Ticket.PermittedTitlesMask, 0);
            f.AddListItem(0x1EC, 0x004, "Permit Mask", Ticket.PermitMask, 0);
            f.AddListItem(0x1F0, 0x001, "Title Export allowed using PRNG key", Ticket.TitleExport, 0);
            f.AddListItem(0x1F1, 0x001, "Common Key index (1=Korean,0=Normal)", Ticket.CommonKeyIndex, 0);
            f.AddListItem(0x1F2, 0x030, "Unknown1", Ticket.Unknown1, 0);
            f.AddListItem(0x222, 0x040, "Content access permissions (bit for each content)", Ticket.ContentPermissions, 0);
            f.AddListItem(0x262, 0x002, "Padding 2", Ticket.Padding2, 0);
            for (int i = 0; i < Ticket.TimeLimitEntries.Length; i++)
                f.AddListItem(0x264 + i * 8, 0x004, "Time Limit Enabled=" + Ticket.TimeLimitEntries[i].EnableTimeLimit + " For", Ticket.TimeLimitEntries[i].TimeLimitSeconds, 1);
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
            return "Ticket files (*.tik)|*.tik";
        }

        public TreeNode GetExplorerTopNode()
        {
            var topNode = new TreeNode("Ticket") { Tag = TreeViewContextTag.Create(this) };

            return topNode;
        }

        public TreeNode GetFileSystemTopNode()
        {
            return null;
        }
    }

}
