using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Upgradeables;

namespace CustomBarnKit
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    public class CustomBarnKit : MonoBehaviour
    {
        private CustomGameVariables customGameVariables;
        private bool loaded = false;
        
        public void Start()
        {
            if (!loaded)
            {
                customGameVariables = new CustomGameVariables();
                GameVariables.Instance = customGameVariables;

                LoadUpgradesPrices();

                loaded = true;
            }
        }

        public void Update()
        {
            if (GameVariables.Instance != customGameVariables)
            {
                log(HighLogic.LoadedScene + " Different GameVariables.Instance");
            }
        }

        // With the help of NoMoreGrind code by nlight
        private void LoadUpgradesPrices()
        {
            List<FieldInfo> fields = new List<FieldInfo>(typeof(UpgradeableFacility).GetFields(BindingFlags.NonPublic | BindingFlags.Instance));
            FieldInfo upgradeLevelsField = fields.FirstOrDefault(f => f.FieldType == typeof(UpgradeableObject.UpgradeLevel[]));

            if (upgradeLevelsField == null)
            {
                log("Error: Unable to find the UpgradeLevel field. A new version of KSP ?");
                return;
            }

            foreach (UpgradeableFacility facility in GameObject.FindObjectsOfType<UpgradeableFacility>())
            {
                Facility facilityType = (Facility)Enum.Parse(typeof(Facility), facility.name);

                float[] prices = getFacilityUpgradePrices(facilityType);

                if (prices == null)
                {
                    log("No upgrades prices set for " + facilityType + ". Skipping");
                    continue;
                }

                UpgradeableObject.UpgradeLevel[] upgradeLevels = (UpgradeableObject.UpgradeLevel[])upgradeLevelsField.GetValue(facility);

                if (upgradeLevels.Length != prices.Length)
                {
                    log("Wrong numbers of upgrade price for " + facility + " expecting " + upgradeLevels.Length + " and have " + prices.Length + ". Check your configs");
                    continue;
                }

                for (int i = 0; i < upgradeLevels.Length; i++)
                {
                    UpgradeableObject.UpgradeLevel level = upgradeLevels[i];
                    level.levelCost = prices[i];
                }
            }
            log("New upgrades prices are loaded");
        }

        private float[] getFacilityUpgradePrices(Facility f)
        {
            switch (f)
            {
                case Facility.Administration:
                    return customGameVariables.administrationUpgrades;
                case Facility.AstronautComplex:
                    return customGameVariables.astronautsUpgrades;
                case Facility.LaunchPad:
                    return customGameVariables.LaunchPadUpgrades;
                case Facility.MissionControl:
                    return customGameVariables.missionUpgrades;
                case Facility.ResearchAndDevelopment:
                    return customGameVariables.rndUpgrades;
                case Facility.Runway:
                    return customGameVariables.RunwayUpgrades;
                case Facility.SpaceplaneHangar:
                    return customGameVariables.SPHUpgrades;
                case Facility.TrackingStation:
                    return customGameVariables.trackingUpgrades;
                case Facility.VehicleAssemblyBuilding:
                    return customGameVariables.VABUpgrades;
                default:
                    return customGameVariables.trackingUpgrades;
            }
        }
        
        public enum Facility
        {
            VehicleAssemblyBuilding,
            TrackingStation,
            SpaceplaneHangar,
            Runway,
            ResearchAndDevelopment,
            MissionControl,
            LaunchPad,
            AstronautComplex,
            Administration
        }

        public static void log(String s)
        {
            MonoBehaviour.print(string.Format("[CustomBarnKit] {0}", s));
        }

        

    }

}
