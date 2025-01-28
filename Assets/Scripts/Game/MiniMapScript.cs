using UnityEngine;

public class MiniMapScript : MonoBehaviour
{
    public Transform player;
    private void LateUpdate()
    {
        Vector3 newPos = player.position;
        newPos.y = transform.position.y;
        transform.position = newPos;
        transform.rotation = Quaternion.Euler(90, player.eulerAngles.y,0f);
    }
}
