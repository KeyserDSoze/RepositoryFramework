using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RepositoryFramework.UnitTest.Repository.Models
{
    public class ClassAnimal
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
    public record ClassAnimalKey(string Id, int Key, Guid ValKey);
    public class ClassAnimalRepository : IRepository<ClassAnimal, ClassAnimalKey>
    {
        private static readonly Dictionary<string, Dictionary<int, Dictionary<Guid, ClassAnimal>>> _dic = new();
        public Task<List<BatchResult<ClassAnimalKey, State<ClassAnimal>>>> BatchAsync(List<BatchOperation<ClassAnimal, ClassAnimalKey>> operations, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<long> CountAsync(QueryOptions<ClassAnimal>? options = null, CancellationToken cancellationToken = default)
        {
            return Task.FromResult((long)_dic.Count);
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

        public Task<IEnumerable<ClassAnimal>> QueryAsync(QueryOptions<ClassAnimal>? options = null, CancellationToken cancellationToken = default)
        {
            return default!;
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
    }
}
