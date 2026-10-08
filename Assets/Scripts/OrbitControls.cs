using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class OrbitCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float distance = 10f;
    [SerializeField] private float minDistance = 4f;
    [SerializeField] private float maxDistance = 30f;

    [SerializeField] private float orbitSpeed = 0.2f;
    [SerializeField] private float zoomSpeed = 0.01f;

    [SerializeField] private float yaw = 45f;
    [SerializeField] private float pitch = 25f;

    void LateUpdate()
    {
        if (target == null)
            return;

        var mouse = Mouse.current;

        if (mouse != null)
        {
            // Hold right-click and drag to orbit.
            if (mouse.leftButton.isPressed)
            {
                Vector2 movement = mouse.delta.ReadValue();

                yaw += movement.x * orbitSpeed;
                pitch -= movement.y * orbitSpeed;
            }

            // Scroll up to zoom in, down to zoom out.
            distance -= mouse.scroll.ReadValue().y * zoomSpeed;
        }



        // Prevent flipping over the poles.
        pitch = Mathf.Clamp(pitch, -80f, 80f);
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        Vector3 position = target.position
            + rotation * new Vector3(0f, 0f, -distance);

        transform.SetPositionAndRotation(position, rotation);
    }
}