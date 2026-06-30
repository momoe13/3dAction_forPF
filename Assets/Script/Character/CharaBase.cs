using System.Collections;
using UnityEngine;

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

    Shoot,

    Hit,
    Knockback,
    Death,

}

public class CharaBase : MonoBehaviour
{

    [SerializeField]
    EffectManager effectManager;

    //---------アニメーション------------
    protected AnimationClipTable animatClipTable;
    AnimType pastAnimType = AnimType.Idle;
    AnimType currentAnimType = AnimType.Idle;

    protected bool isHit = false;

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
    [SerializeField]//←消す
    int atkStep =0;

    [SerializeField]
    Transform atkPos;

    [Header("近距離攻撃範囲")]
    [SerializeField]
    protected BoxCollider[] atkErea;

    [SerializeField]
    ShooterScript shooter;

    //-------------------------------------
    [Header("ドロップアイテム")][SerializeField]
    GameObject dropItem;

    [SerializeField]
    GameObject targetIconPos;

    private void Awake()
    {
        animatClipTable = GetComponent<AnimationClipTable>();
        rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true; // 回転はスクリプトで制御する
        if (dropItem != null)
        { dropItem.SetActive(false); }

            // 初期状態を明示的に再生
            UpdateAnimState(currentAnimType);
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
            Vector3 lookDir = moveDirection;
            lookDir.y = 0; // キャラクターの高さの差を無視する
            Quaternion targetRot = Quaternion.LookRotation(lookDir); 
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime));
        }
    }

    //------近接攻撃-----------------
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

        //ローカル座標をワールド座標に変換してエフェクト生成
        Vector3 worldPos = transform.TransformPoint(atkPos.position);
        effectManager.PlayEffect((Effects)atkStep, this.transform.position,transform.rotation);


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

    //------近接攻撃-----------------


    //-----遠距離攻撃---------------
    protected void ShootAtk(int layerNum)
    {
        UpdateAnimState(AnimType.Shoot);
        shooter.Shoot(layerNum);
    }

    //-----遠距離攻撃---------------


    protected void UpdateAnimState(AnimType newAnimType)
    {
        if (currentAnimType == newAnimType) return;
        currentAnimType = newAnimType;

        animatClipTable.PlayForce(newAnimType);

    }

    protected void AnimationBack()
    {
        if ( currentAnimType ==AnimType.Hit && animatClipTable.GetAnimStateInfo() >= 1.0f) 
        {
            isHit = false;
            currentAnimType = pastAnimType;
            animatClipTable.AnimPlay(currentAnimType);
        }
    }

    /// <summary>
    /// 
    //被ダメージ処理
    //TODO:ダメージ時赤く点滅する
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="hitPos"></param>
    /// <param name="knockbackPower"></param>
    public void Damage(int damage, Vector3 hitPos, float knockbackPower)
    {
        if (isInvincible) return;

        Debug.Log(this.name+ "ダメージ");

        pastAnimType = currentAnimType;
        isHit = true;
        UpdateAnimState(AnimType.Hit);


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

    IEnumerator AnimSave()
    {
        return null;
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
                if(dropItem!=null)
                {
                    dropItem.gameObject.SetActive(true);
                    dropItem.transform.SetParent(null, true);
                }
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

    public Vector3 GetTargetIconPos()
    {
        return targetIconPos.transform.position;
    }
}
