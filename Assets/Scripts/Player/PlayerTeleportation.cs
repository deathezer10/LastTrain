using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Valve.VR;
using Valve.VR.InteractionSystem;
using System.Linq;

public class PlayerTeleportation : MonoBehaviour
{
    [SerializeField] 
    private GameObject _targetMarker;

    [SerializeField]
    private LineRenderer _lineRenderer;

    [SerializeField]
    private float _vertexCount = 25;

    [SerializeField]
    private float _initialVelocity = 1;

    private List<Vector3> _vertexes = new List<Vector3>();

    static readonly float Gravity = 9.81F;

    [SerializeField] 
    GameObject _ownPlayer;

    [SerializeField]
    private float _fadeTime = 0.1f;

    [SerializeField]
    private Color _fadeColor;

    [SerializeField]
    private Camera _camera;

    void Start()
    {
        _targetMarker.SetActive(false);
        //デバイスの入力受け付け
        var input = SteamVR_Input._default;

        this.UpdateAsObservable()                                               //Updateが呼ばれた時
            .Where(_ => input.inActions.GrabPinch.GetStateDown(SteamVR_Input_Sources.RightHand)) //コントローラーのトリガーを押している間
            .Subscribe(_ => _targetMarker.SetActive(true));

        this.UpdateAsObservable()
            .Where(_ => input.inActions.GrabPinch.GetStateUp(SteamVR_Input_Sources.RightHand)) //コントローラーのトリガーを離したとき
            .Subscribe(_ => moveToPoint());                                       //ターゲットマーカーの位置へ移動

        //コントローラの入力の後に読みたい
        this.LateUpdateAsObservable()
            .Where(_ => _targetMarker.activeSelf) //ターゲットマーカーが表示されているとき
            .Subscribe(_ => showOrbit());        //放物線を表示させる

        this.LateUpdateAsObservable()
            .Where(_ => !_targetMarker.activeSelf)          //ターゲットマーカーが非表示のとき
            .Subscribe(_ => _lineRenderer.enabled = false); //放物線を非表示にする
    }

    /// <summary>
    /// 移動先の位置を調整して移動
    /// </summary>
    void moveToPoint()
    {
        FadeManager.Instance._fadeColor = _fadeColor;
        StartCoroutine(FadeManager.Instance.Fading(_fadeTime, _fadeTime, () =>
        {
            _ownPlayer.transform.position = _targetMarker.transform.position;
        }));

        _targetMarker.SetActive(false);
    }

    /// <summary>
    /// 放物線を表示する関数
    /// </summary>
    void showOrbit()
    {
        _lineRenderer.enabled = true;

        //コントローラの向いている角度(x軸回転)をラジアン角へ
        var angleFacing = -Mathf.Deg2Rad * transform.eulerAngles.x;
        var h = transform.position.y;
        var v0 = _initialVelocity;
        var sin = Mathf.Sin(angleFacing);
        var cos = Mathf.Cos(angleFacing);
        var g = Gravity;

        //地面に到達する時間の式 :
        //  t = (v0 * sinθ) / g + √ (v0^2 * sinθ^2) / g^2 + 2 * h / g
        var arrivalTime = GetArrivalTime(v0, sin, cos, h);

        // 目標地点の取得
        var indicationPos = GetIndicationPos(v0, cos, sin, arrivalTime);
        var vec = (indicationPos - this.transform.position).normalized;

        Ray ray = new Ray(_camera.transform.position, vec);
        RaycastHit hit;
        Debug.DrawLine(ray.origin, ray.direction * _initialVelocity, Color.green);

        if (Physics.Raycast(ray, out hit, v0))
        {
            v0 = (hit.point - _camera.transform.position).magnitude;
            if (_ownPlayer.transform.position.y < hit.point.y)
            {
                arrivalTime = GetArrivalTime(v0, sin, cos, h);
            }
            else
            {
                h = hit.point.y;
            }
        }

        for (var i = 0; i < _vertexCount; i++)
        {
            //delta時間あたりのワールド座標(ラインレンダラーの節)
            var delta = i * arrivalTime / _vertexCount;
            var x = v0 * cos * delta;
            var y = v0 * sin * delta - 0.5F * g * square(delta);
            var forward = new Vector3(transform.forward.x, 0, transform.forward.z);
            var vertex = transform.position + forward * x + Vector3.up * y;
            _vertexes.Add(vertex);
        }
        //ターゲットマーカーを頂点の最終地点へ
        var last = _vertexes.Last();
        if (last.IsAnyNan() == false)
        {
            _targetMarker.transform.position = last;
            //LineRendererの頂点の設置
            _lineRenderer.positionCount = _vertexes.Count;
            _lineRenderer.SetPositions(_vertexes.ToArray());
        }
        //リストの初期化
        _vertexes.Clear();
    }

    /// <summary>
    /// 引数の2乗を返す関数
    /// </summary>
    /// <param name="num">Number.</param>
    static float square(float num)
    {
        return Mathf.Pow(num, 2);
    }

    private Vector3 GetIndicationPos(float v0, float cos, float sin, float time)
    {
        //delta時間あたりのワールド座標(ラインレンダラーの節)
        var x = v0 * cos * time;
        var y = v0 * sin * time - 0.5F * Gravity * square(time);
        var forward = new Vector3(this.transform.forward.x, 0, this.transform.forward.z);
        var IndicationPos = this.transform.position + forward * x + Vector3.up * y;

        return IndicationPos;
    } 

    private float GetArrivalTime(float initialVelocity,float sin,float cos,float height)
    {
        float g = Gravity;
        return (initialVelocity * sin) / g + Mathf.Sqrt((square(initialVelocity) * square(sin)) / square(g) + (2F * height) / g);
    }   
}
