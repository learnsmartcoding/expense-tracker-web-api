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
    public interface IUserBudgetService
    {
        Task<IEnumerable<UserBudgetModel>> GetUserBudgetsByUserIdAsync(int userId, int month = 0, int year = 0);
        Task<UserBudgetModel> GetUserBudgetByIdAsync(int userBudgetId);
        Task AddUserBudgetAsync(UserBudgetModel userBudget);
        Task UpdateUserBudgetAsync(UserBudgetModel userBudget);
        Task DeleteUserBudgetAsync(int userBudgetId);
    }

    public class UserBudgetService : IUserBudgetService
    {
        private readonly IUserBudgetRepository _userBudgetRepository;
        private readonly IMapper _mapper;

        public UserBudgetService(IUserBudgetRepository userBudgetRepository, IMapper mapper)
        {
            _userBudgetRepository = userBudgetRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserBudgetModel>> GetUserBudgetsByUserIdAsync(int userId, int month = 0, int year = 0)
        {
            var userBudgets = await _userBudgetRepository.GetUserBudgetsByUserIdAsync(userId,month, year);
            return _mapper.Map<IEnumerable<UserBudgetModel>>(userBudgets);
        }

        public async Task<UserBudgetModel> GetUserBudgetByIdAsync(int userBudgetId)
        {
            var userBudget = await _userBudgetRepository.GetUserBudgetByIdAsync(userBudgetId);
            return _mapper.Map<UserBudgetModel>(userBudget);
        }

        public async Task AddUserBudgetAsync(UserBudgetModel userBudget)
        {
            var userBudgetEntity = _mapper.Map<UserBudget>(userBudget);
            await _userBudgetRepository.AddUserBudgetAsync(userBudgetEntity);
        }

        public async Task UpdateUserBudgetAsync(UserBudgetModel userBudget)
        {
            var userBudgetEntity = _mapper.Map<UserBudget>(userBudget);
            await _userBudgetRepository.UpdateUserBudgetAsync(userBudgetEntity);
        }

        public async Task DeleteUserBudgetAsync(int userBudgetId)
        {
            await _userBudgetRepository.DeleteUserBudgetAsync(userBudgetId);
        }
    }
}
