#region Copyright (c) 2021
/* Tierfive Explorer Extension
 * Copyright (c) 2021
 * 
 * Company	: Tierfive
 * 
 * File     : itemdto.cs 
 * 
 * Contents	: Declaration of item DTO
 * 
 * Author	: Sergey Fasonov
 * 
 */
#endregion

using System;
using System.Collections.Generic;

namespace cfsdrive.logic.services.rest.dto
{
    public class ItemDto: BaseDto
    {
        public int id { get; set; }

        public string FileName { get; set; }
        
        public long SizeBytes { get; set; }

        public long MaxSize { get; set; }
        
        public bool IsFavorite { get; set; }
        
        public string Size { get; set; }
        
        public EntryType Type { get; set; }
        
        public bool IsSubscribed { get; set; }
        
        public int FileType { get; set; }
        
        public int SubType { get; set; }
        
        public int CommentCount { get; set; }
        
        public string MimeType { get; set; }
        
        public PermissionAccess[] Rights { get; set; }
        
        public DateTime CreatedAt { get; set; }
    }

    public class FolderContentDto: BaseDto
    {
        public int TotalRecords { get; set; }

        public List<ItemDto> Items { get; set; }
    }
}