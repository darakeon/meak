using System;
using System.IO;
using System.Net;
using Ak.Generic.Collection;
using Structure.Helpers;

namespace Structure.Data
{
    public class FtpHelper
    {
        public FtpHelper(String seasonID, String episodeID, String sceneID, String password)
        {
            season = seasonID;
            episode = episodeID;
            scene = sceneID;
            this.password = password;
        }

        private String season { get; set; }
        private String episode { get; set; }
        private String scene { get; set; }
        private String password { get; set; }

        public String Upload()
        {
            var request = newRequest();

            copyContent(request);

            String error = null;

            using (var response = (FtpWebResponse) request.GetResponse())
            {
                var success = new[]{ FtpStatusCode.ClosingData, FtpStatusCode.CommandOK, FtpStatusCode.FileActionOK };

                if (!response.StatusCode.IsIn(success))
                {
                    error = response.StatusDescription;
                }

                response.Close();
            }

            return error;
        }

        private FtpWebRequest newRequest()
        {
            var url = Paths.FtpFilePath(Config.FtpUrl, season, episode, scene);
            var request = (FtpWebRequest) WebRequest.Create(url);
            
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(Config.FtpLogin, password);
            request.UsePassive = false;

            return request;
        }

        private void copyContent(FtpWebRequest request)
        {
            var path = new EpisodeXML().PathXML;
            path = Paths.SceneFilePath(path, season, episode, scene);
            
            var fileContents = File.ReadAllBytes(path);
            request.ContentLength = fileContents.Length;

            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(fileContents, 0, fileContents.Length);
                requestStream.Close();
            }
        }
    }
}