#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : folderinfodto.cs 
 * 
 * Contents	: Declaration of Folderinfo DTO
 * 
 * Author	: Sergey Fasonov
 * 
 */
#endregion


using System.Collections.Generic;

namespace cfsdrive.logic.services.rest.dto
{
    public class FolderInfoDto: BaseDto
    {
        public int id { get; set; }

        public List<BreadcrumbDto> Breadcrumbs { get; set; }

        public int TotalRecords { get; set; }

        public PermissionAccess[] Rights { get; set; }
    }
}