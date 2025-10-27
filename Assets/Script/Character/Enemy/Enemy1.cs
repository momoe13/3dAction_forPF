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
        //プレイヤーを発見したら追いかける
        if (followArea.SetFlg()) {
         animator.SetBool("Move", true);
         Vector3 dir=(pl_Pos.position - transform.position).normalized;
         moveDirection = dir.normalized;
         MoveCharacter();

        }
        else
        {
            animator.SetBool("Move", false);
        }
    }

    //攻撃範囲有効化コルーチン
    private IEnumerator Attack()
    {
        //攻撃
        animator.SetBool("Attack", true);
        AttackErea.enabled = true;
        yield return new WaitForSeconds(0.7f);

        //攻撃終了
        animator.SetBool("Attack", false);
        AttackErea.enabled = false;

        yield return null;
    }
}
