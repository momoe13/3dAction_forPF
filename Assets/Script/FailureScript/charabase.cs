using UnityEngine;

[System.Serializable]
public class atkdata
{
    public string animName;
    public float hitStart;
    public float hitEnd;
}
public class charabase : MonoBehaviour
{
    protected bool moveFlg;
    protected float moveAngle;


    [Header("移動処理")]
    protected float moveSpeed = 5f;
    protected float maxSpeed = 10;

    [Header("ジャンプ")]
    [SerializeField] float jumpPower;

    [SerializeField]
    private float rotationSpeed = 10f;


    bool knockbackFlg;


    protected Rigidbody rb;
    protected Vector3 moveDirection;// 現在入力またはAIが指定した移動方向

    [Header("設地判定")]
    protected bool isGrounded;         // 地面に接地しているか
    public Transform groundCheck;   // 足元チェック用
    public float groundRadius = 0.3f;

    [SerializeField]
    protected int hp;
    protected bool deathFlg;
    protected bool invincibleFlg = false;//存在フラグ　死亡したらtrue

    [SerializeField]
    protected Animator animator;


    //---攻撃関連----
    //atk=attack
    [Header("近距離攻撃範囲")]
    [SerializeField]
    protected BoxCollider[] atkErea;

    [SerializeField] AttackData[] attacks;

    int atkIndex = -1;
    protected bool isAtk;
    protected bool nextAtk;
    float atkTimer;



    private string currentStateName;

    protected virtual void Awake()
    {
        // maxSpeed = moveSpeed * 3f;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // 回転はスクリプトで制御する
    }


    //接地判定
    protected void GroundCheck()
    {
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius);
        }
    }
    //ジャンプ処理
    protected void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }

    //前後左右移動
    protected void MoveCharacter()
    {

        if (isAtk || knockbackFlg) return;

        // moveDirectionは派生クラスで設定される（入力 or AI）
        Vector3 velocity = moveDirection * moveSpeed;
        velocity.y = rb.linearVelocity.y; // 重力はRigidbodyに任せる
        rb.linearVelocity = velocity;
        RotateCharacter();
    }


    //回転
    private void RotateCharacter()
    {
        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDirection);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime));
        }
    }

    //被ダメージ処理
    //TODO:ダメージ時赤く点滅する
    public void Damage(int damage, Vector3 hitPos, float knockbackPower)
    {
        if (invincibleFlg) return;


        //攻撃方向を受け取り、その方向に下がる
        Vector3 hitVec = (this.transform.position - hitPos).normalized;

        //Debug.Log(hitVec+"受けたキャラ:"+gameObject.name);
        hitVec.y = rb.linearVelocity.y;
        this.transform.position += hitVec * knockbackPower;
        //rb.AddForce(hitVec * knockbackPower, ForceMode.Impulse);


        hp -= damage;
        if (hp <= 0) { Death(); }
    }


    public void Heal(int healPoint)
    {
        hp += healPoint;
    }

    public void StartAttack()
    {
        if (!isAtk)
        {
            BeginAttack(0);
        }
        else
        {
            nextAtk = true;
        }
    }
    void BeginAttack(int index)
    {
        isAtk = true;
        nextAtk = false;
        atkIndex = index;
        atkTimer = 0f;

        animator.Play(attacks[index].animName, 0, 0f);
    }

    protected virtual void UpdateAttack()
    {
        if (!isAtk) return;
        atkTimer += Time.deltaTime;
        var data = attacks[atkIndex];

        // ヒット判定
        atkErea[atkIndex].enabled =
            atkTimer >= data.hitStart && atkTimer <= data.hitEnd;

        // 終了
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            atkErea[atkIndex].enabled = false;

            if (nextAtk && atkIndex + 1 < attacks.Length)
            {
                BeginAttack(atkIndex + 1);
            }
            else
            {
                EndAttack();
            }
        }
    }
    protected virtual void EndAttack()
    {
        isAtk = false;
        nextAtk = false;
        atkIndex = -1;
        animator.Play("CharacterArmature|Idle");
    }


    protected virtual void Death()
    {
        //TODO:良い感じにしてこのif文消す
        if (this.gameObject.name == "Player") return;
        this.gameObject.SetActive(false);
    }

}
