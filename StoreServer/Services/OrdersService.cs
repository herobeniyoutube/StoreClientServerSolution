using Microsoft.EntityFrameworkCore;
using StoreServer.Entities;
using StoreServer.Models.EFCoreEntitiesCopies;
using System.Runtime.Serialization.Formatters;

namespace StoreServer.Services
{
    /// <summary>
    /// Service provides various tools for managing all Order-related including Order positions
    /// </summary>
    public class OrdersService
    {
        StoreContext db;
        public OrdersService(StoreContext db)
        {
            this.db = db;
        }
        /// <summary>
        /// Creates new order for user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public async Task<Order> AddNewOrderAsync(int id, Order order)
        {
            order.UserId = id;
            var newOrderPositions = from p in order.OrderPosition
                                    select new OrderPositionEntity()
                                    {
                                        OrderId = p.OrderId,
                                        ProductId = p.ProductId,
                                        ProductQuantity = p.ProductQuantity
                                    };

            OrderEntity entity = new OrderEntity()
            {
                UserId = order.UserId,
                OrderPrice = order.OrderPrice,
                OrderDate = order.OrderDate,
                OrderPosition = newOrderPositions.ToList(),
            };

            await db.Orders.AddAsync(entity);
            await db.SaveChangesAsync();
            return order;
        }
        /// <summary>
        /// Gets all orders of user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<OrderEntity>> GetAllOrdersAsync(int id)
        {
            var orders = await db.Orders.Where(x => x.UserId == id).ToListAsync();
            return orders;
        }
        /// <summary>
        /// Deletes exact order
        /// </summary>
        /// <param name="id"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<Order> DeleteOrderAsync(int id, int orderId)
        {
            var orders = (from o in db.Orders.ToList() where o.UserId == id && o.Id == orderId select o);
            if (orders is null) return null;

            OrderEntity order = orders.First();
            db.Orders.Remove(order);
            await db.SaveChangesAsync();

            Order deletedOrder = new Order()
            {
                UserId = order.UserId,
                OrderPrice = order.OrderPrice,
                OrderDate = order.OrderDate
            };

            return deletedOrder;
        }
        /// <summary>
        /// Updates order in the system
        /// </summary>
        /// <param name="id"></param>
        /// <param name="orderId"></param>
        /// <param name="changedOrder"></param>
        /// <returns></returns>
        public async Task<Order> ChangeOrderAsync(int id, int orderId, Order changedOrder)
        {
            var order = await db.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
            order.OrderPrice = changedOrder.OrderPrice;
            var changedOrderPositions = from p in changedOrder.OrderPosition
                                        select new OrderPositionEntity()
                                        {
                                            OrderId = p.OrderId,
                                            ProductId = p.ProductId,
                                            ProductQuantity = p.ProductQuantity
                                        };

            order.OrderPosition = changedOrderPositions.ToList();

            await db.SaveChangesAsync();
            return changedOrder;
        }
        /// <summary>
        /// Gets all positions for exact order
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<List<OrderPositionEntity>> GetOrderPositionsAsync(int orderId)
        {
            var orderPositions = await db.OrderPosition.Where(x => x.OrderId == orderId).ToListAsync();
            return orderPositions;
        }
        /// <summary>
        /// Deletes position from th system
        /// </summary>
        /// <param name="id"></param>
        /// <param name="orderId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        public async Task<OrderPosition> DeleteOrderPositionsAsync(int id, int orderId, int positionId)
        {
            var orderPosition = (from p in db.OrderPosition.ToList()
                                 where p.OrderId == orderId && p.Id == positionId
                                 select p).First();
            if (orderPosition == null) return null;

            db.OrderPosition.Remove(orderPosition);
            await db.SaveChangesAsync();

            OrderPosition deletedPosition = new OrderPosition() 
            { 
                OrderId = orderPosition.OrderId, 
                ProductId = orderPosition.ProductId, 
                ProductQuantity = orderPosition.ProductQuantity 
            }; 
            return deletedPosition;
        }
    }
}
