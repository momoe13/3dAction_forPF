using UnityEngine;

public class atk : MonoBehaviour
{
    /*
     protected virtual void StartAttack()
    {
        if (atkFlg) return;

        atkFlg = true;
        nextAtkFlg = false;
        //TODO:攻撃回数に合わせてアニメーションを再生

        animator.SetBool("Attack", true);
        animator.SetInteger("AtkCount", atkCount);
        Debug.Log(atkCount);
        //animator.SetTrigger("Attack 0");
        StopCoroutine(AttackRoutine());
        StartCoroutine(AttackRoutine());
    }
    private IEnumerator AttackRoutine()
    {
        int curentAtk = atkCount;
        atkErea[atkCount].enabled = true;

        Debug.Log("攻撃" + curentAtk);

        while (true)
        {
            var state = animator.GetCurrentAnimatorStateInfo(0);

            if (state.IsName("CharacterArmature|Punch") || state.IsName("CharacterArmature|SitDown"))
            {

                //今の攻撃アニメが終了したら終わり
                if (state.normalizedTime >= 1f)
                {
                    if (nextAtkFlg)
                    {
                        atkCount++;
                        if (atkCount >= atkErea.Length) atkCount = 0;
                        Debug.Log("コンボ！" + atkCount);
                    }
                    else
                    {
                        animator.SetBool("Attack", false);
                        Debug.Log("終わり");
                        atkFlg = false;
                        atkCount = 0;
                    }
                    break;
                }

            }


            yield return null;
        }
        atkErea[atkCount].enabled = false;

        //次が登録済みならコンボ継続
        if (nextAtkFlg)
        {
            animator.SetInteger("AtkCount", atkCount);
            StartAttack();
        }
        else
        {
            animator.SetBool("Attack", false);
            Debug.Log("終わり");
            atkFlg = false;
            atkCount = 0;
        }
    }

     
     */

    //    if(!atkFlg)
    //    {
    //        atkFlg=true;
    //        nextAtkFlg = false;
    //        atkCount = 0;
    //    }
    //    else
    //    {
    //        Debug.Log("通った！");
    //        nextAtkFlg=false;
    //        atkCount++;
    //    }
    //    atkTime = 0;
    //    endAtkFlg= false;
    //}


    ////中身は派生クラスの各オブジェクトに移動したほうが良いかも
    //protected virtual void UpdateAttack()
    //{
    //    atkTime += Time.deltaTime;

    //    //攻撃時に一歩動く
    //    if (atkTime < 0.1f){moveFlg = true;}
    //    else {moveFlg = false;}

    //    //TODO:攻撃だけでなく、Idleとかのアニメーションも終わるのを待ってる
    //    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
    //    {
    //        //次の動作が登録済み
    //        if (nextAtkFlg && atkCount + 1 < atkErea.Length)
    //        {
    //            StartAttack();
    //        }
    //        //攻撃の終了
    //        else if (endAtkFlg)
    //        {
    //            atkFlg = false;
    //        }
    //        else
    //        {
    //            endAtkFlg = true;
    //            //TODO:攻撃回数に合わせて攻撃アニメーションを変更
    //            StartCoroutine(Attack());

    //        }
    //    }
    //}
    ////攻撃範囲有効化コルーチン
    //protected IEnumerator Attack()
    //{
    //    //攻撃
    //    animator.SetBool("Attack", true);
    //    atkErea[atkCount].enabled = true;
    //    yield return new WaitForSeconds(1f);

    //    //攻撃終了
    //    animator.SetBool("Attack", false);
    //    atkErea[atkCount].enabled = false;

    //    yield return null;
    //}
}
