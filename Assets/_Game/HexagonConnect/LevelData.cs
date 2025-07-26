using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    int numberLevel;
    public int rows;
    public int cols;
    public List<BlockData> blocks = new List<BlockData>();
    public LevelData()
    {
        this.numberLevel = numberLevel;
        this.rows = 8;
        this.cols = 6;
        this.blocks = new List<BlockData>();
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                blocks.Add(new BlockData { col = j, row = i, blockType = BlockType.None });
            }
        }
    }
    public BlockData GetBlock(int col, int row)
    {
        return blocks.Find(block => block.col == col && block.row == row);
    }

}

[System.Serializable]
public class BlockData
{
    public int col;
    public int row;
    public BlockType blockType;
}
