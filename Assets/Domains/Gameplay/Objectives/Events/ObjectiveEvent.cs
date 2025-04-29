using MoreMountains.Tools;

namespace Domains.Gameplay.Objectives
{
    public struct ObjectiveEvent
    {
        private static ObjectiveEvent _e;


        public static void Trigger(string objectiveName, string objectiveType, string objectiveDescription)
        {
            MMEventManager.TriggerEvent(_e);
        }
    }
}