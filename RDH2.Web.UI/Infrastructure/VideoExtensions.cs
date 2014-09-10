using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;
using RDH2.Web.DataModel.Model;

namespace RDH2.Web.UI.Infrastructure
{
    public class VideoExtensions
    {
        #region Constants
        private const String _youTubeVideoFeedUrlKey = "youTubeVideoFeedUrl";
        private const String _entry = "{http://www.w3.org/2005/Atom}entry";
        private const String _id = "{http://www.w3.org/2005/Atom}id";
        private const String _title = "{http://www.w3.org/2005/Atom}title";
        private const String _published = "{http://www.w3.org/2005/Atom}published";
        #endregion


        #region Video Update Method
        /// <summary>
        /// UpdateVideos is called to update the Videos on the YouTube
        /// site to the RDH2.COM database.
        /// </summary>
        public static void UpdateVideos()
        {
            //Create a repository for Videos
            Repository<Video> videos = new Repository<Video>();
            Repository<Account> accounts = new Repository<Account>(videos.Context);
            Repository<Gallery> galleries = new Repository<Gallery>(videos.Context);

            //Get the URL from the appSettings
            String url = ConfigurationManager.AppSettings[VideoExtensions._youTubeVideoFeedUrlKey];

            //Get the Account for Ildiko
            Account ildiko = accounts
                .GetBy(a => a.FirstName == "Ildiko")
                .FirstOrDefault();

            //If the URL is retrieved successfully, get the feed
            if (String.IsNullOrEmpty(url) == false)
            {
                //Try to get the data from the Web
                try
                {
                    //Setup the default gallery for the Video
                    Gallery gallery = galleries
                        .GetBy(g => g.ID == -1)
                        .FirstOrDefault();

                    //Setup variables to get videos until they have
                    //all been retrieved
                    Boolean getMoreVideos = true;
                    Int32 videoCount = 1;

                    //Get the videos until there are no more to get
                    while (getMoreVideos == true)
                    {
                        //Format the URL into an API string
                        String apiUrl = String.Format("{0}?start-index={1}&max-results=50", url, videoCount);

                        //Load the XML into a LINQToXML class
                        XDocument doc = XDocument.Load(apiUrl);

                        //Get the list of Video entry Nodes
                        IEnumerable<XElement> entries = doc.Descendants(VideoExtensions._entry);

                        //If there are no videos in this feed, set the Boolean
                        //not to get any more
                        if (entries.Count() == 0)
                        {
                            getMoreVideos = false;
                        }

                        //Pull the Video entry Nodes out of the feed
                        foreach (XElement e in entries)
                        {
                            //Get the ID from the Element
                            String id = e.Element(VideoExtensions._id).Value;
                            String[] parts = id.Split('/');
                            String videoID = parts[parts.Length - 1];

                            //Get the Video from the database
                            Video video = videos
                                .GetBy(v => v.VideoID == videoID)
                                .FirstOrDefault();

                            //If the Video ID isn't found, add a new one
                            if (video == null)
                            {
                                //Create the new Video object
                                Video newVideo = new Video
                                {
                                    VideoID = videoID,
                                    Title = e.Element(VideoExtensions._title).Value,
                                    Created = Convert.ToDateTime(e.Element(VideoExtensions._published).Value),
                                    CreatedBy = ildiko,
                                    Gallery = gallery
                                };

                                //Save it to the database
                                videos.Add(newVideo);
                            }

                            //Increment the video count
                            videoCount++;
                        }
                    }
                }
                catch { }
            }
        }
        #endregion
    }
}