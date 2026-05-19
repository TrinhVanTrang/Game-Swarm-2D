using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class BulletPoolManager : MonoBehaviour
{
    public static BulletPoolManager instance;

    // Quản lý nhiều pool dựa trên Prefab của viên đạn
    private Dictionary<string, Queue<GameObject>> pools = new Dictionary<string, Queue<GameObject>>();

    void Awake()
    {
        if (this != null) instance = this;
    }

    // Khởi tạo Pool cho một loại vũ khí cụ thể
    public void CreatePool(WeaponData data)
    {
        if (!pools.ContainsKey(data.poolName))
        {
            pools[data.poolName] = new Queue<GameObject>();

            for (int i = 0; i < data.poolLimit; i++)
            {
                GameObject obj = Instantiate(data.prefab);
                obj.SetActive(false);
                pools[data.poolName].Enqueue(obj);
            }
        }
    }

    public GameObject GetBullet(WeaponData weaponData)
    {
        if (pools.ContainsKey(weaponData.poolName) && pools[weaponData.poolName].Count > 0)
        {
            //Debug.Log(pools[weaponData.poolName].Count);
            GameObject obj = pools[weaponData.poolName].Dequeue();

            obj.SetActive(true);
            KnifeBullet proj = obj.GetComponent<KnifeBullet>();
            //Debug.Log("position: " + proj.transform.position.x + ", " + proj.transform.position.y);
            return obj;
        }
        // Nếu hết đạn trong pool, có thể Instantiate thêm tại đây
        Debug.Log("Pool " + weaponData.poolName + " empty!");
        return Instantiate(weaponData.prefab);
        // return null;
    }

    public void ReturnToPool(string poolName, GameObject obj)
    {
        obj.SetActive(false);
        pools[poolName].Enqueue(obj);
    }
}