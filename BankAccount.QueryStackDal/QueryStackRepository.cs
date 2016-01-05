﻿using System;
using System.Collections.Generic;
using System.Linq;
using BankAccount.DbModel.ItemDb;
using BankAccount.ValueTypes;
using BankAccount.ViewModels;
using EventStore;

namespace BankAccount.QueryStackDal
{
    public sealed class QueryStackRepository : IQueryStackRepository
    {
        private readonly IStoreEvents _eventStore;

        public QueryStackRepository(IStoreEvents eventStore)
        {
            _eventStore = eventStore;
        }

        public DetailsBankAccountViewModel GetCustomerById(Guid aggregateId)
        {
            var obj = this.RehydrateDomainModel(aggregateId);

            return new DetailsBankAccountViewModel
            {
                AggregateId = aggregateId,
                Version = obj.Version,
                LastName = obj.Person.LastName,
                FirstName = obj.Person.FirstName,
                IdCard = obj.Person.IdCard,
                IdNumber = obj.Person.IdNumber,
                Dob = obj.Person.Dob,
                Phone = obj.Contact.PhoneNumber,
                Email = obj.Contact.Email,
                Street = obj.Address.Street,
                Hausnumber = obj.Address.Hausnumber,
                Zip = obj.Address.Zip,
                State = obj.Address.State,
                City = obj.Address.City
            };
        }

        public IEnumerable<AccountViewModel> GetAccountsByCustomerId(Guid customerId)
        {
            using (var ctx = new BankAccountDbContext())
            {
                var model = ctx.AccountSet.Where(a => a.CustomerAggregateId == customerId);
                var list = new List<AccountViewModel>();
                foreach (var a in model)
                {
                    list.Add(
                        new AccountViewModel
                        {
                            Currency = a.Currency,
                            CustomerId = a.CustomerAggregateId,
                            AggregateId = a.AggregateId,
                            AccountState = a.AccountState
                        });
                }
                return list;
            }
        }

        public TransferViewModel GetAccountById(Guid aggregateId)
        {
            using (var ctx = new BankAccountDbContext())
            {
                var model = ctx.AccountSet.SingleOrDefault(a => a.AggregateId == aggregateId);
                if (model == null)
                {
                    throw new ArgumentNullException("account");
                }

                return new TransferViewModel
                {
                    AggregateId = model.AggregateId,
                    CustomerId = model.CustomerAggregateId,
                    Version = model.Version
                };
            }
        }

        public IEnumerable<BankAccountViewModel> GetCustomers()
        {
            List<Guid> aggregates;

            using (var ctx = new BankAccountDbContext())
            {
                aggregates = ctx.CustomerSet.Select(c => c.AggregateId).ToList();
            }

            return aggregates
                .Select(this.RehydrateDomainModel)
                .Select(customer =>
                    new BankAccountViewModel
                    {
                        FirstName = customer.Person.FirstName,
                        LastName = customer.Person.LastName,
                        Id = customer.Id
                    })
                .ToList();
        }

        private Domain.CustomerDomainModel RehydrateDomainModel(Guid aggregateId)
        {
            var obj = new Domain.CustomerDomainModel();
            IEnumerable<Commit> commits;

            var latestSnapshot = this._eventStore.Advanced.GetSnapshot(aggregateId, int.MaxValue);
            if (latestSnapshot?.Payload != null)
            {
                obj = (Domain.CustomerDomainModel)Convert.ChangeType(latestSnapshot.Payload, latestSnapshot.Payload.GetType());
                commits = this._eventStore.Advanced.GetFrom(aggregateId, latestSnapshot.StreamRevision + 1, int.MaxValue).ToList();
            }
            else
            {
                commits = this._eventStore.Advanced.GetFrom(aggregateId, 0, int.MaxValue).ToList();
            }

            foreach (var c in commits)
            {
                obj.LoadsFromHistory(c.Events);
            }

            return obj;
        }
    }
}
