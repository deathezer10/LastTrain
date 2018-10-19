using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(BoxCollider))]
public abstract class VRGUIBase : MonoBehaviour
{

    public abstract void OnPointerEntered();

    // Called once every Update when the laser pointer is on the collider
    public abstract void OnPointerStay();

    public abstract void OnPointerExit();

}

#if UNITY_EDITOR
[CustomEditor(typeof(VRGUIBase), true)]
class VRGUIInspector : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }
    
}
#endif