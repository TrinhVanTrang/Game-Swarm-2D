using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public void UpdateCameraPosition(Transform player)
    {
        if (player == null) return;
        transform.position = new Vector3(
            player.position.x,
            player.position.y,
            -10
        );
    }

    void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
