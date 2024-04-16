

using PokeAPI.Controllers;
using PokeAPI.Models;

namespace PokeApi.Test
{
    public class UnitTest1
    {
        [Fact]
        public async void PokemonName()
        {
            var controller = new PokemonController();

            ResponseModel result = await controller.Get("Mew");

            Assert.Equal("mew", result.pokemon.name);
            Assert.Equal("200 OK", result.statusCode);
        }

        [Fact]
        public async void PokemonTranslate()
        {
            var controller = new PokemonController();

            ResponseModel result = await controller.GetTranslation("Mewtwo");

            Assert.Equal("200 OK", result.statusCode);
            Assert.Equal("Created by a scientist after years of horrific gene splicing and dna engineering experiments,  it was.", result.pokemon.standard_description.flavor_text);
        }
    }
}