
namespace BankWork.Domain.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Account
    {
        public static Account GenerateInstance(string name)
        {
            return new Account()
            {
                SerialNo = 0,
                Name = name,
                DepositRecords = new List<DepositRecord>(),
                CreateDateTime = DateTime.Now,
            };
        }

        public long SerialNo { get; set; }

        public string Name { get; set; }

        public List<DepositRecord> DepositRecords { get; set; }

        public DateTime CreateDateTime { get; set; }

        public long TotalPoints()
            => DepositRecords.Sum(p => p.Points);

        public bool Deposit(long points)
        {
            if (TotalPoints() + points < 0)
            {
                return false;
            }

            DepositRecords.Add(new DepositRecord()
            {
                Points = points,
                CreateDateTime = DateTime.Now
            });

            return true;
        }
    }
}
