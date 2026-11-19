using UnityEngine;
using UnityEngine.UI;

namespace ScreenManager
{
    [RequireComponent(typeof(Button))]
    public class ButtonBack : MonoBehaviour
    {
        private void Awake()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(Execute);
        }
        void Execute()
        {
            ScreenManager.Instance.Pop();
        }
    }
}