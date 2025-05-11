using System.Collections.Generic;
using CompassNavigatorPro;
using Domains.Input.Scripts;
using Domains.UI_Global.Events;
using UnityEngine;

namespace Domains.Scripts_that_Need_Sorting
{
    public class CompassNavigatorController : MonoBehaviour
    {
        [SerializeField] private List<CompassProPOI> compassProPOIs = new();
        private CompassPro _compassPro;

        private MyRewiredInputManager _inputManager;

        private bool hasUsedMoreInfo;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            _compassPro = FindFirstObjectByType<CompassPro>();
            if (_compassPro != null)
            {
                _compassPro.showOnScreenIndicators = false;
                _compassPro.showOffScreenIndicators = false;
                _compassPro.UpdateSettings();
                UnityEngine.Debug.Log("Compass settings initialized");
            }
            else
            {
                UnityEngine.Debug.LogWarning("CompassPro component not found.");
            }

            _inputManager = MyRewiredInputManager.Instance;

            if (_inputManager == null)
                UnityEngine.Debug.LogError("CompassNavigatorController: No MyRewiredInputManager found in the scene.");
        }

        // Update is called once per frame
        private void Update()
        {
            if (_inputManager.IsGetMoreInfoPressed())
            {
                if (_compassPro != null)
                {
                    if (!hasUsedMoreInfo)
                    {
                        TutorialEvent.Trigger(TutorialEventType.PlayerUsedMoreInfo);
                        hasUsedMoreInfo = true;
                    }

                    _compassPro.showOnScreenIndicators = true;
                    _compassPro.showOffScreenIndicators = true;
                    _compassPro.UpdateSettings();
                }
            }
            else
            {
                if (_compassPro != null)
                {
                    _compassPro.showOnScreenIndicators = false;
                    _compassPro.showOffScreenIndicators = false;
                    _compassPro.UpdateSettings();
                }
            }
        }
    }
}