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
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IUserRepository _userRepository;

        public ExpenseService(IExpenseRepository expenseRepository, IUserRepository userRepository)
        {
            _expenseRepository = expenseRepository;
            _userRepository = userRepository;
        }

        // Expense related methods
        public async Task<ExpenseModel> GetExpenseByIdAsync(int expenseId, int month = 0, int year = 0)
        {
            var expense = await _expenseRepository.GetExpenseByIdAsync(expenseId);
            return expense != null ? MapToExpenseModel(expense) : null;
        }

        public async Task<IEnumerable<ExpenseModel>> GetExpensesByUserIdAsync(int userId, int month = 0, int year = 0)
        {
            var expenses = await _expenseRepository.GetExpensesByUserIdAsync(userId,month, year);
            return expenses.Select(MapToExpenseModel).ToList();
        }

        public async Task<IEnumerable<ExpenseModel>> GetExpensesByFamilyIdAsync(int familyId, int month = 0, int year = 0)
        {
            var userIds = await _userRepository.GetUsersByFamilyIdAsync(familyId);
            var expenses = new List<Expense>();
            foreach (var userId in userIds.Select(u => u.UserId))
            {
                var userExpenses = await _expenseRepository.GetExpensesByUserIdAsync(userId, month, year);
                expenses.AddRange(userExpenses);
            }
            return expenses.Select(MapToExpenseModel).ToList();
        }

        public async Task AddExpenseAsync(ExpenseModel expenseModel)
        {
            var expense = MapToExpenseEntity(expenseModel);
            await _expenseRepository.AddExpenseAsync(expense);
            expenseModel.ExpenseId = expense.ExpenseId; // Return the new ExpenseId
        }

        public async Task UpdateExpenseAsync(ExpenseModel expenseModel)
        {
            var expense = MapToExpenseEntity(expenseModel);
            await _expenseRepository.UpdateExpenseAsync(expense);
        }

        public async Task DeleteExpenseAsync(int expenseId)
        {
            await _expenseRepository.DeleteExpenseAsync(expenseId);
        }

        // ExpenseType related methods
        public async Task<IEnumerable<ExpenseTypeModel>> GetAllExpenseTypesAsync()
        {
            var expenseTypes = await _expenseRepository.GetAllExpenseTypesAsync();
            return expenseTypes.Select(MapToExpenseTypeModel).ToList();
        }

        // ExpenseCategory related methods
        public async Task<IEnumerable<ExpenseCategoryModel>> GetAllExpenseCategoriesAsync()
        {
            var expenseCategories = await _expenseRepository.GetAllExpenseCategoriesAsync();
            return expenseCategories.Select(MapToExpenseCategoryModel).ToList();
        }

        // CreditCard related methods
        public async Task<IEnumerable<CreditCardModel>> GetAllCreditCardsAsync()
        {
            var creditCards = await _expenseRepository.GetAllCreditCardsAsync();
            return creditCards.Select(MapToCreditCardModel).ToList();
        }

        // Mapping methods
        private ExpenseModel MapToExpenseModel(Expense expense)
        {
            return new ExpenseModel
            {
                ExpenseId = expense.ExpenseId,
                UserId = expense.UserId,
                ExpenseAmount = expense.ExpenseAmount,
                ExpenseCategoryId = expense.ExpenseCategoryId,
                ExpenseTypeId = expense.ExpenseTypeId,
                CreditCardId = expense.CreditCardId,
                ExpenseDescription = expense.ExpenseDescription,
                ExpenseDate = expense.ExpenseDate,
                ExpenseItemsModel = expense.ExpenseItems.Select(s => new ExpenseItemModel()
                {
                    ExpenseId = s.ExpenseId ?? 0,
                    ExpenseItemId = s.ExpenseItemId,
                    UserId = s.UserId,
                    ExpenseAmount = s.ExpenseAmount,
                    ExpenseCategoryId = s.ExpenseCategoryId,
                    ExpenseTypeId = s.ExpenseTypeId,
                    CreditCardId = s.CreditCardId,
                    ExpenseDescription = s.ExpenseDescription,
                    ExpenseDate = s.ExpenseDate,

                }).ToList()
            };
        }

        private Expense MapToExpenseEntity(ExpenseModel expenseModel)
        {
            return new Expense
            {
                ExpenseId = expenseModel.ExpenseId,
                UserId = expenseModel.UserId,
                ExpenseAmount = expenseModel.ExpenseItemsModel.Any() ? 
                expenseModel.ExpenseItemsModel.Sum(s => s.ExpenseAmount) : expenseModel.ExpenseAmount,
                ExpenseCategoryId = expenseModel.ExpenseCategoryId,
                ExpenseTypeId = expenseModel.ExpenseTypeId,
                CreditCardId = expenseModel.CreditCardId,
                ExpenseDescription = expenseModel.ExpenseDescription,
                ExpenseDate = expenseModel.ExpenseDate,
                ExpenseItems = expenseModel.ExpenseItemsModel.Select(s => new ExpenseItem()
                {
                    ExpenseId = s.ExpenseId,
                    ExpenseItemId = s.ExpenseItemId,
                    UserId = s.UserId,
                    ExpenseAmount = s.ExpenseAmount,
                    ExpenseCategoryId = s.ExpenseCategoryId,
                    ExpenseTypeId = s.ExpenseTypeId,
                    CreditCardId = s.CreditCardId,
                    ExpenseDescription = s.ExpenseDescription,
                    ExpenseDate = s.ExpenseDate
                }).ToList()
            };
        }

        private ExpenseTypeModel MapToExpenseTypeModel(ExpenseType expenseType)
        {
            return new ExpenseTypeModel
            {
                ExpenseTypeId = expenseType.ExpenseTypeId,
                ExpenseTypeName = expenseType.ExpenseTypeName
            };
        }

        private ExpenseCategoryModel MapToExpenseCategoryModel(ExpenseCategory expenseCategory)
        {
            return new ExpenseCategoryModel
            {
                ExpenseCategoryId = expenseCategory.ExpenseCategoryId,
                ExpenseCategoryName = expenseCategory.ExpenseCategoryName
            };
        }

        private CreditCardModel MapToCreditCardModel(CreditCard creditCard)
        {
            return new CreditCardModel
            {
                CreditCardId = creditCard.CreditCardId,
                CardLastFourDigit = creditCard.CardLastFourDigit,
                CreditCardName = creditCard.CreditCardName
            };
        }
    }

}
