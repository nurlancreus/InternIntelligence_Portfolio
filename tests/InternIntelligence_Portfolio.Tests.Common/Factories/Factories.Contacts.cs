using InternIntelligence_Portfolio.Application.DTOs.Contact;

namespace InternIntelligence_Portfolio.Tests.Common.Factories
{
    public static partial class Factories
    {
        public static class Contacts
        {
            public static IEnumerable<CreateContactRequestDTO> GenerateMultipleValidCreateContactRequestDTOs(byte count = 3)
            {
                for (int i = 1; i <= count; i++)
                {
                    yield return new CreateContactRequestDTO
                    {
                        FirstName = $"{Constants.Constants.Contacts.FirstName_Valid}-{i}",
                        LastName = $"{Constants.Constants.Contacts.LastName_Valid}-{i}",
                        Email = Constants.Constants.Contacts.Email_Valid,
                        Message = $"{Constants.Constants.Contacts.Message_Valid}-{i}",
                        Subject = Constants.Constants.Contacts.Subject_Valid,
                    };
                }
            }
            public static CreateContactRequestDTO GenerateValidCreateContactRequestDTO()
            {
                return new CreateContactRequestDTO
                {
                    FirstName = Constants.Constants.Contacts.FirstName_Valid,
                    LastName = Constants.Constants.Contacts.LastName_Valid,
                    Email = Constants.Constants.Contacts.Email_Valid,
                    Message = Constants.Constants.Contacts.Message_Valid,
                    Subject = Constants.Constants.Contacts.Subject_Valid,
                };
            }

            public static AnswerContactRequestDTO GenerateValidAnswerContactRequestDTO()
            {
                return new AnswerContactRequestDTO
                {
                    Message = Constants.Constants.Contacts.Response_Valid,
                };
            }

            public static CreateContactRequestDTO GenerateInValidCreateContactRequestDTO()
            {
                return new CreateContactRequestDTO
                {
                    FirstName = Constants.Constants.Contacts.FirstName_InValid,
                    LastName = Constants.Constants.Contacts.LastName_InValid,
                    Email = Constants.Constants.Contacts.Email_InValid,
                    Message = Constants.Constants.Contacts.Message_InValid,
                    Subject = Constants.Constants.Contacts.Subject_InValid,
                };
            }

            public static AnswerContactRequestDTO GenerateInValidAnswerContactRequestDTO()
            {
                return new AnswerContactRequestDTO
                {
                    Message = Constants.Constants.Contacts.Response_InValid,
                };
            }
        }
    }
}
