using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Core.Items
{  
    [CreateAssetMenu(menuName = "Items/Item Database", fileName = "ItemDatabase", order = 0)]
    public class ItemDatabase : ScriptableObject
    {
        [SerializeField] private List<ItemData> itemDatabase;

        public T GetItem<T>(int itemID) where T : ItemData
        {
            if(itemID < 0 || itemID >= itemDatabase.Count) return null;

            return (T)itemDatabase[itemID];
        }

        private void OnValidate() 
        {
            HashSet<ItemData> set = new HashSet<ItemData>();

            for (int id = 0; id < itemDatabase.Count; id++)
            {
                if (!itemDatabase[id]) continue;

                if (set.Contains(itemDatabase[id]))
                {
                    Debug.LogWarning($"Item Database: {name} already contains the ItemData '{itemDatabase[id].name}'");
                    return;
                }

                itemDatabase[id].SetItemID(id + 1);
                set.Add(itemDatabase[id]);
            }
        }
    }
}
