using System;
using UnityEngine;
using System.Collections;

public class Cursor : MonoBehaviour
{
    private Transform cursor;
    private Camera mainCamera;
    private PlayerInputHandler inputHandler;

    [SerializeField] private Transform playerCenter;
    [SerializeField] private Transform target;
    [SerializeField] private Vector2 threshold;

    private Vector3 mousePosition;
    private Vector3 targetPosition;

    private void Awake()
    {
        cursor = GetComponent<Transform>();
        inputHandler = GetComponentInParent<PlayerInputHandler>();

        target = transform.Find("CameraTarget");
        mainCamera = Camera.main;
    }

    private void Update()
    {
        UpdateCursorPosition();
        UpdateCameraTargetPosition();
    }

    private void UpdateCursorPosition()
    {
        mousePosition = Input.mousePosition;
        mousePosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 10));

        cursor.position = Vector2.Lerp(cursor.position, mousePosition, Time.time);
    }

    private void UpdateCameraTargetPosition()
    {
        targetPosition = (playerCenter.position + cursor.position) / 2;

        //Debug.Log($"{targetPosition}, {firePoint.position}");
        //Debug.Log(firePoint.position - cursor.position);

        targetPosition.x = Mathf.Clamp(targetPosition.x, playerCenter.position.x - threshold.x, playerCenter.position.x + threshold.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, playerCenter.position.y - threshold.y, playerCenter.position.y + threshold.y);

        target.position = Vector2.Lerp(target.position, targetPosition, Time.time);
    }

    void OnDrawGizmos()
    {
        UnityEditor.Handles.DrawDottedLine(playerCenter.position, cursor.position, 1f);
        UnityEditor.Handles.DrawDottedLine(target.position, cursor.position, 1f);
        UnityEditor.Handles.DrawDottedLine(playerCenter.position, target.position, 1f);

        UnityEditor.Handles.DrawWireDisc(targetPosition, Vector3.forward, 0.4f);
        UnityEditor.Handles.DrawWireDisc(cursor.position, Vector3.forward, 0.4f);

        UnityEditor.Handles.DrawLine(new Vector2(playerCenter.position.x - threshold.x, playerCenter.position.y + threshold.y), new Vector2(playerCenter.position.x + threshold.x, playerCenter.position.y + threshold.y));
        UnityEditor.Handles.DrawLine(new Vector2(playerCenter.position.x - threshold.x, playerCenter.position.y + threshold.y), new Vector2(playerCenter.position.x - threshold.x, playerCenter.position.y - threshold.y));
        UnityEditor.Handles.DrawLine(new Vector2(playerCenter.position.x + threshold.x, playerCenter.position.y + threshold.y), new Vector2(playerCenter.position.x + threshold.x, playerCenter.position.y - threshold.y));
        UnityEditor.Handles.DrawLine(new Vector2(playerCenter.position.x + threshold.x, playerCenter.position.y - threshold.y), new Vector2(playerCenter.position.x - threshold.x, playerCenter.position.y - threshold.y));
    }
}
