using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CallButtom : MonoBehaviour
{

    private  bool _isCall = false;

    private GameObject Button;

    public GameObject Train;
    public GameObject Station;

    public float speed;
    public float comeTime = 3.0f;
    
    void Update()
    {
        // Test 
        comeTime -= Time.deltaTime;

        if (comeTime < 0) {_isCall = true;}

        if (_isCall)
        {
            ChangeColor();
            ArriveTrain();
            comeTime = 99999;
        }

        // If the button is pressed

        /* ボタンを作る
        
        Colliderでコントローラーとボタンが接触していたら 
        && 
        コントローラーが押されていたら
    
         */

    }

    void ArriveTrain()
    {
        Train.transform.LookAt
            (Station.transform.position);

        Train.transform.DOMove
            (Station.transform.position, speed).SetEase(Ease.OutExpo);

        _isCall = false;
    }

    // オブジェクトの色を変える
    void ChangeColor()
    {
        GetComponent<Renderer>().material.color = Color.red;
    }
}
