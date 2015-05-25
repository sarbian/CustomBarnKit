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
        private static CustomGameVariables customGameVariables;
        private static bool varLoaded = false;
        private bool upgradeLoaded = false;
        
        public void Start()
        {
            if (!varLoaded)
            {
                customGameVariables = new CustomGameVariables(GameVariables.Instance);
                GameVariables.Instance = customGameVariables;
                varLoaded = true;

                log(customGameVariables.ToString());
            }

            if (varLoaded && !upgradeLoaded)
            {
                LoadUpgradesPrices();

                upgradeLoaded = true;
            }
        }

        public void Update()
        {
#if DEBUG
            if (GameSettings.MODIFIER_KEY.GetKey() && Input.GetKeyDown(KeyCode.T))
            {
                customGameVariables.Test();
            }
#endif
        }

        // With the help of NoMoreGrind code by nlight
        private void LoadUpgradesPrices()
        {
            log("Loading new upgrades prices");

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
            log("New upgrades prices are Loaded");
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
