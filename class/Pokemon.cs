using System.Text.Json.Serialization;

namespace PokeAPI
{
    public class Pokemon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public Sprites Sprites { get; set; }
        public List<TypeInfo>? Types { get; set; }
        public List<AbilityInfo>? Abilities { get; set; }
    }

    public class Sprites
    {
        [JsonPropertyName("front_default")]
        public string FrontDefault { get; set; }
    }

    public class TypeInfo
    {
        public Type Type { get; set; }
    }

    public class Type
    {
        public string Name { get; set; }
    }

    public class AbilityInfo
    {
        public Ability Ability { get; set; }
    }

    public class Ability
    {
        public string Name { get; set; }
    }
}