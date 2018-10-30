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
    class MoveData
    {
        public float h;
        public float v0;
        public float sin;
        public float cos;
        public float arrivalTime;

        public MoveData(Transform trans, float initialVelocity)
        {
            //コントローラの向いている角度(x軸回転)をラジアン角へ
            var angleFacing = -Mathf.Deg2Rad * trans.eulerAngles.x;
            h = trans.position.y;
            v0 = initialVelocity;
            sin = Mathf.Sin(angleFacing);
            cos = Mathf.Cos(angleFacing);
            var g = Gravity;
            arrivalTime = (v0 * sin) / g + Mathf.Sqrt((square(v0) * square(sin)) / square(g) + (2F * h) / g);
        }

        public void UpdateArrivalTime()
        {
            var g = Gravity;
            arrivalTime = (v0 * sin) / g + Mathf.Sqrt((square(v0) * square(sin)) / square(g) + (2F * h) / g);
        }
    }
    [Tag,SerializeField]
    private List<string> _getOnTags;

    [SerializeField] 
    private GameObject _targetMarker;

    [SerializeField]
    private LineRenderer _lineRenderer;

    [SerializeField]
    private float _vertexCount = 25;

    [SerializeField]
    private float _initialVelocity = 1;

    static readonly float Gravity = 9.81F;

    [SerializeField] 
    GameObject _ownPlayer;

    [SerializeField]
    private float _fadeTime = 0.1f;

    [SerializeField]
    private Color _fadeColor;

    [SerializeField]
    private Camera _camera;

    [SerializeField]
    private SteamVR_Input_Sources handType;
    [SerializeField]
    private SteamVR_Action_Boolean padAction;

    /// <summary>
    /// テレポートのアシストに使用するものの表示設定関数
    /// </summary>
    /// <param name="active"></param>
    private void TeleportAssistActive(bool active)
    {
        _targetMarker.SetActive(active);
        _lineRenderer.enabled = active;
    }

    private void Awake() {
        TeleportAssistActive(false);
    }

    void Start()
    {
        this.UpdateAsObservable()
            .Where(_ => padAction.GetStateUp(handType)) //コントローラーのトリガーを離したとき
            .Subscribe(_ => moveToPoint());                                       //ターゲットマーカーの位置へ移動

        //コントローラの入力の後に読みたい
        this.LateUpdateAsObservable()
            .Where(_ => padAction.GetState(handType)) // コントローラーのパッドを押している間
            .Subscribe(_ => showOrbit());        //放物線を表示させる
    }

    /// <summary>
    /// 移動先の位置を調整して移動
    /// </summary>
    void moveToPoint()
    {
        if (_lineRenderer.enabled)
        {
            FadeManager.Instance._fadeColor = _fadeColor;
            StartCoroutine(FadeManager.Instance.Fading(_fadeTime, _fadeTime, () =>
            {
                _ownPlayer.transform.position = _targetMarker.transform.position;
            }));
        }

        TeleportAssistActive(false);
    }

    /// <summary>
    /// 放物線を表示する関数
    /// </summary>
    void showOrbit()
    {
        //コントローラの向いている角度(x軸回転)をラジアン角へ
        var data = new MoveData(transform, _initialVelocity);

        // 目標地点の取得
        var indicationPos = GetIndicationPos(data.v0, data.cos, data.sin, data.arrivalTime);
        var vec = (indicationPos - this.transform.position).normalized;

        var hit = IsVisible(_camera.transform.position, vec);

        if (hit == null)
        {
            TeleportAssistActive(false);

            return;
        }

        // 床の上の高さにする
        //data.height = hit.Value.transform.position.y + hit.Value.collider.bounds.extents.y;
        data.UpdateArrivalTime();

        // 設定
        SetLineMarker(data);
    }

    /// <summary>
    /// 見えている？
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="dir"></param>
    /// <returns></returns>
    private RaycastHit? IsVisible(Vector3 pos,Vector3 dir)
    {
        Ray ray = new Ray(pos, dir);
        RaycastHit hit;
        Debug.DrawLine(ray.origin, ray.direction * _initialVelocity, Color.green);

        if (Physics.Raycast(ray, out hit, 20.0f) == false) return null;

        var colliderTag = hit.collider.tag;
        foreach (var tag in _getOnTags)
        {
            if(colliderTag == tag)
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

        for (var i = 0; i < _vertexCount; i++)
        {
            //delta時間あたりのワールド座標(ラインレンダラーの節)
            var delta = i * data.arrivalTime / _vertexCount;
            var x = data.v0 * data.cos * delta;
            var y = data.v0 * data.sin * delta - 0.5F * Gravity * square(delta);
            var forward = new Vector3(transform.forward.x, 0, transform.forward.z);
            Vector3 vertex = transform.position + forward * x + Vector3.up * y;

            if(vertex.IsAnyNan()) return;
            
            vertexes.Add(vertex);
        }

        TeleportAssistActive(true);

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
    static float square(float num)
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
    private Vector3 GetIndicationPos(float v0, float cos, float sin, float time)
    {
        //delta時間あたりのワールド座標(ラインレンダラーの節)
        var x = v0 * cos * time;
        var y = v0 * sin * time - 0.5F * Gravity * square(time);
        var forward = new Vector3(this.transform.forward.x, 0, this.transform.forward.z);
        var IndicationPos = this.transform.position + forward * x + Vector3.up * y;

        return IndicationPos;
    }
}
