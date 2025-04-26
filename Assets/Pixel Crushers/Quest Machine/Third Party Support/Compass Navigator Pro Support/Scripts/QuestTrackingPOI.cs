// Copyright © Pixel Crushers. All rights reserved.

using UnityEngine;

namespace PixelCrushers.QuestMachine
{

    /// <summary>
    /// Ties a POI to a quest. The POI is only visible when the quest is being tracked.
    /// </summary>
    [RequireComponent(typeof(CompassNavigatorPro.CompassProPOI))]
    public class QuestTrackingPOI : MonoBehaviour, IMessageHandler
    {
        [HelpBox("Sets visibility of this POI only when specified quest is being tracked.", HelpBoxMessageType.None)]
        public Quest quest;
        public StringField questID;

        [HelpBox("If not blank, specified quest node must be active. If blank, only main quest needs to be active.", HelpBoxMessageType.None)]
        public StringField requiredActiveQuestNodeID;

        [Tooltip("If not blank, check the quest state on this quester.")]
        public StringField questerID;

        protected CompassNavigatorPro.CompassProPOI m_poi;

        protected virtual void Awake()
        {
            m_poi = GetComponent<CompassNavigatorPro.CompassProPOI>();
            m_poi.enabled = false;
        }

        protected virtual void OnEnable()
        {
            var id = (quest != null) ? quest.id : questID;
            MessageSystem.AddListener(this, QuestMachineMessages.QuestTrackToggleChangedMessage, id);
            MessageSystem.AddListener(this, QuestMachineMessages.QuestStateChangedMessage, id);
        }

        protected virtual void OnDisable()
        {
            MessageSystem.RemoveListener(this);
        }

        protected virtual string GetQuestIDString()
        {
            return StringField.GetStringValue(quest != null ? quest.id : questID);
        }

        public void OnMessage(MessageArgs messageArgs)
        {
            switch (messageArgs.message)
            {
                case QuestMachineMessages.QuestTrackToggleChangedMessage:
                    m_poi.enabled = (bool)messageArgs.firstValue && AreTrackConditionsMet();
                    break;
                case QuestMachineMessages.QuestStateChangedMessage:
                    var questState = QuestMachine.GetQuestState(GetQuestIDString(), StringField.GetStringValue(questerID));
                    var isQuestActive = questState == QuestState.Active;
                    m_poi.enabled = isQuestActive && AreTrackConditionsMet();
                    break;
            }
        }

        protected virtual bool AreTrackConditionsMet()
        {
            return (StringField.IsNullOrEmpty(requiredActiveQuestNodeID) ||
                (QuestMachine.GetQuestNodeState(GetQuestIDString(), StringField.GetStringValue(requiredActiveQuestNodeID)) == QuestNodeState.Active));
        }
    }
}
