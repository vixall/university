using System;

namespace CMS.BusinessLayer
{
    public class OrderItem
    {
        public OrderItem() { }

        public OrderItem(int orderItemId)
        {
            this.OrderItemId = orderItemId;
        }

        public int OrderItemId { get; private set; }
        public int Quantity { get; set; }
        public Product Product { get; set; }
        public decimal? PurchasePrice { get; set; }

        public OrderItem Retrieve(int orderItemId)
        {
            return new OrderItem();
        }

        public bool Save()
        {
            return true;
        }

        public bool Validate()
        {
            var isValid = true;
            if (Quantity <= 0) isValid = false;
            if (Product == null) isValid = false;
            if (PurchasePrice == null) isValid = false;
            return isValid;
        }
    }
}