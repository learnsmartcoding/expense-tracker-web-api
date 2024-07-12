using AutoMapper;
using ExpenseTracker.Core.Entities;
using ExpenseTracker.Core.Models;
using ExpenseTracker.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Service
{
    public interface ICreditCardService
    {
        Task<IEnumerable<CreditCardModel>> GetCreditCardsByUserIdAsync(int userId);
        Task<CreditCardModel> GetCreditCardByIdAsync(int creditCardId);
        Task AddCreditCardAsync(CreditCardModel creditCard);
        Task UpdateCreditCardAsync(CreditCardModel creditCard);
        Task DeleteCreditCardAsync(int creditCardId);
    }
    public class CreditCardService : ICreditCardService
    {
        private readonly ICreditCardRepository _creditCardRepository;
        private readonly IMapper _mapper;

        public CreditCardService(ICreditCardRepository creditCardRepository, IMapper mapper)
        {
            _creditCardRepository = creditCardRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CreditCardModel>> GetCreditCardsByUserIdAsync(int userId)
        {
            var creditCards = await _creditCardRepository.GetCreditCardsByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<CreditCardModel>>(creditCards);
        }

        public async Task<CreditCardModel> GetCreditCardByIdAsync(int creditCardId)
        {
            var creditCard = await _creditCardRepository.GetCreditCardByIdAsync(creditCardId);
            return _mapper.Map<CreditCardModel>(creditCard);
        }

        public async Task AddCreditCardAsync(CreditCardModel creditCard)
        {
            var creditCardEntity = _mapper.Map<CreditCard>(creditCard);
            await _creditCardRepository.AddCreditCardAsync(creditCardEntity);
        }

        public async Task UpdateCreditCardAsync(CreditCardModel creditCard)
        {
            var creditCardEntity = _mapper.Map<CreditCard>(creditCard);
            await _creditCardRepository.UpdateCreditCardAsync(creditCardEntity);
        }

        public async Task DeleteCreditCardAsync(int creditCardId)
        {
            await _creditCardRepository.DeleteCreditCardAsync(creditCardId);
        }
    }



}
