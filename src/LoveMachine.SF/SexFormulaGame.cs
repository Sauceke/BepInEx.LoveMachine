using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using LoveMachine.Core.Common;
using LoveMachine.Core.Game;
using UnityEngine;

namespace LoveMachine.SF
{
    internal class SexFormulaGame : GameAdapter
    {
        private GameObject femaleRoot;
        private Animator reikaAnim;
        private Traverse<bool> sexMode;
        private Traverse<int> sexPosition;
        private Traverse<int> sexStyle;
        
        protected override MethodInfo[] StartHMethods => new[]
        {
            AccessTools.Method("SexMode_System, Assembly-CSharp:StartSexAnimation")
        };
        
        protected override MethodInfo[] EndHMethods  => new[]
        {
            AccessTools.Method("GameProcess_System, Assembly-CSharp:BackToMainMenu")
        };
        
        protected override Dictionary<Bone, string> FemaleBoneNames => new Dictionary<Bone, string>
        {
            { Bone.Vagina, "Upper_Vaginal" },
            { Bone.Mouth, "Tongue" },
            { Bone.LeftHand, "IndexFinger1_L" },
            { Bone.RightHand, "IndexFinger1_R" },
            { Bone.LeftFoot, "BigToe1_L" },
            { Bone.RightFoot, "BigToe1_R" },
            { Bone.LeftBreast, "Left_Nipple" },
            { Bone.RightBreast, "Right_Nipple" }
        };

        protected override Transform PenisBase =>
            GameObject.Find("[Character]/Player/Main/DeformationSystem/Penis_root").transform;

        protected override float PenisSize => 0.1f;
        protected override int AnimationLayer => 0;
        protected override int HeroineCount => 1;
        protected override int MaxHeroineCount => 1;
        protected override bool IsHardSex => false;

        protected override Animator GetFemaleAnimator(int girlIndex) => reikaAnim;

        protected override GameObject GetFemaleRoot(int girlIndex) => femaleRoot;

        protected override string GetPose(int girlIndex) => $"{sexPosition.Value}.{sexStyle.Value}";

        protected override bool IsIdle(int girlIndex) => !sexMode.Value;

        protected override IEnumerator UntilReady(object instance)
        {
            yield return new WaitForSeconds(1f);
            var sexModeSystem = Traverse.Create(instance);
            reikaAnim = sexModeSystem.Field<Animator>("reikaAnim").Value;
            sexMode = sexModeSystem.Field<bool>("sexMode");
            sexPosition = sexModeSystem.Field<int>("sexPosition");
            sexStyle = sexModeSystem.Field<int>("sexStyle");
            femaleRoot = GameObject.Find("[Character]/Reika/Main/DeformationSystem/Root_M");
        }
    }
}