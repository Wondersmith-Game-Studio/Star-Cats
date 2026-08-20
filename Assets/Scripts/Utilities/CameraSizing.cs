using UnityEngine;

[RequireComponent(typeof(Camera))]
public class OptimizedCameraSizer : MonoBehaviour
{
    public float sceneWidth = 10;
    private Camera _camera;
    private int _lastWidth;
    private int _lastHeight;

    void Awake()
    {
        _camera = GetComponent<Camera>();
        _lastWidth = Screen.width;
        _lastHeight = Screen.height;
        ResizeCamera();
    }

    void LateUpdate()
    {
        // Only recalculate if resolution changed
        if (Screen.width != _lastWidth || Screen.height != _lastHeight)
        {
            _lastWidth = Screen.width;
            _lastHeight = Screen.height;
            ResizeCamera();
        }
    }

    void ResizeCamera()
    {
        // Use camera.pixelWidth instead of Screen.width
        if (_camera.pixelWidth == 0) return; // Prevent division by zero

        float unitsPerPixel = sceneWidth / _camera.pixelWidth;
        float desiredHalfHeight = 0.5f * unitsPerPixel * _camera.pixelHeight;
        _camera.orthographicSize = desiredHalfHeight;
    }   
}   