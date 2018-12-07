using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;

public class TutorialManager : MonoBehaviour
{

    public enum PosterState { None, Poster1, Poster2, Poster3 }

    [SerializeField]
    private bool m_TutorialEnabled = true;
    public bool tutorialEnabled {
        get { return m_TutorialEnabled; }
        set { m_TutorialEnabled = value; }
    }

    [SerializeField]
    private SteamVR_Action_Boolean _padAction;

    [SerializeField]
    private Negi.Outline m_TutorialPoster1;
    [SerializeField]
    private Negi.Outline m_TutorialPoster2;
    [SerializeField]
    private Negi.Outline m_TutorialPoster3;

    [SerializeField]
    private List<Material> m_TutorialPosterMaterials = new List<Material>();

    [SerializeField]
    Color m_EmissionHighlightColor;

    private int m_CurrentPosterMaterialIndex = 0;

    private Vector3 m_playerCheckpointPos = new Vector3(1f, 0.83f, -5f);
    private Vector3 m_playerCheckpointRot = new Vector3(0, -90f, 0);

    private GameObject m_RightDiscObject;
    private List<GameObject> m_TriggerObjects = new List<GameObject>(), m_GripObjects = new List<GameObject>();

    private Material m_InitialControllerMat;
    public Material m_HighlightControllerMat;


    void Start()
    {
        tutorialEnabled = Checkpoint.CHECKPOINT_ACTIVATED ? false : true;

        if (tutorialEnabled)
        {
            StartCoroutine(TutorialDelay());
        }
        else  // Skipping tutorial to checkpoint position * or just move this check to a seperate check script on the player object
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            player.transform.position = m_playerCheckpointPos;
            player.transform.eulerAngles = m_playerCheckpointRot;
        }
    }

    private IEnumerator TutorialDelay()
    {
        yield return new WaitForSeconds(1f);

        StartTutorial();
    }

    void OnEnable()
    {
        //Subscribe to the event that is called by SteamVR_RenderModel, when the controller mesh + texture has been loaded completely.
        SteamVR_Events.RenderModelLoaded.Listen(OnControllerLoaded);
    }

    private void OnDisable()
    {
        SteamVR_Events.RenderModelLoaded.Remove(OnControllerLoaded);
    }

    /// <summary>
    /// Call this method when the RenderModelLoaded event is triggered.
    /// </summary>
    /// <param name="args">SteamVR_RenderModel renderModel, bool success</param>
    void OnControllerLoaded(SteamVR_RenderModel renderModel, bool success)
    {
        if (success)
        {
            Debug.Log("Successfully loaded " + renderModel.transform.parent.name + " model");
            GetControllerParts(renderModel.gameObject);
        }
    }

    private void GetControllerParts(GameObject modelObject)
    {
        m_InitialControllerMat = modelObject.transform.Find("trigger").GetComponent<MeshRenderer>().material;

        // Set up the new material with emission as a highlight
        m_HighlightControllerMat.CopyPropertiesFromMaterial(m_InitialControllerMat);
        m_HighlightControllerMat.SetColor("_EmissionColor", m_EmissionHighlightColor);
        m_HighlightControllerMat.EnableKeyword("_EMISSION");

        // Find and store the objects for each part to be highlighted
        if (modelObject.transform.parent.name == "Controller (right)")
        {
            m_RightDiscObject = modelObject.transform.Find("trackpad").gameObject;

            m_TriggerObjects.Add(modelObject.transform.Find("trigger").gameObject);
            m_GripObjects.Add(modelObject.transform.Find("lgrip").gameObject);
            m_GripObjects.Add(modelObject.transform.Find("rgrip").gameObject);
        }
        else if (modelObject.transform.parent.name == "Controller (left)")
        {
            m_TriggerObjects.Add(modelObject.transform.Find("trigger").gameObject);
            m_GripObjects.Add(modelObject.transform.Find("lgrip").gameObject);
            m_GripObjects.Add(modelObject.transform.Find("rgrip").gameObject);
        }

    }

    void StartTutorial()
    {

        SetPoster(PosterState.Poster1);

        IDisposable padObserver = null;
        
        padObserver = this.UpdateAsObservable()
           .Where(_ => _padAction.GetStateUp(SteamVR_Input_Sources.Any))
           .Subscribe(_ =>
           {
               padObserver.Dispose();
               SetPoster(PosterState.Poster2);
           }
           );

    }

    public void SetPoster(PosterState poster)
    {
        if (m_TutorialPoster1 == null)
            return;

        m_TutorialPoster1.enabled = false;
        m_TutorialPoster2.enabled = false;
        m_TutorialPoster3.enabled = false;

        StopAllCoroutines();

        switch (poster)
        {
            case PosterState.None:
                break;
            case PosterState.Poster1:
                m_TutorialPoster1.enabled = true;
                StartCoroutine(StartPosterAnimation(PosterState.Poster1));
                HighlightControllerParts(1);
                break;
            case PosterState.Poster2:
                m_TutorialPoster2.enabled = true;
                StartCoroutine(StartPosterAnimation(PosterState.Poster1));
                StartCoroutine(StartPosterAnimation(PosterState.Poster2));
                HighlightControllerParts(2);
                break;
            case PosterState.Poster3:
                m_TutorialPoster3.enabled = true;
                StartCoroutine(StartPosterAnimation(PosterState.Poster1));
                StartCoroutine(StartPosterAnimation(PosterState.Poster2));
                StartCoroutine(StartPosterAnimation(PosterState.Poster3));
                HighlightControllerParts(3);
                break;
            default:
                Debug.LogError("Invalid poster assigned");
                break;
        }

    }

    IEnumerator StartPosterAnimation(PosterState target)
    {
        while (target != PosterState.None)
        {
            yield return new WaitForSeconds(0.75f);
            
            var posterMat = m_TutorialPosterMaterials[m_CurrentPosterMaterialIndex];

            switch (target)
            {
                case PosterState.None:
                    break;
                case PosterState.Poster1:
                    if (m_TutorialPoster1 == null)
                        break;

                    m_CurrentPosterMaterialIndex = (int)Mathf.Repeat(++m_CurrentPosterMaterialIndex, m_TutorialPosterMaterials.Count);
                    m_TutorialPoster1.GetComponent<Renderer>().material = posterMat;
                    break;
                case PosterState.Poster2:
                    if (m_TutorialPoster2 == null)
                        break;

                    m_TutorialPoster2.GetComponent<Renderer>().material = posterMat;
                    break;
                case PosterState.Poster3:
                    if (m_TutorialPoster3 == null)
                        break;

                    m_TutorialPoster3.GetComponent<Renderer>().material = posterMat;
                    break;
                default:
                    Debug.LogError("Invalid poster assigned");
                    break;
            }
        }
    }

    private void HighlightControllerParts(int currentPoster)
    {
        if (currentPoster == 1)
        {
            m_RightDiscObject.GetComponent<MeshRenderer>().material = m_HighlightControllerMat;
        }
        else if (currentPoster == 2)
        {
            m_RightDiscObject.GetComponent<MeshRenderer>().material = m_InitialControllerMat;

            foreach (GameObject part in m_TriggerObjects)
            {
                part.GetComponent<MeshRenderer>().material = m_HighlightControllerMat;
            }
        }
        else if (currentPoster == 3)
        {
            m_RightDiscObject.GetComponent<MeshRenderer>().material = m_InitialControllerMat;

            foreach (GameObject part in m_TriggerObjects)
            {
                part.GetComponent<MeshRenderer>().material = m_InitialControllerMat;
            }

            foreach (GameObject part in m_GripObjects)
            {
                part.GetComponent<MeshRenderer>().material = m_HighlightControllerMat;
            }
        }
    }

    public void SetOriginalControllerMaterials()
    {
        m_RightDiscObject.GetComponent<MeshRenderer>().material = m_InitialControllerMat;

        foreach (GameObject part in m_TriggerObjects)
        {
            part.GetComponent<MeshRenderer>().material = m_InitialControllerMat;
        }

        foreach (GameObject part in m_GripObjects)
        {
            part.GetComponent<MeshRenderer>().material = m_InitialControllerMat;
        }
    }

}
