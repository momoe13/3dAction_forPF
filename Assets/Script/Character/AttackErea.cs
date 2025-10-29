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
            chara.Damage(damage);
        }
    }
}
