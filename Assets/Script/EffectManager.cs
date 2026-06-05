using UnityEngine;

public enum Effects
{ 
    HitEffect,
    FirstSlash, SecondSlash, ThreeSlash,
}


public class EffectManager : MonoBehaviour
{
    [SerializeField] ParticleSystem[] particle;

    [SerializeField] GameObject[] effects;

    public void PlayEffect(Effects effectNum,Vector3 instantPos ,Quaternion rotate)
    {
        Instantiate(effects[(int)effectNum],instantPos,rotate);   
    }

}
