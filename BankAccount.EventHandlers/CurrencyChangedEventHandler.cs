﻿using System;
using BankAccount.CommandStackDal.Storage.Abstraction;
using BankAccount.EventHandlers.Base;
using BankAccount.Events;
using BankAccount.Infrastructure.EventHandling;
using BankAccount.Infrastructure.Storage;

namespace BankAccount.EventHandlers
{
    public class CurrencyChangedEventHandler : BaseBankAccountEventHandler, IEventHandler<CurrencyChangedEvent>
    {
        private readonly ICommandStackRepository<Domain.BankAccount> _repository;

        public CurrencyChangedEventHandler(ICommandStackRepository<Domain.BankAccount> repository, ICommandStackDatabase database)
            : base(database)
        {
            if (repository == null)
            {
                throw new InvalidOperationException("Repository is not initialized.");
            }

            this._repository = repository;
        }

        public void Handle(CurrencyChangedEvent handle)
        {
            var ba = this._repository.GetById(handle.AggregateId);

            ba.Money.Currency = handle.Currency;

            this.Database.AddToCache(ba);
        }
    }
}
