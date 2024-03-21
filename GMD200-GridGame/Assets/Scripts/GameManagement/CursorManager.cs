using UnityEngine;
public class CursorManager : MonoBehaviour
{
    [SerializeField] private CursorLockMode cursorState;

    public delegate void SetCursor(CursorLockMode lockMode);
    public static SetCursor setCursorMode;

    //Set default state in this scene
    //private void Awake() => Cursor.lockState = cursorState;

    void SetCursorMode(CursorLockMode lockMode) => Cursor.lockState = lockMode;

    private void OnEnable() => setCursorMode += SetCursorMode;
    private void OnDisable() => setCursorMode += SetCursorMode;
}
