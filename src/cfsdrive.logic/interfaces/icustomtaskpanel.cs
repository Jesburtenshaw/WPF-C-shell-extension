#region Copyright (c) 2018
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : icustomtaskpanel.cs
 * 
 * Contents	: Provides a base interface for custom task panel
 * 
 * Author	:  Sergey Fasonov
 * 
 */
#endregion

using System;

namespace cfsdrive.logic.interfaces
{
    public interface ICustomTaskPanel
    {
        /// <summary>
        /// Task panel identifier
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Task Panel Title
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Returns a UserControl which is the container for task panel elements
        /// </summary>
        object UserControl { get; }

        /// <summary>
        /// Task panel is destoryed
        /// </summary>
        void Destoyed();

        IntPtr ParentWindow { get; set; }
    }
}
