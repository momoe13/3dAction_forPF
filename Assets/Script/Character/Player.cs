using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player :CharaBase
{
    [Header("Camera参照")][SerializeField]
    Transform cameraTransform;

    int MaxHP=30;

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
        if (isDeath)
        {
            if (Input.GetKeyDown(KeyCode.Return)) StateReset();
            return;
        }
        InputMove();

        if (Input.GetMouseButtonDown(0)) InputAtk();

        UpdateAtk();
    }

    private void InputMove()
    {
        if (isAtk) return;

        float h = Input.GetAxisRaw("Horizontal"); // A,Dキー
        float v = Input.GetAxisRaw("Vertical");   // W,Sキー

        AnimType nextAnim = SetMove(h,v);

        if (Input.GetKeyDown(KeyCode.J))
        {
            isInvincible = true;
            Vector3 stepDir = transform.forward;
            rb.linearVelocity = stepDir * 3.0f;
            nextAnim = AnimType.Rolling;
        }

        //ローリング終了
        if (isInvincible)
        {
            if (animatClipTable.GetAnimStateInfo() >= 1.0f)
            {
                isInvincible = false;

            }
        }

        UpdateAnimState(nextAnim);

        MoveCharacter();
    }

    private AnimType SetMove(float h, float v)
    {
        AnimType nextAnim;

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


            bool isDash = Input.GetKey(KeyCode.LeftShift);


            //三項演算子（shift押してたら走る、押してないなら歩く。）
            nextAnim = isDash ? AnimType.Run : AnimType.Walk;

            //三項演算子（ダッシュならtargetSpeedは最大速度に、押してないなら通常速度に。）
            float targetSpeed = isDash ? maxSpeed : maxSpeed / 3f;

            moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, Time.deltaTime * 5);

        }
        else
        {
            moveDirection = Vector3.zero;

            nextAnim = AnimType.Idle;
        }

        return nextAnim;
    }

 


    //復活用初期化処理
    private void StateReset()
    {
        hp = MaxHP;
        isDeath= false;

        UpdateAnimState(AnimType.Idle);
    }
    
    protected override void Death()
    {
        UpdateAnimState(AnimType.Death);
        isDeath = true;
    }

    public Vector2 SetHP()
    {
        Vector2 plHp;
        plHp.x = hp;
        plHp.y = MaxHP;
        return plHp;
    }

}
