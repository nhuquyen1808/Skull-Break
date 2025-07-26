using Cysharp.Threading.Tasks;
using Mopsicus.InfiniteScroll;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelMap : MonoBehaviour
{

    [SerializeField] private InfiniteScroll Scroll;
    [SerializeField] private int levelAmount;
    [SerializeField] private int currentLevel;
    [SerializeField] private Image imgCharacter;

    ItemLevelMap targetLevel;
    bool turnOnStar;
    private void Start()
    {
       
    }
    private void OnEnable()
    {
        var level = PlayerPrefs.GetInt("level", 1);
        Init(level);
    }
    void InitCharacter()
    {
        int id = 1;
    }
    public void Init()
    {
            InitCharacter();
    }
    private void Init(int currentLevel)
    {
        this.currentLevel = currentLevel;
        Scroll.OnFill += OnFillItem;
        Scroll.OnHeight += OnHeightItem;
        Scroll.Init();

        levelAmount = currentLevel + GameConfig.BONUS_LEVEL_AMOUNT;
        Scroll.InitData(levelAmount);
        Scroll.ForceVerticleToRealIndex(1, levelAmount,10);
        //Scroll.MoveToRealIndex(currentLevel,0, 0.5f);
    }

    void OnFillItem(int index, GameObject item)
    {
        int realLevel = levelAmount - index;
        int starAmount = 0;
        bool isDoneLevel = realLevel < currentLevel;


        var itemComponent = item.GetComponent<ItemLevelMap>();
        itemComponent.Init(realLevel, starAmount, isDoneLevel, false, currentLevel, turnOnStar); 
        if (realLevel == currentLevel)
        {
            itemComponent.SetAvatar(imgCharacter.gameObject.transform);
        }

        //itemComponent.SetAction(OnClickLevel);
    }

    int OnHeightItem(int index)
    {
        return 400;
    }
}
