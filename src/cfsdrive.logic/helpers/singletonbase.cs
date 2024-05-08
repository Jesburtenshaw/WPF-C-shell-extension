#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : SingletonBase.cs 
 * 
 * Contents	: Implementation the generic singleton class
 * 
 * Author	: Sergey Fasonov
 * 
 */
#endregion

using System;

namespace cfsdrive.logic.helpers
{
    /// <summary>
    /// A base class for the singleton design pattern.
    /// </summary>
    /// <typeparam name="T">Class type of the singleton</typeparam>
    public abstract class SingletonBase<T> where T : class
    {
        /// <summary>
        /// Static instance. Needs to use lambda expression
        /// to construct an instance (since constructor is private).
        /// </summary>
        private static readonly Lazy<T> SInstance = new Lazy<T>(CreateInstanceOfT);

        /// <summary>
        /// Gets the instance of this singleton.
        /// </summary>
        public static T Instance => SInstance.Value;

        /// <summary>
        /// Creates an instance of T via reflection since T's constructor is expected to be private.
        /// </summary>
        /// <returns></returns>
        private static T CreateInstanceOfT()
        {
            return Activator.CreateInstance(typeof(T), true) as T;
        }
    }

}
