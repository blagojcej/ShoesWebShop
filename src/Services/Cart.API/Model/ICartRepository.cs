using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cart.API.Model
{
    public interface ICartRepository
    {
        Task<Cart> GetCartAsync(string cardId);
        IEnumerable<string> GetUsers();
        Task<Cart> UpdateCartAsync(Cart basket);
        Task<bool> DeleteCartAsync(string id);
    }
}