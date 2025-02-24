﻿namespace InternIntelligence_Portfolio.Application.DTOs.Contact
{
    public record UpdateContactRequestDTO
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; } 
        public string? LastName { get; set; } 
        public string? Email { get; set; } 
        public string? Message { get; set; } 
        public string? Subject { get; set; } 
    }
}
