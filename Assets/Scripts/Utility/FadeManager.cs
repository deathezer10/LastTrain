using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

/// <summary>
/// シーン遷移時のフェードイン・アウトを制御するためのクラス
/// </summary>
public class FadeManager :SingletonMonoBehaviour<FadeManager>
{
    [SerializeField]
    private UnityEngine.UI.Image _image;

    // 現在の状態
    private bool _isFading = false;
    public bool IsFading
    {
        get { return _isFading; }
        private set { _isFading = value; }
    }

    // フェードの色
    public Color _fadeColor = Color.black;
    private Color _defoColor = Color.black;
    public Color ColorReset(){
        return _fadeColor = _defoColor;
    }

    private void Awake() {
        _defoColor = _fadeColor;
    }


    /// <summary>
    /// フェードイン
    /// </summary>
    /// <param name="interval"></param>
    /// <returns></returns>
    public IEnumerator FadeIn(float interval)
    {
        // Fade 開始
        this.IsFading = true;
        _image.color = _fadeColor;

        var tween = _image.DOFade(1, interval);
        yield return tween.WaitForCompletion();

        // フェード終わり
        this.IsFading = false;
    }
    /// <param name="callback"></param>
    public IEnumerator FadeIn(float interval,System.Action callback)
    {
        // フェード
        yield return StartCoroutine(FadeIn(interval));

        if (callback != null)
        {
            // コールバック
            callback();
        }
    }

    /// <summary>
    /// フェードアウト
    /// </summary>
    /// <param name="interval"></param>
    /// <returns></returns>
    public IEnumerator FadeOut(float interval)
    {
        // フェード開始
        this.IsFading = true;

        var tween = _image.DOFade(0, interval);
        yield return tween.WaitForCompletion();

        // フェード終わり
        this.IsFading = false;
    }

    public IEnumerator Fading(float inTime, float outTime, System.Action callback = null)
    {
        yield return StartCoroutine(FadeIn(inTime,callback));

        yield return StartCoroutine(FadeOut(outTime));
    }
}