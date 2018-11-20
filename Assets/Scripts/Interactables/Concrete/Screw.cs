using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screw : GrabbableObject
{
    private ScrewDriver m_ScrewDriver;
    public GameObject grenadeParticlePrefab;
    private float Turnspeed;
    public float screwDistance = 0.03f;
    private Vector3 OriginalPosition;
    public int Axis;
    public float x, y, z;
    private bool bIsLoose = false;
    public string Origin;
    public int rotateAroundAxis;
    private RaycastHit[] rayCastHits;
    private int screwhits;
    private AudioPlayer audioPlayer;
    public override bool hideControllerOnGrab => base.hideControllerOnGrab;

    public delegate void Unscrewed(string _object);
    public static event Unscrewed OnLoose;

    // Use this for initialization
    void Start()
    {
        m_ScrewDriver = FindObjectOfType<ScrewDriver>();
        OriginalPosition = transform.localPosition;
        audioPlayer = GetComponent<AudioPlayer>();
        m_DropSoundHandler.SetImpactNoiseData(new DropSoundHandler.ImpactNoiseData { soundType = DropSoundHandler.DropSoundType.Metal });
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Tip" || other.tag == "ScrewDriver")
        {
            m_ScrewDriver.bIsScrewing = false;
            audioPlayer.Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!bIsLoose)
            if (other.name == "Tip")
            {
                m_ScrewDriver.bIsScrewing = true;
                audioPlayer.Play();
            }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!bIsLoose)
        {
            if (other.name == "Tip")
            {
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
                                    m_ScrewDriver.bIsScrewing = false;
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
                                    m_ScrewDriver.bIsScrewing = false;
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
                                    m_ScrewDriver.bIsScrewing = false;
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
                                    m_ScrewDriver.bIsScrewing = false;
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
                                    m_ScrewDriver.bIsScrewing = false;
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
                                    m_ScrewDriver.bIsScrewing = false;
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

    public override void OnGrab()
    {
        base.OnGrab();
    }

    public override void OnGrabStay()
    {
        base.OnGrabStay();
    }

    public override void OnGrabReleased()
    {
        base.OnGrabReleased();
        StartCoroutine(secret());
    }

    private IEnumerator secret()
    {
        yield return new WaitForSeconds(2);
        
        //rayCastHits = Physics.SphereCastAll(transform.position, 0.01f, Vector3.down, 0.2f);
        rayCastHits = Physics.RaycastAll(transform.position, Vector3.down, 0.2f);
        Debug.DrawRay(transform.position, Vector3.down, Color.red, 5.0f, false);

        for (int hits = 0; hits < rayCastHits.Length; hits++)
        {
            if (rayCastHits[hits].transform.gameObject.GetComponent<Screw>() != null)
            {
                if (transform.position.y > rayCastHits[hits].transform.position.y)
                    screwhits += 1;
               
            }
        }

        if (screwhits >= 3)
        {
            Instantiate(grenadeParticlePrefab, transform.position, grenadeParticlePrefab.transform.rotation, null);
        }

        else
        {
            screwhits = 0;
        }
      
        yield return true;
    }
}




