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
    /// <summary>
    /// 移動の際使用するデータ
    /// </summary>
    private class MoveData
    {
        public float h { get; private set; }
        public float v0 { get; private set; }
        public float sin { get; private set; }
        public float cos { get; private set; }
        public float arrivalTime { get; private set; }
        public float height { get; set; }

        public MoveData(Transform trans, float initialVelocity)
        {
            //コントローラの向いている角度(x軸回転)をラジアン角へ
            var angleFacing = -Mathf.Deg2Rad * trans.eulerAngles.x;
            h = trans.position.y;
            v0 = initialVelocity;
            sin = Mathf.Sin(angleFacing);
            cos = Mathf.Cos(angleFacing);
            var g = Gravity;
            arrivalTime = (v0 * sin) / g + Mathf.Sqrt((Square(v0) * Square(sin)) / Square(g) + (2F * h) / g);
        }
    }

    [Tag, SerializeField]
    private List<string> _getOnTags;

    [SerializeField]
    private GameObject _targetMarker;

    [SerializeField]
    private LineRenderer _lineRenderer;

    [SerializeField]
    private float _vertexCount = 25;

    [SerializeField]
    private float _initialVelocity = 1;

    static readonly float Gravity = 9.81f;

    [SerializeField]
    private float _fadeTime = 0.1f;

    [SerializeField]
    private Color _fadeColor;

    [SerializeField]
    private Camera _camera;

    private PlayerViveController _controller;

    [SerializeField]
    private SteamVR_Action_Boolean _padAction;

    [SerializeField]
    private Color _possibleColor;
    [SerializeField]
    private Color _impossibleColor;

    private void Awake()
    {
        TeleportAssistActive(false);
        _controller = this.GetComponent<PlayerViveController>();
    }

    void Start()
    {
        this.UpdateAsObservable()
            .Where(_ => _padAction.GetStateUp(_controller.CurrentHand))
            .Subscribe(_ => MoveToPoint());

        //コントローラの入力の後に読みたい
        this.LateUpdateAsObservable()
            .Where(_ => _padAction.GetState(_controller.CurrentHand))
            .Subscribe(_ => ShowOrbit());
    }

    /// <summary>
    /// 移動先の位置を調整して移動
    /// </summary>
    private void MoveToPoint()
    {
        if (_targetMarker.activeSelf)
        {
            FadeManager.Instance._fadeColor = _fadeColor;
            StartCoroutine(FadeManager.Instance.Fading(_fadeTime, _fadeTime, () =>
            {
                _controller.Teleport(_targetMarker.transform.position);
            }));
        }

        TeleportAssistActive(false);
    }

    /// <summary>
    /// 放物線を表示する関数
    /// </summary>
    private void ShowOrbit()
    {
        //コントローラの向いている角度(x軸回転)をラジアン角へ
        var data = new MoveData(transform, _initialVelocity);

        // 目標地点の取得
        var indicationPos = GetIndicationPos(data);
        var vec = (indicationPos - this.transform.position).normalized;

        var hit = IsVisible(_camera.transform.position, vec);

        if (hit != null)
        {
            // 床の上の高さにする
            data.height = hit.Value.transform.position.y + hit.Value.collider.bounds.extents.y;
            _lineRenderer.startColor = _possibleColor;
            _lineRenderer.endColor = _possibleColor;
            _targetMarker.SetActive(true);
        }
        else
        {
            _targetMarker.SetActive(false);
            _lineRenderer.startColor = _impossibleColor;
            var end = _impossibleColor;
            end.a = 0.0f;
            _lineRenderer.endColor = end;
        }

        // 設定
        SetLineMarker(data);
    }

    /// <summary>
    /// 見えている？
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="dir"></param>
    /// <returns></returns>
    private RaycastHit? IsVisible(Vector3 pos, Vector3 dir)
    {
        Ray ray = new Ray(pos, dir);
        RaycastHit hit;
        Debug.DrawLine(ray.origin, ray.direction * _initialVelocity, Color.green);

        if (Physics.Raycast(ray, out hit, 20.0f, -1, QueryTriggerInteraction.Ignore) == false) return null;

        Debug.Log($"{hit.transform.gameObject}");

        var colliderTag = hit.collider.tag;
        foreach (var tag in _getOnTags)
        {
            if (colliderTag == tag)
            {
                return hit;
            }
        }
        return null;
    }

    /// <summary>
    /// ラインとマーカーの設定
    /// </summary>
    /// <param name="data"></param>
    private void SetLineMarker(MoveData data)
    {
        var vertexes = new List<Vector3>();

        for (var i = 0; i <= _vertexCount; i++)
        {
            //delta時間あたりのワールド座標(ラインレンダラーの節)
            var delta = i * data.arrivalTime / _vertexCount;
            var x = data.v0 * data.cos * delta;
            var y = data.v0 * data.sin * delta - 0.5f * Gravity * Square(delta);
            var forward = new Vector3(transform.forward.x, 0, transform.forward.z);
            Vector3 vertex = transform.position + forward * x + Vector3.up * y;
            vertex.y += i * data.height / _vertexCount;

            if (vertex.IsAnyNan()) return;

            vertexes.Add(vertex);
        }

        _lineRenderer.enabled = true;

        //ターゲットマーカーを頂点の最終地点へ
        var last = vertexes.Last();
        _targetMarker.transform.position = last;

        //LineRendererの頂点の設置
        _lineRenderer.positionCount = vertexes.Count;
        _lineRenderer.SetPositions(vertexes.ToArray());

        //リストの初期化
        vertexes.Clear();
    }

    /// <summary>
    /// 引数の2乗を返す関数
    /// </summary>
    /// <param name="num">Number.</param>
    static float Square(float num)
    {
        return Mathf.Pow(num, 2);
    }

    /// <summary>
    /// 到達目標地点を求める
    /// </summary>
    /// <param name="v0"></param>
    /// <param name="cos"></param>
    /// <param name="sin"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    private Vector3 GetIndicationPos(MoveData data)
    {
        //delta時間あたりのワールド座標(ラインレンダラーの節)
        var x = data.v0 * data.cos * data.arrivalTime;
        var y = data.v0 * data.sin * data.arrivalTime - 0.5F * Gravity * Square(data.arrivalTime);
        var forward = new Vector3(this.transform.forward.x, 0, this.transform.forward.z);
        var indicationPos = this.transform.position + forward * x + Vector3.up * y;

        return indicationPos;
    }

    /// <summary>
    /// テレポートのアシストに使用するものの表示設定関数
    /// </summary>
    /// <param name="active"></param>
    private void TeleportAssistActive(bool active)
    {
        _targetMarker.SetActive(active);
        _lineRenderer.enabled = active;
    }
}
