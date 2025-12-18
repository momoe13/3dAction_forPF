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



    private void Update()
    {
      //  UpdateAttack();

        attackSpawnTimer += Time.deltaTime;
        if(attackSpawnTimer > attackInterval)
        {
           if (!isAtk) StartAttack();
            attackSpawnTimer = 0;
        }
        //プレイヤーを発見したら追いかける
        if (followArea.SetFlg()) {

            //TODO:01　Updateで常にアニメーション設定しない！
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


}
