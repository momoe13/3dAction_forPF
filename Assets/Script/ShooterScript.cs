using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class ShooterScript : MonoBehaviour
{
    //シングルトンの作成
    public static ShooterScript instance;


    [SerializeField] Transform instantPos;

    public ObjectPool<BulletScript>pool;

    private int MaxPoolSize = 30;
    
    [SerializeField] 
    BulletScript bullet;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);
    }

    private void Start()
    {

            SetupPool();
    }


    public void Shoot(int layerNum)
    {
        BulletScript obj = pool.Get();


        obj.transform.position = instantPos.position;
        obj.transform.rotation = instantPos.rotation;

        
        StartCoroutine(obj.Fire(instantPos.forward, layerNum));
    }




    private void SetupPool()
    {  // オブジェクトプールのインスタンスを生成
        pool = new ObjectPool<BulletScript>(
            CreateBullet,       // オブジェクト生成の際の処理
            OnGetBullet,         // オブジェクトを取り出す際の処理
            OnReturnBullet,       // オブジェクトを返却する際の処理
            OnDestroyBullet,    // プールが上限を超えた場合の処理
            true,                   // すでにプール内にいるオブジェクトを返却した際にエラー表示するか
            2,                      // 初期のプールの容量
            MaxPoolSize);           // プール内オブジェクトの上限数
    }


    /// <summary>
    /// 矢生成の際の処理
    /// </summary>
    /// <remarks>新しいオブジェクトをインスタンス化し、
    /// オブジェクトプールで使用できるように準備する</remarks>
    /// <returns>A new <see cref="GameObject"/> プールされたアイテムを表すインスタンス</returns>
    BulletScript CreateBullet()
    {
        return Instantiate(bullet);    // オブジェクトを生成してプールに渡す処理
    }

    /// <summary>
    /// オブジェクトを取り出す際の処理
    /// </summary>
    /// <param name="obj"></param>
    void OnGetBullet(BulletScript obj)
    {
        obj.gameObject.SetActive(true);
    }

    /// <summary>
    /// オブジェクトを返却する際の処理
    /// </summary>
    /// <param name="obj"></param>
    private void OnReturnBullet(BulletScript obj)
    {
        obj.gameObject.SetActive(false);   // オブジェクトを非アクティブにする処理
    }

    /// プールが上限を超えた場合の処理
    private void OnDestroyBullet(BulletScript obj)
    {
        Destroy(obj.gameObject);    // オブジェクトを破壊する処理
    }

    // 他のクラスから敵を取り出すための処理
    public void GetBullet()
    {
        pool.Get();
    }

    // 他のクラスから敵をプールに戻すための処理
    public void ReleaseBullet(BulletScript obj)
    {
        pool.Release(obj);
    }
}
