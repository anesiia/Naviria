using NUnit.Framework;
using NaviriaAPI.Mappings;
using NaviriaAPI.Entities;
using NaviriaAPI.DTOs;
using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using System;

namespace NaviriaAPI.Tests.Mappings
{
    //public class QuoteMapperTests
    //{
    //    // Позитивний тест: перетворення Entity → DTO
    //    [Test]
    //    public void TC01_ToDto_MapsCorrectly()
    //    {
    //        var entity = new QuoteEntity
    //        {
    //            Id = "123",
    //            Text = "The journey of a thousand miles begins with one step.",
    //            Language = "English"
    //        };

    //        var dto = QuoteMapper.ToDto(entity);

    //        Assert.That(dto.Id, Is.EqualTo("123"));
    //        Assert.That(dto.Text, Is.EqualTo("The journey of a thousand miles begins with one step."));
    //        Assert.That(dto.Language, Is.EqualTo("English"));
    //    }

    //    // Позитивний тест: перетворення DTO → Entity
    //    [Test]
    //    public void TC02_ToEntity_FromDto_MapsCorrectly()
    //    {
    //        var dto = new QuoteDto
    //        {
    //            Id = "456",
    //            Text = "Life is what happens when you're busy making other plans.",
    //            Language = "English"
    //        };

    //        var entity = QuoteMapper.ToEntity(dto);

    //        Assert.That(entity.Id, Is.EqualTo("456"));
    //        Assert.That(entity.Text, Is.EqualTo("Life is what happens when you're busy making other plans."));
    //        Assert.That(entity.Language, Is.EqualTo("English"));
    //    }

    //    // Позитивний тест: перетворення CreateDto → Entity
    //    [Test]
    //    public void TC03_ToEntity_FromCreateDto_MapsCorrectly()
    //    {
    //        var createDto = new QuoteCreateDto
    //        {
    //            Text = "In the middle of difficulty lies opportunity.",
    //            Language = "English"
    //        };

    //        var entity = QuoteMapper.ToEntity(createDto);

    //        Assert.That(entity.Id, Is.Empty); 
    //        Assert.That(entity.Text, Is.EqualTo("In the middle of difficulty lies opportunity."));
    //        Assert.That(entity.Language, Is.EqualTo("English"));
    //    }

    //    // Позитивний тест: перетворення UpdateDto → Entity
    //    [Test]
    //    public void TC04_ToEntity_FromUpdateDto_MapsCorrectly()
    //    {
    //        var updateDto = new QuoteUpdateDto
    //        {
    //            Text = "It always seems impossible until it's done.",
    //            Language = "English"
    //        };

    //        var entity = QuoteMapper.ToEntity("789", updateDto);

    //        Assert.That(entity.Id, Is.EqualTo("789"));
    //        Assert.That(entity.Text, Is.EqualTo("It always seems impossible until it's done."));
    //        Assert.That(entity.Language, Is.EqualTo("English"));
    //    }

//    }
}
