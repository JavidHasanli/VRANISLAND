using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{

    public bool showController = false;
    public InputDeviceCharacteristics controllerCharacteristics;
    public GameObject handModelPrefab;

    [SerializeField] private List<GameObject> controllerPrefabs = new List<GameObject>();
    
    private InputDevice _targetDevice;
    private GameObject _spawnedController;
    private GameObject _spawnedHandModel;
    private Animator _handAnimator;
    
    void Start()
    {
        TryInitialize();
    }

    private void TryInitialize()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        foreach (InputDevice device in devices)
        {
            Debug.Log(device.name + " " + device.characteristics);
        }

        if (devices.Count > 0 && showController)
        {
            _targetDevice = devices[0];
            GameObject prefab = controllerPrefabs.Find(controller => controller.name == _targetDevice.name);
            if (prefab)
                _spawnedController = Instantiate(prefab, transform);
            else
            {
                Debug.LogError("Did not find corresponding controller model. Using default controller model.");
                _spawnedController = Instantiate(controllerPrefabs[0], transform);
            }
        }
        else if (!showController)
        {
            _spawnedHandModel = Instantiate(handModelPrefab, transform);
            _handAnimator = _spawnedHandModel.GetComponent<Animator>();
        }
    }

    private void UpdateHandAnimation()
    {
        if(_targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))       
            _handAnimator.SetFloat("Trigger", triggerValue);       
        else
            _handAnimator.SetFloat("Trigger", 0);

        if (_targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
            _handAnimator.SetFloat("Grip", gripValue);
        else
            _handAnimator.SetFloat("Grip", 0);
    }
    
    void Update()
    {

        //GetControllerButtons();

        //if (!_targetDevice.isValid)
        //{
        //    Debug.Log("Trying to initialize device.");
        //    TryInitialize();
        //}
        //else
        //    UpdateHandAnimation();
    }

    private void GetControllerButtons()
    {
        // Get primary button
        // Check if primary button pressed or not
        if (_targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue) && primaryButtonValue)
            Debug.Log("Pressing Primary Button");
        
        /*******************************************************/
        
        // Get Trigger Button
        // Get The Value Of Trigger Button
        if (_targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue) && triggerValue > 0.1f)
            Debug.Log($"Trigger pressed: {triggerValue}");
        
        /*******************************************************/
        
        // Get TouchPad
        // Get TouchPad Value
        if (_targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisValue) && primary2DAxisValue != Vector2.zero)
            Debug.Log($"Primary Touchpad: {primary2DAxisValue}");
    }

}
