using InternIntelligence_Portfolio.Application.DTOs.User;

namespace InternIntelligence_Portfolio.Tests.Common.Factories
{
    public static partial class Factories
    {
        public static class Users
        {
            public static ChangeProfilePictureRequestDTO GenerateValidChangeProfilePictureRequestDTO()
            {
                return new ChangeProfilePictureRequestDTO
                {
                    NewProfilePictureFile = Utilities.GenerateMockFile(
                        Constants.Constants.Users.ProfilePicture_FileName_Valid,
                        Constants.Constants.Users.ProfilePicture_ContentType_Valid,
                        Constants.Constants.Users.ProfilePicture_MaxFileSizeBytes)
                };
            }

            public static ChangeProfilePictureRequestDTO GenerateInValidChangeProfilePictureRequestDTO(bool oversized = false)
            {
                return new ChangeProfilePictureRequestDTO
                {
                    NewProfilePictureFile = Utilities.GenerateMockFile(
                        Constants.Constants.Users.ProfilePicture_FileName_InValid,
                        Constants.Constants.Users.ProfilePicture_ContentType_InValid,
                        oversized
                            ? Constants.Constants.Users.ProfilePicture_OversizedFileSizeBytes
                            : Constants.Constants.Users.ProfilePicture_MaxFileSizeBytes)
                };
            }
        }
    }
}
