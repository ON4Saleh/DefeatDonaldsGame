using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] Transform playerCamera;
    private float xRotation = 0f;
    private float xSenetivity = 40f;
    private float ySenetivity = 40f;

    public void cameraLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        float smoothMouseX = Mathf.Lerp(0, mouseX, 0.1f);
        float smoothMouseY = Mathf.Lerp(0, mouseY, 0.1f);

        xRotation -= (smoothMouseY * Time.deltaTime) * ySenetivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        transform.Rotate(Vector3.up * (smoothMouseX * Time.deltaTime) * xSenetivity);
    }
}