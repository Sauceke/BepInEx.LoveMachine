using System.Collections;
using System.Reflection;
using HarmonyLib;
using LoveMachine.Core.Common;
using LoveMachine.Core.Game;
using UnityEngine;

namespace LoveMachine.KFC2;

public class KinkyFightClub2Adapter : GameAdapter
{
    private GameObject maleRoot;
    private GameObject femaleRoot;
    private Animator femaleAnimator;

    protected override MethodInfo[] StartHMethods => new[]
    {
        AccessTools.Method("SexSystem, Assembly-CSharp:StartSex")
    };
    
    protected override MethodInfo[] EndHMethods => new[]
    {
        AccessTools.Method("SexSystem, Assembly-CSharp:Start"),
        AccessTools.Method("SexSystem, Assembly-CSharp:Escape")
    };
    
    protected override Dictionary<Bone, string> FemaleBoneNames => new()
    {
        { Bone.Vagina, "stick" },
        { Bone.Mouth, "MouthLower" },
        { Bone.LeftHand, "hand_L" },
        { Bone.RightHand, "hand_R" }
    };
    
    protected override Transform PenisBase => FindBoneByPath(maleRoot, "stick");
    protected override int AnimationLayer => 0;
    protected override int HeroineCount => 1;
    protected override int MaxHeroineCount => 1;
    protected override bool IsHardSex => true;

    protected override Animator GetFemaleAnimator(int girlIndex) => femaleAnimator;

    protected override GameObject GetFemaleRoot(int girlIndex) => femaleRoot;

    protected override string GetPose(int girlIndex) =>
        GetAnimatorStateInfo(0).fullPathHash.ToString();

    protected override bool IsIdle(int girlIndex) => false;

    protected override IEnumerator UntilReady(object instance)
    {
        yield return new WaitForSeconds(5f);
        maleRoot = GameObject.Find("Player/ArmatureFem_000/Global/Position");
        femaleRoot = GameObject.Find("Enemy/ArmatureFem_000/Global/Position");
        femaleAnimator = femaleRoot.transform.parent.parent.parent.GetComponent<Animator>();
    }
}