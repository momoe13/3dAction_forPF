using System.Collections.Generic;
using UnityEngine;

//public class AnimationClipInfo
//{
//    public string clipName;
//    public AnimationClip animClip;
//    public bool isLoop;
//    public float length;
//}


public class AnimationClipTable : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    //[SerializeField]    
    //private Dictionary<AnimType, AnimationClipInfo> clipTable;

    // 今再生中
    private AnimType currentAnim = AnimType.Idle;



    /// <summary>
    /// アニメーション再生
    /// </summary>
    /// <param name="type"></param>
    /// <param name="fadeTime"></param>
    public void AnimPlay(AnimType type, float fadeTime = 0.1f)
    {
        if (currentAnim == type) return;

        currentAnim = type;
        animator.CrossFade(type.ToString(), fadeTime, 0, 0f);
    }

    /// <summary>
    /// 割り込み再生
    /// </summary>
    /// <param name="type"></param>
    public void PlayForce(AnimType type)
    {
        if (currentAnim == type) return;

        currentAnim = type;
        animator.Play(type.ToString(), 0, 0f);
    }

    public float GetAnimStateInfo()
    {
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

}
