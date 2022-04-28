using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DD.Core.Items
{  
    [CreateAssetMenu(menuName = "Items/Item Database", fileName = "ItemDatabase", order = 0)]
    public class ItemDatabase : ScriptableObject
    {
        [SerializeField] private List<Item> itemDatabase;

        public T GetItem<T>(int itemID) where T : Item
        {
            if(itemID < 0 || itemID >= itemDatabase.Count) return null;

            return (T)itemDatabase[itemID];
        }

        private void OnValidate() 
        {
            HashSet<Item> set = new HashSet<Item>();

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
