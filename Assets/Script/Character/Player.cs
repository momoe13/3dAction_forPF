using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Player :CharaBase
{
    [Header("Camera Reference")]
    public Transform cameraTransform;



    [Header("近距離攻撃範囲")]
    [SerializeField]
    BoxCollider AttackErea;

    bool deathFlg;

    int MaxHP;
    private void Start()
    {
        AttackErea.enabled = false;
        MaxHP = hp;
    }
    private void Update()
    {
        //復活
        if (deathFlg)
        {
            if (Input.GetKeyDown(KeyCode.Return)) StateReset();
        }
        else
        {
            GroundCheck();
            HandleInput();
        }
    }


    //TODO:アニメーション遷移フラグをキャラクターベースに移す
    private void HandleInput()
    {
        float h = Input.GetAxisRaw("Horizontal"); // A,Dキー
        float v = Input.GetAxisRaw("Vertical");   // W,Sキー

        Vector3 inputDir = new Vector3(h, 0, v).normalized;

        if (inputDir.magnitude > 0.1f)
        {
            // カメラの向きを基準にして移動方向を回転
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;

            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            moveDirection = (camForward * v + camRight * h).normalized;
            animator.SetBool("Move", true);
        }
        else
        {
            moveDirection = Vector3.zero;

            animator.SetBool("Move", false);
        }
        //ダッシュ処理
        //三項演算子（シフトが押されてたらtargetSpeedは最大速度に、押してないなら通常速度に。）
        float targetSpeed = Input.GetKey(KeyCode.LeftShift) ? maxSpeed : maxSpeed / 3f;
       
        moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, Time.deltaTime*5);
        animator.SetBool("Dash", Input.GetKey(KeyCode.LeftShift));

        MoveCharacter();
        
        if(Input.GetKeyDown(KeyCode.Space))
        {
                Jump();
        }

       
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            attackFlg = true;
            StartCoroutine(Attack());
        }
        //TODO:CharaBaseに引っ越し
        //死亡処理
        if (Input.GetKeyDown(KeyCode.K)) { 
            
        }

    }


    //攻撃範囲有効化コルーチン
    private IEnumerator Attack()
    {   
        //攻撃
        animator.SetBool("Attack", true);
        
        //アニメーションに合わせて攻撃判定をさせるためちょっと待機
        yield return new WaitForSeconds(0.2f);

        AttackErea.enabled = attackFlg;
        yield return new WaitForSeconds(0.7f);

        //攻撃終了
        attackFlg = false;
        animator.SetBool("Attack", false);
        AttackErea.enabled = attackFlg;

        yield return null;
    }

    //復活用初期化処理
    private void StateReset()
    {
        hp = MaxHP;
        deathFlg= false;
        animator.SetBool("Death", false);
        animator.Play("CharacterArmature|Idle");
    }
    
    protected override void Death()
    {
        animator.SetBool("Death", true);
        deathFlg = true;
    }

    public Vector2 SetHP()
    {
        Vector2 plHp;
        plHp.x = hp;
        plHp.y = MaxHP;
        return plHp;
    }
}
