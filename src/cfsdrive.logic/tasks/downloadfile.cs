﻿#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : downloadfile.cs
 * 
 * Contents	: Implementation of the logic to download file
 * 
 * Author	: Sergey Fasonov
 * 
 */
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using castleshield.securemail.core.tasks;
using cfsdrive.logic.helpers;
using cfsdrive.logic.models;
using cfsdrive.logic.services.rest;
using cfsdrive.logic.services.rest.dto;
using cfsdrive.logic.services.rest.models;

namespace cfsdrive.logic.tasks
{
    public class DownloadFile : BaseTaskWithUser
    {
        private string _downloadFileName = string.Empty;
        private int _itemId = 0;
        private string _fileName = string.Empty;
        private Action<long, long> _progressCallback = null;
        private CancellationToken _cancellationToken;

        public DownloadFile(int itemId, string fileName, LoggedUser user, CancellationToken ct, Action<long, long> progressCallback, Action<ITaskResult> completionCallback) :
            base("DownloadFile", user, completionCallback)
        {
            _itemId = itemId;
            _progressCallback = progressCallback;
            _fileName = fileName;
            _cancellationToken = ct;
        }

        protected override void OnRun()
        {
            _downloadFileName = GetFileName();
            RestApiService apiService = GetRestApiService();
            bool result = apiService.DownloadFile(ApiUrls.Files.Download, null, new object[] { _itemId.ToString() }, GetFileName(), _cancellationToken, _progressCallback);
            if (!result)
            {
                File.Delete(_downloadFileName);
                _downloadFileName = string.Empty;
            }
        }

        protected override void ExecutionFinished(ITaskResult result)
        {
            try
            {
                if (result.Success)
                {
                    result.Result = _downloadFileName;
                }

                _completionCallback?.Invoke(result);
            }
            catch (Exception ex)
            {
                Api.Instance.Logger.LogError(ex, $"The exception occured in the ExecutionFinished of task {Name}");
            }

            Api.Instance.Logger.LogInfo($"Task {Name} runs the continuous task.");
        }
        
        private string GetFileName()
        {
            string downloadPath = KnownFolders.GetPath(KnownFolder.Downloads);
            downloadPath = Path.Combine(downloadPath, Api.Instance.Configuration.Settings.DownloadFolderName);
            if (!Directory.Exists(downloadPath))
                Directory.CreateDirectory(downloadPath);

            _fileName = Path.Combine(downloadPath, _fileName);
            _fileName = PathHelper.NextAvailableFilename(_fileName);
            return _fileName;
        }

    }
}