#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : breadcrumbdto.cs 
 * 
 * Contents	: Declaration of Breadcrumbs DTO
 * 
 * Author	: Sergey Fasonov
 * 
 */
#endregion


namespace cfsdrive.logic.services.rest.dto
{
    public class BreadcrumbDto: BaseDto
    {
        public int id { get; set; }

        public string Name { get; set; }
    }
}