using ExpenseTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Data
{
    public interface ICreditCardRepository
    {
        Task<IEnumerable<CreditCard>> GetCreditCardsByUserIdAsync(int userId);
        Task<CreditCard> GetCreditCardByIdAsync(int creditCardId);
        Task AddCreditCardAsync(CreditCard creditCard);
        Task UpdateCreditCardAsync(CreditCard creditCard);
        Task DeleteCreditCardAsync(int creditCardId);
    }


}
