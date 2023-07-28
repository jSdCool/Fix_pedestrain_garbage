using ColossalFramework.Plugins;
using UE = UnityEngine;

namespace Fix_pedestrain_garbage
{
    internal static class Debug
    {
        public const bool Enabled = false;

        private const string Prefix = "[Fix Pedestrian Garbage] ";

        public static void Log(string str)
        {
            var message = Prefix + str;
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, message);
            UE.Debug.Log(message);
        }

        public static void LogWarning(string str)
        {
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Warning, Prefix + str);
            UE.Debug.LogWarning(str);
        }

        public static void LogError(string str)
        {
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Error, Prefix + str);
            UE.Debug.LogError(str);
        }
    }
}
