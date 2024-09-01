using System.Collections;
using System.Reflection;
using HarmonyLib;
using LoveMachine.Core.Common;
using LoveMachine.Core.Game;
using UnityEngine;

namespace LoveMachine.SVS;

public class SamabakeGame : GameAdapter
{
    private static readonly string[] idleStateNames =
    {
        "Idle", "InsertIdle", "Orgasm_A", "BDrink_A", "BOrgasm_A", "Orgasm_IN_A", "BOrgasm_IN_A",
        "BVomit_A", "Vomit_A", "OrgasmM_OUT_A", "BOrgasmM_OUT_A", "Drink_A", "Drop_A"
    };

    private static readonly string[] orgasmStateNames =
    {
        "BOrgasmS_ST", "OrgasmF", "BOrgasmF", "OrgasmF_ST", "BOrgasmF_ST", "OrgasmM_IN",
        "BOrgasmM_IN", "OrgasmM_IN_ST", "BOrgasmM_IN_ST", "OrgasmM_OUT", "BOrgasmM_OUT",
        "OrgasmM_OUT_ST", "BOrgasmM_OUT_ST", "OrgasmS", "BOrgasmS", "OrgasmS_IN", "BOrgasmS_IN",
        "OrgasmS_ST"
    };

    private GameObject[] females;
    private Animator[] femaleAnimators;
    private Transform[] penises;
    private Traverse controller;

    private string AnimationName => controller.Property("_data").Property<string>("Name").Value;

    protected override MethodInfo[] StartHMethods =>
        new[] { AccessTools.Method("SV.H.HScene, Assembly-CSharp:Start") };

    protected override MethodInfo[] EndHMethods =>
        new[] { AccessTools.Method("SV.H.HScene, Assembly-CSharp:End") };

    protected override Dictionary<Bone, string> FemaleBoneNames => new Dictionary<Bone, string>
    {
        { Bone.LeftBreast, "k_f_munenipL_00" },
        { Bone.RightBreast, "k_f_munenipR_00" },
        { Bone.Vagina, "cf_n_pee" },
        { Bone.Anus, "k_f_ana_00" },
        { Bone.LeftButt, "k_f_siriL_00" },
        { Bone.RightButt, "k_f_siriR_00" },
        { Bone.Mouth, "cf_J_MouthCavity" },
        { Bone.LeftHand, "cf_j_index03_L" },
        { Bone.RightHand, "cf_j_index03_R" },
        { Bone.LeftFoot, "k_f_toeL_00" },
        { Bone.RightFoot, "k_f_toeR_00" },
    };

    protected override Transform PenisBase => throw new NotImplementedException();
    protected override Transform[] PenisBases => penises;
    protected override float PenisSize => 0.07f;
    protected override int AnimationLayer => 0;
    protected override int HeroineCount => females.Length;
    protected override int MaxHeroineCount => 2;
    protected override bool IsHardSex => false;

    protected override Animator GetFemaleAnimator(int girlIndex) => femaleAnimators[girlIndex];

    protected override GameObject GetFemaleRoot(int girlIndex) => females[girlIndex];

    protected override string GetPose(int girlIndex) =>
        $"{AnimationName}.{GetAnimatorStateInfo(girlIndex).fullPathHash}";

    protected override bool IsIdle(int girlIndex) =>
        idleStateNames.Any(GetAnimatorStateInfo(girlIndex).IsName);

    protected override bool IsOrgasming(int girlIndex) =>
        orgasmStateNames.Any(GetAnimatorStateInfo(girlIndex).IsName);

    protected override IEnumerator UntilReady(object hscene)
    {
        yield return new WaitForSeconds(10f);
        var animators = Traverse.Create(hscene)
            .Property<IEnumerable<object>>("Actors").Value
            .Select(Traverse.Create)
            .Select(hActor => hActor.Property<Animator>("Animator").Value)
            .ToArray();
        femaleAnimators = animators
            .Where(anim => anim.transform.parent.parent.name.StartsWith("chaF"))
            .ToArray();
        females = femaleAnimators.Select(anim => anim.gameObject).ToArray();
        penises = animators
            .Where(anim => anim.transform.parent.parent.name.StartsWith("chaM"))
            .Select(anim => FindDeepChildrenByPath(anim.gameObject, "k_f_tamaC_00").First())
            .ToArray();
        controller = Traverse.Create(hscene)
            .Property("_postureMainSelecter")
            .Property("_subSelecter")
            .Property("Controller");
    }
}