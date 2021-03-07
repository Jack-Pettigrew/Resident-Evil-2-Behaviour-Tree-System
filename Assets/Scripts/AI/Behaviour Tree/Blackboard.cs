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

        /// <summary>
        /// Returns variable from Blackboard dictionary. Allocates memory.
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns>'object' typed variable. NULL on not found.</returns>
        public static object GetFromBlackboard(string keyName)
        {
            object temp;

            blackboardDictionary.TryGetValue(keyName.ToLower(), out temp);

            return temp;
        }

        /// <summary>
        /// Retrieves variable from Blackboard dictionary without allocating memory (Faster than GetFromBlackboard).
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="blackboardObject"></param>
        /// <returns>Success?</returns>
        public static bool GetFromBlackboardNonAlloc(string keyName, out object blackboardObject)
        {
            if(blackboardDictionary.TryGetValue(keyName.ToLower(), out blackboardObject))
            {
                return true;
            }
            else
            {
                return false;
            }

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