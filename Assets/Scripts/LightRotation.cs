using UnityEngine;

public class LightRotation : MonoBehaviour
{
    public float rotationSpeed = 20f; // Degrees per second

    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }
}