using InternIntelligence_Portfolio.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternIntelligence_Portfolio.Tests.Common.Constants
{
    public static partial class Constants
    {
        public static class Users
        {
            public const string ProfilePicture_FileName_Valid = "profile_picture.png";
            public const string ProfilePicture_ContentType_Valid = "image/png";

            public const string ProfilePicture_FileName_InValid = "document.pdf";
            public const string ProfilePicture_ContentType_InValid = "application/pdf";

            public static readonly int ProfilePicture_MaxFileSizeBytes = DomainConstants.User.UserProfilePictureMaxSizeInMb * 1024 * 1024;
            public static readonly int ProfilePicture_OversizedFileSizeBytes = ProfilePicture_MaxFileSizeBytes + 1024 * 1024; // 1MB over limit
        }
    }
}
