using System;
using System.Collections.Generic;
using UnityEngine;

namespace CustomBarnKit
{
    public class CustomGameVariables : GameVariables
    {

        // Those values are present in GameVariables but I did
        // not add anything to change them yet

        //public AnimationCurve reputationAddition;
        //public AnimationCurve reputationSubtraction;
        //public float mentalityFundsTrivial = 1.1f;
        //public float mentalityFundsSignificant = 1.2f;
        //public float mentalityFundsExceptional = 1.3f;
        //public float mentalityReputationTrivial = 1.1f;
        //public float mentalityReputationSignificant = 1.2f;
        //public float mentalityReputationExceptional = 1.3f;
        //public float mentalityScienceTrivial = 1.1f;
        //public float mentalityScienceSignificant = 1.2f;
        //public float mentalityScienceExceptional = 1.3f;
        //public float mentalityExpiryTrivial = 1.1f;
        //public float mentalityExpirySignificant = 1.2f;
        //public float mentalityExpiryExceptional = 1.3f;
        //public float mentalityDeadlineTrivial = 1.1f;
        //public float mentalityDeadlineSignificant = 1.2f;
        //public float mentalityDeadlineExceptional = 1.3f;

        // Editor
        public float[] VABUpgrades;
        public float[] SPHUpgrades;
        public float[] LaunchPadUpgrades;
        public float[] RunwayUpgrades;
        public float actionGroupsCustomUnlock = 0.6f;
        public float actionGroupsStockUnlock = 0.4f;
        public float unlockedFuelTransfer = 0.2f;
        public float[] craftMassLimit;
        public Vector3[] craftSizeLimit;
        public int[] partCountLimit;

        // Astronauts Complex
        public float[] astronautsUpgrades;
        public float recruitHireBaseCost = 10000f;
        public float recruitHireFlatRate = 1.25f;
        public float recruitHireRateModifier = 0.015f;
        public float unlockedEVA = 0.2f;
        public float unlockedEVAClamber = 0.6f;
        public float unlockedEVAFlags = 0.4f;
        public int[] activeCrewLimit;
        public float[] crewLevelLimit;

        // Mission control
        public float[] missionUpgrades;
        public float unlockedFlightPlanning = 0.4f;
        public int[] activeContractsLimit;
        // Not used for now
        public Dictionary<string, float[]> scoreSituationCustom = new Dictionary<string, float[]>();
        public float[] scoreSituationHome;
        public float[] scoreSituationOther;

        //public float partRecoveryValueFactor = 0.9f;
        //public float resourceRecoveryValueFactor = 0.95f;
        //public float reputationKerbalDeath = 10f;
        //public float reputationKerbalRecovery = 25f;

        //public float contractPrestigeTrivial = 1f;
        //public float contractPrestigeSignificant = 1.25f;
        //public float contractPrestigeExceptional = 1.5f;

        //public float contractDestinationWeight = 1f;
        //public float contractFundsAdvanceFactor = 1f;
        //public float contractFundsCompletionFactor = 1f;
        //public float contractFundsFailureFactor = 1f;
        //public float contractReputationCompletionFactor = 1f;
        //public float contractReputationFailureFactor = 1f;
        //public float contractScienceCompletionFactor = 1f;

        // Tracking Station
        public float[] trackingUpgrades;
        public float unlockedSpaceObjectDiscovery = 0.6f;
        public float orbitDisplayMode = -1;
        public int[] patchesAheadLimit;
        public int[] trackedObjectLimit;

        // Administration
        public float[] administrationUpgrades;
        public int[] activeStrategyLimit;
        public float[] strategyCommitRange;

        // R&D
        public float[] rndUpgrades;
        public float[] dataToScienceRatio;
        public float[] scienceCostLimit;

        public CustomGameVariables()
        {
            ConfigNode[] configs = GameDatabase.Instance.GetConfigNodes("CUSTOMBARNKIT");

            if (configs == null || configs.Length == 0)
            {
                print("No config to load");
                return;
            }

            if (configs.Length > 1)
            {
                print("More than 1 CustomBarnKit node found. Loading the first one");
            }

            ConfigNode config = configs[0];

            if (config.HasNode("EDITOR"))
            {
                ConfigNode editor = config.GetNode("EDITOR");

                LoadValue(editor, "VABUpgrades", ref VABUpgrades);
                LoadValue(editor, "SPHUpgrades", ref SPHUpgrades);
                LoadValue(editor, "launchPadUpgrades", ref LaunchPadUpgrades);
                LoadValue(editor, "runwayUpgrades", ref RunwayUpgrades);

                LoadValue(editor, "actionGroupsCustomUnlock", ref actionGroupsCustomUnlock);
                LoadValue(editor, "actionGroupsStockUnlock", ref actionGroupsStockUnlock);
                LoadValue(editor, "unlockedFuelTransfer", ref unlockedFuelTransfer);
                LoadValue(editor, "craftMassLimit", ref craftMassLimit);
                LoadValue(editor, "craftSizeLimit", ref craftSizeLimit);
                LoadValue(editor, "partCountLimit", ref partCountLimit);
            }

            if (config.HasNode("ASTRONAUTS"))
            {
                ConfigNode editor = config.GetNode("ASTRONAUTS");

                LoadValue(editor, "upgrades", ref astronautsUpgrades);

                LoadValue(editor, "recruitHireBaseCost", ref recruitHireBaseCost);
                LoadValue(editor, "recruitHireFlatRate", ref recruitHireFlatRate);
                LoadValue(editor, "recruitHireRateModifier", ref recruitHireRateModifier);
                LoadValue(editor, "unlockedEVA", ref unlockedEVA);
                LoadValue(editor, "unlockedEVAClamber", ref unlockedEVAClamber);
                LoadValue(editor, "unlockedEVAFlags", ref unlockedEVAFlags);
                LoadValue(editor, "activeCrewLimit", ref activeCrewLimit);
                LoadValue(editor, "crewLevelLimit", ref crewLevelLimit);
            }

            if (config.HasNode("MISSION"))
            {
                ConfigNode editor = config.GetNode("MISSION");

                LoadValue(editor, "upgrades", ref missionUpgrades);

                LoadValue(editor, "unlockedFlightPlanning", ref unlockedFlightPlanning);
                LoadValue(editor, "activeContractsLimit", ref activeContractsLimit);
                LoadValue(editor, "scoreSituationHome", ref scoreSituationHome);
                LoadValue(editor, "scoreSituationOther", ref scoreSituationOther);

                LoadValue(editor, "partRecoveryValueFactor", ref partRecoveryValueFactor);
                LoadValue(editor, "resourceRecoveryValueFactor", ref resourceRecoveryValueFactor);
                LoadValue(editor, "reputationKerbalDeath", ref reputationKerbalDeath);
                LoadValue(editor, "reputationKerbalRecovery", ref reputationKerbalRecovery);


                LoadValue(editor, "contractPrestigeTrivial", ref contractPrestigeTrivial);
                LoadValue(editor, "contractPrestigeSignificant", ref contractPrestigeSignificant);
                LoadValue(editor, "contractPrestigeExceptional", ref contractPrestigeExceptional);

                LoadValue(editor, "contractDestinationWeight", ref contractDestinationWeight);
                LoadValue(editor, "contractFundsAdvanceFactor", ref contractFundsAdvanceFactor);
                LoadValue(editor, "contractFundsCompletionFactor", ref contractFundsCompletionFactor);
                LoadValue(editor, "contractFundsFailureFactor", ref contractFundsFailureFactor);
                LoadValue(editor, "contractReputationCompletionFactor", ref contractReputationCompletionFactor);
                LoadValue(editor, "contractReputationFailureFactor", ref contractReputationFailureFactor);
                LoadValue(editor, "contractScienceCompletionFactor", ref contractScienceCompletionFactor);
            }

            if (config.HasNode("TRACKING"))
            {
                ConfigNode editor = config.GetNode("TRACKING");

                LoadValue(editor, "upgrades", ref trackingUpgrades);

                LoadValue(editor, "unlockedSpaceObjectDiscovery", ref unlockedSpaceObjectDiscovery);
                LoadValue(editor, "orbitDisplayMode", ref orbitDisplayMode);
                LoadValue(editor, "patchesAheadLimit", ref patchesAheadLimit);
                LoadValue(editor, "trackedObjectLimit", ref trackedObjectLimit);
            }

            if (config.HasNode("ADMINISTRATION"))
            {
                ConfigNode editor = config.GetNode("ADMINISTRATION");

                LoadValue(editor, "upgrades", ref administrationUpgrades);

                LoadValue(editor, "activeStrategyLimit", ref activeStrategyLimit);
                LoadValue(editor, "strategyCommitRange", ref strategyCommitRange);
            }

            if (config.HasNode("RESEARCH"))
            {
                ConfigNode editor = config.GetNode("RESEARCH");

                LoadValue(editor, "upgrades", ref rndUpgrades);

                LoadValue(editor, "dataToScienceRatio", ref dataToScienceRatio);
                LoadValue(editor, "scienceCostLimit", ref scienceCostLimit);
            }
        }

        public override int GetActiveContractsLimit(float mCtrlNormLevel)
        {
            if (activeContractsLimit == null || activeContractsLimit.Length != 4)
                return base.GetActiveContractsLimit(mCtrlNormLevel);

            if (mCtrlNormLevel < 0.25f)
            {
                return activeContractsLimit[0];
            }
            if (mCtrlNormLevel < 0.5f)
            {
                return activeContractsLimit[1];
            }
            if (mCtrlNormLevel < 0.75f)
            {
                return activeContractsLimit[2];
            }
            return activeContractsLimit[3];
        }
        public override int GetActiveCrewLimit(float astroComplexNormLevel)
        {
            if (activeCrewLimit == null || activeCrewLimit.Length != 4)
                return base.GetActiveCrewLimit(astroComplexNormLevel);

            if (astroComplexNormLevel < 0.25f)
            {
                return activeCrewLimit[0];
            }
            if (astroComplexNormLevel < 0.5f)
            {
                return activeCrewLimit[1];
            }
            if (astroComplexNormLevel < 0.75f)
            {
                return activeCrewLimit[2];
            }
            return activeCrewLimit[3];
        }
        public override int GetActiveStrategyLimit(float adminNormLevel)
        {
            if (activeStrategyLimit == null || activeStrategyLimit.Length != 4)
                return base.GetActiveStrategyLimit(adminNormLevel);

            if (adminNormLevel < 0.25f)
            {
                return activeStrategyLimit[0];
            }
            if (adminNormLevel < 0.5f)
            {
                return activeStrategyLimit[1];
            }
            if (adminNormLevel < 0.75f)
            {
                return activeStrategyLimit[2];
            }
            return activeStrategyLimit[3];
        }

        //public override float GetContractDestinationWeight(CelestialBody body)

        //public override float GetContractFundsAdvanceFactor(Contract.ContractPrestige prestige)

        //public override float GetContractFundsCompletionFactor(Contract.ContractPrestige prestige)

        //public override float GetContractFundsFailureFactor(Contract.ContractPrestige prestige)

        //public override float GetContractLevelLimit(float mCtrlNormLevel)

        //public override float GetContractPrestigeFactor(Contract.ContractPrestige prestige)

        //public override float GetContractReputationCompletionFactor(Contract.ContractPrestige prestige)

        //public override float GetContractReputationFailureFactor(Contract.ContractPrestige prestige)

        //public override float GetContractScienceCompletionFactor(Contract.ContractPrestige prestige)

        public override float GetCraftMassLimit(float editorNormLevel)
        {
            if (craftMassLimit == null || craftMassLimit.Length != 5)
                return base.GetCraftMassLimit(editorNormLevel);

            if (editorNormLevel < 0.2f)
            {
                return craftMassLimit[0];
            }
            if (editorNormLevel < 0.4f)
            {
                return craftMassLimit[1];
            }
            if (editorNormLevel < 0.6f)
            {
                return craftMassLimit[2];
            }
            if (editorNormLevel < 0.8f)
            {
                return craftMassLimit[3];
            }
            return craftMassLimit[4];
        }
        
        public override Vector3 GetCraftSizeLimit(float editorNormLevel)
        {
            if (craftSizeLimit == null || craftSizeLimit.Length != 5)
                return base.GetCraftSizeLimit(editorNormLevel);

            if (editorNormLevel < 0.2f)
            {
                return craftSizeLimit[0];
            }
            if (editorNormLevel < 0.4f)
            {
                return craftSizeLimit[1];
            }
            if (editorNormLevel < 0.6f)
            {
                return craftSizeLimit[2];
            }
            if (editorNormLevel < 0.8f)
            {
                return craftSizeLimit[3];
            }
            return craftSizeLimit[4];
        }
        
        public override float GetCrewLevelLimit(float astroComplexNormLevel)
        {
            if (crewLevelLimit == null || crewLevelLimit.Length != 3)
                return base.GetCrewLevelLimit(astroComplexNormLevel);

            if (astroComplexNormLevel < 0.33f)
            {
                return crewLevelLimit[0];
            }
            if (astroComplexNormLevel < 0.66f)
            {
                return crewLevelLimit[1];
            }
            return crewLevelLimit[2];
        }
        
        public override float GetDataToScienceRatio(float RnDnormLevel)
        {
            if (dataToScienceRatio == null || dataToScienceRatio.Length != 4)
                return base.GetDataToScienceRatio(RnDnormLevel);

            if (RnDnormLevel < 0.25f)
            {
                return dataToScienceRatio[0];
            }
            if (RnDnormLevel < 0.5f)
            {
                return dataToScienceRatio[1];
            }
            if (RnDnormLevel < 0.75f)
            {
                return dataToScienceRatio[2];
            }
            return dataToScienceRatio[3];
        }

        //public override string GetEVALockedReason(Vessel v, ProtoCrewMember crew)

        //public override float GetExperimentLevel(float RnDnormLevel)

        //public override float GetMentalityDeadlineFactor(float mentalityFactor, Contract.ContractPrestige prestige)

        //public override float GetMentalityExpiryFactor(float mentalityFactor, Contract.ContractPrestige prestige)

        //public override float GetMentalityFundsFactor(float mentalityFactor, Contract.ContractPrestige prestige)

        //public override float GetMentalityReputationFactor(float mentalityFactor, Contract.ContractPrestige prestige)

        //public override float GetMentalityScienceFactor(float mentalityFactor, Contract.ContractPrestige prestige)
        
        public override GameVariables.OrbitDisplayMode GetOrbitDisplayMode(float tsNormLevel)
        {
            if (orbitDisplayMode < 0)
                return base.GetOrbitDisplayMode(tsNormLevel);

            if (tsNormLevel < orbitDisplayMode)
            {
                return GameVariables.OrbitDisplayMode.AllOrbits;
            }
            return GameVariables.OrbitDisplayMode.PatchedConics;
        }
        
        public override int GetPartCountLimit(float editorNormLevel)
        {
            if (partCountLimit == null || partCountLimit.Length != 5)
                return base.GetPartCountLimit(editorNormLevel);

            if (editorNormLevel < 0.2f)
            {
                return partCountLimit[0];
            }
            if (editorNormLevel < 0.4f)
            {
                return partCountLimit[1];
            }
            if (editorNormLevel < 0.6f)
            {
                return partCountLimit[2];
            }
            if (editorNormLevel < 0.8f)
            {
                return partCountLimit[3];
            }
            return int.MaxValue;
        }
        
        public override int GetPatchesAheadLimit(float tsNormLevel)
        {
            if (patchesAheadLimit == null || patchesAheadLimit.Length != 3)
                return base.GetPatchesAheadLimit(tsNormLevel);

            if (tsNormLevel < 0.25f)
            {
                return Math.Min(patchesAheadLimit[0], GameSettings.CONIC_PATCH_LIMIT);
            }
            if (tsNormLevel < 0.5f)
            {
                return Math.Min(patchesAheadLimit[1], GameSettings.CONIC_PATCH_LIMIT);
            }
            if (tsNormLevel < 0.75f)
            {
                return Math.Min(patchesAheadLimit[3], GameSettings.CONIC_PATCH_LIMIT);
            }
            return GameSettings.CONIC_PATCH_LIMIT;
        }
        
        //public override float GetRecoveredPartValue(float pValue)

        //public override float GetRecoveredResourceValue(float rscValue)

        //public static float GetRecruitHireCost(int currentActive, float baseCost, float flatRate, float rateModifier)

        public override float GetRecruitHireCost(int currentActive)
        {
            return GameVariables.GetRecruitHireCost(currentActive, recruitHireBaseCost, recruitHireFlatRate, recruitHireRateModifier);
        }
        
        public override float GetScienceCostLimit(float RnDnormLevel)
        {
            if (scienceCostLimit == null || scienceCostLimit.Length != 4)
                return base.GetScienceCostLimit(RnDnormLevel);

            if (RnDnormLevel < 0.25f)
            {
                return scienceCostLimit[0];
            }
            if (RnDnormLevel < 0.5f)
            {
                return scienceCostLimit[1];
            }
            if (RnDnormLevel < 0.75f)
            {
                return scienceCostLimit[2];
            }
            return scienceCostLimit[3];
        }
        
        public override float GetStrategyCommitRange(float adminNormLevel)
        {
            if (strategyCommitRange == null || strategyCommitRange.Length != 2)
                return base.GetStrategyCommitRange(adminNormLevel);

            if (adminNormLevel < 0.3f)
            {
                return strategyCommitRange[0];
            }
            if (adminNormLevel < 0.6f)
            {
                return strategyCommitRange[1];
            }
            return 1f;
        }

        //public override float GetStrategyLevelLimit(float adminNormLevel)
        
        public override int GetTrackedObjectLimit(float tsNormLevel)
        {
            if (trackedObjectLimit == null || trackedObjectLimit.Length != 4)
                return base.GetTrackedObjectLimit(tsNormLevel);

            if (tsNormLevel < 0.25f)
            {
                return trackedObjectLimit[0];
            }
            if (tsNormLevel < 0.5f)
            {
                return trackedObjectLimit[1];
            }
            if (tsNormLevel < 0.75f)
            {
                return trackedObjectLimit[2];
            }
            return trackedObjectLimit[3];
        }

        
        // public override UntrackedObjectClass MinTrackedObjectSize(float tsNormLevel)
        
        //public override float ScoreFlightEnvelope(float altitude, float altEnvelope, float speed, float speedEnvelope)
        
        public override float ScoreSituation(Vessel.Situations sit, CelestialBody where)
        {
            if (scoreSituationHome == null || scoreSituationHome.Length != 8 ||
                scoreSituationOther == null || scoreSituationOther.Length != 8)
                return base.ScoreSituation(sit, where);

            float[] scoreSituation;
            if (where == Planetarium.fetch.Home)
            {
                scoreSituation = scoreSituationHome;
            }
            else if (scoreSituationCustom.ContainsKey(where.bodyName))  // bodyName can have space ?
            {
                scoreSituation = scoreSituationCustom[where.bodyName];
                if (scoreSituation.Length != 8)
                    scoreSituation = scoreSituationOther;
            }
            else
            {
                scoreSituation = scoreSituationOther;
            }

            switch (sit)
            {
                case Vessel.Situations.PRELAUNCH:
                    return scoreSituation[0];

                case Vessel.Situations.LANDED:
                    return scoreSituation[1];

                case Vessel.Situations.SPLASHED:
                    return scoreSituation[2];

                case Vessel.Situations.FLYING:
                    return scoreSituation[3];

                case Vessel.Situations.SUB_ORBITAL:
                    return scoreSituation[4];

                case Vessel.Situations.ORBITING:
                    return scoreSituation[5];

                case Vessel.Situations.ESCAPING:
                    return scoreSituation[6];

                case Vessel.Situations.DOCKED:
                    return scoreSituation[7];

                default:
                    return scoreSituation[7];
            }
        }
        public override bool UnlockedActionGroupsCustom(float editorNormLevel)
        {
            return editorNormLevel > actionGroupsCustomUnlock;
        }
        public override bool UnlockedActionGroupsStock(float editorNormLevel)
        {
            return editorNormLevel > actionGroupsStockUnlock;
        }
        public override bool UnlockedEVA(float astroComplexNormLevel)
        {
            return astroComplexNormLevel > unlockedEVA;
        }
        public override bool UnlockedEVAClamber(float astroComplexNormLevel)
        {
            return astroComplexNormLevel > unlockedEVAClamber;
        }
        public override bool UnlockedEVAFlags(float astroComplexNormLevel)
        {
            return astroComplexNormLevel >= unlockedEVAFlags;
        }
        public override bool UnlockedFlightPlanning(float mCtrlNormLevel)
        {
            return mCtrlNormLevel > unlockedFlightPlanning;
        }
        public override bool UnlockedFuelTransfer(float editorNormLevel)
        {
            return editorNormLevel > unlockedFuelTransfer;
        }
        public override bool UnlockedSpaceObjectDiscovery(float tsNormLevel)
        {
            return tsNormLevel > unlockedSpaceObjectDiscovery;
        }



        private static void LoadValue(ConfigNode node, string key, ref float param)
        {
            if (node.HasValue(key))
            {
                string s = node.GetValue(key);
                float val;
                if (float.TryParse(s, out val))
                {
                    param = val;
                }
                else
                {
                    CustomBarnKit.log("Fail to parse \"" + s + "\" into a float for key " + key);
                }
            }
        }

        private static void LoadValue(ConfigNode node, string key, ref int param)
        {
            if (node.HasValue(key))
            {
                string s = node.GetValue(key);
                int val;
                if (int.TryParse(s, out val))
                {
                    param = val;
                }
                else
                {
                    CustomBarnKit.log("Fail to parse \"" + s + "\" into an int for key " + key);
                }
            }
        }

        private static void LoadValue(ConfigNode node, string key, ref float[] param)
        {
            if (node.HasValue(key))
            {
                string s = node.GetValue(key);
                string[] split = s.Split(',');

                float[] result = new float[split.Length];

                for (int i = 0; i < split.Length; i++)
                {
                    string v = split[i];
                    if (!float.TryParse(v, out result[i]))
                    {
                        CustomBarnKit.log("Fail to parse \"" + s + "\" into a float array for key " + key);
                        return;
                    }
                }
                param = result;
            }
        }

        private static void LoadValue(ConfigNode node, string key, ref int[] param)
        {
            if (node.HasValue(key))
            {
                string s = node.GetValue(key);
                string[] split = s.Split(',');

                int[] result = new int[split.Length];

                for (int i = 0; i < split.Length; i++)
                {
                    string v = split[i];
                    if (!int.TryParse(v, out result[i]))
                    {
                        CustomBarnKit.log("Fail to parse \"" + s + "\" into an int array for key " + key);
                        return;
                    }
                }
                param = result;
            }
        }

        private static void LoadValue(ConfigNode node, string key, ref Vector3[] param)
        {
            if (node.HasNode(key))
            {
                ConfigNode subnode = node.GetNode(key);

                if (subnode.HasValue("size"))
                {
                    string[] values = subnode.GetValues("size");

                    Vector3[] result = new Vector3[values.Length];

                    for (int i = 0; i < values.Length; i++)
                    {
                        result[i] = ConfigNode.ParseVector3(values[i]);
                        if (result[i] == Vector3.zero)
                        {
                            CustomBarnKit.log("Fail to parse into a Vector array for key " + key + " the node\n" + subnode.ToString());
                            return;
                        }
                    }
                    param = result;
                }
            }
        }


    }
}