using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Cursor : MonoBehaviour
{
    private Camera mainCamera => Camera.main;
    [SerializeField] private ClampModes clampMode = ClampModes.circle;
    [SerializeField] private Vector2 clampThreshold = new Vector2(2f, 2f);
    [SerializeField] private float clampRadius = 2.5f;

    private PlayerInputHandler inputHandler;
    [SerializeField] private Transform playerCenter;
    [SerializeField] private Transform target;
    private Vector3 mousePosition;
    private Vector3 targetPosition;

    private void Awake()
    {
        inputHandler = GetComponentInParent<PlayerInputHandler>();

        target = transform.Find("CameraTarget");
    }

    private void Update()
    {
        UpdateMousePosition();
        UpdateCursorPosition();
        UpdateCameraTargetPosition();
    }

    private void UpdateMousePosition()
    {
        mousePosition = inputHandler.mousePositionInput;
    }

    private void UpdateCursorPosition()
    {
        transform.position = mousePosition;
    }

    private void UpdateCameraTargetPosition()
    {
        if (clampMode == ClampModes.rectangle)
        {
            ClampTargetInRect();
        }
        else if (clampMode == ClampModes.circle)
        {
            ClampTargetInCircle();
        }
    }

    private void ClampTargetInRect()
    {
        targetPosition = (playerCenter.position + mousePosition) / 2;

        targetPosition.x = Mathf.Clamp(targetPosition.x, playerCenter.position.x - clampThreshold.x, playerCenter.position.x + clampThreshold.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, playerCenter.position.y - clampThreshold.y, playerCenter.position.y + clampThreshold.y);

        target.position = targetPosition;
    }

    private void ClampTargetInCircle()
    {
        targetPosition = (mousePosition - playerCenter.position) / 2;

        targetPosition = Vector3.ClampMagnitude(targetPosition, clampRadius);

        target.position = playerCenter.position + targetPosition;
    }

    void OnDrawGizmos()
    {
        UnityEditor.Handles.DrawDottedLine(playerCenter.position, transform.position, 1f);
        UnityEditor.Handles.DrawDottedLine(target.position, transform.position, 1f);
        UnityEditor.Handles.DrawDottedLine(playerCenter.position, target.position, 1f);

        UnityEditor.Handles.DrawWireDisc(target.position, Vector3.forward, 0.2f);
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, 0.2f);

        if (clampMode == ClampModes.rectangle)
        {
            UnityEditor.Handles.DrawLine(new Vector2(playerCenter.position.x - clampThreshold.x, playerCenter.position.y + clampThreshold.y), new Vector2(playerCenter.position.x + clampThreshold.x, playerCenter.position.y + clampThreshold.y));
            UnityEditor.Handles.DrawLine(new Vector2(playerCenter.position.x - clampThreshold.x, playerCenter.position.y + clampThreshold.y), new Vector2(playerCenter.position.x - clampThreshold.x, playerCenter.position.y - clampThreshold.y));
            UnityEditor.Handles.DrawLine(new Vector2(playerCenter.position.x + clampThreshold.x, playerCenter.position.y + clampThreshold.y), new Vector2(playerCenter.position.x + clampThreshold.x, playerCenter.position.y - clampThreshold.y));
            UnityEditor.Handles.DrawLine(new Vector2(playerCenter.position.x + clampThreshold.x, playerCenter.position.y - clampThreshold.y), new Vector2(playerCenter.position.x - clampThreshold.x, playerCenter.position.y - clampThreshold.y));
        }
        else if (clampMode == ClampModes.circle)
        {
            UnityEditor.Handles.DrawWireDisc(playerCenter.position, Vector3.forward, clampRadius);
        }
    }

    private enum ClampModes
    {
        rectangle,
        circle
    }

#if UNITY_EDITOR
    //[CustomEditor(typeof(Cursor))]
    class CursorEditor : Editor
    {
        private Cursor _cursor;

        private void OnEnable()
        {
            _cursor = (Cursor)target;
            if (_cursor == null) return;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            _cursor.playerCenter = (Transform)EditorGUILayout.ObjectField("Player Center", _cursor.playerCenter, typeof(Transform), true);
            _cursor.target = (Transform)EditorGUILayout.ObjectField("Target", _cursor.target, typeof(Transform), true);

            _cursor.clampMode = (ClampModes)EditorGUILayout.EnumPopup("Clamp Mode", _cursor.clampMode);

            if (_cursor.clampMode == ClampModes.rectangle)
            {
                _cursor.clampThreshold = EditorGUILayout.Vector2Field("Clamp Threshold", _cursor.clampThreshold);
            }
            else if (_cursor.clampMode == ClampModes.circle)
            {
                _cursor.clampRadius = EditorGUILayout.FloatField("Clamp Radius", _cursor.clampRadius);
            }

            EditorUtility.SetDirty(target);
        }
    }
#endif
}
