#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : ipopupservice.cs
 * 
 * Contents	: Provides a base interface to display message boxes
 * 
 * Author	:  Sergey Fasonov
 * 
 */
#endregion

using System;
using System.Collections.Generic;

namespace cfsdrive.logic.interfaces
{
    public interface IPopupService
    {
        /// <summary>
        /// Shows the standard "not implemented" message
        /// </summary>
        void ShowNotImplementedFunctionality();

        /// <summary>
        /// Displays a message box contained an information message
        /// </summary>
        /// <param name="text"></param>
        void DisplayInformation(string text);

        /// <summary>
        /// Displays a message box contained a question message
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        bool DisplayQuestion(string text);

        /// <summary>
        /// Displays a message box contained an information message
        /// </summary>
        /// <param name="text"></param>
        void DisplayWarning(string text);

        /// <summary>
        /// Displays a message box contained a question message
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        void DisplayError(string text);

        /// <summary>
        /// Shows the About View
        /// </summary>
        /// <param name="parentWindow"></param>
        /// <returns></returns>
        void ShowAbout(IntPtr parentWindow);

        /// <summary>
        /// Shows the About View
        /// </summary>
        /// <param name="parentWindow"></param>
        /// <returns></returns>
        void ShowLogin(IntPtr parentWindow);

        /// <summary>
        /// Shows the new folder window
        /// </summary>
        /// <param name="parentid"></param>
        /// <param name="parentWindow"></param>
        void ShowNewFolder(int parentId, IntPtr parentWindow);

        /// <summary>
        /// Shows the rename folder dialog
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="parentWindow"></param>
        void ShowRenameFolder(int parentId, object itemDto, IntPtr parentWindow);

        /// <summary>
        /// Shows the remove items dialog
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <param name="parentWindow"></param>
        void ShowRemoveItems(int parentId, List<int> selectedIds, IntPtr parentWindow);
    }
}
