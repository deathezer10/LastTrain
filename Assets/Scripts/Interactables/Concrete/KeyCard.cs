using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;


public class KeyCard : GrabbableObject {
	[SerializeField]
	private bool success = false;
    public bool IsSuccess() { return success; }

    public PlayerViveController playerController { get; private set; }

    public SteamVR_Input_Sources playerHand { get; private set; }

    public override bool hideControllerOnGrab { get { return true; } }

    private void Start()
    {
        m_DropSoundHandler.SetImpactNoiseData(new DropSoundHandler.ImpactNoiseData { soundType = DropSoundHandler.DropSoundType.Plastic});
    }

    public override void OnControllerEnter(PlayerViveController currentController)
    {
        base.OnControllerEnter(currentController);

        playerController = currentController;
        playerHand = playerController.GetCurrentHand();
    }
}
