using UnityEngine;

public class DotLineAnimator : MonoBehaviour
{
    [SerializeField] private Material animatedMaterial;
    [SerializeField] private float scrollSpeed = 1f;

    private float offsetX;

    private void Update()
    {
        offsetX -= Time.deltaTime * scrollSpeed;
        animatedMaterial.mainTextureOffset = new Vector2(offsetX, 0f);
    }
}
