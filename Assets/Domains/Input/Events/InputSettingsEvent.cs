using MoreMountains.Tools;

namespace Domains.Input.Events
{
    public enum InputSettingsEventType
    {
        InvertYAxis
    }

    public struct InputSettingsEvent
    {
        private static InputSettingsEvent _e;

        public InputSettingsEventType EventType;

        public bool? BoolValue;
        public float? FloatValue;


        public static void Trigger(InputSettingsEventType inputSettingsEventType
            , bool? boolValue = null, float? floatValue = null)
        {
            _e.EventType = inputSettingsEventType;
            _e.BoolValue = boolValue;
            _e.FloatValue = floatValue;
            MMEventManager.TriggerEvent(_e);
        }
    }
}