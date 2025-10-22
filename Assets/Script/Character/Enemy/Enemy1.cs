using UnityEngine;
using System.Collections;

public class Enemy1 : CharaBase
{
    [SerializeField]
    Transform pl_Pos;

    [SerializeField]
    FollowArea followArea;

    [Header("攻撃用変数やコライダー")]
    [SerializeField]
    float attackInterval;
    float attackSpawnTimer=0;

    [SerializeField]
    BoxCollider AttackErea;


    private void Update()
    {
        attackSpawnTimer += Time.deltaTime;
        if(attackSpawnTimer > attackInterval)
        {
            StartCoroutine(Attack());
            attackSpawnTimer = 0;
        }

        if (pl_Pos != null&&followArea.SetFlg()) { 
         Vector3 dir=(pl_Pos.position - transform.position).normalized;
         moveDirection = dir.normalized;
         MoveCharacter();

        }
    }

    //攻撃範囲有効化コルーチン
    private IEnumerator Attack()
    {
        //攻撃
        AttackErea.enabled = true;
        yield return new WaitForSeconds(0.7f);

        //攻撃終了
        AttackErea.enabled = false;

        yield return null;
    }
}
