using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        private Dictionary<Bone, string> femaleBoneNames;
        private Animator femaleAnimator;
        private Transform penisBase;
        private Traverse<int> sexAnim;
        
        protected override MethodInfo[] StartHMethods =>
            new[] { AccessTools.Method("SexManager, Assembly-CSharp:SexInitualize") };
        
        protected override MethodInfo[] EndHMethods =>
            new[] { AccessTools.Method("SexManager, Assembly-CSharp:SexFinish") };

        protected override Dictionary<Bone, string> FemaleBoneNames => femaleBoneNames;

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
                .GetComponentInChildren(Type.GetType("CharacterStateManager, Assembly-CSharp"))
                .gameObject;
            femaleAnimator = femaleRoot.GetComponent<Animator>() ??
                femaleRoot.GetComponentInChildren<Animator>();
            penisBase = (GameObject.Find("Orc_jo_Left_Scrotum1") ??
                GameObject.Find("Orc_Rig:Nut1_L")).transform;
            sexAnim = Traverse.Create(instance).Field<int>("SexAnim");
            var bones = femaleRoot.GetComponentsInChildren<Transform>();
            // bone naming in OM is a disaster
            femaleBoneNames = new Dictionary<Bone, string>();
            femaleBoneNames[Bone.Vagina] = bones
                .First(bone => bone.name.Contains("Vagina") || bone.name.Contains("vagina"))
                .name;
            femaleBoneNames[Bone.Mouth] = bones
                .FirstOrDefault(bone => bone.name.Contains("Mouth"))?
                .name ?? femaleBoneNames[Bone.Vagina];
            femaleBoneNames[Bone.LeftBreast] = bones
                .FirstOrDefault(bone => bone.name.Contains("Breast8_L"))?
                .name ?? femaleBoneNames[Bone.Vagina];
            femaleBoneNames[Bone.RightBreast] = bones
                .FirstOrDefault(bone => bone.name.Contains("Breast8_R"))?
                .name ?? femaleBoneNames[Bone.Vagina];
        }
    }
}