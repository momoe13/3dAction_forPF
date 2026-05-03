using UnityEngine;
using UnityEngine.Splines;

public class CameraMove : MonoBehaviour
{ 
    // スプライン
    [SerializeField] private SplineContainer _splineContainer;

    // スプラインに沿って移動させる対象
    [SerializeField] private Transform _followTarget;

    // 補間の割合
    [SerializeField, Range(0, 1)] private float _percentage;

    private void Update()
    {
        // 念のためNullチェック
        if (_splineContainer == null || _followTarget == null)
            return;

        // 計算した位置（ワールド座標）をターゲットに代入
        _followTarget.position = _splineContainer.EvaluatePosition(_percentage);
    }
}
