using UnityEngine.UI;
using UnityEngine.Events;

namespace DD.UI
{
    public class ContextMenuOption
    {
        public string menuOptionTitle;
        public Button.ButtonClickedEvent onClickCallback;

        public ContextMenuOption(string menuOptionTitle, UnityAction menuOptionCallback)
        {
            this.menuOptionTitle = menuOptionTitle;
            onClickCallback = new Button.ButtonClickedEvent();
            onClickCallback.AddListener(menuOptionCallback);
        }
    }
}