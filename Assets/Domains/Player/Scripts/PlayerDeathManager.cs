﻿using Domains.Input.Scripts;
using Domains.Player.Events;
using Domains.Scene.Scripts;
using Domains.UI_Global.Events;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Serialization;

namespace Domains.Player.Scripts
{
    public class PlayerDeathManager : MonoBehaviour, MMEventListener<PlayerStatusEvent>

    {
        private static PlayerDeathManager _instance;

        [FormerlySerializedAs("teleportWhenOutOfFuel")] [Header("Game Mechanics")]
        public bool autoResetWhenOutOfFuel = true; // Toggle in inspector

        public int monetaryPenalty = 400;

        [FormerlySerializedAs("staminaPenaltyMultiplier")]
        public float fuelPenaltyMultiplier = 0.2f;

        public float healthPenaltyMultiplier = 0.2f;

        public MMFeedbacks deathFeedbacks;
        public MMFeedbacks outOfFuelFeedbacks;

        [FormerlySerializedAs("_sceneRestarter")] [SerializeField]
        private MMSceneRestarter sceneRestarter;

        [SerializeField] private Transform spawnPoint;
        [SerializeField] private GameObject playerCamera;


        private void Awake()
        {
            _instance = this;
        }


        private void OnEnable()
        {
            this.MMEventStartListening();
        }

        private void OnDisable()
        {
            this.MMEventStopListening();
        }

        public void OnMMEvent(PlayerStatusEvent eventType)
        {
            if (eventType.EventType == PlayerStatusEventType.Died)
            {
                SetPostDeathStats();
                deathFeedbacks?.PlayFeedbacks();
                AlertEvent.Trigger(AlertReason.Died, "You have died!", "Game Over");
            }


            if (eventType.EventType == PlayerStatusEventType.OutOfFuel)

            {
                outOfFuelFeedbacks?.StopFeedbacks();
                AlertEvent.Trigger(AlertReason.OutOfFuel, "You are out of fuel! Hold E to Reset", "Out of Fuel");
            }

            if (eventType.EventType == PlayerStatusEventType.SoftReset)
            {
                SetPostFuelOutStats();
                outOfFuelFeedbacks?.PlayFeedbacks();
                AlertEvent.Trigger(AlertReason.OutOfFuel, "You are out of fuel!", "Out of Fuel");
            }
        }

        private void ManualReset()
        {
            // Apply the same penalties as the automatic reset
            SetPostFuelOutStats();
            outOfFuelFeedbacks?.PlayFeedbacks();
            AlertEvent.Trigger(AlertReason.OutOfFuel, "Manual reset activated!", "Reset to Base");

            // Trigger the reset event manually
            PlayerStatusEvent.Trigger(PlayerStatusEventType.OutOfFuel);
        }

// In PlayerDeathManager.cs
        public void SetPostFuelOutStats()
        {
            var maximumFuel = PlayerFuelManager.MaxFuelPoints;
            var recoveryAmount = fuelPenaltyMultiplier * maximumFuel;

            // Ensure a minimum amount (e.g., 10) if the penalty calculation is too low
            recoveryAmount = Mathf.Max(recoveryAmount, 10f);

            // Use SetCurrentFuel to set the fuel amount
            FuelEvent.Trigger(FuelEventType.SetCurrentFuel, recoveryAmount, maximumFuel);
            CurrencyEvent.Trigger(CurrencyEventType.LoseCurrency, GetRescueExpense());

            // Ensure the UI is updated
            FuelEvent.Trigger(FuelEventType.NotifyListeners, recoveryAmount, maximumFuel);

            SaveManager.Instance.SaveAll();

            // Debug the fuel reset
            UnityEngine.Debug.Log($"Set fuel to {recoveryAmount} after running out of fuel");
        }

        public int GetRescueExpense()
        {
            var playerDepth = -playerCamera.transform.position.y;

            if (playerDepth < 0) playerDepth = 0;

            var rescueExpense = Mathf.FloorToInt(playerDepth / 10) * monetaryPenalty;

            return rescueExpense;
        }


        public void SetPostDeathStats()
        {
            var maximumHealth = PlayerHealthManager.MaxHealthPoints;
            var maxFuel = PlayerFuelManager.MaxFuelPoints;
            var currentCurrency = PlayerCurrencyManager.CompanyCredits;
            FuelEvent.Trigger(FuelEventType.SetCurrentFuel, fuelPenaltyMultiplier * maxFuel, maxFuel);
            HealthEvent.Trigger(HealthEventType.SetCurrentHealth, healthPenaltyMultiplier * maximumHealth);
            CurrencyEvent.Trigger(CurrencyEventType.LoseCurrency, GetRescueExpense());

            SaveManager.Instance.SaveAll();
        }
    }
}