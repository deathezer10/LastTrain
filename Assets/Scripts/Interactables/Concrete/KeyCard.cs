using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCard : GrabbableObject {
	[SerializeField]
	private bool success = false;
    public bool IsSuccess() { return success; }

    public PlayerViveController playerController { get; private set; }

    public HandSource playerHand { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        
    }


    private void OnTriggerLeave(Collider other)
    {
       
    }

    public override bool hideControllerOnGrab { get { return true; } }

    public override void OnControllerEnter(PlayerViveController currentController)
    {
        base.OnControllerEnter(currentController);

        playerController = currentController;
        playerHand = playerController.GetCurrentHand();
    }

    public override void OnControllerExit()
    {
        base.OnControllerExit();
    }

    public override void OnControllerStay()
    {

    }

    public override void OnGrab()
    {

    }

    public override void OnGrabReleased()
    {

    }

    public override void OnUse()
    {

    }

    public override void OnUseDown()
    {
    }

    public override void OnUseUp()
    {
    }

}
