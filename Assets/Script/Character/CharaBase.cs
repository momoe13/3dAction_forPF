using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public enum AnimType
{
    Idle,
    Walk,
    Run,
    Rolling,
    Punch,
    SitDown,
    Victory,

    Attack1, 
    Attack2,
    Attack3,

    Hit,
    Knockback,
    Death,

}

public class CharaBase : MonoBehaviour
{

    //---------アニメーション------------
    protected AnimationClipTable animatClipTable;
    AnimType nowAnimType=AnimType.Idle;

    //--------------------------------

    protected Rigidbody rb;

    [Header("移動処理")]
    protected Vector3 moveDirection;
    protected float moveSpeed = 5f;
    protected float maxSpeed = 10;

    [Header("ジャンプ")]
    [SerializeField] float jumpPower;


    [SerializeField]
    private float rotationSpeed = 5f;

    [SerializeField]
    protected int hp;
    protected bool isInvincible=false;
    protected bool isDeath=false;

    //------------攻撃-----------------

    [SerializeField]
    protected bool isAtk=false;
    bool nextAtk=false;
    [SerializeField]
    int atkStep =0;


    [Header("近距離攻撃範囲")]
    [SerializeField]
    protected BoxCollider[] atkErea;

    //-------------------------------------



    private void Awake()
    {
        animatClipTable = GetComponent<AnimationClipTable>();
        rb = GetComponent<Rigidbody>();

        // 初期状態を明示的に再生
        UpdateAnimState(nowAnimType);
    }

  

    protected void InputAtk()
    { // 攻撃してない → 1段目開始
        if (!isAtk)
        {
            StartAttack(1);
            return;
        }
        // 攻撃中 ＆ 受付中 → 次段へ
        if ( atkStep < 3)
        {
            nextAtk = true;
        }
    }

    private void StartAttack(int step)
    {
        isAtk = true;
        nextAtk = false;
        atkStep = step;

        AnimType type= (AnimType)((int)AnimType.Attack1 + (step - 1));

        if(atkStep-1<=atkErea.Length) atkErea[atkStep - 1].enabled = true;


        UpdateAnimState(type);
    }

    protected void UpdateAtk()
    {
        if (!isAtk) return;

        // 攻撃終了
        if (animatClipTable.GetAnimStateInfo() >= 1.0f)
        {
            if (atkStep - 1 <= atkErea.Length) atkErea[atkStep-1].enabled = false;

            if (nextAtk)
            {
                StartAttack(atkStep + 1);
                return ;
            }

            EndAtk();
        }
    }

    private void EndAtk()
    {
        isAtk = false;
        atkStep = 0;
        UpdateAnimState(AnimType.Idle);
    }

    protected void UpdateAnimState(AnimType newAnimType)
    {
        if (nowAnimType == newAnimType) return;
        nowAnimType = newAnimType;

        //Debug.Log(newAnimType);
        animatClipTable.PlayForce(newAnimType);

    }



    //被ダメージ処理
    //TODO:ダメージ時赤く点滅する
    public void Damage(int damage, Vector3 hitPos, float knockbackPower)
    {
        if (isInvincible) return;


        //攻撃方向を受け取り、その方向に下がる
        Vector3 hitVec = (this.transform.position - hitPos).normalized;

        //Debug.Log(hitVec+"受けたキャラ:"+gameObject.name);
        hitVec.y = rb.linearVelocity.y;
        this.transform.position += hitVec * knockbackPower;
        //rb.AddForce(hitVec * knockbackPower, ForceMode.Impulse);

        //攻撃中断
        isAtk = false;
        atkStep = 0;


        hp -= damage;
        if (hp <= 0) {
            isDeath = true;
            Death(); }
    }


    /// <summary>
    /// 前後左右移動
    /// </summary>
    /// <param name="moveDirection"> 移動方向</param>
    protected void MoveCharacter()
    {
        // moveDirectionは派生クラスで設定される（入力 or AI）
        Vector3 velocity = moveDirection * moveSpeed;
        velocity.y = rb.linearVelocity.y; // 重力はRigidbodyに任せる
        rb.linearVelocity = velocity;
        RotateCharacter();
    }
    private void RotateCharacter()
    {
        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDirection);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime));
        }
    }
    protected virtual void Death()
    {
        //TODO:良い感じにしてこのif文消す
        if (this.gameObject.name == "Player") return;
        StartCoroutine(DeathAnimation());
    }

    IEnumerator DeathAnimation()
    {
        animatClipTable.PlayForce(AnimType.Death);
        yield return 0.5f;

        while (true)
        {
            if (animatClipTable.GetAnimStateInfo() >= 1.0f)
            {
                this.gameObject.SetActive(false);
                yield break;
            }
            yield return null;
        }

    }

    IEnumerator WaitTime()
    {
        yield return 5.0f;
    }
}
