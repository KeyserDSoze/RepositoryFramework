using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace RepositoryFramework.UnitTest.Unitary
{
    public record Translatable(int Id, int CcnlId, DateTimeOffset From, DateTimeOffset? To,
            int NumberOfMonths,
            int AdditionalHolidays,
            decimal? HolidaysFraction,
            bool IsEasterPaid,
            bool IsHolidayOnlyOnSecondRestDay,
            int RenewalType,
            int ConsolidateState,
            bool Active,
            DateTimeOffset CreationDate,
            DateTimeOffset LastChangeDate,
            string? CreatedBy,
            string? ChangedBy
            )
    {
        public string Currency { get; set; } = "€";
    };
    public sealed class ToTranslateSomething
    {
        public int Idccnl { get; set; }
        public int IdccnlValidita { get; set; }
        public DateTime DataInizio { get; set; }
        public DateTime? DataFine { get; set; }
        public int NumeroMensilita { get; set; }
        public int NumeroGiorniFestivita { get; set; }
        public decimal FrazioneFestivita { get; set; }
        public bool DomenicaPasquaRetribuita { get; set; }
        public bool SoloSecondoGiorno { get; set; }
        public byte IdtipoRinnovo { get; set; }
        public byte Stato { get; set; }
        public bool? Attivo { get; set; }
        public DateTime? DataCreazione { get; set; }
        public DateTime? DataModifica { get; set; }
        public string? UtenteCreazione { get; set; }
        public string? UtenteModifica { get; set; }
    }
    public class TranslatableRepository : IRepository<Translatable, string>
    {
        private readonly List<ToTranslateSomething> _toTranslateSomething = new();
        public Task<BatchResults<Translatable, string>> BatchAsync(BatchOperations<Translatable, string> operations, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IState<Translatable>> DeleteAsync(string key, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IState<Translatable>> ExistAsync(string key, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Translatable?> GetAsync(string key, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IState<Translatable>> InsertAsync(string key, Translatable value, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public ValueTask<TProperty> OperationAsync<TProperty>(OperationType<TProperty> operation, Query query, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async IAsyncEnumerable<IEntity<Translatable, string>> QueryAsync(Query query, CancellationToken cancellationToken = default)
        {
            await Task.Delay(0, cancellationToken);
            foreach (var validitum in query.Filter(_toTranslateSomething))
                yield return IEntity.Default(Guid.NewGuid().ToString(),
                    new Translatable(validitum.IdccnlValidita,
                    validitum.Idccnl,
                    validitum.DataInizio,
                    validitum.DataFine,
                    validitum.NumeroMensilita,
                    validitum.NumeroGiorniFestivita,
                    validitum.FrazioneFestivita,
                    validitum.DomenicaPasquaRetribuita, validitum.SoloSecondoGiorno,
                    validitum.IdtipoRinnovo,
                    validitum.Stato,
                    validitum.Attivo ?? false,
                    validitum.DataCreazione ?? DateTime.UtcNow,
                    validitum.DataModifica ?? DateTime.UtcNow,
                    validitum.UtenteCreazione,
                    validitum.UtenteModifica));
        }

        public Task<IState<Translatable>> UpdateAsync(string key, Translatable value, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
    public class TranslationTest
    {
        private static readonly IServiceProvider? s_serviceProvider;
        static TranslationTest()
        {
            DiUtility.CreateDependencyInjectionWithConfiguration(out var configuration)
                    .AddRepository<Translatable, string, TranslatableRepository>()
                    .Translate<ToTranslateSomething>()
                       .With(x => x.Id, x => x.IdccnlValidita)
                       .With(x => x.CcnlId, x => x.Idccnl)
                       .With(x => x.From, x => x.DataInizio)
                       .With(x => x.To, x => x.DataFine)
                       .With(x => x.NumberOfMonths, x => x.NumeroMensilita)
                       .With(x => x.AdditionalHolidays, x => x.NumeroGiorniFestivita)
                       .With(x => x.HolidaysFraction, x => x.FrazioneFestivita)
                       .With(x => x.IsEasterPaid, x => x.DomenicaPasquaRetribuita)
                       .With(x => x.IsHolidayOnlyOnSecondRestDay, x => x.SoloSecondoGiorno)
                       .With(x => x.RenewalType, x => x.IdtipoRinnovo)
                       .With(x => x.ConsolidateState, x => x.Stato)
                       .With(x => x.Active, x => x.Attivo)
                .Services
                .Finalize(out s_serviceProvider)
                .Populate();
        }
        private readonly IRepository<Translatable, string> _repository;

        public TranslationTest()
        {
            _repository = s_serviceProvider!.GetService<IRepository<Translatable, string>>()!;
        }

        [Fact]
        public async Task TestAsync()
        {
            var items = await _repository.Where(x => x.CcnlId == 4 && x.Active).ToListAsync();
            Assert.Empty(items);
            //FilterTranslation.Instance
        }
    }
}
