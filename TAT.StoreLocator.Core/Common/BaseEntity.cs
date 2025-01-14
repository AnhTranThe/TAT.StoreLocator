﻿using System.ComponentModel.DataAnnotations;

namespace TAT.StoreLocator.Core.Common
{
    /// <summary>
    /// BaseEntity
    /// </summary>
    public abstract class BaseEntity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public DateTimeOffset CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}