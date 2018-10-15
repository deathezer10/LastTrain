using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StationButton : MonoBehaviour {

    public GameObject m_Train;
    const float m_TrainStoppingPoint = 7.4f;

	// Use this for initialization
	void Start ()
    {
        GetComponent<Material>().SetColor("_Color", Color.red);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "GameController")
        {
            GetComponent<Material>().SetColor("_Color", Color.green);
            m_Train.GetComponent<TrainArriver>().BeginArrival();
        }
    }

}
