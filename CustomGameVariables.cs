using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contracts;
using UnityEngine;

namespace CustomBarnKit
{
    public class CustomGameVariables : GameVariables
    {

        //public static CustomGameVariables Instance;

        //public AnimationCurve reputationAddition;
        //
        //public AnimationCurve reputationSubtraction;
        //
        //public float reputationKerbalDeath = 10f;
        //
        //public float reputationKerbalRecovery = 25f;
        //
        //public float partRecoveryValueFactor = 0.9f;
        //
        //public float resourceRecoveryValueFactor = 0.95f;
        //
        //public float contractDestinationWeight = 1f;
        //
        //public float contractPrestigeTrivial = 1f;
        //
        //public float contractPrestigeSignificant = 1.25f;
        //
        //public float contractPrestigeExceptional = 1.5f;
        //
        //public float contractFundsAdvanceFactor = 1f;
        //
        //public float contractFundsCompletionFactor = 1f;
        //
        //public float contractFundsFailureFactor = 1f;
        //
        //public float contractReputationCompletionFactor = 1f;
        //
        //public float contractReputationFailureFactor = 1f;
        //
        //public float contractScienceCompletionFactor = 1f;
        //
        //public float mentalityFundsTrivial = 1.1f;
        //
        //public float mentalityFundsSignificant = 1.2f;
        //
        //public float mentalityFundsExceptional = 1.3f;
        //
        //public float mentalityReputationTrivial = 1.1f;
        //
        //public float mentalityReputationSignificant = 1.2f;
        //
        //public float mentalityReputationExceptional = 1.3f;
        //
        //public float mentalityScienceTrivial = 1.1f;
        //
        //public float mentalityScienceSignificant = 1.2f;
        //
        //public float mentalityScienceExceptional = 1.3f;
        //
        //public float mentalityExpiryTrivial = 1.1f;
        //
        //public float mentalityExpirySignificant = 1.2f;
        //
        //public float mentalityExpiryExceptional = 1.3f;
        //
        //public float mentalityDeadlineTrivial = 1.1f;
        //
        //public float mentalityDeadlineSignificant = 1.2f;
        //
        //public float mentalityDeadlineExceptional = 1.3f;


        //private void Awake()
        //{
        //    if (CustomGameVariables.Instance == null)
        //    {
        //        CustomGameVariables.Instance = this;
        //    }
        //}

        //public override bool EVAIsPossible(bool evaUnlocked, Vessel v)

        public int[] activeContractsLimit;

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
            return int.MaxValue;
        }

        public int[] activeCrewLimit;

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
            return int.MaxValue;
        }

        public int[] activeStrategyLimit;

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


        public float[] craftMassLimit;

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
            return Single.MaxValue;
        }

        public Vector3[] craftSizeLimit;

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
            return new Vector3(Single.MaxValue, Single.MaxValue, Single.MaxValue);
        }

        public float[] crewLevelLimit;

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
            return Single.MaxValue;
        }

        public float[] dataToScienceRatio;

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


        public float orbitDisplayMode = -1;
        
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

        public int[] partCountLimit;

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

        public int[] patchesAheadLimit;

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
                return  Math.Min(patchesAheadLimit[1], GameSettings.CONIC_PATCH_LIMIT);
            }
            if (tsNormLevel < 0.75f)
            {
                return  Math.Min(patchesAheadLimit[3], GameSettings.CONIC_PATCH_LIMIT);
            }
            return GameSettings.CONIC_PATCH_LIMIT;
        }

        //public override float GetRecoveredPartValue(float pValue)

        //public override float GetRecoveredResourceValue(float rscValue)

        //public static float GetRecruitHireCost(int currentActive, float baseCost, float flatRate, float rateModifier)

        public float recruitHireBaseCost = 10000f;
        public float recruitHireFlatRate = 1.25f;
        public float recruitHireRateModifier = 0.015f;

        public override float GetRecruitHireCost(int currentActive)
        {
            return GameVariables.GetRecruitHireCost(currentActive, recruitHireBaseCost, recruitHireFlatRate, recruitHireRateModifier);
        }

        public float[] scienceCostLimit;

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

        public float[] strategyCommitRange;

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

        public int[] trackedObjectLimit;

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

        /*
        public override UntrackedObjectClass MinTrackedObjectSize(float tsNormLevel)
        {
            if (tsNormLevel < 0.2f)
            {
                return UntrackedObjectClass.E;
            }
            if (tsNormLevel < 0.4f)
            {
                return UntrackedObjectClass.D;
            }
            if (tsNormLevel < 0.6f)
            {
                return UntrackedObjectClass.C;
            }
            if (tsNormLevel < 0.8f)
            {
                return UntrackedObjectClass.B;
            }
            return UntrackedObjectClass.A;
        }
        */
        /*
        public override float ScoreFlightEnvelope(float altitude, float altEnvelope, float speed, float speedEnvelope)
        {
            float single = 0f;
            float single1 = 0f;
            if (altEnvelope != 0f)
            {
                float single2 = Mathf.Floor(altitude / 10000f * 1000f) / 1000f;
                float single3 = Mathf.InverseLerp(0f, altitude, altEnvelope);
                single = Mathf.Max(single2 - single3, 0f);
            }
            if (speedEnvelope != 0f)
            {
                float single4 = Mathf.Floor(speed / 1000f * 100f) / 100f;
                float single5 = Mathf.InverseLerp(0f, speed, speedEnvelope);
                single1 = Mathf.Max(single4 - single5, 0f);
            }
            return single + single1;
        }
         */

        public float[] scoreSituationHome;
        public float[] scoreSituationOther;
        public Dictionary<string, float[]> scoreSituationCustom = new Dictionary<string, float[]>();

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

        public float actionGroupsCustomUnlock = 0.6f;

        public override bool UnlockedActionGroupsCustom(float editorNormLevel)
        {
            return editorNormLevel > actionGroupsCustomUnlock;
        }

        public float actionGroupsStockUnlock = 0.4f;
        public override bool UnlockedActionGroupsStock(float editorNormLevel)
        {
            return editorNormLevel > actionGroupsStockUnlock;
        }

        public float unlockedEVA = 0.2f;

        public override bool UnlockedEVA(float astroComplexNormLevel)
        {
            return astroComplexNormLevel > unlockedEVA;
        }

        public float unlockedEVAClamber = 0.6f;

        public override bool UnlockedEVAClamber(float astroComplexNormLevel)
        {
            return astroComplexNormLevel > unlockedEVAClamber;
        }

        public float unlockedEVAFlags = 0.4f;

        public override bool UnlockedEVAFlags(float astroComplexNormLevel)
        {
            return astroComplexNormLevel >= unlockedEVAFlags;
        }

        public float unlockedFlightPlanning = 0.4f;

        public override bool UnlockedFlightPlanning(float mCtrlNormLevel)
        {
            return mCtrlNormLevel > unlockedFlightPlanning;
        }

        public float unlockedFuelTransfer = 0.2f;

        public override bool UnlockedFuelTransfer(float editorNormLevel)
        {
            return editorNormLevel > unlockedFuelTransfer;
        }

        public float unlockedSpaceObjectDiscovery = 0.6f;

        public override bool UnlockedSpaceObjectDiscovery(float tsNormLevel)
        {
            return tsNormLevel > unlockedSpaceObjectDiscovery;
        }
    }
}