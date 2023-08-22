namespace pizzaweb.Models
{
    public class ShopingCart
    {
        public List<ShopingCartItem> Items  { get; set; }

        public ShopingCart() 
        {
            this.Items = new List<ShopingCartItem>();
        }

        public void AddToCartShopingCartItem(ShopingCartItem item, int Quantity)
        {
            var checkExists = Items.FirstOrDefault(x => x.ProductId == item.ProductId);
            if (checkExists != null) 
            {
                checkExists.Quantity += Quantity;
                checkExists.TotalPrice = checkExists.Price * checkExists.Quantity;
            }
            else
            {
                Items.Add(item);
            }
        }

        public void Remove(int id)
        {
            var checkExits = Items.SingleOrDefault(x => x.ProductId ==  id);
            if (checkExits != null)
            {
                Items.Remove(checkExits);

            }
        }

        public void UpldateQuantity(int id, int quantity)
        {
            var checkExits = Items.SingleOrDefault(x => x.ProductId == id);
            if (checkExits != null)
            {
                checkExits.Quantity = quantity;
                checkExits.TotalPrice = checkExits.Price * checkExits.Quantity;

            }
        }

        public decimal GetTotal()
        {
            return Items.Sum(x => x.TotalPrice);
        }

        public decimal GetTotalQuantity()
        {
            return Items.Sum(x => x.Quantity);
        }

        public void ClearCart()
        {
            Items.Clear();
        }
    }

    public class ShopingCartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public string ProductImg { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
