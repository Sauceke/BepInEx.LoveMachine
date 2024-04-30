using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using LoveMachine.Core.Common;
using LoveMachine.Core.Game;
using UnityEngine;

namespace LoveMachine.OM
{
    public class OrcMassageGame : GameAdapter
    {
        private GameObject femaleRoot;
        private Animator femaleAnimator;
        private Transform penisBase;
        private Traverse<int> sexAnim;
        
        protected override MethodInfo[] StartHMethods =>
            new[] { AccessTools.Method("SexManager, Assembly-CSharp:SexInitualize") };
        
        protected override MethodInfo[] EndHMethods =>
            new[] { AccessTools.Method("SexManager, Assembly-CSharp:SexFinish") };

        protected override Dictionary<Bone, string> FemaleBoneNames => new Dictionary<Bone, string>
        {
            { Bone.Vagina, "Elf_Vaginal" }
        };

        protected override Transform PenisBase => penisBase;
        protected override int AnimationLayer => 0;
        protected override int HeroineCount => 1;
        protected override int MaxHeroineCount => 1;
        protected override bool IsHardSex => true;

        protected override Animator GetFemaleAnimator(int girlIndex) => femaleAnimator;

        protected override GameObject GetFemaleRoot(int girlIndex) => femaleRoot;

        protected override string GetPose(int girlIndex) => sexAnim.Value.ToString();

        protected override bool IsIdle(int girlIndex) => false;

        protected override IEnumerator UntilReady(object instance)
        {
            yield return new WaitForSeconds(5f);
            femaleRoot = GameObject.Find("----------------GameCharacters----------------")
                .GetComponentInChildren(Type.GetType("VipCostumeHolder, Assembly-CSharp"))
                .gameObject;
            femaleAnimator = femaleRoot.GetComponent<Animator>();
            penisBase = GameObject.Find("Orc_jo_Left_Scrotum1").transform;
            sexAnim = Traverse.Create(instance).Field<int>("SexAnim");
        }
    }
}