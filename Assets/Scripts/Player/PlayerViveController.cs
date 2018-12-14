using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

[RequireComponent(typeof(Collider))]
public class PlayerViveController : MonoBehaviour
{
    [SerializeField]
    private SteamVR_Input_Sources _handType;

    [SerializeField]
    private SteamVR_Action_Boolean _grabAction;

    [SerializeField]
    private SteamVR_Action_Boolean _useAction;

    private Transform _ownPlayer;

    public SteamVR_Input_Sources CurrentHand { get { return _handType; } }

    public Vector3 controllerVelocity { get { return transform.root.TransformDirection(GetComponent<SteamVR_Behaviour_Pose>().GetVelocity()); } }
    public Vector3 controllerAngularVelocity { get { return transform.root.TransformDirection(GetComponent<SteamVR_Behaviour_Pose>().GetAngularVelocity()); } }

    private static GameObject m_CurrentLeftObject = null;
    private static GameObject m_CurrentRightObject = null;

    private void Awake()
    {
        m_CurrentLeftObject = null;
        m_CurrentRightObject = null;
        _ownPlayer = transform.parent;
    }

    virtual protected void Update()
    {
        var currentObject = GetCurrentHandObject();

        if (currentObject == null)
            return;

        if (PlayerOriginHandler.IsOutsideOrigin)
        {
            DetachCurrentObject(false);
            return;
        }

        var iObject = currentObject.GetComponent<IInteractable>();

        if (iObject == null)
            return;

        iObject.OnControllerStay();

        var grabbableObject = currentObject.GetComponent<IGrabbable>();

        if (grabbableObject == null)
            return;

        // On Grab (Trigger Down)
        if (_grabAction.GetStateDown(CurrentHand))
        {
            // If other hand is holding this object, unassign it
            if (GetCurrentHandObject(true) != null && GetCurrentHandObject(true) == currentObject.gameObject)
            {
                GetOtherController().DetachCurrentObject(false);
                Debug.Log("gg");
            }

            if (grabbableObject.hideControllerOnGrab)
                ToggleControllerModel(false);

            grabbableObject.OnGrab();

            AssignObjectToHand(CurrentHand, currentObject.gameObject);

            if (currentObject.GetComponent<IStationaryGrabbable>() == null)
            {
                currentObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

                Rigidbody rb = GetComponent<Rigidbody>();
                FixedJoint joint = currentObject.AddComponent<FixedJoint>();
                joint.connectedBody = rb;
                joint.breakForce = 7500;
                joint.breakTorque = Mathf.Infinity;
                joint.enablePreprocessing = false;
            }
        }

        // On Item First Use
        if (_useAction.GetStateDown(CurrentHand))
        {
            iObject.OnUse();
        }

        // On Item Holding Down Use 
        if (_useAction.GetState(CurrentHand))
        {
            iObject.OnUseDown();
        }

        // On Item Use Released
        if (_useAction.GetStateUp(CurrentHand))
        {
            iObject.OnUseUp();
        }

        // On Grab Use
        if (_grabAction.GetState(CurrentHand))
        {
            grabbableObject.OnGrabStay();
        }
        // On Grab Released (Trigger Up)
        if (_grabAction.GetStateUp(CurrentHand))
        {
            DetachCurrentObject(true);
        }
    }

    virtual protected void OnTriggerEnter(Collider other)
    {
        var iObject = other.GetComponent<IInteractable>();

        if (GetCurrentHandObject() == null && iObject != null && GetCurrentHandObject(true) != other.gameObject)
        {
            AssignObjectToHand(GetCurrentHand(), other.gameObject);
            iObject.OnControllerEnter(this);
            Debug.Log("Enter");
        }
    }

    virtual protected void OnTriggerStay(Collider other)
    {
        if (PlayerOriginHandler.IsOutsideOrigin)
            return;

        var iObject = other.GetComponent<IInteractable>();

        if (GetCurrentHandObject() == null && iObject != null && GetCurrentHandObject() != other.gameObject && GetCurrentHandObject(true) != other.gameObject)
        {
            AssignObjectToHand(GetCurrentHand(), other.gameObject);
            Debug.Log("Stay");
        }
    }

    virtual protected void OnTriggerExit(Collider other)
    {
        var iObject = other.GetComponent<IInteractable>();

        if (iObject != null)
        {
            if (other.gameObject == GetCurrentHandObject())
            {
                iObject.OnControllerExit();
                DetachCurrentObject(false);
                Debug.Log("Exit");
            }
        }
    }

    public void ToggleControllerModel(bool toggle)
    {
        transform.Find("Model").gameObject.SetActive(toggle);
    }

    private void AssignObjectToHand(SteamVR_Input_Sources hand, GameObject go)
    {
        if (hand == SteamVR_Input_Sources.LeftHand)
            m_CurrentLeftObject = go;
        else
            m_CurrentRightObject = go;
    }

    private SteamVR_Input_Sources GetOtherHand()
    {
        return (CurrentHand == SteamVR_Input_Sources.LeftHand) ? SteamVR_Input_Sources.RightHand : SteamVR_Input_Sources.LeftHand;
    }

    public SteamVR_Input_Sources GetCurrentHand()
    {
        return CurrentHand;
    }

    public PlayerViveController GetOtherController()
    {
        var controllers = FindObjectsOfType<PlayerViveController>();

        foreach (PlayerViveController controller in controllers)
        {
            if (controller.CurrentHand == GetOtherHand())
                return controller;
        }

        return null;
    }

    public GameObject GetCurrentHandObject(bool otherHand = false)
    {
        if (CurrentHand == SteamVR_Input_Sources.LeftHand)
            return (otherHand) ? m_CurrentRightObject : m_CurrentLeftObject;
        else
            return (otherHand) ? m_CurrentLeftObject : m_CurrentRightObject;
    }

    public static PlayerViveController GetControllerThatHolds(GameObject obj)
    {
        var controllers = FindObjectsOfType<PlayerViveController>();

        foreach (PlayerViveController controller in controllers)
        {
            if (controller.GetCurrentHandObject() == obj)
                return controller;
        }

        return null;
    }

    /// <summary>
    /// Detaches the object that this controller is currently holding
    /// </summary>
    /// <param name="transferVelocity">Transfer the current velocity of the controller to the object?</param>
    public void DetachCurrentObject(bool transferVelocity)
    {
        var currentObject = GetCurrentHandObject();

        if (currentObject != null)
        {
            var grabbableObject = currentObject.GetComponent<IGrabbable>();

            if (grabbableObject != null)
            {
                if (grabbableObject.hideControllerOnGrab)
                    ToggleControllerModel(true);

                grabbableObject.OnGrabReleased();

                AssignObjectToHand(CurrentHand, null);

                if (currentObject.GetComponent<FixedJoint>() != null)
                    Destroy(currentObject.GetComponent<FixedJoint>());

                if (currentObject.GetComponent<IStationaryGrabbable>() == null && transferVelocity)
                {
                    Rigidbody rb = currentObject.GetComponent<Rigidbody>();
                    rb.velocity = controllerVelocity;
                    rb.angularVelocity = controllerAngularVelocity;
                }

                Debug.LogFormat("Detached l {0}", currentObject.name);
            }
        }
    }

    public void Teleport(Vector3 targetPos)
    {
        var diff = _ownPlayer.SetPositionGetDiff(targetPos);

        var obj = GetCurrentHandObject();

        if (obj && obj.GetComponent<IStationaryGrabbable>() == null)
            obj.transform.AddPosition(diff);

        if(obj == null)
        {
            var newObj = GetCurrentHandObject(true);

            if (newObj && newObj.GetComponent<IStationaryGrabbable>() == null)
                newObj.transform.AddPosition(diff);
        }

       


    }

    virtual public void Vibration(
        float secondsFromNow,
        float durationSeconds,
        float frequency,
        float amplitude,
        SteamVR_Input_Sources handSource)
    {
        SteamVR_Input.actionsVibration[0].Execute(secondsFromNow, durationSeconds, frequency, amplitude, handSource);
    }

    public void VibrationCurrentHand(
        float secondsFromNow,
        float durationSeconds,
        float frequency,
        float amplitude)
    {
        Vibration(secondsFromNow, durationSeconds, frequency, amplitude, CurrentHand);
    }

}
