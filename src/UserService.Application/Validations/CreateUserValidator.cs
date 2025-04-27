using FluentValidation;
using Microsoft.AspNetCore.Identity;
using SharedLibrary.Dtos.Users;
using UserService.Domains.Entities;

namespace UserService.Application.Validations;

public class CreateUserValidator : AbstractValidator<CreateUserDto>
{
    private readonly UserManager<User> userManager;
    public CreateUserValidator(UserManager<User> userManager)
    {
        this.userManager = userManager;
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.")
                             .EmailAddress().WithMessage("Invalid email format.")
                             .MustAsync(BeUniqueEmail).WithMessage("Email is already taken.");
    }
    private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(email)) return false;
        var user = await userManager.FindByEmailAsync(email);
        return user == null;
    }
}
