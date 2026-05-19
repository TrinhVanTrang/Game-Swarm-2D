using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public Transform player;
    public List<GameObject> terrainChunks; // Kéo đủ 9 Chunks vào danh sách này
    public float chunkSize = 32f;          // Kích thước cạnh của 1 Chunk

    [Header("Giới Hạn Bản Đồ (World Bounds)")]
    public bool hasBounds = true;
    public Vector2 worldMinBounds;         // Ví dụ: -96, -96 (Rìa tối đa bên trái/dưới)
    public Vector2 worldMaxBounds;         // Ví dụ:  96,  96 (Rìa tối đa bên phải/trên)
    [Tooltip("Khoảng đệm an toàn xung quanh Camera để tránh hiện tượng gạch bị biến mất đột ngột ở rìa màn hình.")]
    public float visibilityBuffer = 5f;

    private Camera mainCamera;
    private float fovThreshold;            // Khoảng cách kích hoạt dịch chuyển

    void Awake()
    {
        // Cache lại Camera chính để tối ưu hiệu suất (gọi Camera.main liên tục trong Update sẽ bị chậm)
        mainCamera = Camera.main;
    }

    void Start()
    {
        // Nếu người chơi cách tâm của Chunk vượt quá 1.5 lần kích thước Chunk, mảnh đó sẽ dịch chuyển
        fovThreshold = chunkSize * 1.5f;
    }

    void Update()
    {
        if (player == null || mainCamera == null) return;

        // 1. TÍNH TOÁN VÙNG NHÌN CỦA CAMERA (Camera Bounds)
        float camHeight = mainCamera.orthographicSize * 2f;
        float camWidth = camHeight * mainCamera.aspect;
        Vector3 camPos = mainCamera.transform.position;

        // Tạo một hộp giới hạn bao quanh Camera + thêm khoảng đệm an toàn
        Bounds cameraBounds = new Bounds(
            new Vector3(camPos.x, camPos.y, 0),
            new Vector3(camWidth + visibilityBuffer * 2f, camHeight + visibilityBuffer * 2f, 10f)
        );

        // 2. VỪA DỊCH CHUYỂN VỪA XỬ LÝ ẨN / HIỆN CHUNK
        foreach (GameObject chunk in terrainChunks)
        {
            Vector3 chunkPos = chunk.transform.position;

            // --- Logic Dịch Chuyển Vòng Lặp ---
            float diffX = player.position.x - chunkPos.x;
            if (Mathf.Abs(diffX) > fovThreshold)
            {
                float directionX = Mathf.Sign(diffX);
                float newX = chunkPos.x + (directionX * chunkSize * 3f);
                if (!hasBounds || (newX >= worldMinBounds.x && newX <= worldMaxBounds.x))
                {
                    chunkPos.x = newX;
                }
            }

            float diffY = player.position.y - chunkPos.y;
            if (Mathf.Abs(diffY) > fovThreshold)
            {
                float directionY = Mathf.Sign(diffY);
                float newY = chunkPos.y + (directionY * chunkSize * 3f);
                if (!hasBounds || (newY >= worldMinBounds.y && newY <= worldMaxBounds.y))
                {
                    chunkPos.y = newY;
                }
            }

            // Cập nhật lại vị trí cho Chunk nếu nó vừa nhảy
            chunk.transform.position = chunkPos;

            // --- Logic Ẩn / Hiện (Culling) ---
            // Tạo một hộp giới hạn cho mảnh đất hiện tại
            Bounds chunkBounds = new Bounds(chunkPos, new Vector3(chunkSize, chunkSize, 10f));

            // Kiểm tra xem hộp của Camera và hộp của Chunk có chạm nhau không
            if (cameraBounds.Intersects(chunkBounds))
            {
                // Nếu chạm nhau và chunk đang ẩn -> Hiện nó lên
                if (!chunk.activeSelf)
                {
                    chunk.SetActive(true);
                }
            }
            else
            {
                // Nếu không chạm nhau và chunk đang hiện -> Ẩn nó đi
                // if (chunk.activeSelf) 
                // {
                //     chunk.SetActive(false);
                // }
                chunk.SetActive(false);
            }
        }
    }
}