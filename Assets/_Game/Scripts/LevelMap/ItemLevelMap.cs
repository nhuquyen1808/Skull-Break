using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemLevelMap : MonoBehaviour
{
    [SerializeField] private int id = 0;
    [SerializeField] private int characterLevel = 0;
    [SerializeField] private bool isDone = false;
    [SerializeField] private bool hasChest = false;
    [SerializeField] private bool turnOnStar = false;
    [SerializeField] private int star = 0;
    [SerializeField] private RectTransform rtfmItem;
    [SerializeField] private RectTransform rtfmHolder;
    [SerializeField] private Text txtID;
  //  [SerializeField] private Image imgCheck;
    [SerializeField] private Sprite spriDone;
    [SerializeField] private Sprite spriCurrent;
    [SerializeField] private Sprite spriNext;
    [SerializeField] private Image imgHolder;
    [SerializeField] private List<Image> lstImageStar;
    [SerializeField] private Image imgPlay;


    [SerializeField] private GameObject gobjChest;
    [SerializeField] private Transform tfmAvatar;

    [SerializeField] private ParticleSystem particleLevelComplete;

    private UnityAction<int, int> actionClick;

    public int ID => id;
    public bool HasChest => hasChest;

    public void Init(int id, int starAmount, bool isDone = false, bool hasChest = false, int characterLevel = 0, bool turnOnStar = false)
    {
        this.star = starAmount;
        this.id = id;
        this.isDone = isDone;
        this.characterLevel = characterLevel;
        this.hasChest = hasChest;
        this.turnOnStar = turnOnStar;
        SetupPos(id);
        //imgPlay.gameObject.SetActive(id == characterLevel);
        txtID.text = $"{id}";
        //imgCheck.gameObject.SetActive(isDone);
        gobjChest.SetActive(hasChest);
        ResetStar();
        if (id<characterLevel)
        {
            imgHolder.sprite = spriDone;
            //imgCheck.sprite = spriDone;
            int star = PlayerPrefs.GetInt($"LevelStar_{id-1}", 0);
            Debug.Log($"star {star} id {id} ");
            for (int i = 0; i < star; i++)
            {
                lstImageStar[i].gameObject.SetActive(true);
            }
        }
        else if (id == characterLevel)
        {
            imgHolder.sprite = spriCurrent;
           // imgCheck.sprite = spriCurrent;
        }
        else
        {
            imgHolder.sprite = spriNext;
         //   imgCheck.sprite = spriNext;
        }
    }
    private void ResetStar()
    {
        for (int i = 0; i < lstImageStar.Count; i++)
        {
            lstImageStar[i].gameObject.SetActive(false);
        }
    }    
    [ContextMenu("test")]
    public void Test()
    {
        InitPassLevel(3).Forget();
    }
    public async UniTask InitPassLevel(int starAmount)
    {
        this.star = starAmount;
        this.isDone = true;
        SetupPos(id);
        //imgCheck.gameObject.SetActive(isDone);
      //  particleLevelComplete.gameObject.SetActive(true);
    }
    public void SetAction(UnityAction<int, int> actionClick)
    {
        this.actionClick = actionClick;
    }
    public void SetAvatar(Transform tfmAvatar)
    {
        this.tfmAvatar = tfmAvatar;
        tfmAvatar.gameObject.SetActive(true);
        tfmAvatar.SetParent(rtfmHolder.transform);
        tfmAvatar.localPosition = Vector3.zero;
        gobjChest.SetActive(false);
    }
    public async UniTask DoMoveAvatar(Transform tfmHolder, Transform tfmAvatar, float duration = 3f)
    {
        this.tfmAvatar = tfmAvatar;
        tfmAvatar.gameObject.SetActive(true);
        tfmAvatar.SetParent(tfmHolder);
        Vector3 currentPos = tfmAvatar.position; // 
        Vector3 targetPosition = rtfmHolder.transform.position; // 


        await tfmAvatar.DOJump(targetPosition, 0.3f, 1, duration);

        tfmAvatar.SetParent(rtfmHolder.transform);
        tfmAvatar.localPosition = Vector3.zero;
        gobjChest.SetActive(false);
    }
    void SetupPos(int id)
    {
        int colAmount = GameConfig.COL_LEVEL_MAP_AMOUNT;

        if (colAmount <= 1)
        {
            rtfmHolder.anchoredPosition = Vector2.zero;
            return;
        }

        int max = colAmount * 2 - 2;
        int index = id % max;
        if (index == 0)
        {
            index = 2;
        }
        else
         if (index > colAmount)
        {
            int sub = index - colAmount;

            index = colAmount - sub;
        }

        // Init pos with index
        //Debug.Log($"id {id} index {index} ");

        float rectWidth = rtfmItem.rect.width;
        //   Debug.Log($"rectWidth {rectWidth} ");
        float width = rectWidth / (colAmount);
        //  Debug.Log($"width {width} ");

        float posX = width * (index - 1) + width / 2 - rectWidth / 2;
        //  Debug.Log($"posX {posX} ");

        Vector3 newPos = new Vector3(posX, 0);

        rtfmHolder.anchoredPosition = newPos;
    }
    public void OnClickLevel()
    {
        if (id == characterLevel)
            MainMenuController.Instance.OnCLickPlay();
    }
}
