using ExpenseTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Data
{
    public interface IEmailCopyRepository
    {
        Task AddEmailCopyAsync(EmailCopy emailCopy);
    }
    public class EmailCopyRepository : IEmailCopyRepository
    {
        private readonly ExpenseTrackerDbContext _context;

        public EmailCopyRepository(ExpenseTrackerDbContext context)
        {
            _context = context;
        }

        public async Task AddEmailCopyAsync(EmailCopy emailCopy)
        {
            _context.EmailCopies.Add(emailCopy);
            await _context.SaveChangesAsync();
        }
    }


}
