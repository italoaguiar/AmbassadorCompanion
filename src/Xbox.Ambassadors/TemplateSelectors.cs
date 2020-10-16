using Microsoft.Xbox.Ambassadors.API;
using Microsoft.Xbox.Ambassadors.API.DataModel.Missions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Xbox.Ambassadors
{
    public class MissionTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DefaultTemplate { get; set; }
        public DataTemplate FeaturedTemplate { get; set; }

        private int index = 0;

        protected override DataTemplate SelectTemplateCore(object item)
        {
            if (index++ == 1)
                return FeaturedTemplate;
            else
                return DefaultTemplate;

        }
    }

    public class QuizzQuestionTemplateSelector : DataTemplateSelector
    {
        public DataTemplate MultiQuestion { get; set; }
        public DataTemplate SingleQuestion { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            Question q = (Question)item;

            switch (q.Type)
            {
                case TypeEnum.Multi:
                    return MultiQuestion;
                default:
                    return SingleQuestion;
            }
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            return SelectTemplateCore(item);
        }
    }

    public class MissionTypeTemplateSelector : DataTemplateSelector
    {
        public DataTemplate XpTemplate { get; set; }
        public DataTemplate SweepstakeTemplate { get; set; }

        public DataTemplate GameTemplate { get; set; }

        public DataTemplate DefaultTemplate { get; set; }
        protected override DataTemplate SelectTemplateCore(object item)
        {
            MissionReward r = item as MissionReward;
            if (r != null)
            {
                switch (r.RewardDefinition.RewardType)
                {
                    case 0:
                    default:
                        return XpTemplate;
                    case 3:
                        return SweepstakeTemplate;
                    case 6:
                        return GameTemplate;
                    case 7:
                        return DefaultTemplate;
                }
            }
            else
            {
                return DefaultTemplate;
            }

        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            return SelectTemplateCore(item);
        }
    }
}
