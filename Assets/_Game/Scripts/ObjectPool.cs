using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject blockPrefab; // Prefab của Block
    [SerializeField] private int poolSize = 162; // Kích thước pool
    private Queue<Block> pool = new Queue<Block>();


    void Start()
    {
        // Tạo pool khi bắt đầu
        for (int i = 0; i < poolSize; i++)
        {
            GameObject blockObject = Instantiate(blockPrefab);
            blockObject.SetActive(false); // Ẩn đối tượng
            pool.Enqueue(blockObject.GetComponent<Block>());
        }
    }

    public Block GetBlock()
    {
        if (pool.Count == 0)
        {
            GameObject blockObject = Instantiate(blockPrefab);
            blockObject.SetActive(false); // Ẩn đối tượng
            pool.Enqueue(blockObject.GetComponent<Block>());
        }
        if (pool.Count > 0)
        {
            Block block = pool.Dequeue();
            block.gameObject.SetActive(true);
            return block;
        }

        return null; // Trả về null nếu pool đã hết
    }

    public void ReturnBlock(Block block)
    {
     //   block.ResetBlock();
        block.gameObject.SetActive(false);
        pool.Enqueue(block);
    }
}
