using Domains.Player.Events;
using Domains.Player.Progression;
using Domains.Player.Scripts;
using Domains.Scene.Scripts;
using UnityEngine;

namespace Domains.Debug
{
    [DefaultExecutionOrder(-100)] // Make this run before other scripts
    public class DataReset : MonoBehaviour
    {
        private void Awake()
        {
            UnityEngine.Debug.Log("PurePrototypeReset: Awake() called.");
            ClearAllSaveData();
        }

        public static void ClearAllSaveData()
        {
            var isEditorMode = !Application.isPlaying;

            if (isEditorMode) UnityEngine.Debug.Log("Running data reset in Editor mode...");

            // Reset stats
            PlayerFuelManager.ResetPlayerFuel();
            PlayerFuelManager.SavePlayerFuel();


            // Reset health
            PlayerHealthManager.ResetPlayerHealth();
            PlayerHealthManager.SavePlayerHealth();


            // Reset currency
            PlayerCurrencyManager.ResetPlayerCurrency();
            PlayerCurrencyManager.SavePlayerCurrency();

            // reset pickables
            PickableManager.ResetPickedItems();
            PickableManager.SaveAllPickedItems();

            DestructableManager.ResetDestructables();
            DestructableManager.SaveAllDestructables();

            // Reset upgrades
            PlayerUpgradeManager.ResetPlayerUpgrades();
            PlayerUpgradeManager.SaveUpgrades();

            // Reset progression
            ProgressionManager.ResetProgression();
            ProgressionManager.SaveAllProgression(true);

            // Try reset Digger. Note this won't work in editor mode
            DiggerEvent.Trigger(DiggerEventType.Delete);

            // Reset digger data if it exists
            // if (DiggerDataManager.Instance != null) DiggerDataManager.Instance.ResetDiggerData();
            // Reset inventory
            PlayerInventoryManager.ResetInventory(); // will avoid clearing scene inventory if not loaded
            InventorySaveUtility.Reset(); // safely wipes saved inventory data


            UnityEngine.Debug.Log("All save data cleared successfully.");
        }
    }
}