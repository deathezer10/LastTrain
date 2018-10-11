using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class MaterialTileAdjuster : MonoBehaviour
{

    public string m_TargetArgument = "_MainTex";
    public Vector2 m_OffsetSpeed;

    private Material m_CurrentMaterial;

    private void Start()
    {
        m_CurrentMaterial = GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        m_CurrentMaterial.SetTextureOffset(m_TargetArgument, m_CurrentMaterial.GetTextureOffset(m_TargetArgument) + m_OffsetSpeed * Time.deltaTime);
    }


}
