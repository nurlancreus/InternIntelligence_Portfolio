﻿namespace InternIntelligence_Portfolio.Domain.Abstractions
{
    public interface IAuditable
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
