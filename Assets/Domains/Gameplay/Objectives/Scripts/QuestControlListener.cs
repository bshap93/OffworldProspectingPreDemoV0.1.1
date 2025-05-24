using Domains.Gameplay.Objectives.Events;
using MoreMountains.Tools;
using UnityEngine;

namespace Domains.Gameplay.Objectives.Scripts
{
    public class QuestControlListener : MonoBehaviour, MMEventListener<QuestControlEvent>
    {
        public void OnMMEvent(QuestControlEvent eventType)
        {
            throw new System.NotImplementedException();
        }

        private void OnEnable()
        {
            this.MMEventStartListening<QuestControlEvent>();
        }

        private void OnDisable()
        {
            this.MMEventStopListening<QuestControlEvent>();
        }
    }
}
