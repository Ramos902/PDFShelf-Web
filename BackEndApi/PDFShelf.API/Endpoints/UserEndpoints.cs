using Microsoft.EntityFrameworkCore;
using PDFShelf.Api.Data;
using PDFShelf.Api.Models;
using PDFShelf.Api.Services;
using PDFShelf.Api.DTOs;

namespace PDFShelf.Api.Endpoints
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/users").WithTags("Users");

            group.MapPost("/register", async (AppDbContext db, PasswordHasher hasher, UserRegisterDto request) =>
            {
                if (await db.Users.AnyAsync(u => u.Email == request.Email))
                    return Results.BadRequest("E-mail já cadastrado.");

                var user = new User
                {
                    Name = request.Name,
                    Email = request.Email,
                    PasswordHash = hasher.Hash(request.Password)
                };

                db.Users.Add(user);
                await db.SaveChangesAsync();

                return Results.Created($"/api/users/{user.Id}", new { user.Id, user.Name, user.Email });
            })
            .WithSummary("Registra um novo usuário")
            .WithDescription("Cria um novo usuário no sistema com nome, e-mail e senha.")
            .WithOpenApi();

            group.MapPost("/login", async (AppDbContext db, PasswordHasher hasher, TokenService tokenService, UserLoginDto request) =>
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
                if (user is null)
                {
                    return Results.BadRequest("Usuário não encontrado.");
                }

                if (!hasher.VerifyPassword(request.Password, user.PasswordHash))
                {
                    return Results.BadRequest("Senha incorreta.");
                }

                var token = tokenService.Generate(user);

                return Results.Ok(new
                {
                    message = "Login realizado com sucesso.",
                    token,
                    user = new { user.Id, user.Name, user.Email, user.Role }
                });
            })
            .WithSummary("Realiza login de usuário")
            .WithDescription("Verifica o e-mail e senha, e retorna um token JWT de autenticação.")
            .WithOpenApi();

            group.MapPut("/update/{id}", async (Guid id, AppDbContext db, PasswordHasher hasher, TokenService tokenService, HttpContext http, UserUpdateDto request) =>
            {
                var token = http.Request.Headers["Authorization"]
                    .ToString()
                    .Replace("Bearer ", "");

                if (string.IsNullOrWhiteSpace(token))
                    return Results.Unauthorized();

                var userIdFromToken = tokenService.GetUserIdFromToken(token);
                var userRoleFromToken = tokenService.GetUserRoleFromToken(token);

                var userToUpdate = await db.Users.FindAsync(id);
                if (userToUpdate == null)
                    return Results.NotFound("Usuário não encontrado.");

                var isOwner = userToUpdate.Id == userIdFromToken;
                var isAdmin = userRoleFromToken == "Admin";

                if (!isOwner && !isAdmin)
                    return Results.Unauthorized();

                if (!string.IsNullOrWhiteSpace(request.Name))
                    userToUpdate.Name = request.Name;

                if (!string.IsNullOrWhiteSpace(request.Email))
                    userToUpdate.Email = request.Email;

                if (!string.IsNullOrWhiteSpace(request.Password))
                    userToUpdate.PasswordHash = hasher.Hash(request.Password);

                await db.SaveChangesAsync();

                return Results.Ok("Usuário atualizado com sucesso!");
            });
        }
    }
}