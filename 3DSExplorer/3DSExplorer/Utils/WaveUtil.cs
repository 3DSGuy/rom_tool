using System.Runtime.InteropServices;

namespace _3DSExplorer
{
    public static class WaveUtil
    {
        [DllImport("WinMM.dll")]
        private static extern bool PlaySound(string fname, int Mod, int flag);

        public static int SND_ASYNC = 0x0001;     // play asynchronously
        public static int SND_FILENAME = 0x00020000; // use file name
        public static int SND_PURGE = 0x0040;     // purge non-static events

        public static void PlayFile(string fname)
        {
            Play(fname, SND_FILENAME|SND_ASYNC);
        }

        public static void Play(string fname, int soundFlags)
        {
            PlaySound(fname, 0, soundFlags);
        }

        public static void StopPlay()
        {
            PlaySound(null, 0, SND_PURGE);
        }
    }
}
