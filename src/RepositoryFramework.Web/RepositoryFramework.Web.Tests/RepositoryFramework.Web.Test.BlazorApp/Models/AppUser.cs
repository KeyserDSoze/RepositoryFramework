using System.Linq.Expressions;

namespace RepositoryFramework.Web.Test.BlazorApp.Models
{
    internal sealed class AppUserDesignMapper : IUiMapper<AppUser, int>
    {
        public void Map(IPropertyUiHelper<AppUser, int> mapper)
        {
            mapper
            .MapDefault(x => x.Email, "Default email")
            .SetTextEditor(x => x.Name, 700)
            .MapDefault(x => x, new AppUser
            {
                Email = "default",
                Groups = new(),
                Id = 1,
                Name = "default",
                Password = "default",
                Settings = new AppSettings
                {
                    Color = "default",
                    Options = "default",
                    Maps = new()
                }
            })
            .MapDefault(x => x.Settings, new AppSettings { Color = "a", Options = "b", Maps = new() })
            .MapChoices(x => x.Groups, async (serviceProvider) =>
            {
                var repository = serviceProvider.GetService<IRepository<AppGroup, string>>();
                List<LabelledPropertyValue> values = new();
                await foreach (var entity in repository.QueryAsync())
                    values.Add(new LabelledPropertyValue
                    {
                        Label = entity.Value.Name,
                        Value = new Group
                        {
                            Id = entity.Value.Id,
                            Name = entity.Value.Name,
                        }
                    });
                return values;
            }, x => x.Name)
            .MapChoices(x => x.Settings.Maps, serviceProvider =>
            {
                return Task.FromResult(new List<LabelledPropertyValue> {
                    "X",
                    "Y",
                    "Z",
                    "A" }.AsEnumerable());
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
}
