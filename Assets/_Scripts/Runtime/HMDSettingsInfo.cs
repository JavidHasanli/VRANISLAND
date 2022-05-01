using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HMDSettingsInfo : MonoBehaviour
{
    private void Start()
    {     
        if (!XRSettings.isDeviceActive)
        {
            Debug.Log("Ho HeadSet Plugged.");
        }
        else if (XRSettings.isDeviceActive && (XRSettings.loadedDeviceName == "Mock HMD" 
            || XRSettings.loadedDeviceName == "MockHMD Display"))
        {
            Debug.Log("Using Mock HMD");
        }
        else Debug.Log($"Using HeadSet {XRSettings.loadedDeviceName}");

    }
}
