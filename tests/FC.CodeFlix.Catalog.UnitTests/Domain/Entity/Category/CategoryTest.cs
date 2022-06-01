using FC.CodeFlix.Catalog.Domain.Exceptions;
using Xunit;
using DomainEntity = FC.CodeFlix.Catalog.Domain.Entity;

namespace FC.CodeFlix.Catalog.UnitTests.Domain.Entity.Category;

public class CategoryTest 
{
    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Instantiate()
    {
        var validData = new {
            Name = "Category name",
            Description = "Category description"
        };
        var datetimeBefore = DateTime.Now;

        var category = new DomainEntity.Category(validData.Name, validData.Description);

        var datetimeAfter = DateTime.Now;

        Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(Guid.Empty, category.Id);
        Assert.NotEqual(default(DateTime), category.CreatedAt);
        Assert.True(category.CreatedAt > datetimeBefore);
        Assert.True(category.CreatedAt < datetimeAfter);
        Assert.True(category.IsActive);
    }   

    [Theory(DisplayName = nameof(InstantiateWithIsActive))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithIsActive(bool isActive)
    {
        var validData = new {
            Name = "Category name",
            Description = "Category description"
        };
        var datetimeBefore = DateTime.Now;

        var category = new DomainEntity.Category(validData.Name, validData.Description, isActive);

        var datetimeAfter = DateTime.Now;

        Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(Guid.Empty, category.Id);
        Assert.NotEqual(default(DateTime), category.CreatedAt);
        Assert.True(category.CreatedAt > datetimeBefore);
        Assert.True(category.CreatedAt < datetimeAfter);
        Assert.Equal(isActive, category.IsActive);
    }

    [Theory(DisplayName = nameof(InstatiateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    public void InstatiateErrorWhenNameIsEmpty(string? name) {
        Action action = 
            () => new DomainEntity.Category(name!, "Category description");

        var exception = Assert.Throws<EntityValidationException>(action);

        Assert.Equal("Name should not be empty or null", exception.Message);
    }

    [Fact(DisplayName = nameof(InstatiateErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstatiateErrorWhenDescriptionIsNull() {
        Action action = 
            () => new DomainEntity.Category("Category name", null!);

        var exception = Assert.Throws<EntityValidationException>(action);

        Assert.Equal("Description should not be null", exception.Message);
    }

    [Theory(DisplayName = nameof(InstatiateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("a")]
    [InlineData("ab")]
    public void InstatiateErrorWhenNameIsLessThan3Characters(string invalidName) {
        Action action = 
            () => new DomainEntity.Category(invalidName, "Category description");

        var exception = Assert.Throws<EntityValidationException>(action);

        Assert.Equal("Name should be at leats 3 characters long", exception.Message);
    }   

    [Fact(DisplayName = nameof(InstatiateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstatiateErrorWhenNameIsGreaterThan255Characters() {
        var invalidName = 
            String.Join(null, Enumerable.Range(1, 256).Select(_ => "a"));

        Action action = 
            () => new DomainEntity.Category(invalidName, "Category description");

        var exception = Assert.Throws<EntityValidationException>(action);

        Assert.Equal("Name should be less or equal 255 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(InstatiateErrorWhenDescriptionIsGreaterThan10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstatiateErrorWhenDescriptionIsGreaterThan10_000Characters() {
        var invalidDescription = 
            String.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a"));

        Action action = 
            () => new DomainEntity.Category("Category name", invalidDescription);

        var exception = Assert.Throws<EntityValidationException>(action);

        Assert.Equal("Description should be less or equal 10_000 characters long", exception.Message);
    } 
}