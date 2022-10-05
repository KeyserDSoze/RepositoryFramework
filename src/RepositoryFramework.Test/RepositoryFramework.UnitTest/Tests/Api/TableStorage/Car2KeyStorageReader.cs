﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryFramework.Infrastructure.Azure.Storage.Table;

namespace RepositoryFramework.UnitTest.Tests.Api.TableStorage
{
    internal sealed class Car2KeyStorageReader : ITableStorageKeyReader<SuperCar, Guid>
    {
        public (string PartitionKey, string RowKey) Read(Guid key)
            => (key.ToString(), string.Empty);

        public Guid Read(SuperCar entity)
            => entity.Id;
    }
}
