using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace DD.UI
{
    public class ContextMenuUI : MonoBehaviour
    {
        // UI Components
        [SerializeField] private GameObject uiButtonPrefab;

        /// <summary>
        /// Sets the menu options displayed in the context menu.
        /// </summary>
        /// <param name="contextMenuOptions">An array of options </param>
        public void SetContextMenu(ContextMenuOption[] contextMenuOptions)
        {
            // Destroy any previous context options
            foreach (Transform childTransform in GetComponentsInChildren<Transform>())
            {
                if(childTransform != transform)
                {
                    Destroy(childTransform.gameObject);
                }
            }

            // Create new context options
            foreach (ContextMenuOption menuOption in contextMenuOptions)
            {
                Button menuButton = Instantiate(uiButtonPrefab).GetComponent<Button>();
                
                menuButton.transform.SetParent(transform);

                menuButton.GetComponentInChildren<TextMeshProUGUI>().text = menuOption.menuOptionTitle;
                menuButton.onClick = menuOption.onClickCallback;
            }
        }

        /// <summary>
        /// Sets the position of the context menu.
        /// </summary>
        /// <param name="position"></param>
        public void SetPosition(Vector2 position) => transform.position = position;
    }

    // MOVE TO OWN CLASS
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