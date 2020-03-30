using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MainUIController : MonoBehaviour {

    public Image gunImage;

    public Sprite pistolSprite;
    public Sprite shotgunSprite;
    public Sprite semiautoSprite;
    public Sprite grenadeLauncherSprite;

    public MultiImagePanel bulletsPanel;
    public MultiImagePanel magazinesPanel;
    

   
    

    // Start is called before the first frame update
    void Start() {
        bulletsPanel.SetMaxImagesCount(15);
        magazinesPanel.SetMaxImagesCount(8);
        magazinesPanel.SetActiveImagesCount(0);

        EventsManager.handler.OnPlayerBulletsCountChanged += (player, count) => {
            if (player == Client.client.mainPlayer)
                bulletsPanel.SetActiveImagesCount(count);
        };
        EventsManager.handler.OnPlayerMagazinesCountChanged += (player, count) => {
            if (player == Client.client.mainPlayer)
               magazinesPanel.SetActiveImagesCount(count);
        };
    }

    // Update is called once per frame
    void Update()
    {
   /*     if (Random.value < 0.01f) {
            bulletsPanel.SetActiveImagesCount(Random.Range(0, 16));
            magazinesPanel.SetActiveImagesCount(Random.Range(0, 5));
        }*/
    }
}
