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
        public static readonly string ID_START_TAG = "<PID>";
        public static readonly string ID_END_TAG = "</PID>";
        public static readonly string MESSAGE_START_TAG = "<MID>";
        public static readonly string MESSAGE_END_TAG = "</MID>";
        public static readonly int MAX_ONE_CHUNK_SIZE = 254 * 1024; //byte cinsinden;
        public static readonly int CHUNK_SIZE = 16 * 1024; // for multi sending.
        public static readonly ulong DC_SAFE_BUFFER_SIZE = 15 * 1024 * 1024; // DC buffer size aslı 16 mb
    }
}
