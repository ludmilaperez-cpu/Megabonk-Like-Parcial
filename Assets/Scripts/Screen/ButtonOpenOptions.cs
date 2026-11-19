using UnityEngine;
using UnityEngine.UI;

namespace ScreenManager 
{
    [RequireComponent(typeof(Button))]
    public class ButtonOpenOptions : MonoBehaviour
    {
        [SerializeField] private SimpleUiScreen _optionsCanva;

        private void Awake()
        {
            var button = GetComponent<Button>();

            button.onClick.AddListener(call:() => ScreenManager.Instance.Push(_optionsCanva));

        }
    }
}