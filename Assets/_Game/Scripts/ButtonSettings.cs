using UnityEngine;
using UnityEngine.UI;

public class ButtonSettings : MonoBehaviour
{
    [SerializeField] private Image buttonImage;
    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;
    private bool isOn = false;

    private void UpdateButtonImage()
    {
        buttonImage.sprite = isOn ? onSprite : offSprite;
    }

    public void SetState(bool state)
    {
        isOn = state;
        UpdateButtonImage();
    }

    public bool GetState()
    {
        return isOn;
    }
}
