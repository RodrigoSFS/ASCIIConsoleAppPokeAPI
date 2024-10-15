using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Colorful;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Console = Colorful.Console;
using System.Text.Json.Serialization;




// Define a class to hold the data for a Pokémon
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

class Program
{
    static async Task Main(string[] args)
    {
        Console.Write("Enter the name of a Pokémon: ");
        string pokemonName = Console.ReadLine()?.ToLower();

        using var client = new HttpClient();

        var pokemon = new Pokemon();
        
        try 
        {
            pokemon = await client.GetFromJsonAsync<Pokemon>($"https://pokeapi.co/api/v2/pokemon/{pokemonName}");
        }
        catch(HttpRequestException ex)
        {
            Console.WriteLine(ex.Message);
        }
        

        if (pokemon.Name != null)
        {
            Console.WriteLine($"Name: {pokemon.Name}");
            Console.WriteLine($"ID: {pokemon.Id}");
            
            Console.WriteLine("Types:");
            foreach (var type in pokemon.Types)
            {
                Console.WriteLine($"- {type.Type.Name}");
            }
            
            Console.WriteLine($"Height: {pokemon.Height}");
            Console.WriteLine($"Weight: {pokemon.Weight}");
            
            Console.WriteLine("Abilities:");
            foreach (var abilityInfo in pokemon.Abilities)
            {
                Console.WriteLine($"- {abilityInfo.Ability.Name}");
            }

            Console.WriteLine($"Sprite URL: {pokemon.Sprites.FrontDefault}");

            if (!string.IsNullOrEmpty(pokemon.Sprites.FrontDefault))
            {
                // This open the image in the default browser.
                Console.WriteLine("Opening sprite in the default web browser...");
                Process.Start(new ProcessStartInfo
                {
                    FileName = pokemon.Sprites.FrontDefault,
                    UseShellExecute = true
                });
            }
        }
        else
        {
            Console.WriteLine("Pokémon not found!");
        }
    }
}
