using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SIPVS.litedb;
using AddItemsDynamically.Models;

namespace SIPVS.Data
{

    public interface IOrderItemService
    {
        bool Delete(int id);
        IEnumerable<OrderItem> FindAll();
        OrderItem FindOne(int id);
        int Insert(OrderItem item);
        bool Update(OrderItem item);


    }

    public class OrderItemService : IOrderItemService
    {

        private LiteDatabase _liteDb;

        public OrderItemService(ILiteDbContext liteDbContext)
        {
            _liteDb = liteDbContext.Database;
        }

        public IEnumerable<OrderItem> FindAll()
        {
            var result = _liteDb.GetCollection<OrderItem>("OrderItem")
                .FindAll();
            return result;
        }

        public OrderItem FindOne(int id)
        {
            return _liteDb.GetCollection<OrderItem>("OrderItem")
                .Find(x => x.Id == id).FirstOrDefault();
        }

        public int Insert(OrderItem item)
        {
            return _liteDb.GetCollection<OrderItem>("OrderItem")
                .Insert(item);
        }

        public bool Update(OrderItem item)
        {
            return _liteDb.GetCollection<OrderItem>("OrderItem")
                .Update(item);
        }

        public bool Delete(int id)
        {
            var to_delete = _liteDb.GetCollection<OrderItem>("OrderItem");
            return to_delete.Delete(id);
        }

    }

}