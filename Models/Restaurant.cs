﻿using System.ComponentModel.DataAnnotations.Schema;

namespace auckland_curry_movement_api.Models
{
    public class Restaurant
    {
        public int? ID { get; set; }
        public string Name { get; set; } = string.Empty;
        [Column("Street Address")]
        public string? StreetAddress { get; set; }
        public string Suburb { get; set; } = string.Empty;
        [Column("Phone Number")]
        public string? PhoneNumber { get; set; }
        public bool IsArchived { get; set; }
        public string? ArchiveReason { get; set; }

        public virtual ICollection<Reservation>? Reservations { get; set; }
        public virtual ICollection<RotY>? RotYs { get; set; }
        public virtual ICollection<Notification>? Notifications { get; set; }
    }
}
