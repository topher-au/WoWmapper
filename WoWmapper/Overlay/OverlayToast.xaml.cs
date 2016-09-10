using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WoWmapper.Overlay
{
    /// <summary>
    /// Interaction logic for OverlayToast.xaml
    /// </summary>
    public partial class OverlayToast : UserControl
    {
        private const double PopupOpacity = 1;
        private readonly int _duration;
        private readonly int _fadeIn;
        private readonly int _fadeOut;
        public OverlayNotification BaseNotification { get; private set; }
        public event EventHandler NotificationCompleted;

        public OverlayToast(OverlayNotification notification)
        {
            BaseNotification = notification;
            _duration = notification.Duration;
            _fadeIn = notification.FadeIn;
            _fadeOut = notification.FadeOut;

            InitializeComponent();
            if (notification.Image != null)
            {
                ImageIcon.Source = notification.Image;
            }
            else
            {
                ImageIcon.Source = OverlayIcons.Random();
            }
            
            NotificationHeader.Text = notification.Header;
            NotificationText.Text = notification.Content;
            UpdateLayout();
            Dispatcher.Invoke(FadeIn);
            new Thread(() =>
            {
                Thread.Sleep(_duration);
                try
                {
                    Dispatcher.Invoke(FadeOut);
                } catch { }
            }).Start();
        }

        public void FadeIn()
        {
            ImageBackground.Height = NotificationText.Height;
            var fadeIn = new DoubleAnimation(0, PopupOpacity, TimeSpan.FromMilliseconds(_fadeIn));
            fadeIn.Completed += (sender, args) =>
            {
                this.BeginAnimation(OpacityProperty, null);
                Opacity = PopupOpacity;
            };

            this.BeginAnimation(OpacityProperty, fadeIn);
        }

        public void FadeOut()
        {
            var fadeOut = new DoubleAnimation(PopupOpacity, 0, TimeSpan.FromMilliseconds(_fadeOut));
            fadeOut.Completed += (sender, args) => { Visibility = Visibility.Collapsed; NotificationCompleted?.Invoke(this, null);};

            this.BeginAnimation(OpacityProperty, fadeOut);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            var finalSize = NotificationText.ActualHeight + NotificationText.Margin.Top + 10;
            ImageBackground.Height = finalSize;
            var marg = ImageBackgroundBottom.Margin;
            marg.Top = finalSize;
            ImageBackgroundBottom.Margin = marg;
            base.OnRender(drawingContext);
        }

        private void NotificationText_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
        }
    }
}
