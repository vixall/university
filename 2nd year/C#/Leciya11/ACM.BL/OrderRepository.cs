using System.Collections.Generic;

namespace CMS.BusinessLayer
{
    public class OrderRepository
    {
        public Order Retrieve(int orderId)
        {
            return new Order();
        }

        public List<Order> Retrieve()
        {
            return new List<Order>();
        }

        public bool Save(Order order)
        {
            return true;
        }
    }
}