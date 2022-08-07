using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace RepositoryFramework.UnitTest.CustomRepository.SpecialKeys.Models
{
    public class ClassAnimalRepository : IRepository<ClassAnimal, ClassAnimalKey>
    {
        private static readonly Dictionary<string, Dictionary<int, Dictionary<Guid, ClassAnimal>>> _dic = new();
        public Task<BatchResults<ClassAnimal, ClassAnimalKey>> BatchAsync(BatchOperations<ClassAnimal, ClassAnimalKey> operations, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public ValueTask<TProperty> OperationAsync<TProperty>(
         OperationType<TProperty> operation,
         QueryOptions<ClassAnimal>? options = null,
         CancellationToken cancellationToken = default)
        {
            if (operation.Operation == Operations.Count)
                return ValueTask.FromResult((TProperty)(object)_dic.Count);
            else
                throw new NotImplementedException();
        }

        public async Task<State<ClassAnimal>> DeleteAsync(ClassAnimalKey key, CancellationToken cancellationToken = default)
        {
            await Task.Delay(0, cancellationToken).NoContext();
            if (_dic.ContainsKey(key.Id) && _dic[key.Id].ContainsKey(key.Key) && _dic[key.Id][key.Key].ContainsKey(key.ValKey))
            {
                _dic[key.Id][key.Key].Remove(key.ValKey);
                return true;
            }

            return false;
        }

        public async Task<State<ClassAnimal>> ExistAsync(ClassAnimalKey key, CancellationToken cancellationToken = default)
        {
            await Task.Delay(0, cancellationToken).NoContext();
            return _dic.ContainsKey(key.Id) && _dic[key.Id].ContainsKey(key.Key) && _dic[key.Id][key.Key].ContainsKey(key.ValKey);
        }

        public async Task<ClassAnimal?> GetAsync(ClassAnimalKey key, CancellationToken cancellationToken = default)
        {
            await Task.Delay(0, cancellationToken).NoContext();
            if (_dic.ContainsKey(key.Id) && _dic[key.Id].ContainsKey(key.Key) && _dic[key.Id][key.Key].ContainsKey(key.ValKey))
            {
                return _dic[key.Id][key.Key][key.ValKey];
            }
            return default;
        }

        public async Task<State<ClassAnimal>> InsertAsync(ClassAnimalKey key, ClassAnimal value, CancellationToken cancellationToken = default)
        {
            await Task.Delay(0, cancellationToken).NoContext();
            if (!_dic.ContainsKey(key.Id))
                _dic.Add(key.Id, new());
            if (!_dic[key.Id].ContainsKey(key.Key))
                _dic[key.Id].Add(key.Key, new());
            if (!_dic[key.Id][key.Key].ContainsKey(key.ValKey))
            {
                _dic[key.Id][key.Key].Add(key.ValKey, value);
                return true;
            }
            return false;
        }

       

        public async Task<State<ClassAnimal>> UpdateAsync(ClassAnimalKey key, ClassAnimal value, CancellationToken cancellationToken = default)
        {
            await Task.Delay(0, cancellationToken).NoContext();
            if (_dic.ContainsKey(key.Id) && _dic[key.Id].ContainsKey(key.Key) && _dic[key.Id][key.Key].ContainsKey(key.ValKey))
            {
                _dic[key.Id][key.Key][key.ValKey] = value;
                return true;
            }
            return false;
        }

        public IAsyncEnumerable<ClassAnimal> QueryAsync(QueryOptions<ClassAnimal>? options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
