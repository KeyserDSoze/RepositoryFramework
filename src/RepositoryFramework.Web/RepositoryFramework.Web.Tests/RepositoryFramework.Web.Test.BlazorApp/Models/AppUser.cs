using System.Security.Cryptography;

namespace RepositoryFramework.Web.Test.BlazorApp.Models
{
    internal sealed class AppUserDesignMapper : IRepositoryUiMapper<AppUser, int>
    {
        public void Map(IRepositoryPropertyUiHelper<AppUser, int> mapper)
        {
            mapper
            .MapDefault(x => x.Email, "Default email")
            .MapDefault(x => x.InternalAppSettings, 23)
            .SetTextEditor(x => x.Name, 700)
            .MapDefault(x => x, new AppUser
            {
                Email = "default",
                Groups = new(),
                Id = 1,
                Name = "default",
                Password = "default",
                InternalAppSettings = new InternalAppSettings
                {
                    Index = 44,
                    Maps = new(),
                    Options = "default options"
                },
                Settings = new AppSettings
                {
                    Color = "default",
                    Options = "default",
                    Maps = new()
                }
            })
            .MapDefault(x => x.Settings, new AppSettings { Color = "a", Options = "b", Maps = new() })
            .MapChoices(x => x.Groups, async (serviceProvider, entity) =>
            {
                var repository = serviceProvider.GetService<IRepository<AppGroup, string>>();
                List<LabelledPropertyValue> values = new();
                await foreach (var item in repository.QueryAsync())
                    values.Add(new LabelledPropertyValue
                    {
                        Label = item.Value.Name,
                        Id = item.Value.Name,
                        Value = new Group
                        {
                            Id = item.Value.Id,
                            Name = item.Value.Name,
                        }
                    });
                return values;
            }, x => x.Name)
            .MapChoices(x => x.Settings.Maps, (serviceProvider, entity) =>
            {
                return Task.FromResult(new List<LabelledPropertyValue> {
                    "X",
                    "Y",
                    "Z",
                    "A" }.AsEnumerable());
            }, x => x)
            .MapChoice(x => x.MainGroup, async (serviceProvider, entity) =>
            {
                var repository = serviceProvider.GetService<IRepository<AppGroup, string>>();
                List<LabelledPropertyValue> values = new();
                await foreach (var item in repository.QueryAsync())
                    values.Add(new LabelledPropertyValue
                    {
                        Label = item.Value.Name,
                        Id = item.Value.Id,
                        Value = item.Value.Id
                    });
                return values;
            }, x => x);
        }
    }
    public sealed class AppUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Group> Groups { get; set; }
        public AppSettings Settings { get; init; }
        public InternalAppSettings InternalAppSettings { get; set; }
        public List<string> Claims { get; set; }
        public string MainGroup { get; set; }
        public string HashedMainGroup => MainGroup.ToHash();
    }
    public sealed class Group
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public sealed class AppSettings
    {
        public string Color { get; set; }
        public string Options { get; set; }
        public List<string> Maps { get; set; }
    }
    public sealed class InternalAppSettings
    {
        public int Index { get; set; }
        public string Options { get; set; }
        public List<string> Maps { get; set; }
    }
}
