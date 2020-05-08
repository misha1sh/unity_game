using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    [RequireComponent(typeof(RectTransform))]
    public class MultiImagePanel : MonoBehaviour {
        private List<Image> images = new List<Image>();
        public Image image;

        private RectTransform panel;
        
        public void OnEnable() {
            panel = GetComponent<RectTransform>();
            image.enabled = false;
        }

        public void SetMaxImagesCount(int count) {
            foreach (var bullet in images) {
                Destroy(bullet.gameObject);
            }
            images.Clear();
        
        
            float bulletOffsetX = (panel.rect.width - image.rectTransform.rect.width) * panel.lossyScale.x / (count - 1);
            if (count == 1)
                bulletOffsetX = 0;
            for (int i = 0; i < count; i++) {
                var go = Instantiate(image, panel.gameObject.transform, true);
                go.enabled = true;
                go.transform.Translate(i * bulletOffsetX, 0, 0);
                images.Add(go);
            }
        }

        public void SetActiveImagesCount(int count) {
            for (int i = 0; i < images.Count; i++) {
                images[i].enabled = i < count;
            }
        }
    }
}