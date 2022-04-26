using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Core.Items
{  
    [CreateAssetMenu(menuName = "Items/Item Database", fileName = "ItemDatabase", order = 0)]
    public class ItemDatabase : ScriptableObject
    {
        [SerializeField] private List<ItemData> itemDatabase;

        private void OnValidate() 
        {
            HashSet<ItemData> set = new HashSet<ItemData>();

            for (int id = 0; id < itemDatabase.Count; id++)
            {
                if (!itemDatabase[id]) continue;

                if (set.Contains(itemDatabase[id]))
                {
                    Debug.LogError($"Item Database: {name} already contains the ItemData '{itemDatabase[id].name}'");
                    return;
                }

                itemDatabase[id].SetItemID(id + 1);
                set.Add(itemDatabase[id]);
            }
        }
    }
}
