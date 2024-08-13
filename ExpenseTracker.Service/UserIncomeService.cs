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
    public interface IUserIncomeService
    {
        Task<IEnumerable<UserIncomeModel>> GetUserIncomesByUserIdAsync(int userId, int month = 0, int year = 0);
        Task<UserIncomeModel> GetUserIncomeByIdAsync(int userIncomeId);
        Task AddUserIncomeAsync(UserIncomeModel userIncome);
        Task UpdateUserIncomeAsync(UserIncomeModel userIncome);
        Task DeleteUserIncomeAsync(int userIncomeId);
    }

    public class UserIncomeService : IUserIncomeService
    {
        private readonly IUserIncomeRepository _userIncomeRepository;
        private readonly IMapper _mapper;

        public UserIncomeService(IUserIncomeRepository userIncomeRepository, IMapper mapper)
        {
            _userIncomeRepository = userIncomeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserIncomeModel>> GetUserIncomesByUserIdAsync(int userId, int month = 0, int year = 0)
        {
            var userIncomes = await _userIncomeRepository.GetUserIncomesByUserIdAsync(userId,month,year);
            return _mapper.Map<IEnumerable<UserIncomeModel>>(userIncomes);
        }

        public async Task<UserIncomeModel> GetUserIncomeByIdAsync(int userIncomeId)
        {
            var userIncome = await _userIncomeRepository.GetUserIncomeByIdAsync(userIncomeId);
            return _mapper.Map<UserIncomeModel>(userIncome);
        }

        public async Task AddUserIncomeAsync(UserIncomeModel userIncome)
        {
            var userIncomeEntity = _mapper.Map<UserIncome>(userIncome);
            await _userIncomeRepository.AddUserIncomeAsync(userIncomeEntity);
        }

        public async Task UpdateUserIncomeAsync(UserIncomeModel userIncome)
        {
            var userIncomeEntity = _mapper.Map<UserIncome>(userIncome);
            await _userIncomeRepository.UpdateUserIncomeAsync(userIncomeEntity);
        }

        public async Task DeleteUserIncomeAsync(int userIncomeId)
        {
            await _userIncomeRepository.DeleteUserIncomeAsync(userIncomeId);
        }
    }
}
