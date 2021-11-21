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
        /// <summary>
        /// Adds a new variable to the Blackboard associated with the given keyName. Won't add if pre-existing keyName used (use update instead).
        /// </summary>
        /// <param name="keyName">Blackboard variable Key.</param>
        /// <param name="value">Blackboard variable value.</param>
        /// <returns>Success?</returns>
        public bool AddToBlackboard(string keyName, object value)
        {
            if (blackboard.ContainsKey(keyName))
                return false;

            blackboard.Add(keyName, value);
            return true;
        }

        /// <summary>
        /// Returns whether the Blackboard variable is set.
        /// </summary>
        /// <param name="keyName">Key of the variable in Blackboard.</param>
        /// <returns></returns>
        public bool IsBlackboardVariableNull(string keyName)
        {
            object variable = null;
            blackboard.TryGetValue(keyName, out variable);
            return variable == null ? true : false;
        }

        /// <summary>
        /// Updates the Blackboard variable with the matching Key with the given value.
        /// </summary>
        /// <param name="keyName">Blackboard variable Key.</param>
        /// <param name="value">Blackboard variable value.</param>
        /// <param name="addIfNotFound">If not found, should it be added to the BlackBoard?</param>
        /// <returns>Success?</returns>
        public bool UpdateBlackboardVariable(string keyName, object value, bool addIfNotFound = false)
        {
            if (blackboard.ContainsKey(keyName))
            {
                blackboard[keyName] = value;
                return true;
            }
            else if (addIfNotFound)
            {
                return AddToBlackboard(keyName, value);
            }
            else
            { 
                return false;
            }
        }

        /// <summary>
        /// Returns variable from Blackboard dictionary. Allocates memory.
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns>'object' typed variable. NULL on not found.</returns>
        public T GetFromBlackboard<T>(string keyName)
        {
            object temp;

            blackboard.TryGetValue(keyName, out temp);

            return (T)temp;
        }

        /// <summary>
        /// Retrieves variable from Blackboard dictionary without allocating memory (Faster than GetFromBlackboard).
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="blackboardObject"></param>
        /// <returns>Success?</returns>
        public bool GetFromBlackboardNonAlloc(string keyName, out object blackboardObject)
        {
            return blackboard.TryGetValue(keyName, out blackboardObject);
        }

        public bool RemoveFromBlackboard(string keyName)
        {
            if (blackboard.Remove(keyName))
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
            string key = keyName;

            if (sharedBlackboard.ContainsKey(key))
                return false;

            sharedBlackboard.Add(key, value);
            return true;
        }

        public static bool UpdateSharedBlackboardVariable(string keyName, object value)
        {
            string key = keyName;

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
        public static T GetFromSharedBlackboard<T>(string keyName)
        {
            object temp;

            sharedBlackboard.TryGetValue(keyName, out temp);

            return (T)temp;
        }

        /// <summary>
        /// Retrieves variable from Blackboard dictionary without allocating memory (Faster than GetFromBlackboard).
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="blackboardObject"></param>
        /// <returns>Success?</returns>
        public static bool GetFromSharedBlackboardNonAlloc(string keyName, out object blackboardObject)
        {
            if (sharedBlackboard.TryGetValue(keyName, out blackboardObject))
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
            if (sharedBlackboard.Remove(keyName))
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