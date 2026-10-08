using System.Collections.Generic;

namespace CMS.BusinessLayer
{
    public class ProductRepository
    {
        public Product Retrieve(int productId)
        {
            return new Product();
        }

        public List<Product> Retrieve()
        {
            return new List<Product>();
        }

        public bool Save(Product product)
        {
            return true;
        }
    }
}