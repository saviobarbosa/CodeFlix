using FC.CodeFlix.Catalog.Application.Interfaces;
using FC.CodeFlix.Catalog.Domain.Entity;
using FC.CodeFlix.Catalog.Domain.Repository;
using Moq;
using Xunit;
using UseCases = FC.CodeFlix.Catalog.Application.UseCases.CreateCategory;

namespace FC.CodeFlix.Catalog.UnitTests.Application.CreateCategory;

public class CreateCategoryTest {
    
    [Fact(DisplayName = nameof(CreateCategory))]
    [Trait("Apllication", "CreateCategory - Use cases")]
    public async void CreateCategory() {
        var repositoryMock = new Mock<ICategoryRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var useCase = new CreateCategory(
            repositoryMock.Object, 
            unitOfWorkMock.Object);
        var input = new CreateCategoryInput(
            "Category Name",
            "Category Description",
            true
        );

        var output = await useCase.Handle(input, CancellationToken.None);

        repositoryMock.Verify(
            repository => repository.Insert(
                It.IsAny<Category>(),
                It.IsAny<CancellationToken>()
            ),
            Times.Once
        );
        unitOfWorkMock.Verify(
            uow => uow.Commit(It.IsAny<CancellationToken>()), 
            Times.Once
        );
        output.ShouldNotBeNull();
        output.Name.Should().Be("Category Name");
        output.Description.Should().Be("Category Description");
        output.IsActive.Should().Be(true);
        (output.Id != null && output.Id != Guid.Empty).Should().BeTrue();
        (output.CreatedAt != null && output.CreatedAt != default(DateTime)).Should().BeTrue();
    }
}