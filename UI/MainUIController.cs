using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUIController : MonoBehaviour {

    public Image gunImage;

    public Sprite pistolSprite;
    public Sprite shotgunSprite;
    public Sprite semiautoSprite;
    public Sprite grenadeLauncherSprite;

    public RectTransform bulletsPanel;
    public Image bulletImage;
    public float bulletHeight;
    
    private List<Image> bullets;

    public void SetMaxBulletsCount(int count) {
        float bulletOffsetX = bulletImage.sprite.rect.width / count;
        
        for (int i = 0; i < count; i++) {
            var go = Instantiate(bulletImage, bulletsPanel.gameObject.transform, true);
            var rt = go.GetComponent<RectTransform>();
            rt.position.Set(bulletOffsetX * i, 0, 0);
        }
    }
    
    // Start is called before the first frame update
    void Start() {
        SetMaxBulletsCount(10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
