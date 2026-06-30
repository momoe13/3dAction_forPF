using UnityEngine;

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

    float distance = 4.0f;

    private void Update()
    {
        if(isDeath)return;

        AnimationBack();

        if(isHit)return;

        UpdateAtk();

        attackSpawnTimer += Time.deltaTime;
        if(attackSpawnTimer > attackInterval)
        {
           if (!isAtk)InputAtk();
            attackSpawnTimer = 0;
        }
        //プレイヤーを発見したら追いかける
        if (followArea.SetFlg()) {

            float dis = Vector3.Distance(pl_Pos.position    , this.transform.position);

            if (dis < distance) return;

            UpdateAnimState(AnimType.Walk);
            Vector3 dir = (pl_Pos.position - transform.position).normalized;
            moveDirection = dir.normalized;
            MoveCharacter();

        }
        else
        {
            UpdateAnimState(AnimType.Idle);
        }
    }


}
