using Domains.UI_Global.Events;
using MoreMountains.Tools;
using UnityEngine;

namespace Domains.UI_Global.Interface
{
    public abstract class UIController : MonoBehaviour, MMEventListener<UIEvent>
    {
        protected bool IsPaused;
        
        public UIController(bool isPaused)
        {
            IsPaused = isPaused;
        }
        
        protected void OnEnable()
        {
            this.MMEventStartListening();
        }
        
        protected void OnDisable()
        {
            this.MMEventStopListening();
        }
        
        
        
        
        public abstract void CloseUI();
        public abstract void OpenUI();
        public abstract void OnMMEvent(UIEvent eventType);
    }
}