using Microsoft.Xbox.Ambassadors.API;
using Windows.UI.Xaml;

namespace Xbox.Ambassadors.ViewModels
{
    public class ToolboxViewModel : ViewModelBase
    {
        public ToolboxViewModel(FrameworkElement parent) : base(parent)
        {

        }

        static ErrorDetail[] errorList;
        bool isLoading;

        public ErrorDetail[] ErrorList
        {
            get => errorList;
            set
            {
                errorList = value;
                RaisePropertyChanged("ErrorList");
            }
        }

        public bool IsLoading
        {
            get => isLoading;
            set
            {
                isLoading = value;
                RaisePropertyChanged("IsLoading");
            }
        }

        public async void Search(string q)
        {
            try
            {
                IsLoading = true;

                ErrorList = await ErrorCode.GetAsync(q);

                IsLoading = false;
            }
            catch
            {
                IsLoading = false;
            }
        }
        public override void UpdateViewModel()
        {

        }
    }
}
