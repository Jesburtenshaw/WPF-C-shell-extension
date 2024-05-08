#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : loginmessage.cs 
 * 
 * Contents	: Implementation the message to notify about successfully login
 * 
 * Author	: Sergey Fasonov
 * 
 */
#endregion


using cfsdrive.logic.models;
using System;

namespace cfsdrive.logic.notification
{
    public class LoginMessage : BaseNotificationMessage
    {
        public LoginMessage(LoggedUser user)
            : base()
        {
            User = user;
        }

        public LoggedUser User { get; private set; }
    }
}
