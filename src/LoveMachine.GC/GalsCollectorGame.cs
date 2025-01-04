using System.Collections;
using System.Reflection;
using HarmonyLib;
using LoveMachine.Core.Common;
using LoveMachine.Core.Game;
using UnityEngine;
using UnityEngine.UI;

namespace LoveMachine.GC;

internal class GalsCollectorGame : GameAdapter
{
    private GameObject[] females;
    private Animator[] femaleAnimators;
    private Button finish;
    private GameObject shot;
    private GameObject replay;

    protected override MethodInfo[] StartHMethods =>
        new[] { AccessTools.Method(typeof(HSceneMonitor), nameof(HSceneMonitor.HSceneStarted)) };
    
    protected override MethodInfo[] EndHMethods =>
        new[] { AccessTools.Method(typeof(HSceneMonitor), nameof(HSceneMonitor.HSceneEnded)) };
    
    protected override Dictionary<Bone, string> FemaleBoneNames => new Dictionary<Bone, string>
    {
        { Bone.Vagina, "pussy_pos" },
        { Bone.Mouth, "Mouth_Bottom_Weight" },
        { Bone.LeftHand, "Left_Shoulder/Left_Upper_Arm/Left_Lower_Arm/Left_Hand/L_Index_0" },
        { Bone.RightHand, "Right_Shoulder/Right_Upper_Arm/Right_Lower_Arm/Right_Hand/R_Index_0" },
        { Bone.LeftFoot, "Left_toe" },
        { Bone.RightFoot, "Right_toe" },
        { Bone.LeftBreast, "Breast_L_2" },
        { Bone.RightBreast, "Breast_R_2" }
    };

    protected override Transform PenisBase =>
        GameObject.Find("/Chara/Male_A/Rig_M/Hips_M/P1").transform;
    
    protected override int AnimationLayer => 0;
    protected override int HeroineCount => females.Length;
    protected override int MaxHeroineCount => 2;
    protected override bool IsHardSex => false;

    protected override Animator GetFemaleAnimator(int girlIndex) => femaleAnimators[girlIndex];

    protected override GameObject GetFemaleRoot(int girlIndex) => females[girlIndex];

    protected override string GetPose(int girlIndex) =>
        GetAnimatorStateInfo(girlIndex).fullPathHash.ToString();

    protected override bool IsIdle(int girlIndex) =>
        !finish.interactable && (!shot.active || replay.active);

    protected override IEnumerator UntilReady(object instance)
    {
        yield return new WaitForSeconds(1f);
        femaleAnimators = GameObject.Find("/Chara")
            .GetComponentsInChildren<Animator>()
            .Where(anim => anim.name.All(char.IsDigit))
            .ToArray();
        females = femaleAnimators
            .Select(anim => anim.transform.Find("Armature").gameObject)
            .ToArray();
        finish = GameObject.Find("/UI").transform
            .Find("UI_Sex_Container/Container/UI_Menu_Action/Bg_Speed_Toggle/FINISH")
            .GetComponent<Button>();
        shot = finish.transform.Find("SHOT_Slider").gameObject;
        replay = finish.transform.Find("Replay_Button").gameObject;
    }
}