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
        var arrivalTime = (v0 * sin) / g + Mathf.Sqrt((square(v0) * square(sin)) / square(g) + (2F * h) / g);

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
        if ((float.IsNaN(last.x) || float.IsNaN(last.y) || float.IsNaN(last.z)) == false)
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
}
