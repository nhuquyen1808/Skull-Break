using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardSkinController : Singleton<CardSkinController>
{
    [SerializeField] private List<Sprite> cardSkins; // List of card skins
    [SerializeField] private List<Sprite> cardSkinsFinal; // List of card skins


    public Sprite GetSprite(int value)
    {
        var sprite = cardSkinsFinal[value];
        return sprite;
    }    
    public void InitListSpriteFinal(int count)
    {
        cardSkinsFinal.Clear();

        if (cardSkins == null || cardSkins.Count == 0 || count <= 0)
            return;

        int maxCount = Mathf.Min(count, cardSkins.Count);
        List<Sprite> tempList = new List<Sprite>(cardSkins);

        for (int i = 0; i < maxCount; i++)
        {
            int randomIndex = Random.Range(0, tempList.Count);
            cardSkinsFinal.Add(tempList[randomIndex]);
            tempList.RemoveAt(randomIndex);
        }
    }
}


public static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}
