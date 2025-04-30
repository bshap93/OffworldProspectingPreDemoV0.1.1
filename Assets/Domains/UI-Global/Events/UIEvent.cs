using MoreMountains.Tools;

namespace Domains.UI_Global.Events
{
    public enum UIEventType
    {
        OpenVendorConsole,
        CloseVendorConsole,
        OpenFuelConsole,
        CloseFuelConsole,
        UpdateFuelConsole,
        CloseUI,
        OpenQuestDialogue,
        CloseQuestDialogue,
        OpenInfoDump,
        CloseInfoDump,
        OpenBriefing,
        CloseBriefing
    }

    public struct UIEvent
    {
        private static UIEvent _e;

        public UIEventType EventType;

        public static void Trigger(UIEventType eventType)
        {
            _e.EventType = eventType;

            MMEventManager.TriggerEvent(_e);
        }
    }
}