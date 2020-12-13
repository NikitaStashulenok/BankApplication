using System;
using System.Collections.Generic;
using System.Text;

namespace BankLibrary
{
    class DepositAccount : Account
    {
        public DepositAccount(decimal sum, int percentage) : base(sum, percentage)
        {
        }
        protected internal override void Open()
        {
            base.OnOpened(new AccountEventArgs($"Открыт новый депозитный счёт! ID счёта: {this.Id}", this.Sum));
        }
        public override void Put(decimal sum)
        {
            if (_days % 30 == 0)
                base.Put(sum);
            else
                base.OnAdded(new AccountEventArgs($"На счёт можно положить только после 30-ти дневного периода", 0));
        }
        public override decimal Withdraw(decimal sum)
        {
            if (_days % 30 == 0)
                return base.Withdraw(sum);
            else
                base.OnWithdrawned(new AccountEventArgs($"Вывести средства можно только после 30-ти дневного периода", 0));
            return 0;
        }
        protected internal override void Calculate()
        {
            if (_days % 30 == 0)
                base.Calculate();
        }
    }
}
