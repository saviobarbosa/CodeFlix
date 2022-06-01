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

        Assert.Equal("Description should be less or equal 10.000 characters long", exception.Message);
    } 

    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Activate() {
        var validData = new {
            Name = "category name",
            Description = "category description"
        };

        var category = new DomainEntity.Category(validData.Name, validData.Description, false);
        category.Activate();

        Assert.True(category.IsActive);
    }

    [Fact(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Deactivate() {
        var validData = new {
            Name = "category name",
            Description = "category description"
        };

        var category = new DomainEntity.Category(validData.Name, validData.Description, false);
        category.Deactivate();

        Assert.False(category.IsActive);
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "Category - Aggregates")]
    public void Update()
    {
        var category = new DomainEntity.Category("Category Name", "Category Description");
        var newValues = new { Name = "New Name", Description = "New Description" };

        category.Update(newValues.Name, newValues.Description);

        Assert.Equal(newValues.Name, category.Name);
        Assert.Equal(newValues.Description, category.Description);
    }

    [Fact(DisplayName = nameof(UpdateOnlyName))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateOnlyName()
    {
        var category = new DomainEntity.Category("Category Name", "Category Description");
        var newValues = new { Name = "New Name"};
        var currentDescription = category.Description;

        category.Update(newValues.Name);

        Assert.Equal(newValues.Name, category.Name);
        Assert.Equal(currentDescription, category.Description);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void UpdateErrorWhenNameIsEmpty(string? name)
    {
        var category = new DomainEntity.Category("Category Name", "Category Description");
        Action action =
            () => category.Update(name!);
        
        var exception = Assert.Throws<EntityValidationException>(action);
        
        Assert.Equal("Name should not be empty or null", exception.Message);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("1")]
    [InlineData("12")]
    [InlineData("a")]
    [InlineData("ca")]
    public void UpdateErrorWhenNameIsLessThan3Characters(string invalidName)
    {
        var category = new DomainEntity.Category("Category Name", "Category Description");
        Action action =
            () => category.Update(invalidName);
        
        var exception = Assert.Throws<EntityValidationException>(action);
        
        Assert.Equal("Name should be at leats 3 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenNameIsGreaterThan255Characters()
    {
        var category = new DomainEntity.Category("Category Name", "Category Description");
        var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
        
        Action action =
            () => category.Update(invalidName);
        
        var exception = Assert.Throws<EntityValidationException>(action);
        
        Assert.Equal("Name should be less or equal 255 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThan10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenDescriptionIsGreaterThan10_000Characters()
    {
        var category = new DomainEntity.Category("Category Name", "Category Description");
        var invalidDescription = String.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());
        
        Action action =
            () => category.Update("Category New Name", invalidDescription);
        
        var exception = Assert.Throws<EntityValidationException>(action);
        
        Assert.Equal("Description should be less or equal 10.000 characters long", exception.Message);
    }
}