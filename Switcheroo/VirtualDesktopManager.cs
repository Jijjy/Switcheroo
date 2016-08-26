using Switcheroo.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Switcheroo
{
    public static class VirtualDesktopManager
    {
        private static Dictionary<IntPtr, int> WindowDesktopMap = new Dictionary<IntPtr, int>();

        private static int _activeDesktopId;
        public static int ActiveDesktopId
        {
            get { return _activeDesktopId; }
            set
            {
                if (_activeDesktopId == value) return;
                _activeDesktopId = value;
                RefreshWindows();
            }
        }

        public static void MoveWindow(AppWindow appWindow, int toDesktopId)
        {
            int fromDesktopId;
            if (WindowDesktopMap.TryGetValue(appWindow.HWnd, out fromDesktopId))
            {
                if (fromDesktopId == toDesktopId)
                {
                    return;
                }
            }

            WindowDesktopMap[appWindow.HWnd] = toDesktopId;
            appWindow.Hidden = (toDesktopId == ActiveDesktopId);
        }

        private static void RefreshWindows() {
            foreach(var appWindow in AppWindow.AllAltTabWindows)
            {
                int desktopId;
                if (WindowDesktopMap.TryGetValue(appWindow.HWnd, out desktopId))
                {
                    appWindow.Hidden = (desktopId == ActiveDesktopId);
                    continue;
                }

                WindowDesktopMap[appWindow.HWnd] = ActiveDesktopId;
                appWindow.Hidden = false;
            }
        }

    }
}
