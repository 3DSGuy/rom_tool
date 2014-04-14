using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Windows.Forms;

namespace _3DSExplorer
{
    class YouTube
    {
        private WebClient _webClient;
        
        public delegate void ProcessFinished(string errorString);
        public delegate void ImageReady(Image image);

        private readonly ProcessFinished _processFinished;
        private readonly ImageReady _imageReady;

        private string _errorMessage;

        public YouTube(ProcessFinished processFinished, ImageReady imageReady)
        {
            _processFinished = processFinished;
            _imageReady = imageReady;
            _webClient = new WebClient {Proxy = null}; //Make connection smooth
            _webClient.DownloadDataCompleted += WebClientDownloadDataCompleted;
        }

        private void WebClientDownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            var ms = new MemoryStream(e.Result);
            try
            {
                var image = Image.FromStream(ms);
                ms.Close();
                _imageReady(image);
            }
            catch
            {
                _imageReady(null);
            }
        }

        #region Private methods

        #region Background Worker methods

        private void BackgroundWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _processFinished(_errorMessage);
        }

        private void BackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            var url = string.Format("http://www.youtube.com/watch?v={0}", e.Argument);
            _errorMessage = string.Empty;
            var request = (HttpWebRequest)WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();
            var responseStream = response.GetResponseStream();
            if (responseStream == null)
            {
                _errorMessage = "Error while reading response from YouTube.";
                return;
            }
            var source = new StreamReader(responseStream, Encoding.UTF8).ReadToEnd();
            
            var found = source.IndexOf("x-flv");
            while (!source.Substring(found, 4).Equals("http"))
                found--;
            source = source.Remove(0, found);
            source = HttpUtility.UrlDecode(source);
            source = HttpUtility.UrlDecode(source); //Twice
            _errorMessage = source.Substring(0,source.IndexOf("&quality"));
        }
        /*
        private void BackgroundWorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _progressChanged(e.ProgressPercentage,(int)e.UserState);
        }*/

        #endregion

        #endregion

        public void GetVideoURL(string videoId)
        {
            var backgroundWorker = new BackgroundWorker {WorkerReportsProgress = true};
            //backgroundWorker.ProgressChanged += BackgroundWorkerProgressChanged;
            backgroundWorker.DoWork += BackgroundWorkerDoWork;
            backgroundWorker.RunWorkerCompleted += BackgroundWorkerRunWorkerCompleted;
            backgroundWorker.RunWorkerAsync(videoId);
        }

        public void DownloadThumbnail(string videoId)
        {
            try
            {
                var thumbImage = string.Format("http://i1.ytimg.com/vi/{0}/0.jpg", videoId);
                _webClient.DownloadDataAsync(new Uri(thumbImage));
            }
            catch
            {
                _imageReady(null);
            }
        }
    }
}
