using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using WoWmapper;

internal static class UsbNotifier
{
    
    private const int DbtDevtypDeviceinterface = 5;

    private static readonly Guid GuidDevinterfaceUsbDevice = new Guid("A5DCBF10-6530-11D2-901F-00C04FB951ED"); // USB devices

    private static IntPtr _notificationHandle;
    private static NotificationWindow _notificationWindow;

    public delegate void DeviceChangeHandler();
    public static event DeviceChangeHandler DeviceConnected;
    public static event DeviceChangeHandler DeviceDisconnected;

    public static void Register()
    {
        if (_notificationWindow != null) return;

        _notificationWindow = new NotificationWindow();
        _notificationWindow.DeviceConnected += NotificationDeviceConnected;
        _notificationWindow.DeviceDisconnected += NotificationDeviceDisconnected;
        _notificationWindow.Show();

        DevBroadcastDeviceinterface dbi = new DevBroadcastDeviceinterface
        {
            DeviceType = DbtDevtypDeviceinterface,
            Reserved = 0,
            ClassGuid = GuidDevinterfaceUsbDevice,
            Name = 0
        };

        dbi.Size = Marshal.SizeOf(dbi);
        IntPtr buffer = Marshal.AllocHGlobal(dbi.Size);
        Marshal.StructureToPtr(dbi, buffer, true);

        _notificationHandle = RegisterDeviceNotification(new WindowInteropHelper(_notificationWindow).Handle, buffer, 0);
        Log.WriteLine($"Registered for USB events with handle [{_notificationHandle}]");
    }

    private static async void NotificationDeviceConnected()
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1500));
        DeviceConnected?.Invoke();
    }

    private static async void NotificationDeviceDisconnected()
    {
        await Task.Delay(TimeSpan.FromMilliseconds(1500));
        DeviceDisconnected?.Invoke();
    }

    public static void Unregister()
    {
        if (_notificationWindow == null) return;
        UnregisterDeviceNotification(_notificationHandle);

        _notificationWindow.DeviceDisconnected -= NotificationDeviceDisconnected;
        _notificationWindow.Close();
        _notificationWindow = null;
        Log.WriteLine($"Unregistered for USB events");
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr RegisterDeviceNotification(IntPtr recipient, IntPtr notificationFilter, int flags);

    [DllImport("user32.dll")]
    private static extern bool UnregisterDeviceNotification(IntPtr handle);

    [StructLayout(LayoutKind.Sequential)]
    private struct DevBroadcastDeviceinterface
    {
        internal int Size;
        internal int DeviceType;
        internal int Reserved;
        internal Guid ClassGuid;
        internal short Name;
    }

    private class NotificationWindow : Window
    {
        private const int DBT_DEVICEARRIVAL = 0x8000;
        private const int DBT_DEVICEREMOVALCOMPLETE = 0x8004;
        private const int WM_DEVICECHANGE = 0x0219;
        private readonly IntPtr HWND_MESSAGE = new IntPtr(-3);

        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        public delegate void DeviceChangeHandler();
        public event DeviceChangeHandler DeviceConnected;
        public event DeviceChangeHandler DeviceDisconnected;

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
            source.AddHook(WndProc);
            SetParent(new WindowInteropHelper(this).Handle, HWND_MESSAGE);
        }
        
        private IntPtr WndProc(IntPtr hWnd, int message, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (message == WM_DEVICECHANGE)
            {
                switch ((int)wParam)
                {
                    case DBT_DEVICEARRIVAL:
                        DeviceConnected?.Invoke();
                        break;
                    case DBT_DEVICEREMOVALCOMPLETE:
                        DeviceDisconnected?.Invoke();
                        break;
                }
            }

            return IntPtr.Zero;
        }
    }
}