#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : basetask.cs 
 * 
 * Contents	: Implementation of absract base task
 * 
 * Author	: Sergey Fasonov
 * 
 */
#endregion

using cfsdrive.logic;
using cfsdrive.logic.services.rest.dto;
using cfsdrive.logic.services.rest.exceptions;
using System;
using System.Threading.Tasks;

namespace castleshield.securemail.core.tasks
{
    public abstract class BaseTask
    {
        protected readonly Action<ITaskResult> _completionCallback = null;

        protected BaseTask(string name, Action<ITaskResult> completionCallback)
        {
            Name = name;
            _completionCallback = completionCallback;
        }

        public string Name { get; private set; }

        public void Execute()
        {
            Api.Instance.Logger.LogInfo("Runs the STA task {0}", Name);
            Task task = Task.Factory.StartNew(OnRun);
            task.ContinueWith(OnFaulted, TaskContinuationOptions.OnlyOnFaulted);
            task.ContinueWith(OnCompleted, TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        protected void OnFaulted(Task t)
        {
            Api.Instance.Logger.LogInfo("The STA task {0} is failed", Name);
            ExecutionFinished(new BaseTaskResult
            {
                Success = false,
                Error = ExtractException(t.Exception),
                Result = null
            });
        }

        protected void OnCompleted(Task t)
        {
            Api.Instance.Logger.LogInfo("The STA task {0} is completed", Name);
            ExecutionFinished(new BaseTaskResult
            {
                Success = true,
                Error = null,
                Result = null
            });
        }

        /// <summary>
        /// Task method to run in the STA
        /// </summary>
        protected virtual void OnRun()
        {
        }

        /// <summary>
        /// Notify the task is finished
        /// </summary>
        /// <param name="result"></param>
        protected virtual void ExecutionFinished(ITaskResult result)
        {
        }

        private ErrorDto ExtractException(Exception exception)
        {
            ErrorDto errorDto = null;

            if ((exception as AggregateException) != null)
            {
                errorDto = ExtractException(((AggregateException)exception)?.InnerException);
            }
            else
            {
                if ((exception as ApiException) != null)
                {
                    errorDto = ((ApiException)exception).Error;
                }
                else
                {
                    errorDto = new ErrorDto
                    {
                        Status = System.Net.HttpStatusCode.BadRequest,
                        Message = exception?.Message
                    };
                }
            }
            return errorDto;
        }
    }

    public class BaseTaskResult : ITaskResult
    {
        public bool Success { get; set; }

        /// <summary>
        /// Error DTO if the error exists
        /// </summary>
        public ErrorDto Error { get; set; }

        /// <summary>
        /// Tasks results object
        /// </summary>
        public object Result { get; set; }
    }
}
