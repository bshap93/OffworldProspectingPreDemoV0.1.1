using Domains.Input.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Domains.UI_Global.Scripts
{
    public class MenuButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private string sceneName = "";

        [SerializeField] private Color highlightColor = Color.green;

        [SerializeField] private float lerpSpeed = 5f;

        private bool enter;

        private Image image;

        private Color normalColor;

        private void Awake()
        {
            image = GetComponent<Image>();

            if (image == null)
            {
                enabled = false;
                return;
            }

            normalColor = image.color;
        }

        private void Update()
        {
            image.color = Color.Lerp(image.color, enter ? highlightColor : normalColor, lerpSpeed * Time.deltaTime);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            MainMenuManager.Instance.GoToScene(sceneName);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            enter = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            enter = false;
        }
    }
}