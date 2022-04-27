using System;
using UnityEngine;
using System.Collections;

public class Cursor : MonoBehaviour
{
    private Camera mainCamera;
    private PlayerInputHandler inputHandler;

    [SerializeField] private Transform playerCenter;
    [SerializeField] private Transform target;
    [SerializeField] private Vector2 threshold;

    private Vector3 mousePosition;
    private Vector3 targetPosition;

    private void Awake()
    {
        inputHandler = GetComponentInParent<PlayerInputHandler>();

        target = transform.Find("CameraTarget");
        
        mainCamera = Camera.main;
    }

    private void Update()
    {
        mousePosition = inputHandler.mousePositionInput;

        UpdateCursorPosition();
        UpdateCameraTargetPosition();
    }

    private void UpdateCursorPosition()
    {
        transform.position = mousePosition;
    }

    private void UpdateCameraTargetPosition()
    {
        targetPosition = (playerCenter.position + mousePosition) / 2;

        targetPosition.x = Mathf.Clamp(targetPosition.x, playerCenter.position.x - threshold.x, playerCenter.position.x + threshold.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, playerCenter.position.y - threshold.y, playerCenter.position.y + threshold.y);

        target.position = targetPosition;
    }

    void OnDrawGizmos()
    {
        UnityEditor.Handles.DrawDottedLine(playerCenter.position, transform.position, 1f);
        UnityEditor.Handles.DrawDottedLine(target.position, transform.position, 1f);
        UnityEditor.Handles.DrawDottedLine(playerCenter.position, target.position, 1f);

        UnityEditor.Handles.DrawWireDisc(targetPosition, Vector3.forward, 0.4f);
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, 0.4f);
 
        UnityEditor.Handles.DrawLine(new Vector2(playerCenter.position.x - threshold.x, playerCenter.position.y + threshold.y), new Vector2(playerCenter.position.x + threshold.x, playerCenter.position.y + threshold.y));
        UnityEditor.Handles.DrawLine(new Vector2(playerCenter.position.x - threshold.x, playerCenter.position.y + threshold.y), new Vector2(playerCenter.position.x - threshold.x, playerCenter.position.y - threshold.y));
        UnityEditor.Handles.DrawLine(new Vector2(playerCenter.position.x + threshold.x, playerCenter.position.y + threshold.y), new Vector2(playerCenter.position.x + threshold.x, playerCenter.position.y - threshold.y));
        UnityEditor.Handles.DrawLine(new Vector2(playerCenter.position.x + threshold.x, playerCenter.position.y - threshold.y), new Vector2(playerCenter.position.x - threshold.x, playerCenter.position.y - threshold.y));
    }
}
