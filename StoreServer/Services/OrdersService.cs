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
            var newOrderPositions = from p in order.Positions
                                    select new OrderPositionEntity()
                                    {
                                        OrderId = p.OrderId,
                                        ProductId = p.ProductId,
                                        ProductQuantity = p.ProductQuantity
                                    };

            OrderEntity entity = new OrderEntity()
            {
                UserId = order.UserId,
                Price = order.Price,
                Date = order.Date,
                Positions = newOrderPositions.ToList(),
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
            var orders = await db.Orders.Include(x => x.Positions).ThenInclude(x => x.Product).Where(x => x.UserId == id).ToListAsync();
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
            var order = (from o in db.Orders.Include(x => x.Positions).ThenInclude(x => x.Product).ToList() where o.UserId == id && o.Id == orderId select o).First();
            if (order is null) return null;

            Order deletedOrder = new Order()
            {
                UserId = order.UserId,
                Price = order.Price,
                Date = order.Date,
                Positions = (from p in order.Positions
                                 select new OrderPosition()
                                 {
                                     OrderId = p.OrderId,
                                     ProductId = p.ProductId,
                                     ProductQuantity = p.ProductQuantity
                                 }).ToList()

            };

            db.Orders.Remove(order);
            await db.SaveChangesAsync();

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
            var order = await db.Orders.Include(x => x.Positions).ThenInclude(x => x.Product).FirstOrDefaultAsync(x => x.Id == orderId);

            order.Price = changedOrder.Price;

            order.Positions = (from p in changedOrder.Positions
                                   select new OrderPositionEntity()
                                   {
                                       OrderId = p.OrderId,
                                       ProductId = p.ProductId,
                                       ProductQuantity = p.ProductQuantity
                                   }).ToList();

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
            var orderPositions = await db.OrderPositions.Where(x => x.OrderId == orderId).ToListAsync();
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
            var orderPosition = (from p in db.OrderPositions.ToList()
                                 where p.OrderId == orderId && p.Id == positionId
                                 select p).First();
            if (orderPosition == null) return null;

            db.OrderPositions.Remove(orderPosition);
            await db.SaveChangesAsync();

            OrderPosition deletedPosition = new OrderPosition()
            {
                OrderId =  orderPosition.OrderId,
                ProductId = orderPosition.ProductId,
                ProductQuantity = orderPosition.ProductQuantity
            };
            return deletedPosition;
        }
    }
}
