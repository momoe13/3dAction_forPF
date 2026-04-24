using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    //カメラの操作スピード
    Vector3 speed;

    //プレイヤー追従
    [SerializeField]
    GameObject playerObj;
    [SerializeField] float height = 1.5f;//カメラの高さ
    [SerializeField] float distance = 15.0f;//カメラとのオフセット
    [SerializeField] float rotAngle = 0.0f;//水平（横）方向のカメラ角度
    [SerializeField] float heightAngle = 10.0f;//垂直（縦）方向

    [SerializeField] float dis_min = 5.0f;//見上げた時のカメラ距離（任意）
    [SerializeField] float dis_mdl = 10.0f;//通常のカメラ距離

    Vector3 nowPos;//現在のプレイヤーの位置
    float nowRotAngele;
    float nowHeightAngle;

    //減衰挙動
    bool enableAtten = true;//減衰挙動フラグ
    [SerializeField] float attenRate = 3.0f;
    [SerializeField] float forwardDistance = 2.0f;
    Vector3 addForward;
    Vector3 prevTargetPos;
    [SerializeField] float rotAngleAttenRate=5.0f;
    [SerializeField] float angleAttenRate = 1.0f;

    //ロックオン機能
    [SerializeField] bool isRockon = true;

    [SerializeField]
    GameObject rockonTargetObj;

    [SerializeField] GameObject searchCircle;
    [SerializeField] GameObject targetIcon;

    LockOnSensor onSensor;

    private void Start()
    {
        nowPos = playerObj.transform.position;
        onSensor = searchCircle.GetComponent<LockOnSensor>();

    }
    private void LateUpdate()
    {
        rotAngle -= speed.x * Time.deltaTime * 50.0f;
        heightAngle += speed.z * Time.deltaTime * 20.0f;
        heightAngle = Mathf.Clamp(heightAngle, -40.0f, 60.0f);
        distance = Mathf.Clamp(distance, 5.0f, 40.0f);

        rockonTargetObj = onSensor.NowTarget;
        //画面の中心位置
        if (enableAtten)
        {
            //位置をズラすための増加量
            var target = playerObj.transform.position;

            if (isRockon) 
            {
                if (rockonTargetObj != null)
                {
                    //ロックオン対象の位置に上書き
                    target = rockonTargetObj.transform.position;
                }
                else isRockon = false;
            
            
            }

            var halfPoint = (playerObj.transform.position + target) / 2;
            var deltaPos = halfPoint - prevTargetPos;//位置の微小増加量
            prevTargetPos = halfPoint;
            deltaPos *= forwardDistance;

            //追加分の移動量
            addForward += deltaPos * Time.deltaTime * 20.0f;
            addForward = Vector3.Lerp(addForward, Vector3.zero, Time.deltaTime * attenRate);//追加分の移動

            nowPos = Vector3.Lerp(nowPos, halfPoint + Vector3.up * height + addForward, Mathf.Clamp01(Time.deltaTime * attenRate));

        }
        else nowPos = playerObj.transform.position + Vector3.up * height;

        //カメラの垂直
        if (enableAtten) nowRotAngele = Mathf.Lerp(nowRotAngele, rotAngle, Time.deltaTime * rotAngleAttenRate);
        else nowRotAngele = rotAngle;

        //カメラの水平角度
        if (enableAtten) nowHeightAngle = Mathf.Lerp(nowHeightAngle, heightAngle, Time.deltaTime * rotAngleAttenRate);
        else nowHeightAngle = heightAngle;

        if (isRockon)
        {
            var dis = Vector3.Distance(playerObj.transform.position,
                 rockonTargetObj.transform.position);

            if (heightAngle > 30)
            {
                distance = Mathf.Lerp(distance, dis_mdl * dis/ 10.0f * heightAngle / 30.0f, Time.deltaTime);
            }
            else if (heightAngle <= 30 && heightAngle >= -3)
            {
                distance = Mathf.Lerp(distance, dis_mdl * dis / 10.0f, Time.deltaTime);
            }
            else if (heightAngle < -3)
            {
                isRockon = false;
                Debug.Log("カメラ");
            }

        }
        else
        {
            //カメラの垂直角度でカメラの角度を変える
            if (heightAngle > 30)
            {
                distance = Mathf.Lerp(distance, 20.0f * heightAngle / 30.0f, Time.deltaTime);
            }
            else if (heightAngle <= 30 && heightAngle >= -3)
            {
                distance = Mathf.Lerp(distance, 20.0f, Time.deltaTime);
            }
            else if (heightAngle < -3)
            {
                distance = Mathf.Lerp(distance, dis_min, Time.deltaTime);
            }
        }

        //カメラ位置変更
        var deg = Mathf.Deg2Rad;
        var cx = Mathf.Sin(nowRotAngele * deg) * Mathf.Cos(nowHeightAngle * deg) * distance;
        var cy = Mathf.Sin(nowHeightAngle * deg) * distance;
        var cz = Mathf.Cos(nowRotAngele * deg) * Mathf.Cos(nowHeightAngle * deg) * distance;
        transform.position =nowPos+new Vector3(cx, cy, cz);

        //カメラをプレイヤーに向ける
        var rot = Quaternion.LookRotation((nowPos - transform.position).normalized);
        transform.rotation =rot;

        TargetItem();

    }

    public void OnCamera(InputAction.CallbackContext context)
    {
        speed =new Vector3(context.ReadValue<Vector2>().x,0f,context.ReadValue<Vector2>().y);

    }

    public void OnRockon(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isRockon = !isRockon;
        }
    }

    private void TargetItem()
    {
        if (isRockon && rockonTargetObj != null)
        {
            targetIcon.SetActive(true);
            
            targetIcon.transform.position = onSensor.GetTargetIconPos(); ;
        }
        else
        {
            targetIcon.SetActive(false);

        }
    }
}
