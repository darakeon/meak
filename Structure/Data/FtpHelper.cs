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
            var fileExists = testEpisode();

            if (fileExists)
                return "File already exists";

            return upload();
        }



        private Boolean testEpisode()
        {
            var request = newRequest(WebRequestMethods.Ftp.DownloadFile);

            return testResponse(request);
        }

        private Boolean testResponse(FtpWebRequest request)
        {
            try
            {
                var fileExists = false;

                using (var response = (FtpWebResponse) request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        if (stream != null)
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                var content = reader.ReadToEnd();

                                if (content.EndsWith("</story>"))
                                {
                                    fileExists = true;
                                }
                            }
                        }
                    }
                }

                return fileExists;
            }
            catch (WebException)
            {
                return false;
            }

        }



        private String upload()
        {
            var request = newRequest(WebRequestMethods.Ftp.UploadFile);

	        var error = createDirectory();

	        if (!String.IsNullOrEmpty(error))
		        return error;

            copyContent(request);

            return getResponse(request);
        }



	    private FtpWebRequest newRequest(String method)
        {
            var url = Paths.FtpFilePath(Config.FtpUrl, season, episode, scene);
            var request = (FtpWebRequest) WebRequest.Create(url);
            
            request.Method = method;
            request.Credentials = createCredentials();
            request.UsePassive = false;

            return request;
        }


	    private String createDirectory()
		{
			var url = Paths.FtpDirectoryPath(Config.FtpUrl, season, episode);
			var request = WebRequest.Create(url);
			request.Method = WebRequestMethods.Ftp.MakeDirectory;
			request.Credentials = createCredentials();

		    String error = null;

			using (var response = (FtpWebResponse)request.GetResponse())
			{
				if (response.StatusCode != FtpStatusCode.PathnameCreated)
				{
					error = response.StatusDescription;
				}
			}

		    return error;
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



        private static String getResponse(FtpWebRequest request)
        {
            String error = null;

            using (var response = (FtpWebResponse)request.GetResponse())
            {
                if (!response.StatusCode.IsIn(ftpSuccessCodes))
                {
                    error = response.StatusDescription;
                }

                response.Close();
            }

            return error;
        }



	    private NetworkCredential createCredentials()
		{
			return new NetworkCredential(Config.FtpLogin, password);
		}

		private static FtpStatusCode[] ftpSuccessCodes
		{
			get
			{
				return new[] { FtpStatusCode.ClosingData, FtpStatusCode.CommandOK, FtpStatusCode.FileActionOK };
			}
		}



    }
}