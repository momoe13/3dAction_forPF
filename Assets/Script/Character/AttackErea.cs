using UnityEngine;

public class AttackErea : MonoBehaviour
{
    [SerializeField]
    int damage;
    BoxCollider coll;
    private void Start()
    {
        coll=GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.TryGetComponent<CharaBase>(out var chara))
        {
            chara.Damage(damage);
        }
    }
}
