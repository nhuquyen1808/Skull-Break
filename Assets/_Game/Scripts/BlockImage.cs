using UnityEngine;

[System.Serializable]
public class BlockImage
{
    public BlockType blockType; // Loại block
    public Sprite image;        // Hình ảnh cho loại block
}
public enum BlockType
{
    None,
    Enemy1,
    Enemy2,
    Enemy3,
    Enemy4,
    Enemy5,
    Enemy6,
}
