using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LocomationController : MonoBehaviour
{

    public XRController leftTeleportRay;
    public XRController rightTeleportRay;
    public InputHelpers.Button teleportActivationButton;
    public float activationTreshold = 0.1f;

    private void Start()
    {
        if (leftTeleportRay)
            leftTeleportRay.gameObject.SetActive(CheckIfActivated(leftTeleportRay));

        if (rightTeleportRay)
            rightTeleportRay.gameObject.SetActive(CheckIfActivated(rightTeleportRay));
    }

    public bool CheckIfActivated(XRController controller)
    {
        InputHelpers.IsPressed(controller.inputDevice, teleportActivationButton, out bool isActivated, activationTreshold);

        return isActivated;
    }

}
