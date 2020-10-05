using System;
using System.Collections.Generic;

namespace AddItemsDynamically.Models
{
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.my.example.com/xml/order")]
        [System.Xml.Serialization.XmlRoot(Namespace = "http://www.my.example.com/xml/order")]        
        public class Order
        {
        [System.Xml.Serialization.XmlAttribute]
        public int Id { get; set; }

        [System.Xml.Serialization.XmlElementAttribute]
        public string Name { get; set; }
        public bool IsUrgent { get; set; }
        public List<OrderItem> Items { get; set; }
        public DateTime Created { get; set; }

        //This should be in ViewModel
        public int NumberOfItems
        {
            get => Items.Count;
        }

        public Order()
        {
            Items = new List<OrderItem>();
        }
    }
}
