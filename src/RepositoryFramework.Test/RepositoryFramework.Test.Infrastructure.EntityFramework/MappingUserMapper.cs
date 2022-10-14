using RepositoryFramework.Infrastructure.EntityFramework;
using RepositoryFramework.Test.Infrastructure.EntityFramework.Models.Internal;

namespace RepositoryFramework.Test.Infrastructure.EntityFramework
{
    public class MappingUserMapper : IRepositoryMap<MappingUser, int, User>
    {
        public MappingUser? Map(User? entity)
        {
            if (entity == null)
                return null;
            return new MappingUser(entity.Identificativo, entity.Nome, entity.IndirizzoElettronico, new(), default);
        }
        public User? Map(MappingUser? entity, int key)
        {
            if (entity == null)
                return null;
            return new User
            {
                Identificativo = key,
                IndirizzoElettronico = entity.Email,
                Nome = entity.Username,
                Cognome = string.Empty,
            };
        }
        public int RetrieveKey(User? entity)
            => entity?.Identificativo ?? 0;
    }
}
