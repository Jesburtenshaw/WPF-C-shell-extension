#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : coreexception.cs 
 * 
 * Contents	: Definition of base exception will be used to create custom exception in the project
 * 
 * Author	: Sergey Fasonov
 * 
 */
#endregion

using System;
using System.Diagnostics;
using System.Reflection;

namespace cfsdrive.logic.exceptions
{
    /// <summary>
    /// Base exception
    /// </summary>
    [Serializable]
    public abstract class CoreException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the CoreException class
        /// </summary>
        protected CoreException()
            : this(string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the NetsyException class
        /// </summary>
        /// <param name="message">the message</param>
        protected CoreException(string message) :
            this(message, null)
        {
        }

        protected CoreException(Exception innerException) :
            this("Internal error was occured", innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the NetsyException class
        /// </summary>
        /// <param name="message">the message</param>
        /// <param name="innerException">the exception to wrap</param>
        protected CoreException(string message, Exception innerException)
            : base(message, innerException)
        {
            string methodName = string.Empty;

            try
            {
                if (innerException != null)
                {
                    AggregateException aggregateException = innerException as AggregateException;
                    if (aggregateException != null && aggregateException.InnerExceptions.Count > 0)
                        innerException = ((AggregateException)innerException).InnerExceptions[0];

                    StackTrace callStack = new StackTrace(innerException, true);
                    StackFrame callingMethodFrame = callStack.GetFrame(0);
                    MethodBase callingMethod = callingMethodFrame.GetMethod();
                    methodName = $"{(callingMethod.DeclaringType != null ? callingMethod.DeclaringType.FullName : string.Empty)}.{callingMethod.Name}";
                }
            }
            catch
            {
                // ignored
            }
            TargetMethodName = methodName;
        }

        /// <summary>
        /// Method name where the exception occured
        /// </summary>
        public string TargetMethodName { get; protected set; }
    }
}
