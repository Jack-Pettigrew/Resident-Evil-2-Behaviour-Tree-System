using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DD.AI.BehaviourTreeSystem
{
    public class Blackboard
    {
        private Dictionary<string, object> blackboard;

        public Blackboard()
        {
            blackboard = new Dictionary<string, object>();
        }

        public void AddToBlackboard(string keyName, object value)
        {
            blackboard.Add(keyName.ToLower(), value);
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
    }
}