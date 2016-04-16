using System;
using System.Collections.Generic;
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

            foreach (UpgradeableFacility facility in GameObject.FindObjectsOfType<UpgradeableFacility>())
            {
                Facility facilityType = (Facility)Enum.Parse(typeof(Facility), facility.name);

                float[] prices = getFacilityUpgradePrices(facilityType);
                int levels = getFacilityLevels(facilityType);
                int[] levelsVisual = getFacilityLevelsVisual(facilityType);

                if (prices == null)
                {
                    log("No upgrades prices set for " + facilityType + ". Skipping");
                    continue;
                }

                if (levels != prices.Length)
                {
                    log("Wrong numbers of upgrade price for " + facility + " expecting " + levels + " and have " + prices.Length + ". Check your configs");
                    continue;
                }

                UpgradeableObject.UpgradeLevel[] upgradeLevels = facility.UpgradeLevels;

                if (facility.UpgradeLevels[facility.FacilityLevel].Spawned)
                    facility.UpgradeLevels[facility.FacilityLevel].Despawn();

                UpgradeableObject.UpgradeLevel[] newUpgradeLevels = new UpgradeableObject.UpgradeLevel[levels];

                for (int i = 0; i < levels; i++)
                {
                    UpgradeableObject.UpgradeLevel level;

                    int orginalLevel = Math.Min(i, upgradeLevels.Length - 1);

                    level = new UpgradeableObject.UpgradeLevel();
                    var sourceLvl = upgradeLevels[orginalLevel];

                    level.levelCost = prices[i];
                    level.levelText = sourceLvl.levelText;
                    level.levelStats = sourceLvl.levelStats;
                    level.facilityPrefab = sourceLvl.facilityPrefab;
                    level.facilityInstance = null;

                    if (levelsVisual.Length == levels)
                    {
                        log(facility.name + " Copying level " + (levelsVisual[i] - 1) + " for level " + (i + 1));
                        //level.facilityPrefab = Instantiate(upgradeLevels[levelsVisual[i] - 1].facilityPrefab);
                        level.facilityPrefab = upgradeLevels[levelsVisual[i] - 1].facilityPrefab;
                    }
                    else
                    {
                        log("Wrong levelsVisual length " + levelsVisual.Length + " for " + facility.name + " expected " + levels);
                    }

                    newUpgradeLevels[i] = level;
                }

                facility.UpgradeLevels = newUpgradeLevels;
                facility.SetupLevels();
                facility.setLevel(facility.FacilityLevel);
                
            }
            log("New upgrades prices are Loaded");
        }

        private float[] getFacilityUpgradePrices(Facility f)
        {
            switch (f)
            {
                case Facility.Administration:
                    return customGameVariables.upgradesAdministration;
                case Facility.AstronautComplex:
                    return customGameVariables.upgradesAstronauts;
                case Facility.LaunchPad:
                    return customGameVariables.upgradesLaunchPad;
                case Facility.MissionControl:
                    return customGameVariables.upgradesMission;
                case Facility.ResearchAndDevelopment:
                    return customGameVariables.upgradesRnD;
                case Facility.Runway:
                    return customGameVariables.upgradesRunway;
                case Facility.SpaceplaneHangar:
                    return customGameVariables.upgradesSPH;
                case Facility.TrackingStation:
                    return customGameVariables.upgradesTracking;
                case Facility.VehicleAssemblyBuilding:
                    return customGameVariables.upgradesVAB;
                default:
                    return customGameVariables.upgradesTracking;
            }
        }


        private int getFacilityLevels(Facility f)
        {
            switch (f)
            {
                case Facility.Administration:
                    return customGameVariables.levelsAdministration;
                case Facility.AstronautComplex:
                    return customGameVariables.levelsAstronauts;
                case Facility.LaunchPad:
                    return customGameVariables.levelsLaunchPad;
                case Facility.MissionControl:
                    return customGameVariables.levelsMission;
                case Facility.ResearchAndDevelopment:
                    return customGameVariables.levelsRnD;
                case Facility.Runway:
                    return customGameVariables.levelsRunway;
                case Facility.SpaceplaneHangar:
                    return customGameVariables.levelsSPH;
                case Facility.TrackingStation:
                    return customGameVariables.levelsTracking;
                case Facility.VehicleAssemblyBuilding:
                    return customGameVariables.levelsVAB;
                default:
                    return customGameVariables.levelsTracking;
            }
        }

        private int[] getFacilityLevelsVisual(Facility f)
        {
            switch (f)
            {
                case Facility.Administration:
                    return customGameVariables.upgradesVisualAdministration;
                case Facility.AstronautComplex:
                    return customGameVariables.upgradesVisualAstronauts;
                case Facility.LaunchPad:
                    return customGameVariables.upgradesVisualLaunchPad;
                case Facility.MissionControl:
                    return customGameVariables.upgradesVisualMission;
                case Facility.ResearchAndDevelopment:
                    return customGameVariables.upgradesVisualRnD;
                case Facility.Runway:
                    return customGameVariables.upgradesVisualRunway;
                case Facility.SpaceplaneHangar:
                    return customGameVariables.upgradesVisualSPH;
                case Facility.TrackingStation:
                    return customGameVariables.upgradesVisualTracking;
                case Facility.VehicleAssemblyBuilding:
                    return customGameVariables.upgradesVisualVAB;
                default:
                    return customGameVariables.upgradesVisualTracking;
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
