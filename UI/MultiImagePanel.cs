using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    /// <summary>
    ///     Класс для панели интерфейса с одинаковыми изображенями
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class MultiImagePanel : MonoBehaviour {
        /// <summary>
        ///     Массив изображений
        /// </summary>
        private List<Image> images = new List<Image>();
        /// <summary>
        ///     Изображение
        /// </summary>
        public Image image;

        /// <summary>
        ///     Панель, на которой показывается данный элемента
        /// </summary>
        private RectTransform panel;
        
        /// <summary>
        ///     Инициализирует переменные
        /// </summary>
        public void OnEnable() {
            panel = GetComponent<RectTransform>();
            image.enabled = false;
        }

        /// <summary>
        ///     Устанавливает максимальное число изображений, которое нужно показывать
        /// </summary>
        /// <param name="count">Максимальное число изображений</param>
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

        /// <summary>
        ///     Устанавливает число изображений, которые нужно показывать сейчас
        /// </summary>
        /// <param name="count">Число изображений</param>
        public void SetActiveImagesCount(int count) {
            for (int i = 0; i < images.Count; i++) {
                images[i].enabled = i < count;
            }
        }
    }
}