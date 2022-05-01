using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DD.UI
{
    public class ContextMenuUI : MonoBehaviour
    {
        // UI Components
        [SerializeField] private GameObject uiButtonPrefab;

        public void ShowContextMenu() => gameObject.SetActive(true);

        public void HideContextMenu() => gameObject.SetActive(false);

        /// <summary>
        /// Sets the menu options displayed in the context menu.
        /// </summary>
        /// <param name="contextMenuOptions">An array of options </param>
        public void SetContextMenu(List<ContextMenuOption> contextMenuOptions)
        {
            // Destroy any previous context options
            foreach (Transform childTransform in GetComponentsInChildren<Transform>())
            {
                if (childTransform != transform)
                {
                    Destroy(childTransform.gameObject);
                }
            }

            // Create new context options
            foreach (ContextMenuOption menuOption in contextMenuOptions)
            {
                // Create button
                Button menuButton = Instantiate(uiButtonPrefab, transform).GetComponent<Button>();

                menuButton.GetComponentInChildren<TextMeshProUGUI>().text = menuOption.menuOptionTitle;

                // Assign required and custom click callbacks
                menuOption.onClickCallback.AddListener(HideContextMenu);
                menuButton.onClick = menuOption.onClickCallback;
            }
        }

        /// <summary>
        /// Sets the position of the context menu.
        /// </summary>
        /// <param name="position"></param>
        public void SetPosition(Vector2 position) => transform.position = position;
    }
}