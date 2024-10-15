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

namespace PokeAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            string pokemonName = "";
            bool valid = false;

            Console.Clear();

            do
            {
                Console.Write("Enter the name of a Pokémon or '0' to exit the application: ");
                pokemonName = Console.ReadLine()?.ToLower();

                if (string.IsNullOrWhiteSpace(pokemonName))
                {
                    //Console.Clear();

                    valid = false;
                    Console.WriteLine("You Typed nothing, enter a Pokemon name or '0' to exit");
                    Console.WriteLine();

                }
                else if (pokemonName == "0")
                {
                    //Console.Clear();

                    Console.WriteLine("Goodbye...");
                    Environment.Exit(0);
                } else
                {
                    valid = CheckSpecialChars(pokemonName);
                }

            } while (valid == false);

            
            
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
                

                // Display the abilities
                Console.WriteLine("Abilities:");
                foreach (var abilityInfo in pokemon.Abilities)
                {
                    Console.WriteLine($"- {abilityInfo.Ability.Name}");
                }

                Console.WriteLine($"Sprite URL: {pokemon.Sprites.FrontDefault}");

                if (!string.IsNullOrEmpty(pokemon.Sprites.FrontDefault))
                {
                    await DisplaySpriteAsAsciiArt(pokemon.Sprites.FrontDefault);
                }
            }
            else
            {
                Console.WriteLine("Pokémon not found!");
            }

            static async Task DisplaySpriteAsAsciiArt(string imageUrl)
            {
                using var client = new HttpClient();
                try
                {
                    var imageData = await client.GetByteArrayAsync(imageUrl);
                    using Image<Rgba32> image = Image.Load<Rgba32>(imageData);

                    // Resize the image for better ASCII representation
                    image.Mutate(x => x.Resize(image.Width, image.Height)); // Adjust resize factor as needed

                    for (int y = 0; y < image.Height; y++)
                    {
                        for (int x = 0; x < image.Width; x++)
                        {
                            var pixel = image[x, y];
                            char asciiChar = GetAsciiChar(pixel);

                            // Print the ASCII character
                            Console.Write(asciiChar);
                        }
                        Console.WriteLine();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            static char GetAsciiChar(Rgba32 pixel)
            {
                // Calculate brightness manually
                float brightness = 0.2126f * pixel.R + 0.7152f * pixel.G + 0.0722f * pixel.B;

                // Map brightness to ASCII characters
                const string chars = "@%#*+=-:. "; // Characters mapped from darkest to lightest
                int index = (int)(brightness / 255.0f * (chars.Length - 1));
                return chars[chars.Length - 1 - index]; // Reverse index to match dark-to-light mapping
            }

            static bool CheckSpecialChars(string text)
            {

                bool flag;
                bool result = true;
                List<char> specialChars = new List<char>
                {
                '`', '~', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '_', '=', '+',
                '[', '{', ']', '}', '\\', '|', ';', ':', '\'', '"', ',', '<', '.', '>', '/', '?',
                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
                };

                foreach (var character in specialChars)
                {
                    flag = text.Contains(character);
                    if (flag == true)
                    {
                        Console.WriteLine($"Não utilize caracteres especiais ou números, caracter inválido: {character}");
                        Console.WriteLine();

                        return false;
                    }
                }

                return result;
            }
        }
    }
}
