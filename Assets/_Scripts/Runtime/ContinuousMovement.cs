using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ContinuousMovement : MonoBehaviour
{

    public XRNode inputSource;

    [SerializeField] private float moveSpeed = 1;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float additionalHeight = 0.2f;
    [SerializeField] private LayerMask groundMask;

    private float fallingSpeed = 0;
    private XROrigin _xrOrigin;
    private Vector2 _inputAxis;
    private CharacterController _characterController;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _xrOrigin = GetComponent<XROrigin>();
    }

    private void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);

        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out _inputAxis);
    }

    private void FixedUpdate()
    {
        CharacterFollowHeadset();

        Quaternion headYaw = Quaternion.Euler(0, _xrOrigin.Camera.transform.eulerAngles.y, 0);
        Vector3 direction = headYaw * new Vector3(_inputAxis.x, 0, _inputAxis.y);

        _characterController.Move(direction * Time.fixedDeltaTime * moveSpeed);

        //gravity

        if (_characterController.isGrounded)
            fallingSpeed = 0;
        else
            fallingSpeed += gravity * Time.fixedDeltaTime;
        _characterController.Move(Vector3.up * fallingSpeed);
    }

    private void CharacterFollowHeadset()
    {
        _characterController.height = _xrOrigin.CameraInOriginSpaceHeight + additionalHeight;
        Vector3 capsuleCenter = transform.InverseTransformPoint(_xrOrigin.Camera.transform.position);
        _characterController.center = new Vector3(capsuleCenter.x, _characterController.height / 2 + _characterController.skinWidth, capsuleCenter.z);
    }

    private bool CheckIfGrounded()
    {
        Vector3 rayStart = transform.TransformPoint(_characterController.center);
        float rayLength = _characterController.center.y + 0.01f;
        bool hasHit = Physics.SphereCast(rayStart, _characterController.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundMask);
        return hasHit;
    }
}
