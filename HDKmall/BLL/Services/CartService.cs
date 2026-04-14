using HDKmall.BLL.Interfaces;
using HDKmall.DAL.Interfaces;
using HDKmall.Models;

namespace HDKmall.BLL.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public CartService(ICartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public ShoppingCart GetOrCreateCart(int? userId, string sessionId)
        {
            ShoppingCart cart = null;

            if (userId.HasValue)
            {
                cart = _cartRepository.GetCartByUserId(userId.Value);
                if (cart == null)
                {
                    cart = new ShoppingCart { UserId = userId.Value };
                    _cartRepository.Add(cart);
                    _cartRepository.SaveChanges();
                }
            }
            else if (!string.IsNullOrEmpty(sessionId))
            {
                cart = _cartRepository.GetCartBySessionId(sessionId);
                if (cart == null)
                {
                    cart = new ShoppingCart { SessionId = sessionId };
                    _cartRepository.Add(cart);
                    _cartRepository.SaveChanges();
                }
            }

            return cart;
        }

        public void AddToCart(int cartId, int productId, int variantId, int quantity)
        {
            var cart = _cartRepository.GetCartById(cartId);
            if (cart == null) return;

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Items.Add(new CartItem { ProductId = productId, Quantity = quantity });
            }

            _cartRepository.Update(cart);
            _cartRepository.SaveChanges();
        }

        public void UpdateCartItem(int cartItemId, int quantity)
        {
            // This would need CartItem repository or context access
            // For now, this is a simplified version
        }

        public void RemoveFromCart(int cartItemId)
        {
            // This would need CartItem repository or context access
        }

        public void ClearCart(int cartId)
        {
            var cart = _cartRepository.GetCartById(cartId);
            if (cart != null)
            {
                cart.Items.Clear();
                _cartRepository.Update(cart);
                _cartRepository.SaveChanges();
            }
        }

        public ShoppingCart GetCartById(int cartId)
        {
            return _cartRepository.GetCartById(cartId);
        }
    }
}
