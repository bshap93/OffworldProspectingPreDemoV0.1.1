// Copyright © Pixel Crushers. All rights reserved.

using UnityEngine;
using System;

namespace PixelCrushers
{

    /// <summary>
    /// Saves the enabled (visible), canBeVisited, and isVisited states of
    /// a Compass Navigator Pro POI.
    /// </summary>
    [AddComponentMenu("Pixel Crushers/Save System/Savers/Compass Navigator Pro/POI Saver")]
    [RequireComponent(typeof(CompassNavigatorPro.CompassProPOI))]
    public class POISaver : Saver
    {

        [Serializable]
        public class SaveData
        {
            public bool enabled;
            public bool canBeVisited;
            public bool isVisited;
        }

        private CompassNavigatorPro.CompassProPOI m_poi = null;
        private CompassNavigatorPro.CompassProPOI poi
        {
            get
            {
                if (m_poi == null) m_poi = GetComponent<CompassNavigatorPro.CompassProPOI>();
                return m_poi;
            }
        }

        public override string RecordData()
        {
            var saveData = new SaveData();
            saveData.enabled = poi.enabled;
            saveData.canBeVisited = poi.canBeVisited;
            saveData.isVisited = poi.isVisited;
            return SaveSystem.Serialize(saveData);
        }

        public override void ApplyData(string data)
        {
            if (data == null) return;
            var saveData = SaveSystem.Deserialize<SaveData>(data);
            if (saveData == null) return;
            poi.enabled = saveData.enabled;
            poi.canBeVisited = saveData.canBeVisited;
            poi.isVisited = saveData.isVisited;
        }

    }

}
