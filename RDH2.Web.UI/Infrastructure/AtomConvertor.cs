using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml;
using RDH2.Web.DataModel.Model;

namespace RDH2.Web.UI.Infrastructure
{
    /// <summary>
    /// ListActivityExtensions contains extension methods for 
    /// a List of Activity objects so that it can be changed 
    /// into an Atom feed.
    /// </summary>
    public static class AtomConvertor
    {
        #region Constants
        private const String _feed = "feed";
        private const String _xmlns = "xmlns";
        private const String _feedXmlns = "http://www.w3.org/2005/Atom";
        private const String _title = "title";
        private const String _titleValue = "Herrick Family Blog";
        private const String _subtitle = "subtitle";
        private const String _subtitleValue = "Blog posts by Robert and Ildiko Herrick";
        private const String _rights = "rights";
        private const String _rightsValueFmt = "Copyright {0} Robert D. Herrick II";
        private const String _author = "author";
        private const String _name = "name";
        private const String _rdh2 = "Robert D. Herrick II";
        private const String _email = "email";
        private const String _rdh2Email = "rdherric@rdh2.com";
        private const String _link = "link";
        private const String _href = "href";
        private const String _linkSelf = "http://www.rdh2.com/Atom/";
        private const String _rel = "rel";
        private const String _self = "self";
        private const String _linkHome = "http://www.rdh2.com";
        private const String _id = "id";
        private const String _entry = "entry";
        private const String _updated = "updated";
        private const String _summary = "summary";
        private const String _type = "type";
        private const String _html = "html";
        #endregion


        #region ToFeed Method
        /// <summary>
        /// ToFeed translates a List of Activity objects to an 
        /// Atom feed for a reader to process.
        /// </summary>
        /// <param name="activities">The List of Activity objects to translate</param>
        /// <param name="urlHelper">The UrlHelper from the Page</param>
        /// <returns>String of Atom data</returns>
        public static String ToFeed(List<Activity> activities, UrlHelper urlHelper)
        {
            //Create the XmlDocument
            XmlDocument doc = new XmlDocument();
            doc.AppendChild(doc.CreateXmlDeclaration("1.0", "Windows-1252", String.Empty));

            //Create the Feed Element
            XmlElement feed = AtomConvertor.CreateFeedNode(doc);

            //Setup the Feed Properties
            AtomConvertor.CreateFeedPropertyNodes(doc, feed, activities, urlHelper);

            //Add each Activity to the Feed
            foreach (Activity activity in activities)
            {
                AtomConvertor.CreateEntryNode(doc, feed, activity, urlHelper);
            }

            //Add the Feed to the Document
            doc.AppendChild(feed);

            //Return the String of XML
            return doc.OuterXml;
        }
        #endregion


        #region XmlElement Methods
        /// <summary>
        /// CreateFeedNode creates the root feed node of the
        /// document.
        /// </summary>
        /// <param name="doc">The XmlDocument to create the element</param>
        /// <returns>XmlElement of feed data</returns>
        private static XmlElement CreateFeedNode(XmlDocument doc)
        {
            //Create a List of Attributes
            List<Tuple<String, String>> xmlns = new List<Tuple<String, String>>
                {
                    new Tuple<String, String>(AtomConvertor._xmlns, AtomConvertor._feedXmlns)
                };

            //Create the Node
            return AtomConvertor.CreateNode(doc, AtomConvertor._feed, xmlns, String.Empty);
        }


        /// <summary>
        /// CreateFeedPropertyNodes creates the Nodes that 
        /// contain the properties of the feed.
        /// </summary>
        /// <param name="doc">The XmlDocument that creates the nodes</param>
        /// <param name="feed">The feed element</param>
        /// <param name="activities">The list of Activities</param>
        private static void CreateFeedPropertyNodes(XmlDocument doc, XmlElement feed, List<Activity> activities, UrlHelper urlHelper)
        {
            //Add the title, subtitle, and copyrights
            feed.AppendChild(AtomConvertor.CreateNode(doc, AtomConvertor._title, null, AtomConvertor._titleValue));
            feed.AppendChild(AtomConvertor.CreateNode(doc, AtomConvertor._subtitle, null, AtomConvertor._subtitleValue));
            feed.AppendChild(AtomConvertor.CreateNode(doc, AtomConvertor._rights, null, 
                String.Format(AtomConvertor._rightsValueFmt, DateTime.Now.Year)));

            //Add the ID to the feed
            feed.AppendChild(AtomConvertor.CreateNode(doc, AtomConvertor._id, null, String.Format("{0}/", AtomConvertor._linkHome)));

            //Add the Author node to the feed
            feed.AppendChild(AtomConvertor.CreateAuthorNode(doc, AtomConvertor._rdh2, AtomConvertor._rdh2Email));

            //Add the self link to the Atom feed
            List<Tuple<String, String>> selfAttrs = new List<Tuple<String, String>>
                {
                    new Tuple<String, String>(AtomConvertor._href, urlHelper.RequestContext.HttpContext.Request.Url.AbsoluteUri),
                    new Tuple<String, String>(AtomConvertor._rel, AtomConvertor._self),
                };

            feed.AppendChild(AtomConvertor.CreateNode(doc, AtomConvertor._link, selfAttrs, String.Empty));

            //Add the RDH2.COM link to the Atom feed
            List<Tuple<String, String>> rdh2Attrs = new List<Tuple<String, String>>
                {
                    new Tuple<String, String>(AtomConvertor._href, String.Format("{0}/", AtomConvertor._linkHome)),
                };

            feed.AppendChild(AtomConvertor.CreateNode(doc, AtomConvertor._link, rdh2Attrs, String.Empty));

            //Add the Updated element
            DateTime updated = DateTime.MinValue;
            if (activities != null && activities.Count > 0)
            {
                updated = activities[0].ActivityDate.ToUniversalTime();
            }

            feed.AppendChild(AtomConvertor.CreateNode(doc, AtomConvertor._updated, null, 
                XmlConvert.ToString(updated, XmlDateTimeSerializationMode.Utc)));
        }


        /// <summary>
        /// CreateAuthorNode creates the node that contains the Author
        /// information.
        /// </summary>
        /// <param name="doc">The XmlDocument that creates the nodes</param>
        /// <returns>XmlElement of Author information</returns>
        private static XmlElement CreateAuthorNode(XmlDocument doc, String authorName, String authorEmail)
        {
            //Create the author node
            XmlElement rtn = doc.CreateElement(AtomConvertor._author);

            //Add the child nodes to the Author node
            rtn.AppendChild(AtomConvertor.CreateNode(doc, AtomConvertor._name, null, authorName));
            rtn.AppendChild(AtomConvertor.CreateNode(doc, AtomConvertor._email, null, authorEmail));

            //Return the result
            return rtn;
        }
        

        /// <summary>
        /// CreateEntryNode creates an entry node for the specified
        /// Activity and adds it to the feed node.
        /// </summary>
        /// <param name="doc">The XmlDocument to create the nodes</param>
        /// <param name="feed">The feed node to which to add the entry</param>
        /// <param name="activity">The Activity to add as an entry</param>
        private static void CreateEntryNode(XmlDocument doc, XmlElement feed, Activity activity, UrlHelper urlHelper)
        {
            //Create the Entry node
            XmlElement entry = doc.CreateElement(AtomConvertor._entry);

            //Add the title to the entry
            entry.AppendChild(AtomConvertor.CreateNode(doc, AtomConvertor._title, null, 
                (activity.ActivityType == ActivityType.Post ? activity.Title : activity.FormatTitle())));
            
            //Add the ID of the entry 
            entry.AppendChild(AtomConvertor.CreateNode(doc, AtomConvertor._id, null, 
                AtomConvertor._linkHome + activity.ToUrl(urlHelper)));

            //Add the Author of the entry
            entry.AppendChild(AtomConvertor.CreateAuthorNode(doc, 
                String.Format("{0} {1}", activity.CreatedBy.FirstName, activity.CreatedBy.LastName),
                AtomConvertor._rdh2Email));

            //Add the link to the Activity to the entry
            List<Tuple<String, String>> linkAttrs = new List<Tuple<String, String>>
            {
                new Tuple<String, String>(AtomConvertor._href, AtomConvertor._linkHome + activity.ToUrl(urlHelper))
            };

            entry.AppendChild(AtomConvertor.CreateNode(doc, AtomConvertor._link, linkAttrs, String.Empty));

            //Add the updated field to the entry
            entry.AppendChild(AtomConvertor.CreateNode(doc, AtomConvertor._updated, null,
                XmlConvert.ToString(activity.ActivityDate.ToUniversalTime(), XmlDateTimeSerializationMode.Utc)));

            //Add the summary to the feed
            List<Tuple<String, String>> summaryAttrs = new List<Tuple<String, String>>
            {
                new Tuple<String, String>(AtomConvertor._type, AtomConvertor._html)
            };

            entry.AppendChild(AtomConvertor.CreateNode(doc, AtomConvertor._summary, summaryAttrs, 
                (activity.ActivityType == ActivityType.Post ? activity.Summary : activity.ToPhotoUrls(urlHelper, AtomConvertor._linkHome))));

            //Add the entry to the feed
            feed.AppendChild(entry);
        }
        #endregion


        #region Helper Methods
        /// <summary>
        /// CreateTextNode creates a node with Text in it.
        /// </summary>
        /// <param name="doc">The XmlDocument used to create the node</param>
        /// <param name="name">The name of the node</param>
        /// <param name="attributes">The attributes of the node</param>
        /// <param name="text">The text of the node</param>
        /// <returns>XmlElement of text data</returns>
        private static XmlElement CreateNode(XmlDocument doc, String name, List<Tuple<String, String>> attributes, String text)
        {
            //Create the element
            XmlElement rtn = doc.CreateElement(name);

            //Set the Attributes on the node if there are any
            if (attributes != null)
            {
                foreach (Tuple<String, String> attribute in attributes)
                {
                    rtn.SetAttribute(attribute.Item1, attribute.Item2);
                }
            }

            //Set the value if there is one
            if (String.IsNullOrEmpty(text) == false)
            {
                rtn.InnerText = text;
            }

            //Return the result
            return rtn;
        }
        #endregion
    }
}