using Microsoft.Xbox.Ambassadors.API;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Xbox.Ambassadors.Auth;

namespace Xbox.Ambassadors.ViewModels
{
    public class HandbookViewModel : ViewModelBase
    {
        public HandbookViewModel(FrameworkElement parent) : base(parent)
        {
            RegisterTimeTrigger(UpdateHandbook, new TimeSpan(5, 0, 0));
            UpdateViewModel();
        }

        bool _isHandbookLoading;
        bool _handbookLoadFailed;
        Document _document;
        HandbookSection _selectedItem;

        public bool IsHandbookLoading
        {
            get => _isHandbookLoading;
            set
            {
                _isHandbookLoading = value;
                RaisePropertyChanged(nameof(IsHandbookLoading));
            }
        }

        public bool HandbookLoadFailed
        {
            get => _handbookLoadFailed;
            set
            {
                _handbookLoadFailed = value;
                RaisePropertyChanged(nameof(HandbookLoadFailed));
            }
        }

        public Document Handbook
        {
            get => _document;
            set
            {
                _document = value;
                RaisePropertyChanged(nameof(Handbook));
            }
        }

        public HandbookSection SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                RaisePropertyChanged(nameof(SelectedItem));
            }
        }

        private async void UpdateHandbook()
        {
            try
            {
                IsHandbookLoading = true;
                HandbookLoadFailed = false;

                var s = await SiteContent.GetAsync(await AccessToken.LoadFromVaultOrGetNew(), SiteContent.HANDBOOK);

                Document doc = new Document();
                doc.HandbookSections = new List<HandbookSection>();

                foreach(var item in s.Documents[0].Document.HandbookSections)
                {
                    //doc.HandbookSections.Add(item);
                    foreach(var item2 in item.SubSections)
                    {
                        doc.HandbookSections.Add(item2);
                    }
                }


                Handbook = doc;
                SelectedItem = Handbook.HandbookSections[0];

                IsHandbookLoading = false;
            }
            catch
            {
                IsHandbookLoading = false;
                HandbookLoadFailed = true;
            }
        }

        public override void UpdateViewModel()
        {
            UpdateHandbook();
        }

        private string htmlBody = null;
        public async Task<string> ToHtml(IList<SectionContent> contents)
        {
            if (htmlBody == null)
            {
                var storageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Html/Handbook.html"));
                Stream fileStream = await storageFile.OpenStreamForReadAsync();

                using (StreamReader sr = new StreamReader(fileStream))
                {
                    htmlBody = await sr.ReadToEndAsync();
                }
            }

            string r = "";
            foreach (SectionContent c in contents)
            {
                if (c.ListType != null)
                {
                    r += "<ul>";
                    foreach (var i in c.ListItemsArray)
                    {
                        r += $"<li>{i.ListItem}</li>";
                    }
                    r += "</ul>";
                }
                else
                {
                    r += $"<div>{c?.ParagraphSection?.ParagraphContent}</div>";
                }
            }

            return htmlBody.Replace("{0}", r);

        }
    }
}
