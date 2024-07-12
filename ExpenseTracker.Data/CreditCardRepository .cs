using ExpenseTracker.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Data
{
    public class CreditCardRepository : ICreditCardRepository
    {
        private readonly ExpenseTrackerDbContext _context;

        public CreditCardRepository(ExpenseTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CreditCard>> GetCreditCardsByUserIdAsync(int userId)
        {
            return await _context.CreditCards
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }

        public async Task<CreditCard> GetCreditCardByIdAsync(int creditCardId)
        {
            return await _context.CreditCards.FindAsync(creditCardId);
        }

        public async Task AddCreditCardAsync(CreditCard creditCard)
        {
            _context.CreditCards.Add(creditCard);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCreditCardAsync(CreditCard creditCard)
        {
            _context.Entry(creditCard).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCreditCardAsync(int creditCardId)
        {
            var creditCard = await _context.CreditCards.FindAsync(creditCardId);
            if (creditCard != null)
            {
                _context.CreditCards.Remove(creditCard);
                await _context.SaveChangesAsync();
            }
        }
    }


}
