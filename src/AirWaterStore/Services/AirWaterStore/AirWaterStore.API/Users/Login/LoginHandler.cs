using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AirWaterStore.API.Users.Login;

public record LoginQuery(
    string Email,
    string Password
    ) : IQuery<LoginResult>;

public record LoginResult(string Token);

internal class LoginHandler(
    UserManager<User> userManager,
    IConfiguration config
    ) : IQueryHandler<LoginQuery, LoginResult>
{
    public async Task<LoginResult> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(query.Email);
        if (user is null)
            throw new BadRequestException("Invalid email.");

        var passwordValid = await userManager.CheckPasswordAsync(user, query.Password);
        if (!passwordValid)
            throw new BadRequestException("Invalid password.");

        var roles = await userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(AppConst.UserIdClaim, user.Id.ToString()),
            new Claim(AppConst.RoleClaim, roles[0]!),
            new Claim(AppConst.UserNameClaim, user.UserName!),
            new Claim(AppConst.IsBanClaim, user.IsBan.ToString()!)
        };

        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return new LoginResult(tokenString);
    }
}
