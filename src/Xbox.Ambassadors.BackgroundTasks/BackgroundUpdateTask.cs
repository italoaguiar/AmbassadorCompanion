using System.Collections.Generic;
using Microsoft.Xbox.Ambassadors.API.Blog;
using System;
using System.Linq;
using Windows.ApplicationModel.Background;
using HtmlAgilityPack;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;

namespace Xbox.Ambassadors.BackgroundTasks
{
    public sealed class BackgroundUpdateTask : IBackgroundTask
    {
        BackgroundTaskDeferral _deferral;
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();

            //await ForumService.Service.UpdateAsync();
            ShowNewBlogPosts();

            _deferral.Complete();
        }

        public void ShowNewBlogPosts()
        {
            try
            {
                var postsTask = Blog.GetPostsAsync().AsAsyncOperation();
                var posts = postsTask.AsTask().Result;

                var lastPosts = AppSettings.LastPostsNotified ?? new List<int>();

                posts = posts.Where(x => x.DateGmt > DateTime.UtcNow.AddDays(-1) && !lastPosts.Contains(x.Id)).ToList();

                if (posts.Count > 0)
                {
                    foreach (var post in posts)
                    {
                        try
                        {
                            var doc = new HtmlDocument();
                            doc.LoadHtml(post.Title.Rendered);

                            string title = doc.DocumentNode.InnerText;
                            title = System.Web.HttpUtility.HtmlDecode(title);

                            doc.LoadHtml(post.Content.Rendered);
                            var content = doc.DocumentNode.InnerText;
                            content = Reduce(System.Web.HttpUtility.HtmlDecode(content), 110);

                            Uri req = new Uri($"{post.Link.Scheme}://{post.Link.Host}{post.JetpackFeaturedMediaUrl.OriginalString}");


                            Notify(title, content, req.AbsoluteUri, $"{post.Link.AbsoluteUri}?source=AmbassadorCompanionApp");

                        }
                        catch (Exception e)
                        {
                            System.Diagnostics.Debug.WriteLine(e);
                        }
                    }

                    foreach(var i in posts.Select(x => x.Id))
                        lastPosts.Add(i);

                    AppSettings.LastPostsNotified = lastPosts;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
        }


        public void Notify(string title, string message, string imageUrl, string launch)
        {
            var notification = $"<toast launch=\"{launch}\" activationType=\"Protocol\">" +
                                    "<visual>" +
                                        "<binding template=\"ToastGeneric\">" +
                                            $"<text hint-maxLines=\"1\">{title}</text>" +
                                            $"<text>{message}</text>" +
                                            $"<image placement=\"hero\" src=\"{imageUrl}\"/>" +
                                        "</binding>" +
                                    "</visual>" +
                                "</toast>";

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(notification);

            // Create the toast notification
            var toastNotif = new ToastNotification(xmlDoc);

            // And send the notification
            ToastNotificationManager.CreateToastNotifier().Show(toastNotif);
        }


        private string Reduce(string content, int len)
        {
            if (content == null || content.Length < len)
                return content;

            return  $"{content.Substring(0, len)}..."; ;
        }
    }
}
