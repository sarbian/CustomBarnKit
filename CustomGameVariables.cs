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

        public readonly int editorLevel = 3;

        public readonly int launchPadLevel = 3;
        public readonly int runwayLevel = 3;
        public readonly float[] VABUpgrades;
        public readonly float[] SPHUpgrades;
        public readonly float[] LaunchPadUpgrades;
        public readonly float[] RunwayUpgrades;
        private readonly float actionGroupsCustomUnlock = 0.6f;
        private readonly float actionGroupsCustomUnlockSPH = 0.6f;
        private readonly float actionGroupsStockUnlock = 0.4f;
        private readonly float actionGroupsStockUnlockSPH = 0.4f;
        private readonly float unlockedFuelTransfer = 0.2f;
        private readonly float[] craftMassLimit;
        private readonly float[] craftMassLimitSPH;
        private readonly Vector3[] craftSizeLimit;
        private readonly Vector3[] craftSizeLimitSPH;
        private readonly int[] partCountLimit;
        private readonly int[] partCountLimitSPH;

        // Astronauts Complex
        public readonly int astronautLevel = 3;
        public readonly float[] astronautsUpgrades;
        private readonly float recruitHireBaseCost = 10000f;
        private readonly float recruitHireFlatRate = 1.25f;
        private readonly float recruitHireRateModifier = 0.015f;
        private readonly float unlockedEVA = 0.2f;
        private readonly float unlockedEVAClamber = 0.6f;
        private readonly float unlockedEVAFlags = 0.4f;
        private readonly int[] activeCrewLimit;
        private readonly float[] crewLevelLimit;
        private readonly bool recruitHireFixedRate = false;

        // Mission control
        public readonly int missionLevel = 3;
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
        public readonly int trackingLevel = 3;
        public readonly float[] trackingUpgrades;
        private readonly float unlockedSpaceObjectDiscovery = 0.6f;
        private readonly float orbitDisplayMode = -1;
        private readonly int[] patchesAheadLimit;
        private readonly int[] trackedObjectLimit;

        // Administration
        public readonly int administrationLevel = 3;
        public readonly float[] administrationUpgrades;
        private readonly int[] activeStrategyLimit;
        private readonly float[] strategyCommitRange;

        // R&D
        public readonly int rndLevel = 3;
        public readonly float[] rndUpgrades;
        private readonly float[] dataToScienceRatio;
        private readonly float[] scienceCostLimit;


        private static bool debug = false;

        public CustomGameVariables(GameVariables orig)
        {
            CustomBarnKit.log("Loading new career/science config");

#if DEBUG
            debug = true;
#endif

            original = orig;

            // Init our values with the one from the default config
            reputationAddition = orig.reputationAddition;
            reputationSubtraction = orig.reputationSubtraction;

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
                if (editor.HasValue("actionGroupsCustomUnlockSPH"))
                {
                    LoadValue(editor, "actionGroupsCustomUnlockSPH", ref actionGroupsCustomUnlockSPH);
                }
                else
                {
                    actionGroupsCustomUnlockSPH = actionGroupsCustomUnlock;
                }

                LoadValue(editor, "actionGroupsStockUnlock", ref actionGroupsStockUnlock);
                if (editor.HasValue("actionGroupsStockUnlockSPH"))
                {
                    LoadValue(editor, "actionGroupsStockUnlockSPH", ref actionGroupsStockUnlockSPH);
                }
                else
                {
                    actionGroupsStockUnlockSPH = actionGroupsStockUnlock;
                }
                LoadValue(editor, "unlockedFuelTransfer", ref unlockedFuelTransfer);

                LoadValue(editor, "craftMassLimit", ref craftMassLimit);
                if (editor.HasValue("craftMassLimitSPH"))
                {
                    LoadValue(editor, "craftMassLimitSPH", ref craftMassLimitSPH);
                }
                else
                {
                    craftMassLimitSPH = craftMassLimit;
                }

                LoadValue(editor, "craftSizeLimit", ref craftSizeLimit);
                if (editor.HasValue("craftSizeLimitSPH"))
                {
                    LoadValue(editor, "craftSizeLimitSPH", ref craftSizeLimitSPH);
                }
                else
                {
                    craftSizeLimitSPH = craftSizeLimit;
                }

                LoadValue(editor, "partCountLimit", ref partCountLimit);
                if (editor.HasValue("partCountLimitSPH"))
                {
                    LoadValue(editor, "partCountLimitSPH", ref partCountLimitSPH);
                }
                else
                {
                    partCountLimitSPH = partCountLimit;
                }
            }

            if (config.HasNode("ASTRONAUTS"))
            {
                ConfigNode editor = config.GetNode("ASTRONAUTS");

                LoadValue(editor, "upgrades", ref astronautsUpgrades);

                LoadValue(editor, "recruitHireBaseCost", ref recruitHireBaseCost);
                LoadValue(editor, "recruitHireFlatRate", ref recruitHireFlatRate);
                LoadValue(editor, "recruitHireRateModifier", ref recruitHireRateModifier);
                LoadValue(editor, "recruitHireFixedRate", ref recruitHireFixedRate);
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
            if (activeContractsLimit == null || activeContractsLimit.Length != missionLevel)
            {
                debugLog("activeContractsLimit wrong size");
                return original.GetActiveContractsLimit(mCtrlNormLevel);
            }

            return NormLevelToArrayValue(mCtrlNormLevel, activeContractsLimit);
        }

        public override int GetActiveCrewLimit(float astroComplexNormLevel)
        {
            if (activeCrewLimit == null || activeCrewLimit.Length != astronautLevel)
            {
                debugLog("activeCrewLimit wrong size");
                return original.GetActiveCrewLimit(astroComplexNormLevel);
            }

            return NormLevelToArrayValue(astroComplexNormLevel, activeCrewLimit);
        }

        public override int GetActiveStrategyLimit(float adminNormLevel)
        {
            if (activeStrategyLimit == null || activeStrategyLimit.Length != administrationLevel)
            {
                debugLog("activeStrategyLimit wrong size");
                return original.GetActiveStrategyLimit(adminNormLevel);
            }
            return NormLevelToArrayValue(adminNormLevel, activeStrategyLimit);
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

        public override float GetCraftMassLimit(float editorNormLevel, bool isPad = true)
        {
            if (craftMassLimit == null || craftMassLimit.Length != editorLevel)
            {
                debugLog("craftMassLimit wrong size");
                return original.GetCraftMassLimit(editorNormLevel, isPad);
            }

            return NormLevelToArrayValue(editorNormLevel, isPad ? craftMassLimit : craftMassLimitSPH);
        }

        public override Vector3 GetCraftSizeLimit(float editorNormLevel, bool isPad = true)
        {
            if (craftSizeLimit == null || craftSizeLimit.Length != editorLevel)
            {
                debugLog("craftSizeLimit wrong size");
                return original.GetCraftSizeLimit(editorNormLevel, isPad);
            }

            return NormLevelToArrayValue(editorNormLevel, isPad ? craftSizeLimit : craftSizeLimitSPH);
        }

        public override float GetCrewLevelLimit(float astroComplexNormLevel)
        {
            if (crewLevelLimit == null || crewLevelLimit.Length != astronautLevel)
            {
                debugLog("crewLevelLimit wrong size");
                return original.GetCrewLevelLimit(astroComplexNormLevel);
            }

            return NormLevelToArrayValue(astroComplexNormLevel, crewLevelLimit);
        }

        public override float GetDataToScienceRatio(float RnDnormLevel)
        {
            if (dataToScienceRatio == null || dataToScienceRatio.Length != rndLevel)
            {
                debugLog("dataToScienceRatio wrong size");
                return original.GetDataToScienceRatio(RnDnormLevel);
            }

            return NormLevelToArrayValue(RnDnormLevel, dataToScienceRatio);
        }

        //public override string GetEVALockedReason(Vessel v, ProtoCrewMember crew)

        //public override float GetExperimentLevel(float RnDnormLevel)

        //public override float GetMentalityDeadlineFactor(float mentalityFactor, Contract.ContractPrestige prestige)

        //public override float GetMentalityExpiryFactor(float mentalityFactor, Contract.ContractPrestige prestige)

        //public override float GetMentalityFundsFactor(float mentalityFactor, Contract.ContractPrestige prestige)

        //public override float GetMentalityReputationFactor(float mentalityFactor, Contract.ContractPrestige prestige)

        //public override float GetMentalityScienceFactor(float mentalityFactor, Contract.ContractPrestige prestige)

        public override int GetPartCountLimit(float editorNormLevel, bool isVAB = true)
        {
            if (partCountLimit == null || partCountLimit.Length != editorLevel)
            {
                debugLog("partCountLimit wrong size");
                return original.GetPartCountLimit(editorNormLevel, isVAB);
            }

            return NormLevelToArrayValue(editorNormLevel, isVAB ? partCountLimit : partCountLimitSPH);
        }

        public override int GetPatchesAheadLimit(float tsNormLevel)
        {
            if (patchesAheadLimit == null || patchesAheadLimit.Length != trackingLevel)
            {
                debugLog("patchesAheadLimit wrong size");
                return original.GetPatchesAheadLimit(tsNormLevel);
            }

            return NormLevelToArrayValue(tsNormLevel, patchesAheadLimit);
        }

        //public override float GetRecoveredPartValue(float pValue)

        //public override float GetRecoveredResourceValue(float rscValue)

        //public static float GetRecruitHireCost(int currentActive, float baseCost, float flatRate, float rateModifier)

        public override float GetRecruitHireCost(int currentActive)
        {
            return recruitHireFixedRate
                ? recruitHireBaseCost
                : GameVariables.GetRecruitHireCost(currentActive, recruitHireBaseCost, recruitHireFlatRate,
                    recruitHireRateModifier);
        }

        public override float GetScienceCostLimit(float RnDnormLevel)
        {
            if (scienceCostLimit == null || scienceCostLimit.Length != rndLevel)
            {
                debugLog("scienceCostLimit wrong size");
                return original.GetScienceCostLimit(RnDnormLevel);
            }

            return NormLevelToArrayValue(RnDnormLevel, scienceCostLimit);
        }

        public override float GetStrategyCommitRange(float adminNormLevel)
        {
            if (strategyCommitRange == null || strategyCommitRange.Length != administrationLevel)
            {
                debugLog("strategyCommitRange wrong size");
                return original.GetStrategyCommitRange(adminNormLevel);
            }

            return NormLevelToArrayValue(adminNormLevel, strategyCommitRange);
        }

        //public override float GetStrategyLevelLimit(float adminNormLevel)

        public override int GetTrackedObjectLimit(float tsNormLevel)
        {
            if (trackedObjectLimit == null || trackedObjectLimit.Length != trackingLevel)
            {
                debugLog("trackedObjectLimit wrong size");
                return original.GetTrackedObjectLimit(tsNormLevel);
            }

            return NormLevelToArrayValue(tsNormLevel, trackedObjectLimit);
        }

        // public override UntrackedObjectClass MinTrackedObjectSize(float tsNormLevel)

        //public override float ScoreFlightEnvelope(float altitude, float altEnvelope, float speed, float speedEnvelope)


        public override float ScoreSituation(Vessel.Situations sit, CelestialBody where)
        {
            if (scoreSituationHome == null || scoreSituationHome.Length != 8 ||
                scoreSituationOther == null || scoreSituationOther.Length != 8)
            {
                debugLog("scoreSituationHome wrong size");
                return original.ScoreSituation(sit, where);
            }

            float[] scoreSituation;
            if (where == Planetarium.fetch.Home)
            {
                scoreSituation = scoreSituationHome;
            }
            else if (scoreSituationCustom.ContainsKey(where.bodyName)) // bodyName can have space ?
            {
                scoreSituation = scoreSituationCustom[where.bodyName];
                if (scoreSituation.Length != 8)
                {
                    scoreSituation = scoreSituationOther;
                }
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

        public override bool UnlockedActionGroupsCustom(float editorNormLevel, bool isVAB = true)
        {
            return editorNormLevel > (isVAB ? actionGroupsCustomUnlock : actionGroupsCustomUnlockSPH - 1.1) / (editorLevel - 1);
        }

        public override bool UnlockedActionGroupsStock(float editorNormLevel, bool isVAB = true)
        {
            return editorNormLevel > (isVAB ? actionGroupsStockUnlock : actionGroupsStockUnlockSPH - 1.1) / (editorLevel - 1);
        }

        public override bool UnlockedEVA(float astroComplexNormLevel)
        {
            return astroComplexNormLevel > (unlockedEVA - 1.1) / (astronautLevel - 1);
        }

        public override bool UnlockedEVAClamber(float astroComplexNormLevel)
        {
            return astroComplexNormLevel > (unlockedEVAClamber - 1.1) / (astronautLevel - 1);
        }

        public override bool UnlockedEVAFlags(float astroComplexNormLevel)
        {
            return astroComplexNormLevel >= (unlockedEVAFlags - 1.1) / (astronautLevel - 1);
        }

        public override bool UnlockedFlightPlanning(float mCtrlNormLevel)
        {
            return mCtrlNormLevel > (unlockedFlightPlanning - 1.1) / (missionLevel - 1);
        }

        public override bool UnlockedFuelTransfer(float editorNormLevel)
        {
            return editorNormLevel > (unlockedFuelTransfer - 1.1) / (editorLevel - 1);
        }

        public override bool UnlockedSpaceObjectDiscovery(float tsNormLevel)
        {
            return tsNormLevel > (unlockedSpaceObjectDiscovery - 1.1) / (trackingLevel - 1);
        }

        public override OrbitDisplayMode GetOrbitDisplayMode(float tsNormLevel)
        {
            if (orbitDisplayMode < 0)
            {
                return original.GetOrbitDisplayMode(tsNormLevel);
            }

            if (tsNormLevel < (orbitDisplayMode - 1.1) / (trackingLevel - 1))
            {
                return OrbitDisplayMode.AllOrbits;
            }
            return OrbitDisplayMode.PatchedConics;
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
            sb.AppendLine("actionGroupsCustomUnlockSPH        " + actionGroupsCustomUnlockSPH.ToString("F2"));
            sb.AppendLine("actionGroupsStockUnlock            " + actionGroupsStockUnlock.ToString("F2"));
            sb.AppendLine("actionGroupsStockUnlockSPH         " + actionGroupsStockUnlockSPH.ToString("F2"));
            sb.AppendLine("unlockedFuelTransfer               " + unlockedFuelTransfer.ToString("F2"));
            sb.AppendLine("craftMassLimit                     " + DumpArray(craftMassLimit));
            sb.AppendLine("craftMassLimitSPH                  " + DumpArray(craftMassLimitSPH));
            sb.AppendLine("craftSizeLimit                     " + DumpArray(craftSizeLimit));
            sb.AppendLine("craftSizeLimitSPH                  " + DumpArray(craftSizeLimitSPH));
            sb.AppendLine("partCountLimit                     " + DumpArray(partCountLimit));
            sb.AppendLine("partCountLimitSPH                  " + DumpArray(partCountLimitSPH));

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

        private void debugLog(string s)
        {
            if (debug)
            {
                CustomBarnKit.log(s);
            }
        }
        
        private string DumpArray<T>(IEnumerable<T> array)
        {
            return array != null ? string.Join(", ", array.Select(x => x.ToString()).ToArray()) : "null";
        }

        private string DumpArray(IEnumerable<float> array)
        {
            return array != null ? string.Join(", ", array.Select(x => x.ToString("F2")).ToArray()) : "null";
        }

        private static T NormLevelToArrayValue<T>(float normLevel, T[] array)
        {
            int index = Mathf.RoundToInt(normLevel * (array.Length - 1));
            index = Mathf.Clamp(index, 0, array.Length - 1); // better safe than sorry
            return array[index];
        }

        private static void LoadValue(ConfigNode node, string key, ref bool param)
        {
            if (node.HasValue(key))
            {
                string s = node.GetValue(key);
                bool val;
                if (bool.TryParse(s, out val))
                {
                    param = val;
                }
                else
                {
                    CustomBarnKit.log("Fail to parse \"" + s + "\" into a param for key " + key);
                }
            }
            else if (debug)
            {
                CustomBarnKit.log("No value " + key);
            }
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
            else if (debug)
            {
                CustomBarnKit.log("No value " + key);
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
                    param = val != -1 ? val : int.MaxValue;
                }
                else
                {
                    CustomBarnKit.log("Fail to parse \"" + s + "\" into an int for key " + key);
                }
            }
            else if (debug)
            {
                CustomBarnKit.log("No value " + key);
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
            else if (debug)
            {
                CustomBarnKit.log("No value " + key);
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
            else if (debug)
            {
                CustomBarnKit.log("No value " + key);
            }
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
                        {
                            result[i] = maxVector3d;
                        }

                        if (result[i] == Vector3.zero)
                        {
                            CustomBarnKit.log("Fail to parse into a Vector array for key " + key + " the node\n" + subnode.ToString());
                            return;
                        }
                    }
                    param = result;
                }
            }
            else if (debug)
            {
                CustomBarnKit.log("No node " + key);
            }
        }
        
        private void Test<Y,T>(Func<Y, T> orig, Func<Y, T> repl, Y lvl, StringBuilder sb)
        {
            if (!Equals(repl(lvl), orig(lvl)))
            {
                sb.AppendLine("Diff for " + repl.Method.Name + " at level " + lvl + " expected " + orig(lvl) + " and got " + repl(lvl));
            }
        }

        private void Test<Y, T>(Func<Y, bool, T> orig, Func<Y, bool, T> repl, Y lvl, bool b, StringBuilder sb)
        {
            if (!Equals(repl(lvl, b), orig(lvl, b)))
            {
                sb.AppendLine("Diff for " + repl.Method.Name + " at level " + lvl + " expected " + orig(lvl, b) + " and got " + repl(lvl, b));
            }
        }

        internal void Test()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Starting Test for CustomGameVariables");

            foreach (ScenarioUpgradeableFacilities.ProtoUpgradeable facility in ScenarioUpgradeableFacilities.protoUpgradeables.Values)
            {
                if (facility.facilityRefs.Count > 0)
                    sb.AppendLine(facility.facilityRefs.First().id.ToString() + " lvl " + facility.facilityRefs.First().FacilityLevel + " / " + facility.facilityRefs.First().MaxLevel);
            }

            for (int lvl = 0; lvl < missionLevel; lvl++)
            {
                float nLevel = lvl / (float)(missionLevel - 1);

                Test(original.GetActiveContractsLimit, GetActiveContractsLimit, nLevel, sb);
                Test(original.UnlockedFlightPlanning, UnlockedFlightPlanning, nLevel, sb);
            }

            for (int lvl = 0; lvl < administrationLevel; lvl++)
            {
                float nLevel = lvl / (float)(administrationLevel - 1);

                Test(original.GetActiveStrategyLimit, GetActiveStrategyLimit, nLevel, sb);
                Test(original.GetStrategyCommitRange, GetStrategyCommitRange, nLevel, sb);
            }

            for (int lvl = 0; lvl < editorLevel; lvl++)
            {
                float nLevel = lvl / (float)(editorLevel - 1);

                Test(original.GetCraftMassLimit, GetCraftMassLimit, nLevel, true, sb);
                Test(original.GetCraftMassLimit, GetCraftMassLimit, nLevel, false, sb);
                Test(original.GetCraftSizeLimit, GetCraftSizeLimit, nLevel, true, sb);
                Test(original.GetCraftSizeLimit, GetCraftSizeLimit, nLevel, false, sb);
                Test(original.UnlockedActionGroupsCustom, UnlockedActionGroupsCustom, nLevel, true, sb);
                Test(original.UnlockedActionGroupsCustom, UnlockedActionGroupsCustom, nLevel, false, sb);
                Test(original.UnlockedActionGroupsStock, UnlockedActionGroupsStock, nLevel, true, sb);
                Test(original.UnlockedActionGroupsStock, UnlockedActionGroupsStock, nLevel, false, sb);
                Test(original.UnlockedFuelTransfer, UnlockedFuelTransfer, nLevel, sb);
                Test(original.GetPartCountLimit, GetPartCountLimit, nLevel, true, sb);
                Test(original.GetPartCountLimit, GetPartCountLimit, nLevel, false, sb);
            }

            for (int lvl = 0; lvl < trackingLevel; lvl++)
            {
                float nLevel = lvl / (float)(trackingLevel - 1);
                Test(original.GetOrbitDisplayMode, GetOrbitDisplayMode, nLevel, sb);
                Test(original.GetTrackedObjectLimit, GetTrackedObjectLimit, nLevel, sb);
                Test(original.GetPatchesAheadLimit, GetPatchesAheadLimit, nLevel, sb);
                Test(original.UnlockedSpaceObjectDiscovery, UnlockedSpaceObjectDiscovery, nLevel, sb);

            }

            for (int lvl = 0; lvl < rndLevel; lvl++)
            {
                float nLevel = lvl / (float)(rndLevel - 1);
                Test(original.GetScienceCostLimit, GetScienceCostLimit, nLevel, sb);
                Test(original.GetDataToScienceRatio, GetDataToScienceRatio, nLevel, sb);
            }

            for (int lvl = 0; lvl < 100; lvl++)
            {
                Test(original.GetRecruitHireCost, GetRecruitHireCost, lvl, sb);
            }


            foreach (CelestialBody bodies in FlightGlobals.Bodies)
            {
                foreach (Vessel.Situations situation in Enum.GetValues(typeof(Vessel.Situations)))
                {
                    if (original.ScoreSituation(situation, bodies) != ScoreSituation(situation, bodies))
                    {
                        debugLog("Diff for ScoreSituation on " + bodies.name + " sit " + situation + " expected " + original.ScoreSituation(situation, bodies) + " and got " + ScoreSituation(situation, bodies));
                    }
                }
            }

            for (int lvl = 0; lvl < astronautLevel; lvl++)
            {
                float nLevel = lvl / (float)(astronautLevel - 1);

                Test(original.GetActiveCrewLimit, GetActiveCrewLimit, nLevel, sb);
                Test(original.GetCrewLevelLimit, GetCrewLevelLimit, nLevel, sb);
                Test(original.UnlockedEVA, UnlockedEVA, nLevel, sb);
                Test(original.UnlockedEVAClamber, UnlockedEVAClamber, nLevel, sb);
                Test(original.UnlockedEVAFlags, UnlockedEVAFlags, nLevel, sb);
            }

            debugLog(sb.ToString());
        }
    }
}