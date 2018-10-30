﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screw : MonoBehaviour
{
    private BoxCollider ColliderTip;
    private ScrewDriver m_ScrewDriver;

    private float Turnspeed = -1.0f;
    private float screwDistance = 0.03f;
    private Vector3 OriginalPosition;
    public int value;
    public float x, y, z;
    private bool bIsLoose = false;
    // Use this for initialization
    void Start()
    {
        ColliderTip = transform.GetComponent<BoxCollider>();
        m_ScrewDriver = FindObjectOfType<ScrewDriver>();
        OriginalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (!bIsLoose)
        {
            if (other.tag == "ScrewDriver")
            {
                if (m_ScrewDriver.bIsScrewing)
                {
                    transform.Rotate(new Vector3(0, 0, -1), Turnspeed);
                    switch (value)
                    {
                        case 1:
                            {
                                if (z < 0)
                                {
                                    screwDistance *= -1;
                                    if (OriginalPosition.z + screwDistance > transform.position.z)
                                    {
                                        bIsLoose = true;
                                        break;
                                    }

                                    else
                                    {
                                        transform.position += new Vector3(0, 0, z);
                                        break;
                                    }
                                    
                                }

                                else
                                {
                                    if (OriginalPosition.z + screwDistance < transform.position.z)
                                    {
                                        bIsLoose = true;
                                        break;
                                    }

                                    else
                                    {
                                        transform.position += new Vector3(0, 0, z);
                                        break;
                                    }
                                } 
                            }



                        case 2:
                            {
                                if (y < 0)
                                {
                                    screwDistance *= -1;
                                    if (OriginalPosition.y + screwDistance > transform.position.y)
                                    {
                                        bIsLoose = true;
                                        break;
                                    }

                                    else
                                    {
                                        transform.position += new Vector3(0, y, 0);
                                        break;
                                    }

                                }

                                else
                                {
                                    if (OriginalPosition.y + screwDistance < transform.position.y)
                                    {
                                        bIsLoose = true;
                                        break;
                                    }

                                    else
                                    {
                                        transform.position += new Vector3(0, y, 0);
                                        break;
                                    }
                                }
                            }


                        case 3:
                            {
                                if (x < 0)
                                {
                                    screwDistance *= -1;
                                    if (OriginalPosition.x + screwDistance > transform.position.x)
                                    {
                                        bIsLoose = true;
                                        break;
                                    }

                                    else
                                    {
                                        transform.position += new Vector3(x, 0, 0);
                                        break;
                                    }

                                }

                                else
                                {
                                    if (OriginalPosition.x + screwDistance < transform.position.x)
                                    {
                                        bIsLoose = true;
                                        break;
                                    }

                                    else
                                    {
                                        transform.position += new Vector3(x, 0, 0);
                                        break;
                                    }
                                }
                            }


                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {

    }


}
