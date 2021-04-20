using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    public class Blackboard
    {
        // Per Instance
        private Dictionary<string, object> blackboard;

        // Shared Blackboard
        private static Dictionary<string, object> sharedBlackboard = new Dictionary<string, object>();

        public Blackboard()
        {
            blackboard = new Dictionary<string, object>();
        }

        #region Instance Blackboard Methods
        public bool AddToBlackboard(string keyName, object value)
        {
            string key = keyName.ToLower();

            if (blackboard.ContainsKey(key))
                return false;

            blackboard.Add(key, value);
            return true;
        }

        public bool UpdateBlackboardVariable(string keyName, object value)
        {
            string key = keyName.ToLower();

            if (blackboard.ContainsKey(key))
            {
                blackboard[key] = value;
                return true;
            }

            return AddToBlackboard(key, value);
        }

        /// <summary>
        /// Returns variable from Blackboard dictionary. Allocates memory.
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns>'object' typed variable. NULL on not found.</returns>
        public object GetFromBlackboard(string keyName)
        {
            object temp;

            blackboard.TryGetValue(keyName.ToLower(), out temp);

            return temp;
        }

        /// <summary>
        /// Retrieves variable from Blackboard dictionary without allocating memory (Faster than GetFromBlackboard).
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="blackboardObject"></param>
        /// <returns>Success?</returns>
        public bool GetFromBlackboardNonAlloc(string keyName, out object blackboardObject)
        {
            if (blackboard.TryGetValue(keyName.ToLower(), out blackboardObject))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RemoveFromBlackboard(string keyName)
        {
            if (blackboard.Remove(keyName.ToLower()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Shared Blackboard Methods
        public static bool AddToSharedBlackboard(string keyName, object value)
        {
            string key = keyName.ToLower();

            if (sharedBlackboard.ContainsKey(key))
                return false;

            sharedBlackboard.Add(key, value);
            return true;
        }

        public static bool UpdateSharedBlackboardVariable(string keyName, object value)
        {
            string key = keyName.ToLower();

            if (sharedBlackboard.ContainsKey(key))
            {
                sharedBlackboard[key] = value;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns variable from Blackboard dictionary. Allocates memory.
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns>'object' typed variable. NULL on not found.</returns>
        public static object GetFromSharedBlackboard(string keyName)
        {
            object temp;

            sharedBlackboard.TryGetValue(keyName.ToLower(), out temp);

            return temp;
        }

        /// <summary>
        /// Retrieves variable from Blackboard dictionary without allocating memory (Faster than GetFromBlackboard).
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="blackboardObject"></param>
        /// <returns>Success?</returns>
        public static bool GetFromSharedBlackboardNonAlloc(string keyName, out object blackboardObject)
        {
            if (sharedBlackboard.TryGetValue(keyName.ToLower(), out blackboardObject))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool RemoveFromSharedBlackboard(string keyName)
        {
            if (sharedBlackboard.Remove(keyName.ToLower()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}