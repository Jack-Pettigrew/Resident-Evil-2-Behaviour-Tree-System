using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DD.AI.BehaviourTree
{
    public static class Blackboard
    {
        private static Dictionary<string, object> blackboardDictionary = new Dictionary<string, object>();

        public static void AddToBlackboard(string keyName, object value)
        {
            blackboardDictionary.Add(keyName.ToLower(), value);
        }

        public static object GetFromBlackboard(string keyName)
        {
            return blackboardDictionary[keyName.ToLower()];
        }

        public static void RemoveFromBlackboard(string keyName)
        {
            if(blackboardDictionary.Remove(keyName.ToLower()))
            {
                // Success
            }
            else
            {
                // Not found
            }
        }
    }

}