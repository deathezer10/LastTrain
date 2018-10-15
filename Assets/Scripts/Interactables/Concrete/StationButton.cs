using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StationButton : MonoBehaviour {

    public GameObject m_Train;

	// Use this for initialization
	void Start ()
    {
        GetComponent<Renderer>().material.SetColor("_Color", Color.red);
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.A))
        {

            m_Train.GetComponent<TrainArriver>().BeginArrival();
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "GameController")
        {
            GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            m_Train.GetComponent<TrainArriver>().BeginArrival();
        }
    }

}
