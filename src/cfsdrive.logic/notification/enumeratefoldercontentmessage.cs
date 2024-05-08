#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : enumeratefoldercontentmessage.cs 
 * 
 * Contents	: Implementation the message to enumerate the folder content
 * 
 * Author	: Sergey Fasonov
 * 
 */
#endregion


using cfsdrive.logic.models;
using System;

namespace cfsdrive.logic.notification
{
    public class EnumerateFolderContentMessage : BaseNotificationMessage
    {
        public EnumerateFolderContentMessage(int folderId)
            : base()
        {
            FolderId = folderId;
        }

        public int FolderId { get; private set; }
    }
}
