using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StoreServer.Entities;
using StoreServer.Models;
using StoreServer.Services;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

string connection = "Server=localhost;Database=StoreDB;Trusted_Connection=True;TrustServerCertificate=True;";
builder.Services.AddDbContext<StoreContext>(options => options.UseSqlServer(connection));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = AuthentificationOptions.ISSUER,
        ValidateAudience = true,
        ValidAudience = AuthentificationOptions.AUDIENCE,
        ValidateLifetime = true,
        IssuerSigningKey = AuthentificationOptions.GetSymmetricSecurityKey(),
        ValidateIssuerSigningKey = true
    };
});
builder.Services.AddTransient<JWTTokenConstructor>();

builder.Services.AddAuthorization();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseDefaultFiles();

app.UseStaticFiles();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Checking for admin rights
app.MapGet("/admin", [Authorize(Roles = "admin")] async (HttpContext context) =>
{
    return Results.Text("логин админа");
});
//registers new user, return jwt token to client
app.MapPost("/register", async (UserEntity user, StoreContext db, JWTTokenConstructor constructor) =>
{
    bool userExists = await db.Users.AnyAsync(x => x.UserLogin == user.UserLogin);
    if (userExists) return Results.Conflict(StatusCodes.Status409Conflict);

    await db.Users.AddAsync(user);
    await db.SaveChangesAsync();


    return Results.Json(constructor.GetToken(user));
});
//user login, returns jwt token 
app.MapPost("/login", async (UserEntity user, StoreContext db, JWTTokenConstructor constructor) =>
{
    string? login = user.UserLogin;
    string? password = user.Password;

    var userEntity = await db.Users.FirstOrDefaultAsync(u => u.UserLogin == user.UserLogin);
    if (userEntity is null) return Results.BadRequest(StatusCodes.Status404NotFound);
    if (password != db.Users.FirstOrDefaultAsync(x => x.UserLogin == login).Result.Password) return Results.BadRequest(StatusCodes.Status406NotAcceptable);


    return Results.Json(constructor.GetToken(userEntity));

});
app.MapGet("/logout", async (HttpContext context) =>
{
     
    return Results.Text("Logged out");
});

//adds new user
app.MapPost("/users", [Authorize] async (UserEntity user, StoreContext db) =>
{

    await db.Users.AddAsync(user);
    await db.SaveChangesAsync();
    return user;
});
//gets all users
app.MapGet("/users", [Authorize(Roles = "admin")] async (StoreContext db) =>
{
    return await db.Users.ToListAsync();
});
//gets user
app.MapGet("/users/{id:int}", [Authorize] async (int id, StoreContext db) =>
{
    UserEntity user = await db.Users.FirstAsync(x => x.Id == id);
    return Results.Json(user);
});
//deletes user 
app.MapDelete("/users/{id:int}", [Authorize] async (int id, StoreContext db) =>
{
    var user = await db.Users.FirstAsync(x => x.Id == id);

    if (user == null) return Results.NotFound(new { message = "Пользователь не найден" });
    if (user.UserLogin == "admin") return Results.BadRequest(new { message = "Нельзя удалить админа" });

    db.Users.Remove(user);
    await db.SaveChangesAsync();
    return Results.Json(user);
});
//changes user
app.MapPut("/users", async (UserEntity updatedUser, StoreContext db) =>
{
    var user = await db.Users.FirstAsync(x => x.Id == updatedUser.Id);
    if (user == null)
    {
        await db.Users.AddAsync(updatedUser);
        user = updatedUser;
        await db.SaveChangesAsync();
        return Results.Json(user);
    }
    else
    {
        user.UserName = updatedUser.UserName;
        user.UserLogin = updatedUser.UserLogin;
        await db.SaveChangesAsync();
        return Results.Json(user);
    }

});
//gets products list
app.MapGet("/products", async (StoreContext db) =>
{
    var list = await db.Products.ToListAsync();
    return Results.Json(list);
});
//adds new product
app.MapPost("/products", [Authorize(Roles = "admin")] async (ProductEntity product, StoreContext db) =>
{
    await db.Products.AddAsync(product);
    await db.SaveChangesAsync();
    return Results.Json(product);
});
//creates new order
app.MapPost("/users/{id:int}/orders", async (int id, OrderEntity order, StoreContext db) =>
{

    order.UserId = id;
    await db.Orders.AddAsync(order);
    await db.SaveChangesAsync();

    return Results.Json(order);
});
//gets user's list which contains his orders
app.MapGet("/users/{id:int}/orders", [Authorize] async (int id, StoreContext db) =>
{
    var orders = await db.Orders.Where(x => x.UserId == id).ToListAsync();
    return Results.Json(orders);
});
//deletes order
app.MapDelete("/users/{id:int}/orders/{orderId:int}", async (int id, int orderId, StoreContext db) =>
{
    var orders = await db.Orders.Where(x => x.UserId == id).ToListAsync();
    if (orders == null) return Results.NotFound(new { message = "У клиента нет активных заказов" });

    bool clientOwnsOrder = orders.Any(x => x.Id == orderId);
    if (!clientOwnsOrder) return Results.NotFound(new { message = "У клиента нет такого заказа" });

    OrderEntity order = orders.FirstOrDefault(x => x.Id == orderId);

    db.Orders.Remove(order);
    await db.SaveChangesAsync();
    return Results.Json(order);
});
 //updates existing order
app.MapPut("/users/{id:int}/orders/{orderId:int}", async (int id, int orderId, OrderEntity newOrder, StoreContext db) =>
{
    var order = db.Orders.First(x => x.Id == orderId);
    order.OrderPrice = newOrder.OrderPrice;
    order.OrderPosition = newOrder.OrderPosition;
    await db.SaveChangesAsync();

    return Results.Json(order);

});
//gets order positions 
app.MapGet("/users/{id:int}/orders/{orderId:int}/position", async (int id, int orderId, StoreContext db) =>
{

    var orderPositions = await db.OrderPosition.Where(x => x.OrderId == orderId).ToListAsync();
    return Results.Json(orderPositions);
});
//deletes position from order
app.MapDelete("/users/{id:int}/orders/{orderId:int}/position/{positionId:int}", async (int id, int orderId, int positionId, StoreContext db) =>
{
    var orders = await db.Orders.Where(x => x.UserId == id).ToListAsync();
    if (orders is null) return Results.NotFound(new { message = "У клиента нет активных заказов" });

    var orderPositions = db.OrderPosition.Where(x => x.OrderId == orderId);
    if (orderPositions is null) return Results.NotFound(new { message = "В заказе нет позиций" });

    OrderPositionEntity position = orderPositions.FirstOrDefault(x => x.Id == positionId);

    db.OrderPosition.Remove(position);
    await db.SaveChangesAsync();
    return Results.Json(position);
});

 




app.UseHttpsRedirection();

app.MapControllers();

app.Run();

