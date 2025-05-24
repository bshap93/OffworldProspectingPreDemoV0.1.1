using System;
using JetBrains.Annotations;
using MoreMountains.Tools;
using PixelCrushers.QuestMachine;

namespace Domains.Gameplay.Objectives.Events
{
    [Serializable]
    public enum QuestEventType
    {
        Default,
    }
    
    public enum ValueType
    {
        None,
        Int,
        String,
    }
    [Serializable]
    public struct QuestControlEvent
    {
        private static QuestControlEvent _e;
        public QuestEventType Type;

        public string Message;
        public string Parameter;

        public ValueType ValueType;
        public int? IntValue;
        [CanBeNull] public string StringValue;

        public static void Trigger(QuestEventType type, string message, string parameter,
            ValueType valueType = ValueType.None, int? intValue = null, [CanBeNull] string stringValue = null)
        {
            _e.Type = type;
            _e.Message = message;
            _e.Parameter = parameter;
            _e.ValueType = valueType;
            _e.IntValue = intValue;
            _e.StringValue = stringValue;

            MMEventManager.TriggerEvent(_e);
        }
        



    }
}