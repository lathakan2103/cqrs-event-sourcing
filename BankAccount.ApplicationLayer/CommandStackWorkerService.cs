﻿using System;
using BankAccount.Commands;
using BankAccount.Configuration;
using BankAccount.ViewModels;

namespace BankAccount.ApplicationLayer
{
    public sealed class CommandStackWorkerService
    {
        public static void AddCustomer(CustomerViewModel vm)
        {
            IoCServiceLocator.CommandBus.Send(
                new CreateCustomerCommand(
                    Guid.NewGuid(),
                    0,
                    vm.FirstName,
                    vm.LastName,
                    vm.IdCard,
                    vm.IdNumber,
                    vm.Dob,
                    vm.Email,
                    vm.Phone,
                    vm.Street,
                    vm.ZIP,
                    vm.Hausnumber,
                    vm.City,
                    vm.State));
        }

        public static void DeleteCustomer(Guid id)
        {
            IoCServiceLocator.CommandBus.Send(
                new DeleteCustomerCommand(id, -1));
        }

        public static void EditPersonDetails(PersonViewModel vm)
        {
            IoCServiceLocator.CommandBus.Send(
                new ChangePersonDetailsCommand(
                    vm.AggregateId,
                    vm.Version,
                    vm.FirstName,
                    vm.LastName,
                    vm.IdCard,
                    vm.IdNumber));
        }

        public static void EditContactDetails(ContactViewModel vm)
        {
            IoCServiceLocator.CommandBus.Send(
                new ChangeContactDetailsCommand(
                    vm.AggregateId,
                    vm.Version,
                    vm.Email,
                    vm.PhoneNumber));
        }

        public static void EditAddressDetails(AddressViewModel vm)
        {
            IoCServiceLocator.CommandBus.Send(
                new ChangeAddressDetailsCommand(
                    vm.AggregateId,
                    vm.Version,
                    vm.Street,
                    vm.Zip,
                    vm.Hausnumber,
                    vm.City,
                    vm.State));
        }

        public static void TransferMoney(TransferViewModel vm)
        {
            IoCServiceLocator.CommandBus.Send(
                new ChangeBalanceCommand(
                    vm.AggregateId,
                    vm.Version,
                    vm.Amount));
        }

        public static void AddAccount(AccountViewModel vm)
        {
            IoCServiceLocator.CommandBus.Send(
                new AddAccountCommand(
                    Guid.NewGuid(),
                    0,
                    vm.CustomerId,
                    vm.Currency));
        }

        public static void DeleteAccount(Guid id)
        {
            IoCServiceLocator.CommandBus.Send(
                new DeleteAccountCommand(id, -1));
        }

        public static void LockAccount(Guid id)
        {
            IoCServiceLocator.CommandBus.Send(
                new LockAccountCommand(id, -1));
        }

        public static void UnlockAccount(Guid id)
        {
            IoCServiceLocator.CommandBus.Send(
                new UnlockAccountCommand(id, -1));
        }
    }
}
