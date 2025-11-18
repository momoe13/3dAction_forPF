using UnityEngine;

public class AttackErea : MonoBehaviour
{
    [SerializeField]
    int damage;


    private void OnTriggerEnter(Collider collision)
    {
        if(collision.CompareTag("Target"))
        {
            CharaBase chara = collision.GetComponentInParent<CharaBase>();

            //ContactPoint contact = collision.GetContact(0);
            //ClosestPoint　指定された位置に最も近いコライダーのポイントを返す。
            //今回の場合、攻撃者の攻撃が当たった場所を返してもらう。
            chara.Damage(damage,collision.ClosestPoint(this.transform.position));
        }
    }
}
