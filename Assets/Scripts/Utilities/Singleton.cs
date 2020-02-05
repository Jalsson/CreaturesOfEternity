//Original file created by Julius Liias 14.11.2019

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Venus.Utilities
{
    /// <summary>
    /// Singleton class. Inherit from this to make a class a singleton.
    /// Makes sure that there exists only one instance of the given class type.
    /// </summary>
    /// <typeparam name="T">The class that inherits from this</typeparam>
    public class Singleton<T> : MonoBehaviour
    {
        /// <summary>
        /// Instance of this class
        /// </summary>
        public static T S_INSTANCE;

        protected virtual void Awake()
        {
            if (S_INSTANCE == null) //Check if there are no previous instance of this
            {
                S_INSTANCE = GetComponent<T>();

                if (S_INSTANCE == null) //Check if GetComponent worked
                {
                    Debug.LogError("ERROR: Did not find the specified component for the Singleton class!");
                }
            }
            else //Throw and error and delete the possible double instance of the class
            {
                Debug.LogWarning(String.Format("ERROR: There's more than one {0} in the scene! This is a singleton class and only one should exist!",
                    GetType().Name, gameObject.name));
                S_INSTANCE = GetComponent<T>();
            }
        }
    }
}