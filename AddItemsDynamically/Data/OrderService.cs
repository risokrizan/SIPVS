using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SIPVS.litedb;
using AddItemsDynamically.Models;

namespace SIPVS.Data
{

    public interface IOrderService
    {
        bool Delete(int id);
        IEnumerable<Order> FindAll();
        Order FindOne(int id);
        int Insert(Order forecast);
        bool Update(Order forecast);


    }

    public class OrderService : IOrderService
    {

        private LiteDatabase _liteDb;

        public OrderService(ILiteDbContext liteDbContext)
        {
            _liteDb = liteDbContext.Database;
        }

        public IEnumerable<Order> FindAll()
        {
            var result = _liteDb.GetCollection<Order>("Order")
                .FindAll();
            return result;
        }

        public Order FindOne(int id)
        {
            return _liteDb.GetCollection<Order>("Order")
                .Find(x => x.Id == id).FirstOrDefault();
        }

        public int Insert(Order forecast)
        {
            return _liteDb.GetCollection<Order>("Order")
                .Insert(forecast);
        }

        public bool Update(Order forecast)
        {
            return _liteDb.GetCollection<Order>("Order")
                .Update(forecast);
        }

        public bool Delete(int id)
        {
            var to_delete = _liteDb.GetCollection<Order>("Order");
            return to_delete.Delete(id);
        }

        // public IEnumerable<Order> GetAllPatientDocum(int PatientId)
        // {

        //     return _liteDb.GetCollection<Order>("Order")
        //         .Find(x => x.Patient.Id == PatientId);
        // }

        // public IEnumerable<Order> GetAllDoctorDocum(int DoctorId){

        //     IEnumerable<Order> arr = _liteDb.GetCollection<Order>("Order")
        //         .Find(x => x.Doctor. );

        // }



    }

}