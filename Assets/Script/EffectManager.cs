using UnityEngine;

public enum Effects
{ 
    HitEffect,
    FirstSlash, SecondSlash, ThreeSlash,
}


public class EffectManager : MonoBehaviour
{
    [SerializeField] ParticleSystem[] particle;


    public void PlayEffect(Effects effectNum,Vector3 instantPos ,Quaternion rotate)
    {
        Instantiate(particle[(int)effectNum],instantPos,rotate);   
    }

}
