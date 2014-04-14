using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;
using _3DSExplorer.Utils;

namespace _3DSExplorer.Modules
{
    public enum RegionLockBitmask
    {
        Japan = 0x1,
        USA = 0x2,
        Europe = 0x4,
        Australia = 0x8,
        China = 0x10,
        Korea = 0x20,
        Taiwan = 0x40
    }

    [Flags]
    public enum ICNDataFlags
    {
        Visable = 0x1,
        AutoBoot = 0x02,
        AllowStereoscopic3D = 0x04,
        RequireAcceptingCTREula = 0x08,
        AutoSaveOnExit = 0x10,
        UsingAnExternalBanner = 0x20,
        UsingGameRating = 0x40,
        UsesSaveData = 0x80,
        UseIconDatabase = 0x100,
        DisableSavedataBackup = 0x400
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ICNLocalizedDescription
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x80)]
        public byte[] FirstTitle;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x100)]
        public byte[] SecondTitle;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x80)]
        public byte[] Publisher;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ICNHeader
    {
        //SMDH - Header
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x4)]
        public char[] Magic;
        public uint Padding0;
        // Entries
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public ICNLocalizedDescription[] Descriptions;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ICNDataHeader
    {
        public ulong ZeroHead;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] Ratings;
        public uint Region;
        public uint MatchMakerId;
        public ulong MatchMakerBitId;
        public uint Flags;
        public byte EULA_Ver_Minor;
        public byte EULA_Ver_Major;
        public UInt16 Reserved;
        public UInt32 OptimalAnimationDefaultFrame;
        public uint Cec;
        public ulong ZeroTail;
    }

    public class ICNContext : IContext
    {
        private string errorMessage = string.Empty;
        public long StartOffset;
        public ICNHeader Header;
        public ICNDataHeader DataHeader;
        public Bitmap SmallIcon, LargeIcon;

        public enum ICNView
        {
            ICN,
            Data,
        };

        enum Localization
        {
            Japanese = 0,
            English,
            French,
            German,
            Italian,
            Spanish,
            Chinese,
            Korean,
            Dutch,
            Portuguese,
            Russian,
            TraditionalChinese
        }

        public bool Open(Stream fs)
        {
            StartOffset = fs.Position;
            Header = MarshalUtil.ReadStruct<ICNHeader>(fs); //read header
            fs.Seek(StartOffset + 0x2000, SeekOrigin.Begin); //Jump to the icons
            DataHeader = MarshalUtil.ReadStruct<ICNDataHeader>(fs); //read data header
            //fs.Seek(0x40, SeekOrigin.Current); //skip header
            SmallIcon = ImageUtil.ReadImageFromStream(fs, 24, 24, ImageUtil.PixelFormat.RGB565);
            LargeIcon = ImageUtil.ReadImageFromStream(fs, 48, 48, ImageUtil.PixelFormat.RGB565);
            return true;
        }

        public string GetErrorMessage()
        {
            return errorMessage;
        }

        public void Create(FileStream fs, FileStream src)
        {
            ImageUtil.WriteImageToStream(SmallIcon, fs, ImageUtil.PixelFormat.RGB565);
            ImageUtil.WriteImageToStream(LargeIcon, fs, ImageUtil.PixelFormat.RGB565);
        }

        public void View(frmExplorer f, int view, object[] values)
        {
            f.ClearInformation();
            switch ((ICNView)view)
            {
                case ICNView.ICN:
                    f.SetGroupHeaders("ICN", "Title Data", "Icons");
                    string pubString, firString, secString;
                    f.AddListItem(0, 4, "Magic (='SMDH')", Header.Magic, 0);
                    f.AddListItem(4, 4, "Padding 0", Header.Padding0, 0);

                    for (var i = 0; i < Header.Descriptions.Length; i++)
                    {
                        pubString = Encoding.Unicode.GetString(Header.Descriptions[i].Publisher);
                        firString = Encoding.Unicode.GetString(Header.Descriptions[i].FirstTitle);
                        secString = Encoding.Unicode.GetString(Header.Descriptions[i].SecondTitle);
                        f.AddListItem(i.ToString(), ((Localization)i).ToString(), firString, secString, pubString, 1);
                    }

                    f.AddListItem(0x2040, 480, "Small Icon", 24, 2);
                    f.AddListItem(0x24c0, 1200, "Large Icon", 48, 2);
                    break;
                case ICNView.Data:
                    f.SetGroupHeaders("Title Data", "Ratings", "Region Lockout", "Flags");
                    f.AddListItem(0, 16, "Ratings*", DataHeader.Ratings, 0);
                    f.AddListItem(16, 4, "Region Lockout*", DataHeader.Region, 0);
                    f.AddListItem(20, 4, "MatchMaker ID", DataHeader.MatchMakerId, 0);
                    f.AddListItem(24, 8, "MatchMaker Bit ID", DataHeader.MatchMakerBitId, 0);
                    f.AddListItem(32, 4, "Flags*", DataHeader.Flags, 0);
                    f.AddListItem("0x024", "2", "EULA Version", DataHeader.EULA_Ver_Major.ToString("D") + "." + DataHeader.EULA_Ver_Minor.ToString("D"), string.Empty, 0);
                    f.AddListItem(38, 4, "Optimal Animation Default Frame", DataHeader.OptimalAnimationDefaultFrame, 0);
                    f.AddListItem(42, 4, "CEC (StreetPass) ID", DataHeader.Cec, 0);
                    
                    
                    if ((DataHeader.Region & (uint)RegionLockBitmask.Japan) == (uint)RegionLockBitmask.Japan)
                    {
                        f.AddListItem(string.Empty, string.Empty, "Japan", "Yes", string.Empty, 2);
                    }
                    else 
                    {
                        f.AddListItem(string.Empty, string.Empty, "Japan", "No", string.Empty, 2);
                    }
                    if ((DataHeader.Region & (uint)RegionLockBitmask.USA) == (uint)RegionLockBitmask.USA)
                    {
                        f.AddListItem(string.Empty, string.Empty, "North America", "Yes", string.Empty, 2);
                    }
                    else
                    {
                        f.AddListItem(string.Empty, string.Empty, "North America", "No", string.Empty, 2);
                    }
                    if ((DataHeader.Region & (uint)RegionLockBitmask.Europe) == (uint)RegionLockBitmask.Europe)
                    {
                        f.AddListItem(string.Empty, string.Empty, "Europe", "Yes", string.Empty, 2);
                    }
                    else
                    {
                        f.AddListItem(string.Empty, string.Empty, "Europe", "No", string.Empty, 2);
                    }
                    if ((DataHeader.Region & (uint)RegionLockBitmask.Australia) == (uint)RegionLockBitmask.Australia)
                    {
                        f.AddListItem(string.Empty, string.Empty, "Australia", "Yes", string.Empty, 2);
                    }
                    else
                    {
                        f.AddListItem(string.Empty, string.Empty, "Australia", "No", string.Empty, 2);
                    }
                    if ((DataHeader.Region & (uint)RegionLockBitmask.China) == (uint)RegionLockBitmask.China)
                    {
                        f.AddListItem(string.Empty, string.Empty, "China", "Yes", string.Empty, 2);
                    }
                    else
                    {
                        f.AddListItem(string.Empty, string.Empty, "China", "No", string.Empty, 2);
                    }
                    if ((DataHeader.Region & (uint)RegionLockBitmask.Korea) == (uint)RegionLockBitmask.Korea)
                    {
                        f.AddListItem(string.Empty, string.Empty, "Korea", "Yes", string.Empty, 2);
                    }
                    else
                    {
                        f.AddListItem(string.Empty, string.Empty, "Korea", "No", string.Empty, 2);
                    }
                    if ((DataHeader.Region & (uint)RegionLockBitmask.Taiwan) == (uint)RegionLockBitmask.Taiwan)
                    {
                        f.AddListItem(string.Empty, string.Empty, "Taiwan", "Yes", string.Empty, 2);
                    }
                    else
                    {
                        f.AddListItem(string.Empty, string.Empty, "Taiwan", "No", string.Empty, 2);
                    }
                    
                    f.AddListItem(0, 1, "CERO (Japan)", (byte)(DataHeader.Ratings[0] & 0x7F), 1);
                    f.AddListItem(0, 1, "ESRB (North America)", (byte)(DataHeader.Ratings[1] & 0x7F), 1);
                    f.AddListItem(0, 1, "USK (Germany)", (byte)(DataHeader.Ratings[3] & 0x7F), 1);
                    f.AddListItem(0, 1, "PEGI (Europe)", (byte)(DataHeader.Ratings[4] & 0x7F), 1);
                    f.AddListItem(0, 1, "PEGI (Portugal)", (byte)(DataHeader.Ratings[6] & 0x7F), 1);
                    f.AddListItem(0, 1, "BBFC (England)", (byte)(DataHeader.Ratings[7] & 0x7F), 1);
                    f.AddListItem(0, 1, "COB (Australia)", (byte)(DataHeader.Ratings[8] & 0x7F), 1);
                    f.AddListItem(0, 1, "GRB (South Korea) ", (byte)(DataHeader.Ratings[9] & 0x7F), 1);
                    f.AddListItem(0, 1, "CGSRR (China) ", (byte)(DataHeader.Ratings[10] & 0x7F), 1);
                    f.AddListItem("", "", "Allow Stereoscopic 3D", (((uint)ICNDataFlags.AllowStereoscopic3D & DataHeader.Flags) > 0).ToString(), "", 3);
                    f.AddListItem("", "", "Require Accepting CTR Eula", (((uint)ICNDataFlags.RequireAcceptingCTREula & DataHeader.Flags) > 0).ToString(), "", 3);
                    f.AddListItem("", "", "Auto Save On Exit", (((uint)ICNDataFlags.AutoSaveOnExit & DataHeader.Flags) > 0).ToString(), "", 3);
                    f.AddListItem("", "", "Auto Boot", (((uint)ICNDataFlags.AutoBoot & DataHeader.Flags) > 0).ToString(), "", 3);
                    f.AddListItem("", "", "Using An External Banner", (((uint)ICNDataFlags.UsingAnExternalBanner & DataHeader.Flags) > 0).ToString(), "", 3);
                    f.AddListItem("", "", "Uses Save Data", (((uint)ICNDataFlags.UsesSaveData & DataHeader.Flags) > 0).ToString(), "", 3);
                    f.AddListItem("", "", "Uses Region Game Ratings", (((uint)ICNDataFlags.UsingGameRating & DataHeader.Flags) > 0).ToString(), "", 3);
                    f.AddListItem("", "", "Uses Icon Database", (((uint)ICNDataFlags.UseIconDatabase & DataHeader.Flags) > 0).ToString(), "", 3);
                    f.AddListItem("", "", "Disable Savedata Backup", (((uint)ICNDataFlags.DisableSavedataBackup & DataHeader.Flags) > 0).ToString(), "", 3);
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
                        var graphics = Graphics.FromImage(iconImage);
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
                    break;
            }
        }

        public string GetFileFilter()
        {
            return "CTR Icon Data (*.icn)|*.icn";
        }

        public TreeNode GetExplorerTopNode()
        {
            var tNode = new TreeNode("ICN") { Tag = TreeViewContextTag.Create(this, (int)ICNView.ICN) };
            tNode.Nodes.Add(new TreeNode("Data") { Tag = TreeViewContextTag.Create(this, (int)ICNView.Data) });
            return tNode;
        }

        public TreeNode GetFileSystemTopNode()
        {
            var tNode = new TreeNode("ICN", 1, 1);
            tNode.Nodes.Add(new TreeNode(TreeListView.TreeListViewControl.CreateMultiColumnNodeText("Small Icon", "24x24")) { Tag = new[] { TreeViewContextTag.Create(this, 0, "Show...", new object[] { SmallIcon }), TreeViewContextTag.Create(this, 1, "Replace...", new object[] { SmallIcon }) } });
            tNode.Nodes.Add(new TreeNode(TreeListView.TreeListViewControl.CreateMultiColumnNodeText("Large Icon", "48x48")) { Tag = new[] { TreeViewContextTag.Create(this, 0, "Show...", new object[] { LargeIcon }), TreeViewContextTag.Create(this, 1, "Replace...", new object[] { LargeIcon }) } });
            return tNode;
        }
    }

}
