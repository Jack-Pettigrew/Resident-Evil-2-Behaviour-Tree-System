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
            if(blackboard.ContainsKey(keyName.ToLower()))
                return false;

            blackboard.Add(keyName.ToLower(), value);
            return true;
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
            if (sharedBlackboard.ContainsKey(keyName.ToLower()))
                return false;

            sharedBlackboard.Add(keyName.ToLower(), value);
            return true;
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