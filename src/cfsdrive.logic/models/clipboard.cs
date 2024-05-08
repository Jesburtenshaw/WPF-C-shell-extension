#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : clipboard.cs 
 * 
 * Contents	: Implementation the clipboard
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

namespace cfsdrive.logic.models
{
    public enum ClipboardType
    {
        None = 0,

        Copy = 1,

        Cut = 2
    }

    public class Clipboard
    {
        public ClipboardType ClipboardType { get; set; }

        public int[] Items = null;
    }
}
