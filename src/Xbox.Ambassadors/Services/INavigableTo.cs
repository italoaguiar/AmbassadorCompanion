using Xbox.Ambassadors.ViewModels;

namespace Xbox.Ambassadors.Services
{
    public interface INavigableTo
    {
        void RefreshModel();
        ViewModelBase GetViewModel();
    }
}
