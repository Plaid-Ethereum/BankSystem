﻿namespace BankSystem.Services.Implementations
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using BankSystem.Models;
    using Data;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Models.BankAccount;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class BankAccountService : BaseService, IBankAccountService
    {
        private readonly IBankAccountUniqueIdHelper uniqueIdHelper;

        public BankAccountService(BankSystemDbContext context, IBankAccountUniqueIdHelper uniqueIdHelper)
            : base(context)
        {
            this.uniqueIdHelper = uniqueIdHelper;
        }

        public async Task<string> CreateAsync(BankAccountCreateServiceModel model)
        {
            if (!this.IsEntityStateValid(model) ||
                !this.Context.Users.Any(u => u.Id == model.UserId))
            {
                return null;
            }

            string generatedUniqueId;

            do
            {
                generatedUniqueId = this.uniqueIdHelper.GenerateAccountUniqueId();
            } while (this.Context.Accounts.Any(a => a.UniqueId == generatedUniqueId));

            if (model.Name == null)
            {
                model.Name = generatedUniqueId;
            }

            var dbModel = Mapper.Map<BankAccount>(model);
            dbModel.UniqueId = generatedUniqueId;

            await this.Context.Accounts.AddAsync(dbModel);
            await this.Context.SaveChangesAsync();

            return dbModel.Id;
        }

        public async Task<string> GetUserAccountUniqueId(string accountId)
            => await this.Context
                .Accounts
                .Where(a => a.Id == accountId)
                .Select(a => a.UniqueId)
                .SingleOrDefaultAsync();

        public async Task<string> GetAccountIdAsync(string uniqueId)
            => await this.Context
                .Accounts
                .Where(a => a.UniqueId == uniqueId)
                .Select(a => a.Id)
                .SingleOrDefaultAsync();

        public async Task<string> GetAccountIdAsync(string cardNumber, DateTime cardExpiryDate, int cardSecurityCode, string cardOwner)
            => await this.Context
                .Accounts
                .Where(a => a.Cards.Any(c => c.Name == cardOwner && c.Number == cardNumber &&
                                            c.SecurityCode == cardSecurityCode && c.ExpiryDate <= cardExpiryDate))
                .Select(a => a.Id)
                .SingleOrDefaultAsync();

        public async Task<T> GetBankAccountAsync<T>(string id)
            where T : BankAccountBaseServiceModel
            => await this.Context
                .Accounts
                .Where(a => a.Id == id)
                .ProjectTo<T>()
                .SingleOrDefaultAsync();

        public async Task<bool> ChangeAccountNameAsync(string accountId, string newName)
        {
            var account = await this.Context
                .Accounts
                .FindAsync(accountId);
            if (account == null)
            {
                return false;
            }

            account.Name = newName;
            this.Context.Update(account);
            await this.Context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<T>> GetAllUserAccountsAsync<T>(string userId)
            where T : BankAccountBaseServiceModel
            => await this.Context
                .Accounts
                .Where(a => a.UserId == userId)
                .ProjectTo<T>()
                .ToArrayAsync();
    }
}
