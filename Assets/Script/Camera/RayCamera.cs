using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RayCamera : MonoBehaviour
{
    [SerializeField] private Transform player;

    // 前フレームで遮蔽物として扱われていたゲームオブジェクトが格納される
    public GameObject[] prevRaycast;
    public List<GameObject> raycastHitsList_ = new List<GameObject>();

    private void Update()
    {

        //二つのオブジェクト間のベクトルを得ます
        Vector3 difference = (player.transform.position - this.transform.position);
        //.normalizedベクトルの正規化を行う
        Vector3 direction = difference.normalized;
        // Ray(開始地点,進む方向)
        Ray ray = new Ray(this.transform.position, direction);


        // Rayが衝突した全てのコライダーの情報を得る
        RaycastHit[] rayCastHits = Physics.RaycastAll(ray);

        // 前フレームで遮蔽物であった全てのGameObjectを保持
        prevRaycast = raycastHitsList_.ToArray();
        raycastHitsList_.Clear();

        //取得したオブジェクトを全て半透明にする
        foreach (RaycastHit hit in rayCastHits)
        {
            MaterialTransparent sampleMaterial = hit.collider.GetComponent<MaterialTransparent>();
           
            if (hit.collider.tag == "StageObj")
            {
                sampleMaterial.ClearMaterialInvoke();
                //次のフレームで使いたいため、不透明にしたオブジェクトを追加する
                raycastHitsList_.Add(hit.collider.gameObject);
            }
        }

        //ヒットしたゲームオブジェクトの差分を求め、今回衝突しなかったオブジェクトを不透明に戻す
        foreach (GameObject _gameObject in prevRaycast.Except<GameObject>(raycastHitsList_))
        {
            MaterialTransparent noSampleMaterial = _gameObject.GetComponent<MaterialTransparent>();
            // 遮蔽物でなくなったGameObjectを不透明に戻す
            if (_gameObject != null)
            {
                noSampleMaterial.NotClearMaterialInvoke();
            }
        }

        Debug.DrawRay(ray.origin, ray.direction*10,Color.green,1,false);
    }
}
