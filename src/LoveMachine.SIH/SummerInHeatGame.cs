﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using LoveMachine.Core.Common;
using LoveMachine.Core.Game;
using UnityEngine;

namespace LoveMachine.SIH
{
    internal class SummerInHeatGame : GameAdapter
    {
        private static readonly int[] activeSteps = { 1, 2, 5 };

        private GameObject femaleRoot;
        private Animator femaleAnimator;
        private Traverse<int> step;

        protected override MethodInfo[] StartHMethods => new[]
        {
            AccessTools.Method("FH_Controller, Assembly-CSharp:Start"),
            AccessTools.Method("Talk_Controller, Assembly-CSharp:Start")
        };

        protected override MethodInfo[] EndHMethods =>
            new[] { AccessTools.Method("FadeManager_GUI, Assembly-CSharp:TransScene") };

        protected override Dictionary<Bone, string> FemaleBoneNames => new Dictionary<Bone, string>
        {
            { Bone.Vagina, "HS_cli" },
            { Bone.LeftHand, "bip01 L Finger1Nub" },
            { Bone.RightHand, "bip01 R Finger1Nub" },
            { Bone.Mouth, "HS_mouth01" },
            { Bone.LeftBreast, "BP_Breast_L03" },
            { Bone.RightBreast, "BP_Breast_R03" },
            { Bone.LeftFoot, "bip01 L Toe4Nub" },
            { Bone.RightFoot, "bip01 R Toe4Nub" }
        };

        protected override Transform PenisBase => GameObject.Find("HS00_penis01_PC01").transform;
        protected override int AnimationLayer => 0;
        protected override int HeroineCount => 1;
        protected override int MaxHeroineCount => 1;
        protected override bool IsHardSex => true;

        protected override Animator GetFemaleAnimator(int girlIndex) => femaleAnimator;

        protected override GameObject GetFemaleRoot(int girlIndex) => femaleRoot;

        protected override string GetPose(int girlIndex) =>
            femaleAnimator.GetCurrentAnimatorStateInfo(0).fullPathHash.ToString();

        protected override bool IsIdle(int girlIndex) => !activeSteps.Contains(step.Value);

        protected override bool IsOrgasming(int girlIndex) => step.Value == 3;

        protected override IEnumerator UntilReady(object instance)
        {
            yield return new WaitForSeconds(5f);
            var controller = Traverse.Create(instance);
            femaleRoot = GameObject.Find("CH_Prefub_A/CHbase");
            femaleAnimator = femaleRoot.GetComponent<Animator>();
            step = instance.GetType().Name == "FH_Controller"
                ? controller.Field<int>("FH_Step")
                : controller.Field<int>("TK_Step");
        }
    }
}