using System;
using CommNet;
using UnityEngine;
using Upgradeables;

namespace CustomBarnKit
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    public class CustomBarnKit : MonoBehaviour
    {
        private static CustomGameVariables customGameVariables;
        private static bool varLoaded = false;
        
        public void Start()
        {
            if (!varLoaded)
            {
                customGameVariables = GameVariables.Instance.gameObject.AddComponent<CustomGameVariables>();
                customGameVariables.Load(GameVariables.Instance);
                GameVariables.Instance = customGameVariables;
                // Make sure the network uses the new config. I could not find an event that only update the TrackingStation
                // But I guess something exist for when you update it....
                if (CommNetNetwork.Instance != null)
                    CommNetNetwork.Reset();
                
                log(customGameVariables.ToString());

                // We have to reload everything at each scene changes because the game mess up some of the values...
                GameEvents.onLevelWasLoaded.Add(LoadUpgradesPrices);
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
        private void LoadUpgradesPrices(GameScenes data)
        {
            if (data == GameScenes.MAINMENU || data == GameScenes.SETTINGS || data == GameScenes.CREDITS)
                return;
            
            log("Loading new upgrades prices");

            foreach (UpgradeableFacility facility in GameObject.FindObjectsOfType<UpgradeableFacility>())
            {
                SpaceCenterFacility facilityType;
                try
                {
                    facilityType = (SpaceCenterFacility) Enum.Parse(typeof(SpaceCenterFacility), facility.name);
                }
                catch (ArgumentException)
                {
                    // Mods can add new Facilities that are not in the stock enum list.
                    continue;
                }

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
                    int originalLevel = Math.Min(i, upgradeLevels.Length - 1);

                    UpgradeableObject.UpgradeLevel level = new UpgradeableObject.UpgradeLevel();
                    var sourceLvl = upgradeLevels[originalLevel];

                    level.Setup(facility);
                    level.levelCost = prices[i];
                    level.levelText = sourceLvl.levelText;
                    level.levelStats = sourceLvl.levelStats;
                    level.facilityPrefab = sourceLvl.facilityPrefab;
                    level.facilityInstance = null;

                    // Only redo the visual on the first load. Doing it on the next loads would mess up the order.
                    if (!varLoaded)
                    {
                        if (levelsVisual.Length == levels)
                        {
                            //log(facility.name + " Copying level " + (levelsVisual[i] - 1) + " for level " + (i + 1));
                            level.facilityPrefab = upgradeLevels[levelsVisual[i] - 1].facilityPrefab;
                        }
                        else
                        {
                            log("Wrong levelsVisual length " + levelsVisual.Length + " for " + facility.name + " expected " + levels);
                        }
                    }
                    newUpgradeLevels[i] = level;
                }

                facility.UpgradeLevels = newUpgradeLevels;
                facility.SetupLevels();
                facility.setLevel(facility.FacilityLevel);
            }
            if (!varLoaded)
                log("New upgrades prices are Loaded");
            varLoaded = true;
        }

        private float[] getFacilityUpgradePrices(SpaceCenterFacility f)
        {
            switch (f)
            {
                case SpaceCenterFacility.Administration:
                    return customGameVariables.upgradesAdministration;
                case SpaceCenterFacility.AstronautComplex:
                    return customGameVariables.upgradesAstronauts;
                case SpaceCenterFacility.LaunchPad:
                    return customGameVariables.upgradesLaunchPad;
                case SpaceCenterFacility.MissionControl:
                    return customGameVariables.upgradesMission;
                case SpaceCenterFacility.ResearchAndDevelopment:
                    return customGameVariables.upgradesRnD;
                case SpaceCenterFacility.Runway:
                    return customGameVariables.upgradesRunway;
                case SpaceCenterFacility.SpaceplaneHangar:
                    return customGameVariables.upgradesSPH;
                case SpaceCenterFacility.TrackingStation:
                    return customGameVariables.upgradesTracking;
                case SpaceCenterFacility.VehicleAssemblyBuilding:
                    return customGameVariables.upgradesVAB;
                default:
                    return customGameVariables.upgradesTracking;
            }
        }


        private int getFacilityLevels(SpaceCenterFacility f)
        {
            switch (f)
            {
                case SpaceCenterFacility.Administration:
                    return customGameVariables.levelsAdministration;
                case SpaceCenterFacility.AstronautComplex:
                    return customGameVariables.levelsAstronauts;
                case SpaceCenterFacility.LaunchPad:
                    return customGameVariables.levelsLaunchPad;
                case SpaceCenterFacility.MissionControl:
                    return customGameVariables.levelsMission;
                case SpaceCenterFacility.ResearchAndDevelopment:
                    return customGameVariables.levelsRnD;
                case SpaceCenterFacility.Runway:
                    return customGameVariables.levelsRunway;
                case SpaceCenterFacility.SpaceplaneHangar:
                    return customGameVariables.levelsSPH;
                case SpaceCenterFacility.TrackingStation:
                    return customGameVariables.levelsTracking;
                case SpaceCenterFacility.VehicleAssemblyBuilding:
                    return customGameVariables.levelsVAB;
                default:
                    return customGameVariables.levelsTracking;
            }
        }

        private int[] getFacilityLevelsVisual(SpaceCenterFacility f)
        {
            switch (f)
            {
                case SpaceCenterFacility.Administration:
                    return customGameVariables.upgradesVisualAdministration;
                case SpaceCenterFacility.AstronautComplex:
                    return customGameVariables.upgradesVisualAstronauts;
                case SpaceCenterFacility.LaunchPad:
                    return customGameVariables.upgradesVisualLaunchPad;
                case SpaceCenterFacility.MissionControl:
                    return customGameVariables.upgradesVisualMission;
                case SpaceCenterFacility.ResearchAndDevelopment:
                    return customGameVariables.upgradesVisualRnD;
                case SpaceCenterFacility.Runway:
                    return customGameVariables.upgradesVisualRunway;
                case SpaceCenterFacility.SpaceplaneHangar:
                    return customGameVariables.upgradesVisualSPH;
                case SpaceCenterFacility.TrackingStation:
                    return customGameVariables.upgradesVisualTracking;
                case SpaceCenterFacility.VehicleAssemblyBuilding:
                    return customGameVariables.upgradesVisualVAB;
                default:
                    return customGameVariables.upgradesVisualTracking;
            }
        }

        public static void log(string s)
        {
            MonoBehaviour.print(string.Format("[CustomBarnKit] {0}", s));
        }

        

    }

}
