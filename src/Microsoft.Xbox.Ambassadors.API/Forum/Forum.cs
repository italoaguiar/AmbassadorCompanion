using HtmlAgilityPack;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Microsoft.Xbox.Ambassadors.API.Forum
{
    public class Forum
    {
        public async Task<List<ForumThread>> GetThreadsAsync(int page = 1)
        {
            var doc = new HtmlDocument();

            using (System.Net.Http.HttpClient cli = new System.Net.Http.HttpClient())
            {
                var input = await cli.GetStringAsync(
                    new System.Uri($"https://answers.microsoft.com/en-us/forum/forumthreadlist?forumId=95c26fcb-807a-4619-8d45-dfec9c2c5684&sort=LastReplyDate&dir=Desc&tab=All&page={page}&threadType=All"));

                doc.LoadHtml(input);
            }

            var threads = doc.DocumentNode.SelectNodes("//*[contains(@class,'c-card')]");

            List<ForumThread> ths = new List<ForumThread>();


            LanguageDetection.LanguageDetector detector = new LanguageDetection.LanguageDetector();
            detector.AddAllLanguages();

            foreach (var t in threads)
            {
                ForumThread th = new ForumThread();
                var title = t.SelectNodes(".//*[contains(@data-bi-id,'thread-link')]");
                th.Title = Clean(title[0].InnerText);

                th.ThreadUri = new Uri(Clean(title[0].Attributes["href"].Value));

                th.Id = title[0].Attributes["data-bi-linkthreadid"].Value;

                var lr  = t.SelectNodes(".//*[contains(@data-bi-id,'thread-question-last-reply-link')]");
                var lrd = t.SelectNodes(".//*[contains(@data-bi-id,'thread-discussion-last-reply-link')]");
                th.LastReply = lr != null ? 
                    Clean(lr[0].InnerText) : 
                    lrd != null ?
                    Clean(lrd[0].InnerText) : 
                    string.Empty;

                var lra = t.SelectNodes(".//*[contains(@data-bi-id,'thread-question-user-last-reply-link')]");
                var lrad = t.SelectNodes(".//*[contains(@data-bi-id,'thread-discussion-user-last-reply-link')]");
                th.LastReplyAuthor = lra != null ?
                    Clean(lra[0].InnerText) : 
                    lrad != null ?
                    Clean(lrad[0].InnerText) : 
                    string.Empty;


                th.Date = Clean(t.SelectNodes(".//*[contains(@class,'text-nowrap')]").Last().InnerText);

                var a = t.SelectNodes(".//*[contains(@data-bi-id,'thread-question-user-start-link')]");
                var ad = t.SelectNodes(".//*[contains(@data-bi-id,'thread-discussion-user-start-link')]");
                th.Author = a != null ?
                    Clean(a[0].InnerText) :
                    ad != null ?
                    Clean(ad[0].InnerText) :
                    string.Empty;

                var desc = t.SelectNodes(".//*[contains(@class,'thread-summary c-paragraph-3 double-line-text')]")[0];
                th.Description = Clean(desc.InnerText);
                th.ShortDescription = th.Description;
                th.HtmlDescription = desc.InnerHtml;

                var stat = t.SelectNodes(".//*[contains(@class,'stat-value')]");
                th.TotalViews = stat[0].InnerText?.Replace("&nbsp;", "");
                th.TotalHelpful = stat[1].InnerText?.Replace("&nbsp;", "");
                th.TotalReplies = stat[2].InnerText?.Replace("&nbsp;", "");
                var ist = t.SelectNodes(".//*[contains(@class,'icon-label')]");
                th.TotalNeedAnswer = ist!= null ? ist[0].InnerText : null;
                th.IsLocked = ist != null ? ist.Any(x=> x.InnerHtml.ToLowerInvariant() == "locked") : false;

                string lang = th.Description;

                var i = lang.ToLower().IndexOf("translation");
                if (i > 0)
                {
                    var aux = lang.Substring(i);
                    
                    var l1 = detector.Detect(aux);

                    aux = lang.Substring(0, i);

                    var l2 = detector.Detect(aux);

                    th.Language = l1 != "en" ? l1 : l2;
                }
                else
                {
                    th.Language = detector.Detect(lang);
                }
                
                ths.Add(th); 
            }

            return ths;
        }

        Regex reg = new Regex(@"&[#\w\d]*;");
        private string Clean(string input)
        {
            return reg.Replace(WebUtility.HtmlDecode(input).Replace("\r","").Replace("\n",""), "").Trim();
        }

    }

    public class ForumThread:INotifyPropertyChanged
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string LastReply { get; set; }
        public string LastReplyAuthor { get; set; }
        public string Author { get; set; }

        private Uri _authorPicture;
        public Uri AuthorPicture
        {
            get { return _authorPicture; }
            set
            {
                _authorPicture = value;
                OnPropertyChanged("AuthorPicture");
            }
        }

        public string Date { get; set; }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged("Description");
            }
        }

        private string _shortDescription;
        public string ShortDescription
        {
            get { return _shortDescription; }
            set
            {
                _shortDescription = value;
                OnPropertyChanged("ShortDescription");
            }
        }

        private string _htmlDescription;
        public string HtmlDescription
        {
            get { return _htmlDescription; }
            set
            {
                _htmlDescription = value;
                OnPropertyChanged("HtmlDescription");
            }
        }

        public string TotalNeedAnswer { get; set; }
        public string TotalReplies { get; set; }
        public string TotalHelpful { get; set; }
        public string TotalViews { get; set; }
        public bool IsLocked { get; set; }
        public Uri ThreadUri { get; set; }
        public string Language { get; set; }
        public string AntiForgeryKey { get; set; }

        private List<ThreadPost> _posts;
        public List<ThreadPost> Posts
        {
            get { return _posts; }
            set
            {
                _posts = value;
                OnPropertyChanged("Posts");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public async void UpdateContent()
        {
            using (System.Net.Http.HttpClient cli = new System.Net.Http.HttpClient())
            {
                var input = await cli.GetStringAsync(ThreadUri);

                var doc = new HtmlDocument();
                doc.LoadHtml(input);

                var content = doc.DocumentNode.SelectNodes("//*[contains(@class,'thread-message-content-body-text thread-full-message')]");
                Description = WebUtility.HtmlDecode(content[0].InnerText.Trim());
                HtmlDescription = content[0].InnerHtml;

                var img = doc.DocumentNode.SelectNodes("//*[contains(@class,'profile-image hidden-rsp')]");
                var url = WebUtility.HtmlDecode(img[0].Attributes["src"].Value.Trim());
                AuthorPicture = new Uri(url);
            }
        }

        public async Task<List<ThreadPost>> GetPosts(int page = 1, int total = 1)
        {
            List<ThreadPost> posts = new List<ThreadPost>();

            using (System.Net.Http.HttpClient cli = new System.Net.Http.HttpClient())
            {
                var input = await cli.GetStringAsync(new System.Uri($"https://answers.microsoft.com/en-us/thread/viewthreadmessages?forum=xbanswers&threadId={Id}&pageNum={page}"));

                var doc = new HtmlDocument();
                doc.LoadHtml(input);

                if (total == 1)
                {
                    var pages = doc.DocumentNode.SelectNodes("//*[contains(@class,'pageNumberLayout')]");

                    if (pages != null)
                    {
                        bool r = int.TryParse(pages.LastOrDefault().InnerText, out total);
                    }
                }

                var pts = doc.DocumentNode.SelectNodes("//*[contains(@class,'thread-message thread-message-enus')]");
                if (pts != null)
                {
                    var p = ParseHtml(pts);
                    posts.AddRange(p);
                }

                if (page < total)
                {
                    var p = await GetPosts(++page, total);
                    posts.AddRange(p);
                }

                Posts = posts;

                return posts;
            }
        }

        private List<ThreadPost> ParseHtml(HtmlNodeCollection c)
        {
            List<ThreadPost> posts = new List<ThreadPost>();

            foreach(var message in c)
            {
                ThreadPost p = new ThreadPost();
                p.Id = message.Attributes["msgid"].Value;
                p.Author = message.SelectNodes(".//*[contains(@data-bi-id,'message-username-link')]")[0].InnerText.Trim();
                var date = message.SelectNodes(".//*[contains(@data-bi-id,'message-user-history-link')]");
                p.Date = DateTime.Parse(date[0].Attributes["data-createddate"].Value);

                var underline = message.SelectNodes(".//*[contains(@class,'noUnderline')]");
                p.AuthorUnderline = underline == null ? null : underline[0].InnerText.Trim();

                var pic = message.SelectNodes(".//*[contains(@class,'profile-image hidden-rsp')]");
                p.AuthorPicture = pic != null ? WebUtility.HtmlDecode(pic[0].Attributes["src"].Value) : string.Empty;

                var text = message.SelectNodes(".//*[contains(@class,'thread-message-content-body-text thread-full-message')]")[0];
                p.Text = text.InnerText.Trim();
                p.Text = WebUtility.HtmlDecode(p.Text);
                p.Html = text.InnerHtml;

                var signature = message.SelectNodes(".//*[contains(@class,'thread-message-content-body-signature')]");
                p.AuthorSignature = signature == null ? null : WebUtility.HtmlDecode(signature[0].InnerText.Trim());

                var abbrev = message.SelectNodes(".//*[contains(@data-bi-id,'message-username-image-link')]");
                p.AuthorAbbrev = abbrev != null ? abbrev[0].InnerText.Trim() : string.Empty;

                var up = message.SelectNodes(".//*[contains(@data-bi-id,'msgVoteBtn')]");
                p.IsAnswer = up == null;

                if (up != null)
                {
                    Regex r = new Regex(";(.*?)&");
                    p.UpVotes = int.Parse(r.Match(up[0].InnerText).Groups[1].ToString());
                }

                posts.Add(p);

            }


            return posts;
        }

        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }

    public class ThreadPost
    {
        public string Id { get; set; }
        public string Author { get; set; }
        public string UserId { get; set; }
        public string Text { get; set; }
        public string Html { get; set; }
        public DateTime Date { get; set; }
        public string AuthorSignature { get; set; }
        public string AuthorUnderline { get; set; }
        public string AuthorPicture { get; set; }
        public string AuthorAbbrev { get; set; }
        public int UpVotes { get; set; }
        public bool IsAnswer { get; set; }
    }
}
