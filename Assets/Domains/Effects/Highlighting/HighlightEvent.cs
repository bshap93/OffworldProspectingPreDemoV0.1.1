using MoreMountains.Tools;

namespace Domains.Effects.Highlighting
{
    public enum HighlightEventType
    {
        ActivateTarget
    }

    public struct HighlightEvent
    {
        public static HighlightEvent _e;


        public HighlightEventType EventType;

        public static void Trigger(HighlightEventType eventType)
        {
            _e.EventType = eventType;
            MMEventManager.TriggerEvent(_e);
        }
    }
}