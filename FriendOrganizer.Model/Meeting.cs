﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace FriendOrganizer.Model
{
    public class Meeting
    {
        #region Constructors and Destructors

        public Meeting()
        {
            Friends = new Collection<Friend>();
        }

        #endregion

        #region Public Properties

        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public ICollection<Friend> Friends { get; set; }
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        #endregion
    }
}