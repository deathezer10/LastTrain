using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screw : MonoBehaviour
{
    private ScrewDriver m_ScrewDriver;

    private float Turnspeed;
    public float screwDistance = 0.03f;
    private Vector3 OriginalPosition;
    public int Axis;
    public float x, y, z;
    private bool bIsLoose = false;
    public string Origin;
    public int rotateAroundAxis;

    public delegate void Unscrewed(string _object);
    public static event Unscrewed OnLoose;

    // Use this for initialization
    void Start()
    {
        m_ScrewDriver = FindObjectOfType<ScrewDriver>();
        OriginalPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {

    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "ScrewDriver")
        {
            other.GetComponent<ScrewDriver>().bIsScrewing = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!bIsLoose)
        {
            if (other.tag == "ScrewDriver")
            {
                other.GetComponent<ScrewDriver>().bIsScrewing = true;

                    Turnspeed = m_ScrewDriver.speed * 0.6f;
                    switch (rotateAroundAxis)
                    {
                        case 1:
                            {
                                transform.Rotate(new Vector3(-1, 0, 0), Turnspeed);
                                break;
                            }

                        case 2:
                            {
                                transform.Rotate(new Vector3(0, -1, 0), Turnspeed);
                                break;
                            }

                        case 3:
                            {
                                transform.Rotate(new Vector3(0, 0, 1), Turnspeed);
                                break;
                            }
                    }




                    switch (Axis)
                    {
                        case 3:
                            {
                                if (z < 0)
                                {
                                    screwDistance *= -1;
                                    if ((OriginalPosition.z + screwDistance) > transform.localPosition.z)
                                    {
                                        bIsLoose = true;
                                        transform.GetComponent<Rigidbody>().isKinematic = false;
                                        transform.GetComponent<Rigidbody>().useGravity = true;
                                        transform.GetComponent<BoxCollider>().isTrigger = false;
                                        OnLoose(Origin);
                                        Destroy(this);
                                        break;
                                    }

                                    else
                                    {
                                        transform.position += new Vector3(0, 0, z * Time.deltaTime);
                                        break;
                                    }

                                }

                                else
                                {
                                   
                                    if ((OriginalPosition.z + screwDistance) < transform.localPosition.z)
                                    {
                                        bIsLoose = true;
                                        transform.GetComponent<Rigidbody>().isKinematic = false;
                                        transform.GetComponent<Rigidbody>().useGravity = true;
                                        transform.GetComponent<BoxCollider>().isTrigger = false;
                                        OnLoose(Origin);
                                        Destroy(this);
                                        break;
                                    }

                                    else
                                    {
                                        transform.position += new Vector3(0, 0, (z * Time.deltaTime));
                                        break;
                                    }
                                }
                            }



                        case 2:
                            {
                                if (y < 0)
                                {
                                    screwDistance *= -1;
                                    if (OriginalPosition.y + screwDistance > transform.localPosition.y)
                                    {
                                        bIsLoose = true;
                                        transform.GetComponent<Rigidbody>().useGravity = true;
                                        transform.GetComponent<Rigidbody>().isKinematic = false;
                                        transform.GetComponent<BoxCollider>().isTrigger = false;
                                        OnLoose(Origin);
                                        Destroy(this);
                                        break;
                                    }

                                    else
                                    {
                                        transform.position += new Vector3(0, y * Time.deltaTime, 0);
                                        break;
                                    }

                                }

                                else
                                {
                                    if (OriginalPosition.y + screwDistance < transform.localPosition.y)
                                    {
                                        bIsLoose = true;
                                        transform.GetComponent<Rigidbody>().useGravity = true;
                                        transform.GetComponent<Rigidbody>().isKinematic = false;
                                        transform.GetComponent<BoxCollider>().isTrigger = false;
                                        OnLoose(Origin);
                                        Destroy(this);
                                        break;
                                    }

                                    else
                                    {
                                        transform.position += new Vector3(0, y * Time.deltaTime, 0);
                                        break;
                                    }
                                }
                            }


                        case 1:
                            {
                                if (x < 0)
                                {

                                    if (OriginalPosition.x - screwDistance > transform.localPosition.x)
                                    {
                                        bIsLoose = true;
                                        transform.GetComponent<Rigidbody>().useGravity = true;
                                        transform.GetComponent<Rigidbody>().isKinematic = false;
                                        transform.GetComponent<BoxCollider>().isTrigger = false;
                                        OnLoose(Origin);
                                        Destroy(this);
                                        break;
                                    }

                                    else
                                    {
                                        transform.position += new Vector3(x * Time.deltaTime, 0, 0);
                                        break;
                                    }

                                }

                                else
                                {
                                    if (OriginalPosition.x + screwDistance < transform.localPosition.x)
                                    {
                                        bIsLoose = true;
                                        transform.GetComponent<Rigidbody>().useGravity = true;
                                        transform.GetComponent<Rigidbody>().isKinematic = false;
                                        transform.GetComponent<BoxCollider>().isTrigger = false;
                                        OnLoose(Origin);
                                        Destroy(this);
                                        break;
                                    }

                                    else
                                    {
                                        transform.position += new Vector3(x * Time.deltaTime, 0, 0);
                                        break;
                                    }
                                }
                            }
                    }
                }
            }
        }
    }




