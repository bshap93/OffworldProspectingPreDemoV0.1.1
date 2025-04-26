// Copyright © Pixel Crushers. All rights reserved.

using System;
using CompassNavigatorPro;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrushers
{
    /// <summary>
    ///     Sends Message System messages and/or invokes UnityEvents when POI activity occurs.
    /// </summary>
    [AddComponentMenu("Pixel Crushers/Common/UnityEvents/Compass Navigator Pro/Compass Pro Event")]
    public class CompassProEvent : MonoBehaviour
    {
        public MessageEvents.MessageToSend onPOIVisitedMessage = new();
        public POIUnityEvent onPOIVisited = new();

        public MessageEvents.MessageToSend onPOIVisibleMessage = new();
        public POIUnityEvent onPOIVisible = new();

        public MessageEvents.MessageToSend onPOIHideMessage = new();
        public POIUnityEvent onPOIHide = new();

        private void Start()
        {
            ConnectEvents();
        }

        private void OnEnable()
        {
            ConnectEvents();
        }

        private void OnDisable()
        {
            DisconnectEvents();
        }

        public void ConnectEvents()
        {
            if (CompassPro.instance == null) return;
            DisconnectEvents();
            CompassPro.instance.OnPOIVisited.AddListener(OnPOIVisited);
            CompassPro.instance.OnPOIVisible.AddListener(OnPOIVisible);
            CompassPro.instance.OnPOIHide.AddListener(OnPOIHide);
        }

        public void DisconnectEvents()
        {
            if (CompassPro.instance == null) return;
            CompassPro.instance.OnPOIVisited.RemoveListener(OnPOIVisited);
            CompassPro.instance.OnPOIVisible.RemoveListener(OnPOIVisible);
            CompassPro.instance.OnPOIHide.RemoveListener(OnPOIHide);
        }

        private void OnPOIVisited(CompassProPOI poi)
        {
            onPOIVisited.Invoke(poi);
            SendToMessageSystem(onPOIVisitedMessage, poi);
        }

        private void OnPOIVisible(CompassProPOI poi)
        {
            onPOIVisible.Invoke(poi);
            SendToMessageSystem(onPOIVisibleMessage, poi);
        }

        private void OnPOIHide(CompassProPOI poi)
        {
            onPOIHide.Invoke(poi);
            SendToMessageSystem(onPOIHideMessage, poi);
        }

        private void SendToMessageSystem(MessageEvents.MessageToSend messageToSend, CompassProPOI poi)
        {
            if (messageToSend == null || StringField.IsNullOrEmpty(messageToSend.message)) return;
            if (messageToSend.target == null)
                MessageSystem.SendMessage(this, messageToSend.message, messageToSend.parameter, poi.name);
            else
                MessageSystem.SendMessageWithTarget(this, messageToSend.target, messageToSend.message,
                    messageToSend.parameter, poi.name);
        }

        [Serializable]
        public class POIUnityEvent : UnityEvent<CompassProPOI>
        {
        }
    }
}