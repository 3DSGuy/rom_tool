using System.ComponentModel;
using System.IO;
using System.Media;

namespace _3DSExplorer.Modules
{
    public enum ModuleType
    {
        Unknown = -1,
        Banner = 0,
        DARC,
        CIA,
        CBMD,
        CGFX,
        CWAV,
        ICN,
        MPO,
        CCI,
        CXI,
        CFA,
        SaveFlash_Decrypted,
        SaveFlash,
        Archive,
        TMD //Contains certificates & ticket so they can't be recognized
    }

    public static class ModuleHelper
    {
        //TODO: get this from the modules
        public const string OpenString = 
            @"All Supported|*.darc;*.bcma;*.3ds;*.cci;*.cxi;*.cfa;*.csu;*.cbmd;*.bin;*.sav;*.tmd;*.cia;*.mpo;*.bnr;*.bcwav;*.cwav;*.cgfx;*.icn;*.zip;*.7z|" +
            "CTR Cartridge Images (*.cci/3ds/csu)|*.3ds;*.cci;*.csu|"+
            "DArchives (darc) |*.darc;*.bcma|" +
            "Archived CCI (zip/7z)|*.zip;*.7z|" +
            "CTR Executable Images (*.cxi)|*.cxi|" +
            "CTR File Archives (*.cfa)|*.cfa|" +
            "CTR Banners (*.bnr)|*.bnr|" +
            "CTR Banner Model Data (*.cbmd)|*.cbmd|" +
            "CTR Graphics (*.cgfx)|*.cgfx|" +
            "CTR Icons (*.icn)|*.icn|" +
            "CTR Importable Archives (*.cia)|*.cia|"+
            "CTR Waves (*.b/cwav)|*.bcwav;*.cwav|"+
            "Save Flash Files (*.sav/bin)|*.bin;*.sav|" +
            "Title Metadata (*.tmd)|*.tmd|" +
            "MPO (3D Images) Files (*.mpo)|*.mpo|" +
            "All Files|*.*";

        public static IContext CreateByType(ModuleType type)
        {
            switch (type)
            {
                case ModuleType.DARC:
                    return new DARCContext();
                case ModuleType.Banner:
                    return new BannerContext();
                case ModuleType.CGFX:
                    return new CGFXContext();
                case ModuleType.CBMD:
                    return new CBMDContext();
                case ModuleType.CIA:
                    return new CIAContext();
                case ModuleType.CWAV:
                    return new CWAVContext();
                case ModuleType.ICN:
                    return new ICNContext();
                case ModuleType.MPO:
                    return new MPOContext();
                case ModuleType.CCI:
                    return new CCIContext();
                case ModuleType.CXI:
                    return new CXIContext();
                case ModuleType.SaveFlash_Decrypted:
                case ModuleType.SaveFlash:
                    return new SaveFlashContext();
                case ModuleType.Archive:
                    return new ArchivedCCIContext();
                case ModuleType.TMD:
                    return new TMDContext();
            }
            return null;
        }

        public static ModuleType GetModuleType(string filePath, FileStream fs)
        {
            var type = ModuleType.Unknown;
            var magic = new byte[4];
            var extension = Path.GetExtension(filePath);
            if (extension != null)
                extension = extension.ToLower();

            switch (extension)
            {
                case ".darc":
                    type = ModuleType.DARC;
                    break;
                case ".zip":
                case ".7z":
                    type = ModuleType.Archive;
                    break;
                case ".cci":
                case ".csu":
                case ".3ds":
                    type = ModuleType.CCI;
                    break;
                case ".cbmd":
                    type = ModuleType.CBMD;
                    break;
                case ".cfa":
                case ".cxi":
                    type = ModuleType.CXI;
                    break;
                case ".bin":
                case ".sav":
                    type = ModuleType.SaveFlash_Decrypted;
                    break;
                case ".tmd":
                    type = ModuleType.TMD;
                    break;
                case ".cia":
                    type = ModuleType.CIA;
                    break;
                case ".icn":
                    type = ModuleType.ICN;
                    break;
                case ".mpo":
                    type = ModuleType.MPO;
                    break;
                case ".bnr":
                    type = ModuleType.Banner;
                    break;
                case ".cgfx":
                    type = ModuleType.CGFX;
                    break;
                case ".cwav":
                case ".bcwav":
                    type = ModuleType.CWAV;
                    break;
                default:
                    fs.Seek(0, SeekOrigin.Begin);
                    fs.Read(magic, 0, 4);
                    if ((magic[0] == 'P' && magic[1] == 'K' && magic[2] == 3 && magic[3] == 4) || (magic[0] == '7' && magic[1] == 'z' && magic[2] == 0xBC && magic[3] == 0xAF))
                        type = ModuleType.Archive;
                    else if (magic[0] < 5 && magic[1] == 0 && magic[2] == 1 && magic[3] == 0)
                        type = ModuleType.TMD;
                    else if (magic[0] == 0x20 && magic[1] == 0x20 && magic[2] == 0 && magic[3] == 0)
                        type = ModuleType.CIA;
                    else if (magic[0] == 0xFF && magic[1] == 0xD8 && magic[2] == 0xFF && magic[3] == 0xE1)
                        type = ModuleType.MPO;
                    else if (magic[0] == 'C' && magic[1] == 'B' && magic[2] == 'M' && magic[3] == 'D') //can be Banner but can't know
                        type = ModuleType.CBMD;
                    else if (magic[0] == 'C' && magic[1] == 'G' && magic[2] == 'F' && magic[3] == 'X')
                        type = ModuleType.CGFX;
                    else if (magic[0] == 'C' && magic[1] == 'W' && magic[2] == 'A' && magic[3] == 'V')
                        type = ModuleType.CWAV;
                    else if (magic[0] == 'S' && magic[1] == 'M' && magic[2] == 'D' && magic[3] == 'H')
                        type = ModuleType.ICN;
                    else if (magic[0] == 'N' && magic[1] == 'C' && magic[2] == 'C' && magic[3] == 'H')
                        type = ModuleType.CXI;
                    else if (magic[0] == 'd' && magic[1] == 'a' && magic[2] == 'r' && magic[3] == 'c')
                        type = ModuleType.DARC;
                    else if (fs.Length >= 0x104) // > 256+4
                    {
                        //CCI CHECK
                        fs.Seek(0x100, SeekOrigin.Current);
                        fs.Read(magic, 0, 4);
                        if (magic[0] == 'N' && magic[1] == 'C' && magic[2] == 'S' && magic[3] == 'D')
                            type = ModuleType.CCI;
                        else if (fs.Length >= 0x10000) // > 64kb
                        {
                            //SAVE Check
                            fs.Seek(0, SeekOrigin.Begin);
                            var crcCheck = new byte[8 + 10 * (fs.Length / 0x1000 - 1)];
                            fs.Read(crcCheck, 0, crcCheck.Length);
                            fs.Read(magic, 0, 2);
                            var calcCheck = CRC16.GetCRC(crcCheck);
                            if (magic[0] == calcCheck[0] && magic[1] == calcCheck[1]) //crc is ok then save
                                type = ModuleType.SaveFlash_Decrypted; //SAVE
                        }
                    }
                    break;
            }
            if (type == ModuleType.SaveFlash_Decrypted)
            {
                if (SaveFlashContext.IsEncrypted(fs))
                    type = ModuleType.SaveFlash;
            }
            return type;
        }
    }

    public static class SaverProcess
    {
        private static readonly BackgroundWorker Bw;
        private const int BufferSize = 0x1000;
        private static ProgressDialog _dialog;
        private static bool _cancelled, _working;
        private static long _length;
        private static string _outFile, _outFileName;
        private static Stream _ins, _outs;

        static SaverProcess()
        {
            Bw = new BackgroundWorker {WorkerReportsProgress = true};
            Bw.RunWorkerCompleted += OnRunWorkerCompleted;
            Bw.ProgressChanged += OnProgressChanged;
            Bw.DoWork += OnDoWork;
        }

        private static void OnDoWork(object sender, DoWorkEventArgs e)
        {
            _outs = File.OpenWrite(_outFile);
            var buffer = new byte[BufferSize];
            while (_outs.Position < _length && !_cancelled)
            {
                var readLength = _ins.Read(buffer, 0, buffer.Length);
                if (_outs.Position + readLength > _length)
                    readLength = (int)(_length - _outs.Position);
                _outs.Write(buffer, 0, readLength);
                Bw.ReportProgress((int)(100 * _outs.Position / _length));
            }
            _ins.Close();
            _outs.Close();            
        }

        static void ProgressDialogCancelClicked(object sender, System.EventArgs e)
        {
            _cancelled = true;
        }

        private static void OnProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _dialog.Value = e.ProgressPercentage;
            _dialog.Message = string.Format("Saving to {0} (%{1}) ... ",_outFileName,e.ProgressPercentage);
        }

        private static void OnRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_cancelled)
                if (File.Exists(_outFile))
                    File.Delete(_outFile);
            _dialog.Close();
            SystemSounds.Beep.Play();
            _working = false;
        }

        public static void Run(string title, Stream inStream, string outFile, long length)
        {
            if (_working) return;
            _dialog = new ProgressDialog(title, 100);
            _dialog.CancelClicked += ProgressDialogCancelClicked;
            _ins = inStream;
            _outFile = outFile;
            _outFileName = Path.GetFileName(_outFile);
            _length = length;
            _cancelled = false;
            _dialog.Show();
            _working = true;
            Bw.RunWorkerAsync();
        }
    }
}
