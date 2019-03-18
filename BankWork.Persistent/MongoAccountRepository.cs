
namespace BankWork.Persistent
{
    using System;
    using System.Collections.Generic;
    using BankWork.Domain.Model;
    using BankWork.Domain.Repository;
    using MongoDB.Bson.Serialization;
    using MongoDB.Driver;

    public class MongoAccountRepository : IAccountRepository
    {
        private const string dbName = "BankWork";
        private const string collectionName = "Account";

        private MongoClient client { get; set; }
        private IMongoDatabase db { get; set; }
        private IMongoCollection<Account> collection { get; set; }

        static MongoAccountRepository()
        {
            BsonClassMap.RegisterClassMap<Account>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
                cm.MapIdMember(c => c.Name);
            });
        }

        public MongoAccountRepository(MongoClient mongoClient)
        {
            client = mongoClient;
            db = client.GetDatabase(dbName);
            collection = db.GetCollection<Account>(collectionName);
            collection.Indexes.CreateOne(Builders<Account>.IndexKeys.Descending(p => p.CreateDateTime));
        }

        public bool Insert(Account account)
        {
            try
            {
                this.collection.InsertOne(account);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Account GetAccountByName(string name)
            => this.collection.Find(p => p.Name == name).FirstOrDefault();

        public bool Update(Account account)
        {
            var filter = Builders<Account>.Filter.And(
                    Builders<Account>.Filter.Eq(p => p.Name, account.Name),
                    Builders<Account>.Filter.Eq(p => p.SerialNo, account.SerialNo)
                );

            var update = Builders<Account>.Update
                 .Set(p => p.DepositRecords, account.DepositRecords)
                 .Inc(p => p.SerialNo, 1);

            return this.collection.UpdateOne(filter, update, new UpdateOptions() { IsUpsert = false }).MatchedCount > 0;
        }

        public IEnumerable<Account> MoreThanDeposit(long points)
        {
            var filter = Builders<Account>.Filter
                .ElemMatch(
                    p => p.DepositRecords,
                    Builders<DepositRecord>.Filter.Gte(x => x.Points, points));

            return this.collection.Find(filter).ToList();
        }

        public IEnumerable<Account> MoreThanDrawal(long points)
        {
            var filter = Builders<Account>.Filter
                .ElemMatch(
                    p => p.DepositRecords,
                    Builders<DepositRecord>.Filter.Lte(x => x.Points, -1 * points));

            return this.collection.Find(filter).ToList();
        }
    }
}
