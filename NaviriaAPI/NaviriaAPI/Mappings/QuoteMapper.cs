﻿using NaviriaAPI.Entities;
using NaviriaAPI.DTOs.Quote;

namespace NaviriaAPI.Mappings
{
    public static class QuoteMapper
    {
        // Entity → DTO (для відправки в API)
        public static QuoteDto ToDto(QuoteEntity entity) =>
            new QuoteDto { Id = entity.Id, Text = entity.Text, Language = entity.Language };

        // 🟩 DTO → Entity (для збереження в БД)
        public static QuoteEntity ToEntity(QuoteDto dto) =>
        new QuoteEntity { Id = dto.Id, Text = dto.Text, Language = dto.Language };

        // CreateDto → Entity (для створення)
        public static QuoteEntity ToEntity(QuoteCreateDto dto) =>
            new QuoteEntity { Text = dto.Text, Language = dto.Language };

        // UpdateDto → Entity (для оновлення)
        public static QuoteEntity ToEntity(string id, QuoteUpdateDto dto) =>
            new QuoteEntity { Id = id, Text = dto.Text, Language = dto.Language };
    }
}
