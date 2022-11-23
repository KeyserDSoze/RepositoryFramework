using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryFramework
{
    public static class BuilderTranslatorExtensions
    {
        public static IQueryTranslationBuilder<T, TKey, TTranslated> Translate<T, TKey, TTranslated>(this IRepositoryBuilder<T, TKey> builder)
        {
            return null;
        }
    }
}
