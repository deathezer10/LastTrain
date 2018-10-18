using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class VRGUIHoverButton : VRGUIBase
{

    public UnityEvent OnHover;
    public UnityEvent OnHoverComplete;
    public UnityEvent OnHoverDisrupted;

    public override void OnPointerEntered()
    {
        GetComponent<Slider>().value = 0;

        if (OnHover != null)
            OnHover.Invoke();
    }

    public override void OnPointerExit()
    {
        Slider slider = GetComponent<Slider>();
        slider.value = 0;
        
        if (OnHoverDisrupted != null && slider.value < 1)
            OnHoverDisrupted.Invoke();
    }

    public override void OnPointerStay()
    {
        Slider slider = GetComponent<Slider>();
        slider.value = Mathf.Clamp01(slider.value + Time.deltaTime);

        if (OnHoverComplete != null && slider.value >= 1)
            OnHoverComplete.Invoke();
    }
}
