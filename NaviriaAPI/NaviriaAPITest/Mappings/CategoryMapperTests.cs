using NUnit.Framework;
using NaviriaAPI.Mappings;
using NaviriaAPI.DTOs;

using NaviriaAPI.Entities;

namespace NaviriaAPI.Tests.Mappings
{
    //public class CategoryMapperTests
    //{
    //    // Позитивний тест для перетворення CategoryEntity в CategoryDto
    //    [Test]
    //    public void TC01_ToDto_MapsCorrectly()
    //    {
    //        var entity = new CategoryEntity
    //        {
    //            Id = "1",
    //            Name = "Technology"
    //        };

    //        var dto = CategoryMapper.ToDto(entity);

    //        Assert.That(dto.Id, Is.EqualTo("1"));
    //        Assert.That(dto.Name, Is.EqualTo("Technology"));
    //    }

    //    // Позитивний тест для перетворення CategoryDto в CategoryEntity
    //    [Test]
    //    public void TC02_ToEntity_FromDto_MapsCorrectly()
    //    {
    //        var dto = new CategoryDto
    //        {
    //            Id = "1",
    //            Name = "Technology"
    //        };

    //        var entity = CategoryMapper.ToEntity(dto);

    //        Assert.That(entity.Id, Is.EqualTo("1"));
    //        Assert.That(entity.Name, Is.EqualTo("Technology"));
    //    }

    //    // Позитивний тест для перетворення CategoryCreateDto в CategoryEntity
    //    [Test]
    //    public void TC03_ToEntity_FromCreateDto_MapsCorrectly()
    //    {
    //        var createDto = new CategoryCreateDto
    //        {
    //            Name = "Technology"
    //        };

    //        var entity = CategoryMapper.ToEntity(createDto);

    //        Assert.That(entity.Id, Is.Empty); 
    //        Assert.That(entity.Name, Is.EqualTo("Technology"));
    //    }

        //// Позитивний тест для перетворення CategoryUpdateDto в CategoryEntity з заданим id
        //[Test]
        //public void TC04_ToEntity_FromUpdateDto_MapsCorrectly()
        //{
        //    var updateDto = new CategoryUpdateDto
        //    {
        //        Name = "Tech & Gadgets"
        //    };

        //    var entity = CategoryMapper.ToEntity("1", updateDto);

        //    Assert.That(entity.Id, Is.EqualTo("1"));
        //    Assert.That(entity.Name, Is.EqualTo("Tech & Gadgets"));
        //}

        
    //}
}
