using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace Xbox.Ambassadors.Controls
{
    [TemplatePart(Name = "PART_LBUTTON", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "PART_RBUTTON", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "PART_SCROLLVIEWER", Type = typeof(ScrollViewer))]
    [TemplateVisualState(GroupName = "CommonStates", Name = "Normal")]
    [TemplateVisualState(GroupName = "CommonStates", Name = "PointerOver")]
    public class ItemsFlipView : ListView
    {
        public ItemsFlipView()
        {
            DefaultStyleKey = typeof(ItemsFlipView);
        }

        protected RepeatButton PART_LBUTTON;
        protected RepeatButton PART_RBUTTON;
        protected ScrollViewer PART_SCROLLVIEWER;
        private int CurrentIndex = 0;

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_LBUTTON = GetTemplateChild("PART_LBUTTON") as RepeatButton;
            PART_RBUTTON = GetTemplateChild("PART_RBUTTON") as RepeatButton;
            PART_SCROLLVIEWER = GetTemplateChild("PART_SCROLLVIEWER") as ScrollViewer;

            if (PART_LBUTTON != null)
            {
                PART_LBUTTON.Click += LBUTTON_Click;
            }

            if (PART_RBUTTON != null)
            {
                PART_RBUTTON.Click += RBUTTON_Click;
            }

            GoToState("Normal");
        }

        protected override void OnPointerEntered(PointerRoutedEventArgs e)
        {
            base.OnPointerEntered(e);

            if (PART_SCROLLVIEWER?.ScrollableWidth > 0)
                GoToState("PointerOver");
        }

        protected override void OnPointerExited(PointerRoutedEventArgs e)
        {
            base.OnPointerExited(e);

            GoToState("Normal");
        }


        private void RBUTTON_Click(object sender, RoutedEventArgs e)
        {
            if (Items.Count > 0)
            {
                double w = 0;
                while (w < ActualWidth && CurrentIndex < Items.Count - 1)
                {
                    var c = (FrameworkElement)ContainerFromIndex(CurrentIndex);

                    if (c.ActualWidth + w > ActualWidth)
                        break;

                    w += c.ActualWidth;
                    CurrentIndex++;
                }
                PART_SCROLLVIEWER.ChangeView(
                    Math.Min(PART_SCROLLVIEWER.ScrollableWidth, PART_SCROLLVIEWER.HorizontalOffset + w),
                    null, null);


            }



        }

        private void LBUTTON_Click(object sender, RoutedEventArgs e)
        {
            if (Items.Count > 0)
            {
                double w = 0;
                while (w < ActualWidth && CurrentIndex > 0)
                {
                    var c = (FrameworkElement)ContainerFromIndex(CurrentIndex);

                    if (c.ActualWidth + w > ActualWidth)
                        break;

                    w += c.ActualWidth;
                    CurrentIndex--;
                }
                PART_SCROLLVIEWER.ChangeView(Math.Max
                    (0, PART_SCROLLVIEWER.HorizontalOffset - w), null, null);


            }
        }

        private void GoToState(string name)
        {
            VisualStateManager.GoToState(this, name, true);
        }
    }
}
