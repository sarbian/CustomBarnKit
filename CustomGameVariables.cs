using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public readonly GameVariables original;

        // Editor
        public readonly float[] VABUpgrades;
        public readonly float[] SPHUpgrades;
        public readonly float[] LaunchPadUpgrades;
        public readonly float[] RunwayUpgrades;
        private readonly float actionGroupsCustomUnlock = 0.6f;
        private readonly float actionGroupsStockUnlock = 0.4f;
        private readonly float unlockedFuelTransfer = 0.2f;
        private readonly float[] craftMassLimit;
        private readonly Vector3[] craftSizeLimit;
        private readonly int[] partCountLimit;

        // Astronauts Complex
        public readonly float[] astronautsUpgrades;
        private readonly float recruitHireBaseCost = 10000f;
        private readonly float recruitHireFlatRate = 1.25f;
        private readonly float recruitHireRateModifier = 0.015f;
        private readonly float unlockedEVA = 0.2f;
        private readonly float unlockedEVAClamber = 0.6f;
        private readonly float unlockedEVAFlags = 0.4f;
        private readonly int[] activeCrewLimit;
        private readonly float[] crewLevelLimit;

        // Mission control
        public readonly float[] missionUpgrades;
        private readonly float unlockedFlightPlanning = 0.4f;
        private readonly int[] activeContractsLimit;
        // Not used for now
        private Dictionary<string, float[]> scoreSituationCustom = new Dictionary<string, float[]>();
        private readonly float[] scoreSituationHome;
        private readonly float[] scoreSituationOther;

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
        public readonly float[] trackingUpgrades;
        private readonly float unlockedSpaceObjectDiscovery = 0.6f;
        private readonly float orbitDisplayMode = -1;
        private readonly int[] patchesAheadLimit;
        private readonly int[] trackedObjectLimit;

        // Administration
        public readonly float[] administrationUpgrades;
        private readonly int[] activeStrategyLimit;
        private readonly float[] strategyCommitRange;

        // R&D
        public readonly float[] rndUpgrades;
        private readonly float[] dataToScienceRatio;
        private readonly float[] scienceCostLimit;

        public CustomGameVariables(GameVariables orig)
        {
            CustomBarnKit.log("Loading new career/science config");

            original = orig;

            // Init our values with the one from the default config
            reputationAddition = orig.reputationAddition;
            reputationSubtraction = orig.reputationSubtraction;
            mentalityFundsTrivial = orig.mentalityFundsTrivial;
            mentalityFundsSignificant = orig.mentalityFundsSignificant;
            mentalityFundsExceptional = orig.mentalityFundsExceptional;
            mentalityReputationTrivial = orig.mentalityReputationTrivial;
            mentalityReputationSignificant = orig.mentalityReputationSignificant;
            mentalityReputationExceptional = orig.mentalityReputationExceptional;
            mentalityScienceTrivial = orig.mentalityScienceTrivial;
            mentalityScienceSignificant = orig.mentalityScienceSignificant;
            mentalityScienceExceptional = orig.mentalityScienceExceptional;
            mentalityExpiryTrivial = orig.mentalityExpiryTrivial;
            mentalityExpirySignificant = orig.mentalityExpirySignificant;
            mentalityExpiryExceptional = orig.mentalityExpiryExceptional;
            mentalityDeadlineTrivial = orig.mentalityDeadlineTrivial;
            mentalityDeadlineSignificant = orig.mentalityDeadlineSignificant;
            mentalityDeadlineExceptional = orig.mentalityDeadlineExceptional;

            partRecoveryValueFactor = orig.partRecoveryValueFactor;
            resourceRecoveryValueFactor = orig.resourceRecoveryValueFactor;
            reputationKerbalDeath = orig.reputationKerbalDeath;
            reputationKerbalRecovery = orig.reputationKerbalRecovery;

            contractPrestigeTrivial = orig.contractPrestigeTrivial;
            contractPrestigeSignificant = orig.contractPrestigeSignificant;
            contractPrestigeExceptional = orig.contractPrestigeExceptional;

            contractDestinationWeight = orig.contractDestinationWeight;
            contractFundsAdvanceFactor = orig.contractFundsAdvanceFactor;
            contractFundsCompletionFactor = orig.contractFundsCompletionFactor;
            contractFundsFailureFactor = orig.contractFundsFailureFactor;
            contractReputationCompletionFactor = orig.contractReputationCompletionFactor;
            contractReputationFailureFactor = orig.contractReputationFailureFactor;
            contractScienceCompletionFactor = orig.contractScienceCompletionFactor; 

        
            ConfigNode[] configs = GameDatabase.Instance.GetConfigNodes("CUSTOMBARNKIT");
        
            if (configs == null || configs.Length == 0)
            {
                CustomBarnKit.log("No config to load");
                return;
            }
        
            if (configs.Length > 1)
            {
                CustomBarnKit.log("More than 1 CustomBarnKit node found. Loading the first one");
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
                return original.GetActiveContractsLimit(mCtrlNormLevel);
        
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
                return original.GetActiveCrewLimit(astroComplexNormLevel);
        
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
                return original.GetActiveStrategyLimit(adminNormLevel);
        
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
                return original.GetCraftMassLimit(editorNormLevel);

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
                return original.GetCraftSizeLimit(editorNormLevel);

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
                return original.GetCrewLevelLimit(astroComplexNormLevel);

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
                return original.GetDataToScienceRatio(RnDnormLevel);

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
        
        // OK

        public override GameVariables.OrbitDisplayMode GetOrbitDisplayMode(float tsNormLevel)
        {
            if (orbitDisplayMode < 0)
                return original.GetOrbitDisplayMode(tsNormLevel);

            if (tsNormLevel < orbitDisplayMode)
            {
                return GameVariables.OrbitDisplayMode.AllOrbits;
            }
            return GameVariables.OrbitDisplayMode.PatchedConics;
        }
        
        public override int GetPartCountLimit(float editorNormLevel)
        {
            if (partCountLimit == null || partCountLimit.Length != 5)
                return original.GetPartCountLimit(editorNormLevel);

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
                return original.GetPatchesAheadLimit(tsNormLevel);

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
                return original.GetScienceCostLimit(RnDnormLevel);

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
                return original.GetStrategyCommitRange(adminNormLevel);

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
                return original.GetTrackedObjectLimit(tsNormLevel);

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
                return original.ScoreSituation(sit, where);

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

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("CustomGameVariables");
            sb.AppendLine("VABUpgrades                        " + DumpArray(VABUpgrades));
            sb.AppendLine("SPHUpgrades                        " + DumpArray(SPHUpgrades));
            sb.AppendLine("LaunchPadUpgrades                  " + DumpArray(LaunchPadUpgrades));
            sb.AppendLine("RunwayUpgrades                     " + DumpArray(RunwayUpgrades));
            sb.AppendLine("actionGroupsCustomUnlock           " + actionGroupsCustomUnlock.ToString("F2"));
            sb.AppendLine("actionGroupsStockUnlock            " + actionGroupsStockUnlock.ToString("F2"));
            sb.AppendLine("unlockedFuelTransfer               " + unlockedFuelTransfer.ToString("F2"));
            sb.AppendLine("craftMassLimit                     " + DumpArray(craftMassLimit));
            sb.AppendLine("craftSizeLimit                     " + DumpArray(craftSizeLimit));
            sb.AppendLine("partCountLimit                     " + DumpArray(partCountLimit));
                                                              
            sb.AppendLine("astronautsUpgrades                 " + DumpArray(astronautsUpgrades));
            sb.AppendLine("recruitHireBaseCost                " + recruitHireBaseCost.ToString("F2"));
            sb.AppendLine("recruitHireFlatRate                " + recruitHireFlatRate.ToString("F2"));
            sb.AppendLine("recruitHireRateModifier            " + recruitHireRateModifier.ToString("F2"));
            sb.AppendLine("unlockedEVA                        " + unlockedEVA.ToString("F2"));
            sb.AppendLine("unlockedEVAClamber                 " + unlockedEVAClamber.ToString("F2"));
            sb.AppendLine("unlockedEVAFlags                   " + unlockedEVAFlags.ToString("F2"));
            sb.AppendLine("activeCrewLimit                    " + DumpArray(activeCrewLimit));
            sb.AppendLine("crewLevelLimit                     " + DumpArray(crewLevelLimit));
                                                              
            sb.AppendLine("missionUpgrades                    " + DumpArray(missionUpgrades));
            sb.AppendLine("unlockedFlightPlanning             " + unlockedFlightPlanning.ToString("F2"));
            sb.AppendLine("activeContractsLimit               " + DumpArray(activeContractsLimit));
            sb.AppendLine("scoreSituationCustom               " + DumpArray(scoreSituationCustom));
            sb.AppendLine("scoreSituationHome                 " + DumpArray(scoreSituationHome));
            sb.AppendLine("scoreSituationOther                " + DumpArray(scoreSituationOther));
            sb.AppendLine("partRecoveryValueFactor            " + partRecoveryValueFactor.ToString("F2"));
            sb.AppendLine("resourceRecoveryValueFactor        " + resourceRecoveryValueFactor.ToString("F2"));
            sb.AppendLine("reputationKerbalDeath              " + reputationKerbalDeath.ToString("F2"));
            sb.AppendLine("reputationKerbalRecovery           " + reputationKerbalRecovery.ToString("F2"));
            sb.AppendLine("contractPrestigeTrivial            " + contractPrestigeTrivial.ToString("F2"));
            sb.AppendLine("contractPrestigeSignificant        " + contractPrestigeSignificant.ToString("F2"));
            sb.AppendLine("contractPrestigeExceptional        " + contractPrestigeExceptional.ToString("F2"));
            sb.AppendLine("contractDestinationWeight          " + contractDestinationWeight.ToString("F2"));
            sb.AppendLine("contractFundsAdvanceFactor         " + contractFundsAdvanceFactor.ToString("F2"));
            sb.AppendLine("contractFundsCompletionFactor      " + contractFundsCompletionFactor.ToString("F2"));
            sb.AppendLine("contractFundsFailureFactor         " + contractFundsFailureFactor.ToString("F2"));
            sb.AppendLine("contractReputationCompletionFactor " + contractReputationCompletionFactor.ToString("F2"));
            sb.AppendLine("contractReputationFailureFactor    " + contractReputationFailureFactor.ToString("F2"));
            sb.AppendLine("contractScienceCompletionFactor    " + contractScienceCompletionFactor.ToString("F2"));

            sb.AppendLine("trackingUpgrades                   " + DumpArray(trackingUpgrades));
            sb.AppendLine("unlockedSpaceObjectDiscovery       " + unlockedSpaceObjectDiscovery.ToString("F2"));
            sb.AppendLine("orbitDisplayMode                   " + orbitDisplayMode.ToString("F2"));
            sb.AppendLine("patchesAheadLimit                  " + DumpArray(patchesAheadLimit));
            sb.AppendLine("trackedObjectLimit                 " + DumpArray(trackedObjectLimit));

            sb.AppendLine("administrationUpgrades             " + DumpArray(administrationUpgrades));
            sb.AppendLine("activeStrategyLimit                " + DumpArray(activeStrategyLimit));
            sb.AppendLine("strategyCommitRange                " + DumpArray(strategyCommitRange));


            sb.AppendLine("rndUpgrades                        " + DumpArray(rndUpgrades));
            sb.AppendLine("dataToScienceRatio                 " + DumpArray(dataToScienceRatio));
            sb.AppendLine("scienceCostLimit                   " + DumpArray(scienceCostLimit));
            
           return sb.ToString();
            

        }


        private string DumpArray<T>(IEnumerable<T> array)
        {
            return array != null ? string.Join(", ", array.Select(x => x.ToString()).ToArray()) : "null";
        }

        private string DumpArray(IEnumerable<float> array)
        {
            return array != null ? string.Join(", ", array.Select(x => x.ToString("F2")).ToArray()) : "null";
        }


        private static void LoadValue(ConfigNode node, string key, ref float param)
        {
            if (node.HasValue(key))
            {
                string s = node.GetValue(key);
                float val;
                if (float.TryParse(s, out val))
                {
                    param = val >= 0 ? val : float.MaxValue;
                }
                else
                {
                    CustomBarnKit.log("Fail to parse \"" + s + "\" into a float for key " + key);
                }
            }
            //else
            //{
            //    CustomBarnKit.log("No value " + key);
            //}
        }

        private static void LoadValue(ConfigNode node, string key, ref int param)
        {
            if (node.HasValue(key))
            {
                string s = node.GetValue(key);
                int val;
                if (int.TryParse(s, out val))
                {
                    param = val != -1 ? val : int.MaxValue;
                }
                else
                {
                    CustomBarnKit.log("Fail to parse \"" + s + "\" into an int for key " + key);
                }
            }
            //else
            //{
            //    CustomBarnKit.log("No value " + key);
            //}
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
                    float val;
                    if (float.TryParse(v, out val))
                    {
                        result[i] = val >= 0 ? val : float.MaxValue;
                    }
                    else
                    {
                        CustomBarnKit.log("Fail to parse \"" + s + "\" into a float array for key " + key);
                        return;
                    }
                }
                param = result;
            }
            //else
            //{
            //    CustomBarnKit.log("No value " + key);
            //}
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
                    int val;
                    if (int.TryParse(v, out val))
                    {
                        result[i] = val >= 0 ? val : int.MaxValue;
                    }
                    else
                    {
                        CustomBarnKit.log("Fail to parse \"" + s + "\" into an int array for key " + key);
                        return;
                    }
                }
                param = result;
            }
            //else
            //{
            //    CustomBarnKit.log("No value " + key);
            //}
        }

        private static readonly Vector3 maxVector3d = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

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

                        if (result[i].x < 0 && result[i].y < 0 && result[i].z < 0)
                            result[i] = maxVector3d;

                        if (result[i] == Vector3.zero)
                        {
                            CustomBarnKit.log("Fail to parse into a Vector array for key " + key + " the node\n" + subnode.ToString());
                            return;
                        }
                    }
                    param = result;
                }
            }
            //else
            //{
            //    CustomBarnKit.log("No node " + key);
            //}
        }


    }
}