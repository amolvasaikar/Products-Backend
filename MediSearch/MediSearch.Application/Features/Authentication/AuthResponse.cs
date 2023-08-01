using FluentValidation;
using MediatR;
using MediSearch.Persistence.IRepositories;
using Microsoft.AspNetCore.Identity;

namespace MediSearch.Application.Features.Authentication
{
    public class AuthResponse
    {
        public string Token { get; set; }
    }

    public class AuthRequest : IRequest<AuthResponse>
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }

    public class AuthRequestValidator : AbstractValidator<AuthRequest>
    {
        public AuthRequestValidator()
        {
            RuleFor(s => s.UserName).NotEmpty();
            RuleFor(s => s.Password).NotEmpty();
        }
    }

    public class AuthHandler : IRequestHandler<AuthRequest, AuthResponse>
    {
        readonly IAccountManager _accountManager;
        readonly PasswordHasher<string> password;
        public AuthHandler(IAccountManager accountManager, PasswordHasher<string> password)

        {
            this.password = password;
            this._accountManager = accountManager;
        }

        public async Task<AuthResponse> Handle(AuthRequest request, CancellationToken cancellationToken)
        {
            var applicationuser = await _accountManager.GetUserByUserNameAsync(request.UserName);

            if (applicationuser == null)
                return null;



            return new AuthResponse { };
            throw new NotImplementedException();
        }
    }
}
