namespace InternIntelligence_Portfolio.Tests.Common.Constants
{
    public static partial class Constants
    {
        public static class Achievements
        {
            public const string Title_Valid = "Completed AI Project";
            public const string Description_Valid = "Successfully built and deployed an AI model.";
            public static readonly DateTime AchievedAt_Valid = DateTime.Parse("2024-02-25T00:00:00Z");

            public const string Updated_Title_Valid = "Completed Advanced AI Project";
            public const string Updated_Description_Valid = "Enhanced AI model with better accuracy.";
            public static readonly DateTime Updated_AchievedAt_Valid = DateTime.Parse("2024-03-10T00:00:00Z");

            public const string Title_InValid = "";
            public const string Description_InValid = "";
            public static readonly DateTime AchievedAt_InValid = DateTime.Parse("2030-02-25T00:00:00Z");
        }
    }
}
