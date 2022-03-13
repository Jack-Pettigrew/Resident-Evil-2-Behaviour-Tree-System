using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.UI
{
    public class ContextMenuUI : MonoBehaviour
    {
        private ContextMenuOption[] contextMenuOptions = null;
        
        // SetPosition
        // CreateContextMenu

        public void SetContextMenu(ContextMenuOption[] contextMenuOptions)
        {
            this.contextMenuOptions = contextMenuOptions;

            // Create the actual UI elements
            // Assign title + callback on click
            // (may no longer need local ContextMenuOption[])
        }
    }

    public class ContextMenuOption
    {
        public string menuOptionTitle;
        
        public delegate void MenuOptionCallback();
        public MenuOptionCallback callback;

        public ContextMenuOption(string menuOptionTitle, MenuOptionCallback menuOptionCallback)
        {
            this.menuOptionTitle = menuOptionTitle;
            this.callback = menuOptionCallback;
        }
    }
}