namespace InternIntelligence_Portfolio.Domain
{
    public static class DomainConstants
    {
        public static class User
        {
            public const int FirstNameMaxLength = 50;
            public const int LastNameMaxLength = 50;
            public const int UserNameMaxLength = 50;
            public const int EmailMaxLength = 50;

            public const int BioMaxLength = 1000;

            public const string UserProfilePictureContainerName = "profile-pictures";
            public const int UserProfilePictureMaxSizeInMb = 2;
        }

        public static class Contact
        {
            public const int FirstNameMaxLength = 50;
            public const int LastNameMaxLength = 50;
            public const int EmailMaxLength = 50;

            public const int MessageMaxLength = 1000;
        }

        public static class File
        {
            public const int NameMaxLength = 100;
            public const int PathMaxLength = 100;
        }

        public static class Achievement
        {
            public const int TitleMaxLength = 255;
            public const int DescriptionMaxLength = 1000;
        }

        public static class Project
        {
            public const int NameMaxLength = 255;
            public const int DescriptionMaxLength = 1000;

            public const string ProjectCoverImageContainerName = "cover-images";
            public const int ProjectCoverImageMaxSizeInMb = 3;

        }

        public static class Skill
        {
            public const int NameMaxLength = 255;
            public const int DescriptionMaxLength = 1000;
        }
    }
}
