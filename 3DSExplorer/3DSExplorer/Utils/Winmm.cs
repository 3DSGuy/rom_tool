using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace _3DSExplorer
{
    public enum WaveFormats : ushort
    {
        PCM = 1,
        ADPCM = 2,
        Float = 3,
        ImaADPCM = 17
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct WaveFormat
    {
        public ushort FormatTag;
        public ushort Channels;
        public uint SampleRate;
        public uint ByteRate;
        public ushort BlockAlign;
        public ushort BitsPerSample;
        //public ushort cbSize;
    }

    public class WinMM
    {
        /* consts
        public const int FRAMELEN = 256;
        public const int FRAMELEN2 = FRAMELEN*2;
        public const int MAXPNAMELEN = 32;
        private const int MMSYSERR_BASE = 0;
        public const int WHDR_DONE = 0x1; //bit 1 is set when buffer is filled

        #region Enums
        public enum MMRESULT
        {
            MMSYSERR_NOERROR = 0,
            MMSYSERR_ERROR = (MMSYSERR_BASE + 1),
            MMSYSERR_BADDEVICEID = (MMSYSERR_BASE + 2),
            MMSYSERR_NOTENABLED = (MMSYSERR_BASE + 3),
            MMSYSERR_ALLOCATED = (MMSYSERR_BASE + 4),
            MMSYSERR_INVALHANDLE = (MMSYSERR_BASE + 5),
            MMSYSERR_NODRIVER = (MMSYSERR_BASE + 6),
            MMSYSERR_NOMEM = (MMSYSERR_BASE + 7),
            MMSYSERR_NOTSUPPORTED = (MMSYSERR_BASE + 8),
            MMSYSERR_BADERRNUM = (MMSYSERR_BASE + 9),
            MMSYSERR_INVALFLAG = (MMSYSERR_BASE + 10),
            MMSYSERR_INVALPARAM = (MMSYSERR_BASE + 11),
            MMSYSERR_HANDLEBUSY = (MMSYSERR_BASE + 12),
            MMSYSERR_INVALIDALIAS = (MMSYSERR_BASE + 13)
        }
        #endregion

        #region Callbacks
        public delegate void WaveDelegate(IntPtr hdrvr, int uMsg, int dwUser, ref WaveHdr wavhdr, int dwParam2);
        #endregion
        */
        #region Structs
        [StructLayout(LayoutKind.Sequential)]
        public struct WaveHdr
        {
            public IntPtr lpData; // pointer to locked data buffer
            public int dwBufferLength; // length of data buffer
            public int dwBytesRecorded; // used for input only
            public IntPtr dwUser; // for client's use
            public int dwFlags; // assorted flags (see defines)
            public int dwLoops; // loop control counter
            public IntPtr lpNext; // PWaveHdr, reserved for driver
            public int reserved; // reserved for driver
        }
        /*
        public struct WaveInCaps
        {
            public short wMid;
            public short wPid;
            public int vDriverVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = WinMM.MAXPNAMELEN)] public string szPname;
            public int dwFormats;
            public short wChannels;
        }*/

        #endregion

        public static byte[] WriteWAVFile(WaveFormat wf, byte[][] rawData)
        {
            var bw = new MemoryStream();
            var rawSize = 0;
            for (var j = 0; j < rawData.Length; j++)
                rawSize += rawData[j].Length;
            //RIFF HEADER
            bw.Write(Encoding.ASCII.GetBytes("RIFF"), 0, 4);
            bw.Write(BitConverter.GetBytes(rawSize + 36), 0, 4);
            bw.Write(Encoding.ASCII.GetBytes("WAVE".ToCharArray()), 0, 4);

            // SUBCHUNK
            bw.Write(Encoding.ASCII.GetBytes("fmt ".ToCharArray()), 0, 4);
            bw.Write(BitConverter.GetBytes(16), 0, 4);
            var chunk = MarshalUtil.StructureToByteArray(wf);
            bw.Write(chunk, 0, chunk.Length);

            // DATA CHUNK
            bw.Write(Encoding.ASCII.GetBytes("data".ToCharArray()), 0, 4);
            bw.Write(BitConverter.GetBytes(rawSize), 0, 4);
            for (var i = 0; i < rawData[0].Length; i += wf.BitsPerSample / 8 )
            {
                for (var j = 0; j < rawData.Length; j++ )
                {
                    bw.WriteByte(rawData[j][i]);
                    if (wf.BitsPerSample == 16)
                        bw.WriteByte(rawData[j][i+1]);
                }
            }

            return bw.ToArray();
        }

        public static void WriteWAVFile(string filePath, WaveFormat wf, byte[][] rawData)
        {
            var fileStream = File.OpenWrite(filePath);
            var rawSize = 0;
            for (var j = 0; j < rawData.Length; j++)
                rawSize += rawData[j].Length;
            //RIFF HEADER
            fileStream.Write(Encoding.ASCII.GetBytes("RIFF"), 0, 4);
            fileStream.Write(BitConverter.GetBytes(rawSize + 36), 0, 4);
            fileStream.Write(Encoding.ASCII.GetBytes("WAVE".ToCharArray()), 0, 4);

            // SUBCHUNK
            fileStream.Write(Encoding.ASCII.GetBytes("fmt ".ToCharArray()), 0, 4);
            fileStream.Write(BitConverter.GetBytes(16), 0, 4);
            var chunk = MarshalUtil.StructureToByteArray(wf);
            fileStream.Write(chunk, 0, chunk.Length);

            // DATA CHUNK
            fileStream.Write(Encoding.ASCII.GetBytes("data".ToCharArray()), 0, 4);
            fileStream.Write(BitConverter.GetBytes(rawSize), 0, 4);
            for (var i = 0; i < rawData[0].Length; i += wf.BitsPerSample / 8)
            {
                for (var j = 0; j < rawData.Length; j++)
                {
                    fileStream.WriteByte(rawData[j][i]);
                    if (wf.BitsPerSample == 16)
                        fileStream.WriteByte(rawData[j][i + 1]);
                }
            }

            fileStream.Close();
        }
/*
        #region WINMM.DLL functions

        private const string mmdll = "winmm.dll";
        // WaveOut calls
        [DllImport(mmdll)]
        public static extern int waveOutGetNumDevs();

        [DllImport(mmdll)]
        public static extern int waveOutPrepareHeader(IntPtr hWaveOut, ref WaveHdr lpWaveOutHdr, uint uSize);

        [DllImport(mmdll)]
        public static extern int waveOutUnprepareHeader(IntPtr hWaveOut, ref WaveHdr lpWaveOutHdr, uint uSize);

        [DllImport(mmdll)]
        public static extern int waveOutWrite(IntPtr hWaveOut, ref WaveHdr lpWaveOutHdr, uint uSize);

        [DllImport(mmdll)]
        public static extern int waveOutOpen(out IntPtr hWaveOut, int uDeviceID, WaveFormat lpFormat,
                                             WaveDelegate dwCallback, int dwInstance, int dwFlags);

        [DllImport(mmdll)]
        public static extern int waveOutReset(IntPtr hWaveOut);

        [DllImport(mmdll)]
        public static extern int waveOutClose(IntPtr hWaveOut);

        [DllImport(mmdll)]
        public static extern int waveOutPause(IntPtr hWaveOut);

        [DllImport(mmdll)]
        public static extern int waveOutRestart(IntPtr hWaveOut);

        [DllImport(mmdll)]
        public static extern int waveOutGetPosition(IntPtr hWaveOut, out int lpInfo, uint uSize);

        [DllImport(mmdll)]
        public static extern int waveOutSetVolume(IntPtr hWaveOut, int dwVolume);

        [DllImport(mmdll)]
        public static extern int waveOutGetVolume(IntPtr hWaveOut, out int dwVolume);

        // WaveIn calls
        [DllImport(mmdll)]
        public static extern int waveInGetNumDevs();

        [DllImport(mmdll)]
        public static extern int waveInGetDevCaps(int uDeviceID, ref WaveInCaps lpCaps, uint uSize);

        [DllImport(mmdll)]
        public static extern int waveInAddBuffer(IntPtr hwi, ref WaveHdr pwh, int cbwh);

        [DllImport(mmdll)]
        public static extern int waveInClose(IntPtr hwi);

        [DllImport(mmdll)]
        public static extern int waveInOpen(out IntPtr phwi, int uDeviceID, WaveFormat lpFormat, WaveDelegate dwCallback,
                                            int dwInstance, int dwFlags);

        [DllImport(mmdll)]
        public static extern int waveInPrepareHeader(IntPtr hWaveIn, ref WaveHdr lpWaveInHdr, uint uSize);

        [DllImport(mmdll)]
        public static extern int waveInUnprepareHeader(IntPtr hWaveIn, ref WaveHdr lpWaveInHdr, uint uSize);

        [DllImport(mmdll)]
        public static extern int waveInReset(IntPtr hwi);

        [DllImport(mmdll)]
        public static extern int waveInStart(IntPtr hwi);

        [DllImport(mmdll)]
        public static extern int waveInStop(IntPtr hwi);

        #endregion
 */
    }
}
