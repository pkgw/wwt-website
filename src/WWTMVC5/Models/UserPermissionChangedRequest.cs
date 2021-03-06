﻿//-----------------------------------------------------------------------
// <copyright file="UserPermissionChangedRequest.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation 2011. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace WWTMVC5.Models
{
    /// <summary>
    /// Class representing the details about a Rating.
    /// </summary>
    [Serializable]
    public class UserPermissionChangedRequest
    {
        /// <summary>
        /// Gets or sets the name of the User.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the identification of the User.
        /// </summary>
        public long UserID { get; set; }

        /// <summary>
        /// Gets or sets the link of the User.
        /// </summary>
        public string UserLink { get; set; }

        /// <summary>
        /// Gets or sets the name of the Community.
        /// </summary>
        public string CommunityName { get; set; }

        /// <summary>
        /// Gets or sets the identification of the community.
        /// </summary>
        public long CommunityID { get; set; }

        /// <summary>
        /// Gets or sets the link for the community.
        /// </summary>
        public string CommunityLink { get; set; }

        /// <summary>
        /// Gets or sets ModeratorID.
        /// </summary>
        public long ModeratorID { get; set; }

        /// <summary>
        /// Gets or sets Moderator Link.
        /// </summary>
        public string ModeratorLink { get; set; }

        /// <summary>
        /// Gets or sets the Role of the user.
        /// </summary>
        public UserRole Role { get; set; }
    }
}