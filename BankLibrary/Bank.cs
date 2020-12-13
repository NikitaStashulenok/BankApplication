using System;
using System.Collections.Generic;
using System.Text;

namespace BankLibrary
{
    //тип счёта
    public enum AccountType
    {
        Ordinary,
        Deposit
    }
    public class Bank<T> where T : Account
    {
        T[] accounts;

        public string Name { get; private set; }

        public Bank(string name)
        {
            this.Name = name;
        }
        //метод создания счёта
        public void Open(AccountType accountType, decimal sum, AccountStateHandler addSumHandler, AccountStateHandler withdrawSumHandler, AccountStateHandler calculationSumHandler, AccountStateHandler closeSumHandler, AccountStateHandler openSumHandler)
        {
            T newAccount = null;

            switch (accountType)
            {
                case AccountType.Ordinary:
                    newAccount = new DemandAccount(sum, 1) as T;
                    break;
                case AccountType.Deposit:
                    newAccount = new DepositAccount(sum, 40) as T;
                    break;
            }
            if (newAccount == null)
                throw new Exception("Ошибка создания счёта");
            //Добавляем новый счёт в массив счетов
            if (accounts == null)
            {
                accounts = new T[] { newAccount };
            }
            else
            {
                T[] tempAccounts = new T[accounts.Length + 1];
                for(int i = 0; i < accounts.Length; i++)
                {
                    tempAccounts[i] = accounts[i];
                }
                tempAccounts[tempAccounts.Length - 1] = newAccount;
                accounts = tempAccounts;
            }
            //установка обработчиков события счёта
            newAccount.Added += addSumHandler;
            newAccount.Calculated += calculationSumHandler;
            newAccount.Withdrawed += withdrawSumHandler;
            newAccount.Opened += openSumHandler;
            newAccount.Closed += closeSumHandler;

            newAccount.Open();
        }
        //добавление средств на счёт
        public void Put(decimal sum, int id)
        {
            T account = FindAccount(id);
            if (account == null)
                throw new Exception("Счёт не найден");
            account.Put(sum);
        }
        // вывод средств
        public void Withdraw(decimal sum, int id)
        {
            T account = FindAccount(id);
            if (account == null)
                throw new Exception("Счёт не найден");
            account.Withdraw(sum);
        }
        // закрытие счёта
        public void Close(int id)
        {
            int index;
            T account = FindAccount(id, out index);
            if (account == null)
                throw new Exception("Счёт не найден");
            account.Close();

            if (accounts.Length <= 1)
                accounts = null;
            else
            {
                //уменьшаем массив счетов, удаляя из него закрытый счёт
                T[] tempAccounts = new T[accounts.Length - 1]; 
                for (int i = 0, j = 0; i < accounts.Length; i++)
                {
                    if(i != index)
                    {
                        tempAccounts[j++] = accounts[i];
                    }
                    accounts = tempAccounts;
                }
            }
        }
        public void CalculatePercentage()
        {
            if (accounts == null) //если массив не создан, выходим из метода
                return;
            for (int i = 0; i < accounts.Length; i++)
            {
                accounts[i].IncrementDays();
                accounts[i].Calculate();
            }
        }
        // поиск счёта по id
        public T FindAccount(int id)
        {
            for (int i = 0; i < accounts.Length; i++)
            {
                if (accounts[i].Id == id)
                    return accounts[i];
            }
            return null;
        }
        //перегруженная версия поиска счёта
        public T FindAccount(int id, out int index)
        {
            for (int i = 0; i < accounts.Length; i++)
            {
                if (accounts[i].Id == id)
                {
                    index = i;
                    return accounts[i];
                }
            }
            index = -1;
            return null;

        }
    }
}