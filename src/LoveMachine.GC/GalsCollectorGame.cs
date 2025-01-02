using System.Collections;
using System.Reflection;
using LoveMachine.Core.Common;
using LoveMachine.Core.Game;
using UnityEngine;

namespace LoveMachine.GC;

internal class GalsCollectorGame : GameAdapter
{
    private GameObject[] females;
    private Animator[] femaleAnimators;
    private Animator maleAnimator;

    protected override MethodInfo[] StartHMethods { protected internal get; }
    protected override MethodInfo[] EndHMethods { protected internal get; }
    
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
        maleAnimator.GetCurrentAnimatorStateInfo(0).fullPathHash == 514553200;

    protected override IEnumerator UntilReady(object instance)
    {
        yield return new WaitForSeconds(1f);
        while (GameObject.Find("/UI/UI_Sex_Container") == null)
        {
            yield return new WaitForSeconds(5f);
        }
        femaleAnimators = GameObject.Find("/Chara")
            .GetComponentsInChildren<Animator>()
            .Where(anim => anim.name.All(char.IsDigit))
            .ToArray();
        females = femaleAnimators
            .Select(anim => anim.transform.Find("Armature").gameObject)
            .ToArray();
        maleAnimator = GameObject.Find("/Chara/Male_A").GetComponent<Animator>();
    }
}