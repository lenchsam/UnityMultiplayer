using UnityEngine;

public class CameraSettings : MonoBehaviour
{
    [SerializeField] Camera _camera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 fixedRotation = new Vector3(90f, 0f, 0f);
        _camera.transform.eulerAngles = fixedRotation;
    }
}
