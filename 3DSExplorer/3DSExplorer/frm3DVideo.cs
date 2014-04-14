using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Web;
using System.Windows.Forms;
using _3DSExplorer.Properties;

namespace _3DSExplorer
{
    public partial class frm3DVideo : Form
    {
        private ConvertionState _state;
        private FfmpegWrapper _ffmpeg;
        private readonly YouTube _youTube;
        private string _processedFile;
        private int _selectedPosition;
        private WebClient _webClient;
        private string _youTubeSavePath;

        delegate void ChangeFormDelegate(bool state);
        delegate void ChangeStatusDelegate(string text);

        private ChangeFormDelegate ChangeForm;
        private ChangeStatusDelegate ChangeStatus;

        enum ConvertionState
        {
            Video2D = -1,
            Left = 0,
            Right = 100,
            Combination = 200
        }

        public frm3DVideo()
        {
            InitializeComponent();
            ChangeForm = ChangeFormMethod;
            ChangeStatus = ChangeStatusMethod;
            CheckFFMpeg();
            _youTube = new YouTube(YouTubeProcessFinished,YouTubeImageReady);
            cmbOrientation.SelectedIndex = 0;
            _webClient = new WebClient { Proxy = null }; //Make connection smooth
            _webClient.DownloadProgressChanged += WebClientDownloadProgressChanged;
            _webClient.DownloadFileCompleted += WebClientDownloadFileCompleted;
            cmbSampleRate.SelectedIndex = 4;
        }

        #region YouTube Methods

        void WebClientDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            ConvertFrom(Path.GetDirectoryName(Application.ExecutablePath) + "\\youtube.flv");
        }

        void WebClientDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            ChangeStatus("Status: Downloading from youtube (" + e.BytesReceived + "/" + e.TotalBytesToReceive+ ")");
        }

        void YouTubeProcessFinished(string errorString)
        {
            _webClient.DownloadFileAsync(new Uri(errorString), _youTubeSavePath);
        }

        void YouTubeImageReady(Image image)
        {
            picThumb.Image = image;
        }

        #endregion

        private void CheckFFMpeg()
        {
            var ffmpegPath = Settings.Default.FFMpegPath;
            if (!File.Exists(ffmpegPath))
            {
                ChangeForm(false);
                ChangeStatus("Status: ffmpeg doesn't exist.");
                btnSet.BackColor = Color.LightCoral;
                return;
            }
            var fs = File.OpenRead(ffmpegPath);
            var magic = new byte[2];
            fs.Read(magic, 0, 2);
            if (!ffmpegPath.ToLower().EndsWith("exe") || magic[0] != 'M' || magic[1] != 'Z')
            {
                ChangeForm(false);
                ChangeStatus("Status: ffmpeg file isn't a valid exe file.");
                btnSet.BackColor = Color.LightCoral;
                return;
            }
            ChangeForm(true);
            _ffmpeg = new FfmpegWrapper(ffmpegPath, FfmpegProgressChanged, FfmpegProcessFinished);
            ChangeStatus("Status: ffmpeg ready.");
            btnSet.BackColor = SystemColors.ButtonFace;
        }

        private void ChangeFormMethod(bool state)
        {
            grpDestination.Enabled = state;
            grpSource.Enabled = state;
            grpVideo.Enabled = state;
            grpAudio.Enabled = state;
            btnGo.Enabled = state;
        }
        private void ChangeStatusMethod(string text)
        {
            lblStatus.Text = text;
        }

        private void CheckForYouTubeString(string text)
        {
            if (string.IsNullOrEmpty(text)) return;
            if (!text.ToLower().StartsWith("http://") || !text.ToLower().Contains("youtu")) return;
            var youtube = new Uri(text);
            var videoId = HttpUtility.ParseQueryString(youtube.Query)["v"];
            if (!string.IsNullOrEmpty(videoId))
            {
                if (txtYoutube.Text != videoId)
                {
                    txtYoutube.Text = videoId;        
                    picThumb.Image = Resources.spinner;
                    Application.DoEvents();
                    _youTube.DownloadThumbnail(videoId);
                }
            }
            else
                picThumb.Image = null;
        }

        private void RadioSourceCheckedChanged(object sender, EventArgs e)
        {
            txtSourceFile.Visible = radSourceFile.Checked;
            btnSourceBrowse.Visible = radSourceFile.Checked;
            txtYoutube.Visible = radSourceYoutube.Checked;
            lblYoutube.Visible = radSourceYoutube.Checked;
            picThumb.Visible = radSourceYoutube.Checked;
        }

        private void txtYoutube_TextChanged(object sender, EventArgs e)
        {
            CheckForYouTubeString(txtYoutube.Text);
        }

        private void btnSourceBrowse_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog()!=DialogResult.OK) return;
            txtSourceFile.Text = openFileDialog.FileName;
        }

        private void chk3D_CheckedChanged(object sender, EventArgs e)
        {
            lblOrientation.Enabled = chk3D.Checked;
            cmbOrientation.Enabled = chk3D.Checked;
        }

        private void tbQuality_Scroll(object sender, EventArgs e)
        {
            txtQuality.Text = tbQuality.Value.ToString();
        }

        private void tbCores_Scroll(object sender, EventArgs e)
        {
            txtCores.Text = tbCores.Value.ToString();
        }

        private void btnDestinationBrowse_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;
            txtOutputFile.Text = saveFileDialog.FileName;
        }

        private void btnSetLocation_Click(object sender, EventArgs e)
        {
            if (openFfmpegDialog.ShowDialog() != DialogResult.OK) return;
            Settings.Default.FFMpegPath = openFfmpegDialog.FileName;
            Settings.Default.Save();
            CheckFFMpeg();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frm3DVideo_Activated(object sender, EventArgs e)
        {
            CheckForYouTubeString(Clipboard.GetText());
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            ChangeForm(false);
            _selectedPosition = cmbOrientation.SelectedIndex;
            progressBar.Value = 0;
            progressBar.Maximum = 300;          
            if (radSourceYoutube.Checked)
            {
                if (txtYoutube.Text == string.Empty)
                {
                    MessageBox.Show("Please enter a youtube video id.");
                    return;
                }
                progressBar.Maximum += 100;
                ChangeStatus("Status: Downloading from youtube");
                _youTubeSavePath = Path.GetDirectoryName(Application.ExecutablePath) + "\\youtube.flv";
                _youTube.GetVideoURL(txtYoutube.Text);
                return; //we will continue after the download is done
            }
            var error = false;
            var errorMessage = string.Empty;
            if (txtSourceFile.Text == string.Empty)
            {
                errorMessage = "You need to enter a source file first";
                error = true;
            }
            if (txtOutputFile.Text == string.Empty)
            {
                errorMessage += Environment.NewLine + "You need to enter an output location first";
                error = true;
            }
            if (error)
            {
                ChangeStatus("Status: Exited with error " + errorMessage);
                MessageBox.Show(errorMessage);
                ChangeForm(true);
            }
            else
                ConvertFrom(txtSourceFile.Text);
        }

        private void ConvertFrom(string inFile)
        {
            _processedFile = inFile;
            if (chk3D.Checked)
            {
                _state = ConvertionState.Left;
                MakeLeft();
            }
            else
            {
                _state = ConvertionState.Video2D;
                Make2D();
            }
        }
    

        private void MakeLeft()
        {
            var position = string.Empty;
            switch (_selectedPosition)
            {
                case 0:
                    position = "0:0";
                    break;
                case 1:
                    position = "400:0";
                    break;
                case 2:
                    position = "0:0";
                    break;
                default: //3
                    position = "0:400";
                    break;
            }
            ChangeStatus("Status: Start making the left video.");
            Application.DoEvents();
            var parameters =
                string.Format(
                    "-y -threads {0} -i \"{1}\" -s 800x240 -vcodec mjpeg {2} -vf crop=400:240:{3} -acodec libmp3lame -ar {4} -ab {5}k -vol {6} -ac {7}",
                    tbCores.Value, _processedFile, (chkAdvanced.Checked ? "-r " + numFps.Value + " -b:v " + txtVideoBitrate.Text : "-q " + tbQuality.Value),
                    position, cmbSampleRate.Text, txtAudioBitrate.Text, numVolume.Value,2);
            _ffmpeg.Convert(0,parameters,Path.GetDirectoryName(Application.ExecutablePath) + "\\left.avi");
        }


        private void MakeRight()
        {
            var position = string.Empty;
            switch (_selectedPosition)
            {
                case 0:
                    position = "400:0";
                    break;
                case 1:
                    position = "0:0";
                    break;
                case 2:
                    position = "0:400";
                    break;
                default: //3
                    position = "0:0";
                    break;
            }
            ChangeStatus("Status: Start making the right video.");
            Application.DoEvents();
            var parameters =
                string.Format(
                    "-y -threads {0} -i \"{1}\" -s 800x240 -vcodec mjpeg {2} -vf crop=400:240:{3} -an",
                    tbCores.Value, _processedFile, (chkAdvanced.Checked ? "-r " + numFps.Value + " -b:v " + txtVideoBitrate.Text : "-q " + tbQuality.Value),
                    position);
            _ffmpeg.Convert(0, parameters, Path.GetDirectoryName(Application.ExecutablePath) + "\\right.avi");
        }
        
        private void Make2D()
        {
            ChangeStatus("Status: Start making 2D video.");
            Application.DoEvents();
            var parameters =
                string.Format(
                    "-y -threads {0} -i \"{1}\" -s 400x240 -vcodec mjpeg {2} -acodec adpcm_ima_wav -ar {3} -ab {4}k -vol {5} -ac {6}",
                    tbCores.Value, _processedFile, (chkAdvanced.Checked ? "-r " + numFps.Value + " -b:v " + txtVideoBitrate.Text : "-q " + tbQuality.Value),
                    cmbSampleRate.Text, txtAudioBitrate.Text, numVolume.Value, 2);
            _ffmpeg.Convert(chkSplit.Checked ? (int)numLimit.Value : 0, parameters, txtOutputFile.Text);
        }

        private void CombineLeftAndRight()
        {
            ChangeStatus("Status: Start combining.");
            Application.DoEvents();
            var parameters =
                string.Format(
                    "-y -threads {0} -i \"{1}\" -i \"{2}\" -vcodec copy -acodec adpcm_ima_wav -ac 2 -vcodec copy -map 0:0 -map 0:1 -map 1:0",
                    tbCores.Value, Path.GetDirectoryName(Application.ExecutablePath) + "\\left.avi", Path.GetDirectoryName(Application.ExecutablePath) + "\\right.avi");
            _ffmpeg.Convert(chkSplit.Checked ? (int)numLimit.Value : 0, parameters, txtOutputFile.Text);
        }

        private void FfmpegProgressChanged(int value, int max)
        {
            var startValue = (int) _state;
            if (startValue < 0) startValue = 0;
            if (radSourceYoutube.Checked)
                startValue += 100;
            progressBar.Value = startValue + (100 * value / max);
            ChangeStatus(string.Format("Status: Making {0} ({1}/{2})", _state, value, max));
        }

        private void FfmpegProcessFinished(string error)
        {
            if (!string.IsNullOrEmpty(error))
            {
                MessageBox.Show(error);
                ChangeForm(true);
                return;
            }
            switch (_state)
            {
                case ConvertionState.Left:
                    _state = ConvertionState.Right;
                    MakeRight();
                    break;
                case ConvertionState.Right:
                    _state = ConvertionState.Combination;
                    CombineLeftAndRight();
                    break;
                case ConvertionState.Combination:
                    ChangeForm(true);
                    if (chkDeleteTempFiles.Checked)
                    {
                        File.Delete(Path.GetDirectoryName(Application.ExecutablePath) + "\\left.avi");
                        File.Delete(Path.GetDirectoryName(Application.ExecutablePath) + "\\right.avi");
                        File.Delete(Path.GetDirectoryName(Application.ExecutablePath) + "\\youtube.flv");
                    }
                    ChangeStatus("Status: Video file saved successfuly.");
                    progressBar.Value = 0;
                    break;
                case ConvertionState.Video2D:
                    ChangeForm(true);
                    ChangeStatus("Status: Video file saved successfuly.");
                    progressBar.Value = 0;
                    break;
            }
        }

        private void chkAdvanced_CheckedChanged(object sender, EventArgs e)
        {
            txtQuality.Visible = !chkAdvanced.Checked;
            lblQuality.Visible = !chkAdvanced.Checked;
            lblQualityBest.Visible = !chkAdvanced.Checked;
            lblQualityWorst.Visible = !chkAdvanced.Checked;
            tbQuality.Visible = !chkAdvanced.Checked;
            lblFPS.Visible = chkAdvanced.Checked;
            numFps.Visible = chkAdvanced.Checked;
            lblVideoBitRate.Visible = chkAdvanced.Checked;
            txtVideoBitrate.Visible = chkAdvanced.Checked;
            lblKbps.Visible = chkAdvanced.Checked;
        }

        private void chkSplit_CheckedChanged(object sender, EventArgs e)
        {
            numLimit.Enabled = chkSplit.Checked;
        }


    }
}
