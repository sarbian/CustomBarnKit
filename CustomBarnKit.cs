using CommNet;
using System;
using UnityEngine;
using Upgradeables;

namespace CustomBarnKit
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
    public class CustomBarnKit : MonoBehaviour
    {
        private static CustomGameVariables customGameVariables;
        private static bool varLoaded = false;

        public void Start()
        {
            // One instance for the whole session: it owns the GameEvents callbacks registered below
            DontDestroyOnLoad(gameObject);

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
            }
        }

#if DEBUG
        // Modifier + T compares every override with the stock value at each level and logs the differences
        public void Update()
        {
            if (GameSettings.MODIFIER_KEY.GetKey() && Input.GetKeyDown(KeyCode.T))
            {
                customGameVariables.Test();
            }
        }
#endif

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

                // The model standing on the facility is kept if the level the save selects uses the same prefab (see below)
                UpgradeableObject.UpgradeLevel spawnedLevel = null;
                for (int i = 0; i < upgradeLevels.Length; i++)
                {
                    if (!upgradeLevels[i].Spawned)
                        continue;
                    if (spawnedLevel == null)
                        spawnedLevel = upgradeLevels[i];
                    else
                        upgradeLevels[i].Despawn();
                }
                GameObject liveInstance = spawnedLevel != null ? spawnedLevel.facilityInstance : null;
                GameObject livePrefab = spawnedLevel != null ? spawnedLevel.facilityPrefab : null;

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
                        if (levelsVisual == null || levelsVisual.Length != levels)
                        {
                            log("Wrong upgradesVisual length " + (levelsVisual == null ? 0 : levelsVisual.Length) + " for " + facility.name + " expected " + levels + ", keeping the stock models");
                        }
                        else if (levelsVisual[i] < 1 || levelsVisual[i] > upgradeLevels.Length)
                        {
                            log("upgradesVisual value " + levelsVisual[i] + " for " + facility.name + " level " + (i + 1) + " is outside 1.." + upgradeLevels.Length + ", keeping the stock model");
                        }
                        else
                        {
                            level.facilityPrefab = upgradeLevels[levelsVisual[i] - 1].facilityPrefab;
                        }
                    }
                    newUpgradeLevels[i] = level;
                }

                // setLevel instantiates the selected level's prefab and destroys the model standing on the facility
                // transform. For the levels that use the standing model, blank the prefab so that spawn does nothing,
                // then hand the standing instance to the selected level: no destroy and no instantiate per scene load.
                // A level with another prefab keeps it, and its spawn replaces the standing model as before.
                if (liveInstance != null)
                {
                    spawnedLevel.facilityInstance = null;
                    for (int i = 0; i < levels; i++)
                    {
                        if (newUpgradeLevels[i].facilityPrefab == livePrefab)
                            newUpgradeLevels[i].facilityPrefab = null;
                    }
                }

                facility.UpgradeLevels = newUpgradeLevels;

                // Use the current save state to (re)init the facility
                if (ScenarioUpgradeableFacilities.protoUpgradeables.TryGetValue( facility.id, out ScenarioUpgradeableFacilities.ProtoUpgradeable protoUpgradeable) &&
                    protoUpgradeable.configNode != null)
                {
                    facility.Load(protoUpgradeable.configNode);
                }
                else
                {
                    facility.SetupLevels();
                    facility.setLevel(HighLogic.CurrentGame.Mode == Game.Modes.CAREER ? facility.FacilityLevel : facility.MaxLevel);
                }

                if (liveInstance != null)
                {
                    for (int i = 0; i < levels; i++)
                    {
                        if (newUpgradeLevels[i].facilityPrefab == null)
                            newUpgradeLevels[i].facilityPrefab = livePrefab;
                    }

                    UpgradeableObject.UpgradeLevel current = facility.CurrentLevel;
                    if (current != null && !current.Spawned && current.facilityPrefab == livePrefab)
                        current.facilityInstance = liveInstance;
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
