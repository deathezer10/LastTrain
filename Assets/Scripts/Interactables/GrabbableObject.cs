﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Useful for dynamic objects such as balls, fire extinguisher, keycard
/// </summary>
[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class GrabbableObject : MonoBehaviour, IGrabbable, IInteractable
{
    protected List<Negi.Outline> _outlines = null;

    private void Awake()
    {
        SetOutline();
    }

    private void SetOutline()
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
            if (pRenderer) continue;

            var outline = renderer.gameObject.SafeAddComponent<Negi.Outline>();
            outline.enabled = false;
            _outlines.Add(outline);
        }
    }

    /// <summary>
    /// アウトラインの有無設定
    /// </summary>
    /// <param name="active"></param>
    private void SetEnableOutline(bool active)
    {
        if (_outlines != null)
            foreach (var outline in _outlines)
            {
                if (outline.Renderer != null && outline.Renderer.enabled)
                    outline.enabled = active;
            }
    }

    public virtual bool hideControllerOnGrab { get { return true; } }

    public virtual void OnControllerEnter(PlayerViveController currentController)
    {
        SetEnableOutline(true);
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
    }

    public virtual void OnGrabStay()
    {
        SetEnableOutline(false);
    }

    public virtual void OnGrabReleased() { }
    public virtual void OnUseDown() { }
    public virtual void OnUse() { }
    public virtual void OnUseUp() { }
}
