﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RepositoryFramework.Test.Infrastructure.EntityFramework;
using RepositoryFramework.Test.Infrastructure.EntityFramework.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace RepositoryFramework.UnitTest.Repository
{
    public class EntityFrameworkTest
    {
        private static readonly IServiceProvider ServiceProvider;
        static EntityFrameworkTest()
        {
            DiUtility.CreateDependencyInjectionWithConfiguration(out var configuration)
                .AddDbContext<SampleContext>(options =>
                {
                    options.UseSqlServer(configuration["Database:ConnectionString"]);
                }, ServiceLifetime.Scoped)
                .AddRepository<AppUser, AppUserKey, AppUserStorage>()
                .Translate<User>()
                    .With(x => x.Id, x => x.Identificativo)
                    .With(x => x.Username, x => x.Nome)
                    .With(x => x.Email, x => x.IndirizzoElettronico)
                .Services
                .Finalize(out ServiceProvider);
        }
        private readonly IRepository<AppUser, AppUserKey> _repository;
        public EntityFrameworkTest()
        {
            _repository = ServiceProvider.GetService<IRepository<AppUser, AppUserKey>>()!;
        }

        [Fact]
        public async Task AllCommandsAndQueryAsync()
        {
            foreach (var appUser in await _repository.ToListAsync())
            {
                await _repository.DeleteAsync(new AppUserKey(appUser.Id));
            }
            var user = new AppUser(3, "Arnold", "Arnold@gmail.com", new());
            var result = await _repository.InsertAsync(new AppUserKey(3), user);
            Assert.True(result.IsOk);
            user = user with { Id = result.Value!.Id };
            var key = await CheckAsync("Arnold");

            result = await _repository.UpdateAsync(key, user with { Username = "Fish" });
            Assert.True(result.IsOk);
            await CheckAsync("Fish");

            async Task<AppUserKey> CheckAsync(string name)
            {
                var items = await _repository.Where(x => x.Id > 0).QueryAsync().ToListAsync();
                Assert.Single(items);
                var actual = await _repository.FirstOrDefaultAsync(x => x.Id > 0);
                Assert.NotNull(actual);
                var key = new AppUserKey(actual!.Id);
                var item = await _repository.GetAsync(key);
                Assert.NotNull(item);
                Assert.Equal(name, item!.Username);
                var result = await _repository.ExistAsync(key);
                Assert.True(result.IsOk);
                return key;
            }

            result = await _repository.DeleteAsync(key);
            Assert.True(result.IsOk);

            result = await _repository.ExistAsync(key);
            Assert.False(result.IsOk);

            var items = await _repository.Where(x => x.Id > 0).ToListAsync();
            Assert.Empty(items);

            var batchOperation = _repository.CreateBatchOperation();
            for (int i = 0; i < 10; i++)
                batchOperation.AddInsert(new AppUserKey(i), new AppUser(i, $"User {i}", $"Email {i}", new()));
            await batchOperation.ExecuteAsync();

            items = await _repository.Where(x => x.Id >= 0).ToListAsync();
            Assert.Equal(10, items.Count);

            Expression<Func<AppUser, object>> orderPredicate = x => x.Id;
            var page = await _repository.Where(x => x.Id > 0).OrderByDescending(orderPredicate).PageAsync(1, 2);
            Assert.True(page.Items.First().Id > page.Items.Last().Id);

            batchOperation = _repository.CreateBatchOperation();
            await foreach (var appUser in _repository.QueryAsync())
                batchOperation.AddUpdate(new AppUserKey(appUser.Id), new AppUser(appUser.Id, $"User Updated {appUser.Id}", $"Email Updated {appUser.Id}", new()));
            await batchOperation.ExecuteAsync();

            items = await _repository.Where(x => x.Id >= 0).ToListAsync();
            Assert.Equal(10, items.Count);
            Assert.Equal($"Email Updated {items.First().Id}", items.First().Email);

            var max = await _repository.MaxAsync(x => x.Id);
            var min = await _repository.MinAsync(x => x.Id);
            int preSum = 0;
            for (int i = min; i <= max; i++)
                preSum += i;
            var preAverage = (int)((decimal)preSum / ((decimal)max + 1 - (decimal)min));
            var sum = await _repository.SumAsync(x => x.Id);
            Assert.Equal(preSum, sum);
            var average = await _repository.AverageAsync(x => x.Id);
            Assert.InRange(preAverage, average - 1, average + 1);

            batchOperation = _repository.CreateBatchOperation();
            foreach (var appUser in await _repository.QueryAsync().ToListAsync())
                batchOperation.AddDelete(new AppUserKey(appUser.Id));
            await batchOperation.ExecuteAsync();

            items = await _repository.Where(x => x.Id > 0).QueryAsync().ToListAsync();
            Assert.Empty(items);
        }
    }
}
