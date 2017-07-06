using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
			var request = newRequest(sceneUrl, WebRequestMethods.Ftp.DownloadFile);

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
            catch (WebException e)
            {
                return false;
            }

        }



        private String upload()
        {
	        var error = createDirectories();

	        if (!String.IsNullOrEmpty(error))
		        return error;

			return createEpisode();
        }



		private String createDirectories()
		{
			var error = createDirectoryIfNotExists(Config.FtpUrl, seasonUrl);

			if (!String.IsNullOrEmpty(error))
				return error;

			return createDirectoryIfNotExists(seasonUrl, episodeUrl);
		}

		private String createDirectoryIfNotExists(String parentUrl, String directoryUrl)
		{
			String error = null;

			var directories = getChildren(parentUrl);

			if (directories == null)
				return "Cannot read directory";

			var directoryName = directoryUrl.Substring(Config.FtpUrl.Length);

			if (!directories.Contains(directoryName))
				error = createDirectory(directoryUrl);

			return error;
		}

	    private IEnumerable<String> getChildren(String url)
	    {
			var request = newRequest(url, WebRequestMethods.Ftp.ListDirectory);

			String[] directories;

			using (var response = (FtpWebResponse)request.GetResponse())
			{
				using (var stream = response.GetResponseStream())
				{
					if (stream == null)
					{
						directories = null;
					}
					else
					{
						using (var reader = new StreamReader(stream))
						{
							directories = reader.ReadToEnd().Split(Environment.NewLine.ToCharArray());
						}
					}
				}

				response.Close();
			}

		    return directories;
	    }

	    private String createDirectory(String url)
	    {
			var request = newRequest(url, WebRequestMethods.Ftp.MakeDirectory);
			return handleResponse(request, FtpStatusCode.PathnameCreated);
	    }


	    private string createEpisode()
		{
			var request = newRequest(sceneUrl, WebRequestMethods.Ftp.UploadFile);

			copyEpisodeContent(request);

			return handleResponse(request, ftpSuccessCodes);
		}
		
		private void copyEpisodeContent(FtpWebRequest request)
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



		private string seasonUrl
		{
			get { return Paths.FtpDirectoryPath(Config.FtpUrl, season); }
		}

		private string episodeUrl
		{
			get { return Paths.FtpDirectoryPath(Config.FtpUrl, season, episode); }
		}

		private string sceneUrl
		{
			get { return Paths.FtpFilePath(Config.FtpUrl, season, episode, scene); }
		}

		private static FtpStatusCode[] ftpSuccessCodes
		{
			get { return new[] { FtpStatusCode.ClosingData, FtpStatusCode.CommandOK, FtpStatusCode.FileActionOK }; }
		}
		
		
		
		private FtpWebRequest newRequest(String url, String method)
        {
            var request = (FtpWebRequest) WebRequest.Create(url);
            
            request.Method = method;
            request.Credentials = createCredentials();
			request.UsePassive = false;

            return request;
        }

	    private NetworkCredential createCredentials()
		{
			return new NetworkCredential(Config.FtpAddress + "|" + Config.FtpLogin, password);
		}

		private static String handleResponse(FtpWebRequest request, params FtpStatusCode[] rightAnswers)
		{
			String error = null;

			using (var response = (FtpWebResponse)request.GetResponse())
			{
				if (!response.StatusCode.IsIn(rightAnswers))
				{
					error = response.StatusDescription;
				}

				response.Close();
			}

			return error;
		}


    }
}