using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace _3DSExplorer
{
    public class FfmpegWrapper
    {
        private static bool _busy;

        private readonly string _ffpmegPath;

        public delegate void ProgressChanged(int value, int max);
        public delegate void ProcessFinished(string errorString);

        private readonly ProgressChanged _progressChanged;
        private readonly ProcessFinished _processFinished;

        private string _errorMessage;
        private int _timeLimit;

        private bool _accepted, _started;
        private int _duration, _timeProgress;
        private BackgroundWorker _backgroundWorker;
        private TimeSpan _durationTime;
        private string _filename;
        private string _filePrefix;
        private string _filePostfix;

        public FfmpegWrapper(string ffpmegPath, ProgressChanged progressChanged, ProcessFinished processFinished)
        {
            _ffpmegPath = ffpmegPath;
            _progressChanged = progressChanged;
            _processFinished = processFinished;
        }

        #region Private methods

        private static int ConvertTimeToInt(string timeString)
        {
            return (int)TimeSpan.Parse(timeString).TotalSeconds;
        }

        #region Background Worker methods

        private void BackgroundWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _busy = false;
            _processFinished(_errorMessage);
        }

        private void BackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            var tsTimeLimit = TimeSpan.FromSeconds(_timeLimit);
            var limited = _timeLimit > 0;
            var args = (limited ? "-t " + tsTimeLimit : string.Empty) + e.Argument;
            _duration = 1;
            _durationTime = TimeSpan.FromSeconds(_duration); 
            _timeProgress = 0;

            var part2AndUp = false;
            var tsStartTime = TimeSpan.Zero;
            var count = 0;
            while (tsStartTime.CompareTo(_durationTime) < 0)
            {
                var process = new Process
                {
                    StartInfo =
                        {
                            FileName = _ffpmegPath,
                            Arguments =
                            string.Format("{0} {1} \"{2}\"", 
                                part2AndUp ? "-ss " + tsStartTime : string.Empty, 
                                args,
                                part2AndUp ? _filePrefix + count++ + _filePostfix : _filename),
                            CreateNoWindow = true,
                            RedirectStandardError = true,
                            RedirectStandardOutput = false,
                            UseShellExecute = false
                        }
                };
                process.ErrorDataReceived += ProcessErrorDataReceived;
                _started = false;
                _accepted = false;
                if (!process.Start())
                {
                    _errorMessage = "Error starting ffmpeg.exe";
                    return;
                }
                process.BeginErrorReadLine();
                process.WaitForExit();
                if (!_started)
                    break;
                _errorMessage = string.Empty;
                if (!part2AndUp)
                    part2AndUp = true;

                if (!limited) break;
                tsStartTime = tsStartTime.Add(tsTimeLimit);
            }
        }

        void ProcessErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            var line = e.Data;
            if (line == null) return;
            _errorMessage = line;

            if (!_accepted && line.StartsWith("Input #"))
                _accepted = true;
            if (!_accepted) return;
            if (!_started && line.StartsWith("frame"))
                _started = true;
            else if (!line.StartsWith("fr"))
            {
                //get duration
                if (line.StartsWith("  Duration:"))
                {
                    _duration = ConvertTimeToInt(line.Substring(line.IndexOf('0'), 11));
                    _durationTime = TimeSpan.FromSeconds(_duration);
                }
            }
            else
            {
                _timeProgress = ConvertTimeToInt(line.Substring(line.IndexOf("time=") + 5, 11));
                _backgroundWorker.ReportProgress(_timeProgress, _duration);
            }
        }

        private void BackgroundWorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _progressChanged(e.ProgressPercentage,(int)e.UserState);
        }

        #endregion

        #endregion

        public void Convert(int timeLimit, string args, string outputFilename)
        {
            if (_busy) return;
            _timeLimit = timeLimit;
            _filename = outputFilename;
            _filePrefix = Path.GetDirectoryName(_filename) + "\\" + Path.GetFileNameWithoutExtension(_filename);
            _filePostfix = Path.GetExtension(_filename);
            _backgroundWorker = new BackgroundWorker {WorkerReportsProgress = true};
            _backgroundWorker.ProgressChanged += BackgroundWorkerProgressChanged;
            _backgroundWorker.DoWork += BackgroundWorkerDoWork;
            _backgroundWorker.RunWorkerCompleted += BackgroundWorkerRunWorkerCompleted;
            _backgroundWorker.RunWorkerAsync(args);
            _busy = true;
        }

    }
}
