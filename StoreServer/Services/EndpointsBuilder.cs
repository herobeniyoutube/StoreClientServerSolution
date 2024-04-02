using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using StoreServer.Entities;
using StoreServer.Models.EFCoreEntitiesCopies;
using System.Text.Json;
using System.Xml;

namespace StoreServer.Services
{
    /// <summary>
    /// Creates endpoints for the app
    /// </summary>
    public static class EndpointsBuilder
    {
        /// <summary>
        /// Creates endpoints for the app
        /// </summary>
        /// <param name="app"></param>
        public static void DefineEndpoints(this WebApplication app)
        {
            //Checking for admin rights
            app.MapGet("/admin", [Authorize(Roles = "admin")] async (HttpContext context) =>
            {
                return Results.Text("логин админа");
            });
            //registers new user, return jwt token to client
            app.MapPost("/register", async (User user, UsersService service) =>
            {
                var token = await service.RegisterAsync(user);
                if (token == null) return Results.BadRequest(StatusCodes.Status409Conflict);

                return Results.Json(token);
            });
            //user login, returns jwt token 
            app.MapPost("/login", async (User user, UsersService service) =>
            {
                var token = await service.LoginAsync(user);
                if (token is null) return Results.BadRequest(StatusCodes.Status404NotFound);
                return Results.Json(token);
            });
            app.MapGet("/logout", async (HttpContext context) =>
            {

                return Results.Text("Logged out");
            });

            //adds new user
            app.MapPost("/users", [Authorize] async (User user, UsersService service) =>
            {
                User newUser = await service.AddUserAsync(user);
                return Results.Json(newUser);
            });
            //gets all users
            app.MapGet("/users", [Authorize(Roles = "admin")] async (UsersService service) =>
            {
                var users = await service.GetUsersAsync();
                if (users == null) return Results.BadRequest(StatusCodes.Status404NotFound);
                return Results.Json(users);
            });
            //gets user
            app.MapGet("/users/{id:int}", [Authorize] async (int id, UsersService service) =>
            {
                User user = await service.GetUserAsync(id);
                if (user == null) return Results.BadRequest(StatusCodes.Status404NotFound);
                return Results.Json(user);
            });
            //deletes user 
            app.MapDelete("/users/{id:int}", [Authorize] async (int id, UsersService service) =>
            {
                var user = service.DeleteUserAsync(id);
                if (user == null) return Results.NotFound(new { message = "Пользователь не найден" });
                return Results.Json(user);
            });
            //changes user
            //поменять на users/id:int
            app.MapPut("/users", [Authorize] async (User user, UsersService service) =>
            {
                var test = service.ChangeUserAsync(user);
                if (test is null) return Results.BadRequest(StatusCodes.Status404NotFound);

                return Results.Json(user);
            });
            //gets products list
            app.MapGet("/products", [Authorize] async (ProductsService service) =>
            {
                var products = await service.GetProductsAsync();
                return products;
            });
            //adds new product
            app.MapPost("/products", [Authorize(Roles = "admin")] async (Product product, ProductsService service) =>
            {
                Product newProduct = await service.AddNewProductAsync(product);
                return Results.Json(product);
            });
            //creates new order
            app.MapPost("/users/{id:int}/orders", [Authorize] async (int id, Order order, OrdersService service) =>
            {
                var newOrder = await service.AddNewOrderAsync(id, order);
                return Results.Json(newOrder);
            });
            //gets all orders for exact user
            app.MapGet("/users/{id:int}/orders", [Authorize] async (int id, OrdersService service) =>
            {
                var orders = await service.GetAllOrdersAsync(id);
                return orders;
            });
            //deletes order
            app.MapDelete("/users/{id:int}/orders/{orderId:int}", [Authorize] async (int id, int orderId, OrdersService service) =>
            {
                var order = await service.DeleteOrderAsync(id, orderId);
                return Results.Json(order);
            });
            //updates existing order
            app.MapPut("/users/{id:int}/orders/{orderId:int}", [Authorize] async (int id, int orderId, Order order, OrdersService service) =>
            {
                var changedOrder = await service.ChangeOrderAsync(id, orderId, order);
                return Results.Json(order);
            });
            //gets all order positions 
            app.MapGet("/users/{id:int}/orders/{orderId:int}/position", [Authorize] async (int id, int orderId, OrdersService service) =>
            {
                var orderPositions = await service.GetOrderPositionsAsync(orderId);
                return Results.Json(orderPositions);
            });
            //deletes position from order
            app.MapDelete("/users/{id:int}/orders/{orderId:int}/position/{positionId:int}", [Authorize] async (int id, int orderId, int positionId, OrdersService service) =>
            {
                var position = await service.DeleteOrderPositionsAsync(id, orderId, positionId);
                return Results.Json(position);
            });
        }
    }
}
