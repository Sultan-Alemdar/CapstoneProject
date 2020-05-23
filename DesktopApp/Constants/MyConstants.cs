using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopApp.ViewModels;

namespace DesktopApp.Constants
{
    public class MyConstants
    {
        public static readonly string OPERATIONS_VIEW_MODEL_FULL_NAME = (typeof(OperationsViewModel)).FullName;
        public static readonly string SETTINGS_VIEW_MODEL_FULL_NAME = (typeof(SettingsViewModel)).FullName;
        public static readonly string REMOTE_CONNECTION_VIEW_MODEL_FULL_NAME = (typeof(RemoteConnectionViewModel)).FullName;
        public static readonly string REMOTE_ADAPTER_VIEW_MODEL_FULL_NAME = (typeof(WebRTCAdapter.Adapters.AdapterViewModel)).FullName;
    }
}
