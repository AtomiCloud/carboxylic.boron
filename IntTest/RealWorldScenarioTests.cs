using System;
using System.Threading.Tasks;
using CarboxylicBoron;
using FluentAssertions;
using Xunit;

namespace IntTest;

/// <summary>
/// This test class simulates real-world usage of the Boron domain problem framework
/// in the context of a simple application with business logic and validation.
/// </summary>
public class RealWorldScenarioTests
{
    #region Domain Problems

    // Application-specific domain problems
    private class ValidationProblem : IDomainProblem
    {
        public string Id => "validation-error";
        public string Title => "Validation Error";
        public string Detail { get; }
        public string Namespace => "app.validation";

        public ValidationProblem(string detail)
        {
            Detail = detail;
        }
    }

    private class NotFoundProblem : IDomainProblem
    {
        public string Id => "resource-not-found";
        public string Title => "Resource Not Found";
        public string Detail { get; }
        public string Namespace => "app.resources";

        public string ResourceType { get; }
        public string ResourceId { get; }

        public NotFoundProblem(string resourceType, string resourceId)
        {
            ResourceType = resourceType;
            ResourceId = resourceId;
            Detail = $"{resourceType} with id '{resourceId}' was not found.";
        }
    }

    private class AuthorizationProblem : IDomainProblem
    {
        public string Id => "authorization-error";
        public string Title => "Authorization Error";
        public string Detail => "You do not have permission to perform this action.";
        public string Namespace => "app.security";

        public string RequiredPermission { get; }

        public AuthorizationProblem(string requiredPermission)
        {
            RequiredPermission = requiredPermission;
        }
    }

    #endregion

    #region Service Layer (simulated)

    // Simulated model
    private class User
    {
        public string Id { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    // Simulated service
    private class UserService
    {
        public Task<User> GetUserAsync(string userId, string currentUserId)
        {
            // Simulate authorization check
            if (userId != currentUserId && !IsAdmin(currentUserId))
            {
                throw new AuthorizationProblem("ViewUser").ToException();
            }

            // Simulate not found
            if (userId == "non-existent")
            {
                throw new NotFoundProblem("User", userId).ToException();
            }

            return Task.FromResult(
                new User
                {
                    Id = userId,
                    Username = "testuser",
                    Email = "test@example.com",
                }
            );
        }

        public Task<User> CreateUserAsync(User user)
        {
            // Simulate validation
            if (string.IsNullOrEmpty(user.Username))
            {
                throw new ValidationProblem("Username is required").ToException();
            }

            if (string.IsNullOrEmpty(user.Email))
            {
                throw new ValidationProblem("Email is required").ToException();
            }

            if (!user.Email.Contains("@"))
            {
                throw new ValidationProblem("Email is invalid").ToException();
            }

            // Simulate successful creation
            user.Id = Guid.NewGuid().ToString();
            return Task.FromResult(user);
        }

        private bool IsAdmin(string userId)
        {
            return userId == "admin-user";
        }
    }

    #endregion

    private readonly UserService _userService = new();

    [Fact]
    public async Task UserService_GetUser_ShouldThrowNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        string nonExistentId = "non-existent";
        string currentUserId = "admin-user"; // Use admin to bypass auth checks

        // Act
        Func<Task> act = async () => await _userService.GetUserAsync(nonExistentId, currentUserId);

        // Assert
        var exception = await act.Should().ThrowAsync<DomainProblemException>();
        exception.Which.Problem.Should().BeAssignableTo<NotFoundProblem>();
        var problem = exception.Which.Problem as NotFoundProblem;

        problem!.ResourceType.Should().Be("User");
        problem.ResourceId.Should().Be(nonExistentId);
        problem.Id.Should().Be("resource-not-found");
    }

    [Fact]
    public async Task UserService_GetUser_ShouldThrowAuthorizationProblem_WhenNotAuthorized()
    {
        // Arrange
        string userId = "other-user";
        string currentUserId = "regular-user"; // Not admin, not same user

        // Act
        Func<Task> act = async () => await _userService.GetUserAsync(userId, currentUserId);

        // Assert
        var exception = await act.Should().ThrowAsync<DomainProblemException>();
        exception.Which.Problem.Should().BeAssignableTo<AuthorizationProblem>();
        var problem = exception.Which.Problem as AuthorizationProblem;

        problem!.RequiredPermission.Should().Be("ViewUser");
        problem.Title.Should().Be("Authorization Error");
    }

    [Fact]
    public async Task UserService_CreateUser_ShouldThrowValidationProblem_WithInvalidData()
    {
        // Arrange
        var invalidUser = new User { Username = "testuser", Email = "invalid-email" };

        // Act
        Func<Task> act = async () => await _userService.CreateUserAsync(invalidUser);

        // Assert
        var exception = await act.Should().ThrowAsync<DomainProblemException>();
        exception.Which.Problem.Should().BeAssignableTo<ValidationProblem>();
        var problem = exception.Which.Problem as ValidationProblem;

        problem!.Detail.Should().Be("Email is invalid");
        problem.Title.Should().Be("Validation Error");
    }

    [Fact]
    public async Task UserService_CreateUser_ShouldSucceed_WithValidData()
    {
        // Arrange
        var validUser = new User { Username = "testuser", Email = "valid@example.com" };

        // Act
        var createdUser = await _userService.CreateUserAsync(validUser);

        // Assert
        createdUser.Should().NotBeNull();
        createdUser.Id.Should().NotBeEmpty();
        createdUser.Username.Should().Be("testuser");
        createdUser.Email.Should().Be("valid@example.com");
    }

    [Fact]
    public void TryCatchPattern_ShouldHandleDifferentProblemTypes()
    {
        // Arrange
        var problems = new IDomainProblem[]
        {
            new ValidationProblem("Test validation error"),
            new NotFoundProblem("User", "123"),
            new AuthorizationProblem("ViewUser"),
        };

        foreach (var problem in problems)
        {
            // Act
            try
            {
                throw problem.ToException();
            }
            catch (DomainProblemException ex) when (ex.Problem is ValidationProblem)
            {
                // Assert
                ex.Problem.Should().BeOfType<ValidationProblem>();
                ex.Problem.Title.Should().Be("Validation Error");
            }
            catch (DomainProblemException ex) when (ex.Problem is NotFoundProblem)
            {
                // Assert
                ex.Problem.Should().BeOfType<NotFoundProblem>();
                ex.Problem.Title.Should().Be("Resource Not Found");
            }
            catch (DomainProblemException ex) when (ex.Problem is AuthorizationProblem)
            {
                // Assert
                ex.Problem.Should().BeOfType<AuthorizationProblem>();
                ex.Problem.Title.Should().Be("Authorization Error");
            }
        }
    }
}
