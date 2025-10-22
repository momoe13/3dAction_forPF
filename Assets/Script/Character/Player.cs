using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Player :CharaBase
{
    [Header("Camera Reference")]
    public Transform cameraTransform;

    [SerializeField]
    private Animator animator;


    [Header("近距離攻撃範囲")]
    [SerializeField]
    BoxCollider AttackErea;
    bool attackFlg;

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
        if (attackFlg) return;
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

        MoveCharacter();
        
        if(Input.GetButtonDown("Jump"))
        {
                Jump();
        }

        //TODO:加速、減速処理　整える
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveSpeed *= 1.5f;
            animator.SetBool("Dash", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed /= 1.5f;
            animator.SetBool("Dash",false);
        }
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            attackFlg = true;
            StartCoroutine(Attack());
        }
        //TODO:CharaBaseに引っ越し
        if (Input.GetKeyDown(KeyCode.K)) { 
            animator.SetBool("Death", true);
            deathFlg = true;
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
}
