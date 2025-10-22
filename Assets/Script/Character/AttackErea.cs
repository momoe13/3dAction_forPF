using UnityEngine;

public class AttackErea : MonoBehaviour
{
    [SerializeField]
    int damage;


    private void OnTriggerEnter(Collider collision)
    {
        //TODO:この状態だと子オブジェクトにあるスクリプトを取得することができない！

        //if (collision.gameObject.TryGetComponent<CharaBase>(out var chara))
        //{
        //    chara.Damage(damage);
        //}
        if(collision.CompareTag("Player"))
        {
            CharaBase chara = collision.GetComponentInParent<CharaBase>();
            chara.Damage(damage);
        }
    }
}
