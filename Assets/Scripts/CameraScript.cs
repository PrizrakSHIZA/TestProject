using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform playerObj;

    public float rotationSpeed;

    void Update()
    {
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        player.forward = viewDir.normalized;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDir = player.forward * verticalInput + player.right * horizontalInput;

        if (inputDir != Vector3.zero)
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
    }
}
