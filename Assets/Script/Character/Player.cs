using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Player :CharaBase
{
    [Header("Camera参照")][SerializeField]
    Transform cameraTransform;



    
    int MaxHP;

    private void Start()
    {
        for (int i = 0; i < atkErea.Length; i++)
        {
            atkErea[i].enabled = false;
        }
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

            UpdateAttack();
        }
    }


    //TODO:アニメーション遷移フラグをキャラクターベースに移す
    private void HandleInput()
    {
        float h = Input.GetAxisRaw("Horizontal"); // A,Dキー
        float v = Input.GetAxisRaw("Vertical");   // W,Sキー

        //Debug.Log(h+ " " + v);

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
            if (!isAtk) { animator.SetBool("Move", true); }
        }
        else
        {
            moveDirection = Vector3.zero;

            animator.SetBool("Move", false);
        }
        //---ダッシュ処理------
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
            if (!isAtk) StartAttack();
            else
            {
                nextAtk = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            invincibleFlg = true;
            Vector3 stepDir = transform.forward;
            rb.linearVelocity = stepDir * 3.0f;
            animator.SetBool("Roll", true);
        }

        //ローリング終了
        if (invincibleFlg)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                animator.SetBool("Roll", false);
                invincibleFlg = false;

            }
        }

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

    private void EndRoll()
    {
        animator.SetBool("Roll", false);
        invincibleFlg = false;
    }
}
