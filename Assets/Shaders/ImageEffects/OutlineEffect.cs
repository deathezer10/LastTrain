using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.XR;

namespace cakeslice
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Camera))]
    [ExecuteInEditMode]
    public class OutlineEffect : MonoBehaviour
    {
        private static OutlineEffect _instance;
        public static OutlineEffect Instance
        {
            get
            {
                if(Equals(_instance, null))
                {
                    return _instance = FindObjectOfType(typeof(OutlineEffect)) as OutlineEffect;
                }

                return _instance;
            }
        }
        private OutlineEffect() { }

        private readonly LinkedSet<Outline> _outlines = new LinkedSet<Outline>();

        [Range(1.0f, 6.0f)]
        public float _lineThickness = 1.25f;
        [Range(0, 10)]
        public float _lineIntensity = .5f;
        [Range(0, 1)]
        public float _fillAmount = 0.2f;

        public Color _lineColor0 = Color.red;
        public Color _lineColor1 = Color.green;
        public Color _lineColor2 = Color.blue;

        public bool _additiveRendering = false;

        public bool _backfaceCulling = true;

        [Header("These settings can affect performance!")]
        public bool _cornerOutlines = false;
        public bool _addLinesBetweenColors = false;

        [Header("Advanced settings")]
        public bool _scaleWithScreenSize = true;
        [Range(0.1f, .9f)]
        public float _alphaCutoff = .5f;
        public bool flipY = false;
        public Camera _sourceCamera;

        [HideInInspector]
        public Camera _outlineCamera;
        Material _outline1Material;
        Material _outline2Material;
        Material _outline3Material;
        Material _outlineEraseMaterial;
        Shader _outlineShader;
        Shader _outlineBufferShader;
        [HideInInspector]
        public Material _outlineShaderMaterial;
        [HideInInspector]
        public RenderTexture _renderTexture;
        [HideInInspector]
        public RenderTexture _extraRenderTexture;

        CommandBuffer _commandBuffer;

        Material GetMaterialFromID(int ID)
        {
            if(ID == 0)
                return _outline1Material;
            else if(ID == 1)
                return _outline2Material;
            else
                return _outline3Material;
        }
        List<Material> _materialBuffer = new List<Material>();
        Material CreateMaterial(Color emissionColor)
        {
            Material m = new Material(_outlineBufferShader);
            m.SetColor("_Color", emissionColor);
            m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            m.SetInt("_ZWrite", 0);
            m.DisableKeyword("_ALPHATEST_ON");
            m.EnableKeyword("_ALPHABLEND_ON");
            m.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            m.renderQueue = 3000;
            return m;
        }

        private void Awake()
        {
            _instance = this;
        }

        void Start()
        {
            CreateMaterialsIfNeeded();
            UpdateMaterialsPublicProperties();

            if(_sourceCamera == null)
            {
                _sourceCamera = GetComponent<Camera>();

                if(_sourceCamera == null)
                    _sourceCamera = Camera.main;
            }

            if(_outlineCamera == null)
            {
                GameObject cameraGameObject = new GameObject("Outline Camera");
                cameraGameObject.transform.parent = _sourceCamera.transform;
                _outlineCamera = cameraGameObject.AddComponent<Camera>();
                _outlineCamera.enabled = false;
            }

            _renderTexture = new RenderTexture(_sourceCamera.pixelWidth, _sourceCamera.pixelHeight, 16, RenderTextureFormat.Default);
            _extraRenderTexture = new RenderTexture(_sourceCamera.pixelWidth, _sourceCamera.pixelHeight, 16, RenderTextureFormat.Default);
            UpdateOutlineCameraFromSource();

            _commandBuffer = new CommandBuffer();
            _outlineCamera.AddCommandBuffer(CameraEvent.BeforeImageEffects, _commandBuffer);
        }

        public void OnPreRender()
        {
            if(_commandBuffer == null)
                return;

            CreateMaterialsIfNeeded();

            if(_renderTexture == null || _renderTexture.width != _sourceCamera.pixelWidth || _renderTexture.height != _sourceCamera.pixelHeight)
            {
                _renderTexture = new RenderTexture(_sourceCamera.pixelWidth, _sourceCamera.pixelHeight, 16, RenderTextureFormat.Default);
                _extraRenderTexture = new RenderTexture(_sourceCamera.pixelWidth, _sourceCamera.pixelHeight, 16, RenderTextureFormat.Default);
                _outlineCamera.targetTexture = _renderTexture;
            }
            UpdateMaterialsPublicProperties();
            UpdateOutlineCameraFromSource();
            _outlineCamera.targetTexture = _renderTexture;
            _commandBuffer.SetRenderTarget(_renderTexture);

            _commandBuffer.Clear();
            if(_outlines != null)
            {
                foreach(Outline outline in _outlines)
                {
                    LayerMask l = _sourceCamera.cullingMask;

                    if(outline != null && l == (l | (1 << outline._originalLayer)))
                    {
                        for(int v = 0; v < outline.Renderer.sharedMaterials.Length; v++)
                        {
                            Material m = null;

                            if(outline.Renderer.sharedMaterials[v].mainTexture != null && outline.Renderer.sharedMaterials[v])
                            {
                                foreach(Material g in _materialBuffer)
                                {
                                    if(g.mainTexture == outline.Renderer.sharedMaterials[v].mainTexture)
                                    {
                                        if(outline._eraseRenderer && g.color == _outlineEraseMaterial.color)
                                            m = g;
                                        else if(g.color == GetMaterialFromID(outline._color).color)
                                            m = g;
                                    }
                                }

                                if(m == null)
                                {
                                    if(outline._eraseRenderer)
                                        m = new Material(_outlineEraseMaterial);
                                    else
                                        m = new Material(GetMaterialFromID(outline._color));
                                    m.mainTexture = outline.Renderer.sharedMaterials[v].mainTexture;
                                    _materialBuffer.Add(m);
                                }
                            }
                            else
                            {
                                if(outline._eraseRenderer)
                                    m = _outlineEraseMaterial;
                                else
                                    m = GetMaterialFromID(outline._color);
                            }

                            if(_backfaceCulling)
                                m.SetInt("_Culling", (int)UnityEngine.Rendering.CullMode.Back);
                            else
                                m.SetInt("_Culling", (int)UnityEngine.Rendering.CullMode.Off);

                            _commandBuffer.DrawRenderer(outline.GetComponent<Renderer>(), m, 0, 0);
                            MeshFilter mL = outline.GetComponent<MeshFilter>();
                            if(mL)
                            {
                                for(int i = 1; i < mL.sharedMesh.subMeshCount; i++)
                                    _commandBuffer.DrawRenderer(outline.GetComponent<Renderer>(), m, i, 0);
                            }
                            SkinnedMeshRenderer sMR = outline.GetComponent<SkinnedMeshRenderer>();
                            if(sMR)
                            {
                                for(int i = 1; i < sMR.sharedMesh.subMeshCount; i++)
                                    _commandBuffer.DrawRenderer(outline.GetComponent<Renderer>(), m, i, 0);
                            }
                        }
                    }
                }
            }

            _outlineCamera.Render();
        }

        private void OnEnable()
        {
            Outline[] o = FindObjectsOfType<Outline>();

            foreach(Outline oL in o)
            {
                oL.enabled = false;
                oL.enabled = true;
            }
        }

        void OnDestroy()
        {
            if(_renderTexture != null)
                _renderTexture.Release();
            if(_extraRenderTexture != null)
                _extraRenderTexture.Release();
            DestroyMaterials();
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            _outlineShaderMaterial.SetTexture("_OutlineSource", _renderTexture);

            if(_addLinesBetweenColors)
            {
                Graphics.Blit(source, _extraRenderTexture, _outlineShaderMaterial, 0);
                _outlineShaderMaterial.SetTexture("_OutlineSource", _extraRenderTexture);
            }
            Graphics.Blit(source, destination, _outlineShaderMaterial, 1);
        }

        private void CreateMaterialsIfNeeded()
        {
            if(_outlineShader == null)
                _outlineShader = Resources.Load<Shader>("OutlineShader");
            if(_outlineBufferShader == null)
            {
                _outlineBufferShader = Resources.Load<Shader>("OutlineBufferShader");
            }
            if(_outlineShaderMaterial == null)
            {
                _outlineShaderMaterial = new Material(_outlineShader);
                _outlineShaderMaterial.hideFlags = HideFlags.HideAndDontSave;
                UpdateMaterialsPublicProperties();
            }
            if(_outlineEraseMaterial == null)
                _outlineEraseMaterial = CreateMaterial(new Color(0, 0, 0, 0));
            if(_outline1Material == null)
                _outline1Material = CreateMaterial(new Color(1, 0, 0, 0));
            if(_outline2Material == null)
                _outline2Material = CreateMaterial(new Color(0, 1, 0, 0));
            if(_outline3Material == null)
                _outline3Material = CreateMaterial(new Color(0, 0, 1, 0));
        }

        private void DestroyMaterials()
        {
            foreach(Material m in _materialBuffer)
                DestroyImmediate(m);
            _materialBuffer.Clear();
            DestroyImmediate(_outlineShaderMaterial);
            DestroyImmediate(_outlineEraseMaterial);
            DestroyImmediate(_outline1Material);
            DestroyImmediate(_outline2Material);
            DestroyImmediate(_outline3Material);
            _outlineShader = null;
            _outlineBufferShader = null;
            _outlineShaderMaterial = null;
            _outlineEraseMaterial = null;
            _outline1Material = null;
            _outline2Material = null;
            _outline3Material = null;
        }

        public void UpdateMaterialsPublicProperties()
        {
            if(_outlineShaderMaterial)
            {
                float scalingFactor = 1;
                if(_scaleWithScreenSize)
                {
                    // If Screen.height gets bigger, outlines gets thicker
                    scalingFactor = Screen.height / 360.0f;
                }

                // If scaling is too small (height less than 360 pixels), make sure you still render the outlines, but render them with 1 thickness
                if(_scaleWithScreenSize && scalingFactor < 1)
                {
                    if(XRSettings.isDeviceActive && _sourceCamera.stereoTargetEye != StereoTargetEyeMask.None)
                    {
                        _outlineShaderMaterial.SetFloat("_LineThicknessX", (1 / 1000.0f) * (1.0f / XRSettings.eyeTextureWidth) * 1000.0f);
                        _outlineShaderMaterial.SetFloat("_LineThicknessY", (1 / 1000.0f) * (1.0f / XRSettings.eyeTextureHeight) * 1000.0f);
                    }
                    else
                    {
                        _outlineShaderMaterial.SetFloat("_LineThicknessX", (1 / 1000.0f) * (1.0f / Screen.width) * 1000.0f);
                        _outlineShaderMaterial.SetFloat("_LineThicknessY", (1 / 1000.0f) * (1.0f / Screen.height) * 1000.0f);
                    }
                }
                else
                {
                    if(XRSettings.isDeviceActive && _sourceCamera.stereoTargetEye != StereoTargetEyeMask.None)
                    {
                        _outlineShaderMaterial.SetFloat("_LineThicknessX", scalingFactor * (_lineThickness / 1000.0f) * (1.0f / XRSettings.eyeTextureWidth) * 1000.0f);
                        _outlineShaderMaterial.SetFloat("_LineThicknessY", scalingFactor * (_lineThickness / 1000.0f) * (1.0f / XRSettings.eyeTextureHeight) * 1000.0f);
                    }
                    else
                    {
                        _outlineShaderMaterial.SetFloat("_LineThicknessX", scalingFactor * (_lineThickness / 1000.0f) * (1.0f / Screen.width) * 1000.0f);
                        _outlineShaderMaterial.SetFloat("_LineThicknessY", scalingFactor * (_lineThickness / 1000.0f) * (1.0f / Screen.height) * 1000.0f);
                    }
                }
                _outlineShaderMaterial.SetFloat("_LineIntensity", _lineIntensity);
                _outlineShaderMaterial.SetFloat("_FillAmount", _fillAmount);
                _outlineShaderMaterial.SetColor("_LineColor1", _lineColor0 * _lineColor0);
                _outlineShaderMaterial.SetColor("_LineColor2", _lineColor1 * _lineColor1);
                _outlineShaderMaterial.SetColor("_LineColor3", _lineColor2 * _lineColor2);
                if(flipY)
                    _outlineShaderMaterial.SetInt("_FlipY", 1);
                else
                    _outlineShaderMaterial.SetInt("_FlipY", 0);
                if(!_additiveRendering)
                    _outlineShaderMaterial.SetInt("_Dark", 1);
                else
                    _outlineShaderMaterial.SetInt("_Dark", 0);
                if(_cornerOutlines)
                    _outlineShaderMaterial.SetInt("_CornerOutlines", 1);
                else
                    _outlineShaderMaterial.SetInt("_CornerOutlines", 0);

                Shader.SetGlobalFloat("_OutlineAlphaCutoff", _alphaCutoff);
            }
        }

        void UpdateOutlineCameraFromSource()
        {
            _outlineCamera.CopyFrom(_sourceCamera);
            _outlineCamera.renderingPath = RenderingPath.Forward;
            _outlineCamera.backgroundColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            _outlineCamera.clearFlags = CameraClearFlags.SolidColor;
            _outlineCamera.rect = new Rect(0, 0, 1, 1);
            _outlineCamera.cullingMask = 0;
            _outlineCamera.targetTexture = _renderTexture;
            _outlineCamera.enabled = false;
#if UNITY_5_6_OR_NEWER
            _outlineCamera.allowHDR = false;
#else
            outlineCamera.hdr = false;
#endif
        }

        public void AddOutline(Outline outline)
        {
            if(!_outlines.Contains(outline))
                _outlines.Add(outline);
        }

        public void RemoveOutline(Outline outline)
        {
            if(_outlines.Contains(outline))
                _outlines.Remove(outline);
        }
    }
}