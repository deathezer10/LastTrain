using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Useful for dynamic objects such as balls, fire extinguisher, keycard
/// </summary>
[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public abstract class GrabbableObject : MonoBehaviour, IGrabbable, IInteractable
{
    [SerializeField]
    protected List<Negi.Outline> _outlines = null;
    private void Awake()
    {
        _outlines = new List<Negi.Outline>();
        var objects = this.gameObject.transform.GetAllChild();
        objects.Add(this.gameObject);

        foreach (var obj in objects)
        {
            var renderer = obj.GetComponent<Renderer>();
            if (renderer == null) continue;

            // particle ignore
            var pRenderer = renderer as ParticleSystemRenderer;
            if(pRenderer) continue;

            var outline = renderer.gameObject.SafeAddComponent<Negi.Outline>();
            _outlines.Add(outline);
        }
        SetEnableOutline(false);
    }

    /// <summary>
    /// アウトラインの有無設定
    /// </summary>
    /// <param name="active"></param>
    private void SetEnableOutline(bool active)
    {
        foreach (var outline in _outlines)
        {
            outline.enabled = active;
        }
    }

    public virtual bool hideControllerOnGrab { get { return true; } }

    public virtual void OnControllerEnter(PlayerViveController currentController)
    {
    }

    public virtual void OnControllerExit()
    {
        SetEnableOutline(false);
    }

    public virtual void OnControllerStay()
    {
        SetEnableOutline(true);
    }

    public virtual void OnGrab()
    {
        SetEnableOutline(false);
    }

    public virtual void OnGrabStay()
    {
        SetEnableOutline(false);
    }

    public virtual void OnGrabReleased()
    {
        SetEnableOutline(false);
    }

    public virtual void OnUseDown() { }
    public virtual void OnUse() { }
    public virtual void OnUseUp() { }
}
