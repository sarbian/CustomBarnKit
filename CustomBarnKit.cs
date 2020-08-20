using CommNet;
using System;
using System.Reflection;
using UnityEngine;
using Upgradeables;

namespace CustomBarnKit
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    public class CustomBarnKit : MonoBehaviour
    {
        internal static CustomGameVariables customGameVariables;
        private static bool varLoaded = false;
        private static bool brokeStuff = false;

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
                GameEvents.onLevelWasLoaded.Add(LoadUpgradesPricesSceneChange);

                // The "Switch Editor" button also reset everything
                GameEvents.onEditorRestart.Add(LoadUpgradesPrices);
                
                BreakStuff();
            }
        }

        private void BreakStuff()
        {
            if (brokeStuff)
                return;

            MethodInfo originalCall = typeof(ModuleEvaChute).GetMethod("CanCrewMemberUseParachute", BindingFlags.Public | BindingFlags.Static);
            MethodInfo improvedCall = typeof(CustomGameVariables).GetMethod("CanCrewMemberUseParachute", BindingFlags.Public | BindingFlags.Static);

            if (originalCall != null && improvedCall != null)
                Detourer.TryDetourFromTo(originalCall, improvedCall);
            else
                print("BreakStuff " + (originalCall != null) + " " + (improvedCall != null));
            brokeStuff = true;
        }

        //private float nextTick = 0;

        public void Update()
        {
            if (GameSettings.MODIFIER_KEY.GetKey() && Input.GetKeyDown(KeyCode.T))
            {
                customGameVariables.Test();
            }

            //if (((EditorDriver.fetch != null && EditorDriver.fetch.restartingEditor )|| Time.unscaledTime > nextTick) && customGameVariables != null)
            //{
            //    nextTick = Time.unscaledTime + 5;
            //    StringBuilder sb = new StringBuilder();
            //    sb.AppendLine();
            //    sb.AppendLine($" {EditorDriver.fetch != null && EditorDriver.fetch.restartingEditor} {customGameVariables.levelsTracking:F3} {customGameVariables.upgradesTracking.Length}");
            //    foreach (ScenarioUpgradeableFacilities.ProtoUpgradeable facility in ScenarioUpgradeableFacilities.protoUpgradeables.Values)
            //    {
            //        if (facility.facilityRefs.Count > 0)
            //        {
            //            sb.AppendLine(facility.facilityRefs.First().id + 
            //                          " lvl " + facility.facilityRefs.First().FacilityLevel + 
            //                          " / " + facility.facilityRefs.First().MaxLevel + 
            //                          " = " + facility.facilityRefs.First().GetNormLevel());
            //            sb.AppendLine( facility.configNode.CountNodes + " " + facility.configNode.CountValues + " " +  facility.configNode.ToString());
            //        }
            //    }
            //    sb.AppendLine("------------");
            //    log(sb.ToString());
            //}
        }

        // With the help of NoMoreGrind code by nlight
        private void LoadUpgradesPricesSceneChange(GameScenes data)
        {
            if (data == GameScenes.MAINMENU || data == GameScenes.SETTINGS || data == GameScenes.CREDITS)
                return;

            LoadUpgradesPrices();
        }

        private void LoadUpgradesPrices()
        {
            log("Loading new upgrades prices");

            foreach (UpgradeableFacility facility in GameObject.FindObjectsOfType<UpgradeableFacility>())
            {
                SpaceCenterFacility facilityType;
                try
                {
                    facilityType = (SpaceCenterFacility)Enum.Parse(typeof(SpaceCenterFacility), facility.name);
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

                for (int i = 0; i < upgradeLevels.Length; i++)
                {
                    UpgradeableObject.UpgradeLevel oldLevel = upgradeLevels[i];
                    if (oldLevel.Spawned)
                        oldLevel.Despawn();
                }

                UpgradeableObject.UpgradeLevel[] newUpgradeLevels = new UpgradeableObject.UpgradeLevel[levels];

                for (int i = 0; i < levels; i++)
                {
                    int originalLevel = Math.Min(i, upgradeLevels.Length - 1);

                    UpgradeableObject.UpgradeLevel level = new UpgradeableObject.UpgradeLevel();
                    var sourceLvl = upgradeLevels[originalLevel];

                    level.Setup(facility);
                    level.levelCost = prices[i];
                    level.levelText = sourceLvl.levelText;
                    level.levelStats = new KSCFacilityLevelText
                    {
                        facility = sourceLvl.levelStats.facility,
                        linePrefix =  sourceLvl.levelStats.linePrefix,
                        textBase =  sourceLvl.levelStats.textBase
                    };
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

                // Use the current save state to (re)init the facility
                if (ScenarioUpgradeableFacilities.protoUpgradeables.TryGetValue( facility.id, out ScenarioUpgradeableFacilities.ProtoUpgradeable protoUpgradeable))
                {
                    facility.Load(protoUpgradeable.configNode);
                }
                else
                {
                    facility.SetupLevels();
                    facility.setLevel(facility.FacilityLevel);
                }
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
