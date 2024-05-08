#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : enums.cs 
 * 
 * Contents	: Declaration of enums for dto
 * 
 * Author	: Sergey Fasonov
 * 
 */
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cfsdrive.logic.services.rest.dto
{
    public enum PermissionAccess
    {
        Read = 0,

        Write = 1,

        AddTags = 2,

        AddComments = 3,

        Share = 4,

        Undocummented = 5
    }

    public enum EntryType
    {
        Unknown = 0,

        File = 1, 

        Directory = 2, 

        Hdd = 3,

        Tombstone = 4
    }
}
