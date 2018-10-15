using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StationButton : MonoBehaviour {

    public GameObject m_Train;
    private const float m_ToggleOffset = -0.02f;
    private bool m_Toggled = false;

	// Use this for initialization
	void Start ()
    {
        GetComponent<Renderer>().material.SetColor("_Color", Color.red);
    }
	
    private void OnTriggerEnter(Collider other)
    {
        if (m_Toggled == false && other.tag == "GameController")
        {
            m_Toggled = true;

            GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            m_Train.GetComponent<TrainArriver>().BeginArrival();

            Vector3 newPos = transform.position;
            newPos.x += m_ToggleOffset;

            transform.position = newPos;
        }
    }

}
