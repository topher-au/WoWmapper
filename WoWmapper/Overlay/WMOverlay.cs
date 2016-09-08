using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WoWmapper.Properties;

namespace WoWmapper.Overlay
{
    public class WMOverlay
    {
        private OverlayWindow _overlay;
        public bool IsRunning => _overlay != null;
        public bool CrosshairVisible => _overlay.ImageCrosshair.IsVisible;

        public WMOverlay()
        {
            
        }

        public void Start()
        {
            if (IsRunning) return;

            Log.WriteLine("Creating overlay window...");
            _overlay = new OverlayWindow();
            SetAlignment((HorizontalAlignment)Settings.Default.NotificationH, (VerticalAlignment)Settings.Default.NotificationV);
        }

        public void Stop()
        {
            if (!IsRunning) return;

            Log.WriteLine("Closing overlay window...");
            _overlay.CloseOverlay();
            _overlay = null;
        }

        public void PopupNotification(OverlayNotification notification)
        {
            if (!IsRunning) return;

            Log.WriteLine($"Show notification: {notification.Header}: {notification.Content}");
            _overlay.PopupNotification(notification);
        }

        public void SetCrosshairState(bool visible, int x = 0, int y = 0)
        {
            if (!IsRunning) return;

            Log.WriteLine($"Set crosshair {visible}: {x}, {y}");
            _overlay.SetCrosshairState(visible, x, y);
        }

        public void SetAlignment(HorizontalAlignment h, VerticalAlignment v)
        {
            if (!IsRunning) return;

            Log.WriteLine($"Aligning notifications: {h}, {v}");
            _overlay.StackNotifications.HorizontalAlignment = h;
            _overlay.StackNotifications.VerticalAlignment = v;
        }
    }
}
