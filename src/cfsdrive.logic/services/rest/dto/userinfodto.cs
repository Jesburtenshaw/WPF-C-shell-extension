#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : userinfodto.cs 
 * 
 * Contents	: Declaration of Error DTO
 * 
 * Author	: Sergey Fasonov
 * 
 */
#endregion


namespace cfsdrive.logic.services.rest.dto
{
    public class UserInfoDto: BaseDto
    {
        public int id { get; set; }

        public string DisplayName { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsAuthenticated { get; set; }

        public bool IsTFARequired { get; set; }

        public int UserType { get; set; }
    }
}