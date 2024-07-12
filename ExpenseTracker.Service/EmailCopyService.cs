using AutoMapper;
using ExpenseTracker.Core.Entities;
using ExpenseTracker.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Service
{
    public interface IEmailCopyService
    {
        Task SaveEmailCopyAsync(EmailCopy emailCopyModel);
    }
    public class EmailCopyService : IEmailCopyService
    {
        private readonly IEmailCopyRepository _emailCopyRepository;
        private readonly IMapper _mapper;

        public EmailCopyService(IEmailCopyRepository emailCopyRepository, IMapper mapper)
        {
            _emailCopyRepository = emailCopyRepository;
            _mapper = mapper;
        }

        public async Task SaveEmailCopyAsync(EmailCopy emailCopyModel)
        {            
            await _emailCopyRepository.AddEmailCopyAsync(emailCopyModel);
        }
    }

}
