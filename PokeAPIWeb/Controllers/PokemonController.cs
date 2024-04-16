using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PokeAPI.Models;
using System.Net.Http.Headers;
using System.Xml.Linq;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PokeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {

        HttpClient client;

        public PokemonController() 
        {
        
            client = new HttpClient();

        }

        // GET api/<PokemonController>/pokemonName
        [HttpGet("{name}")]
        public async Task<ResponseModel> Get(string name)
        {

            ResponseModel responseModel = new ResponseModel();

            //Input Cleanup
            name = name.Trim();
            name = name.ToLower();

            
            PokemonModel pokemon = new PokemonModel();

            try
            {
                HttpResponseMessage response = await client.GetAsync("https://pokeapi.co/api/v2/pokemon-species/" + name);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                //Deserialization to object
                pokemon = JsonConvert.DeserializeObject<PokemonModel>(responseBody);
            }
            catch
            {

                responseModel.statusCode = "500 could not get pokemon, are you sure the name is spelled correctly?";
                responseModel.pokemon = pokemon;

                return responseModel;
            }
            


            //Keeping to the assignment, We only ask for a pokemon name as input,
            //in a production environment I would also have requested a language and either randomized the flavor text choice or chosen it by index.
            //i.e. taking "LangCode" as an extra input

            //pokemon.setRequestedFlavorText(LangCode);
            pokemon.setRequestedFlavorText("en");

            responseModel.pokemon = pokemon;
            responseModel.statusCode = "200 OK";

            return responseModel;
        }


        // GET api/<PokemonController>/translated/pokemonName
        [HttpGet("translated/{name}")]
        public async Task<ResponseModel> GetTranslation(string name)
        {

            ResponseModel responseModel = await Get(name);

            //If we could not retrieve a pokemon description, return immediately
            if(responseModel.pokemon.standard_description == null)
            {
                return responseModel;
            }

            //Saving this to set if APIs fail
            string temp = responseModel.pokemon.standard_description.flavor_text;

            responseModel.pokemon.setRequestedFlavorText("en");

            //In case the funtranslations API is down for whatever reason or we get rate limited, it would be best to implement a timeout, this should happen automatically but it would be best if try/catched

            try
            {
                if (responseModel.pokemon.habitat.name.Equals("cave") || responseModel.pokemon.is_legendary)
                {
                    await YodaTranslate(responseModel.pokemon);
                }
                else
                {
                    await ShakespeareTranslate(responseModel.pokemon);
                }
            }catch (Exception ex)
            {
                responseModel.pokemon.standard_description.flavor_text = temp;
            }
            

            return responseModel;
        }


        //Since this excercise is based on the usage of only pokemon, I have used models for the next two functions, in a production environment I would probably just pass the string
        //to make it more reusable and not limited to the PokemonModel.
        private async Task<PokemonModel> YodaTranslate(PokemonModel pokemon)
        {

            var text = new { text = pokemon.standard_description.flavor_text };

            string json = JsonConvert.SerializeObject(text);

            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("https://api.funtranslations.com/translate/yoda.json", content);

            return await TranslatePokemon(pokemon, response);
        }

        

        private async Task<PokemonModel> ShakespeareTranslate(PokemonModel pokemon)
        {
             var text = new { text = pokemon.standard_description.flavor_text };

            string json = JsonConvert.SerializeObject(text);

            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("https://api.funtranslations.com/translate/shakespeare.json", content);

            return await TranslatePokemon(pokemon, response);
        }

        private async Task<PokemonModel> TranslatePokemon(PokemonModel pokemon, HttpResponseMessage response)
        {

            string responseBody = await response.Content.ReadAsStringAsync();

            TranslationModel translation = JsonConvert.DeserializeObject<TranslationModel>(responseBody);

            pokemon.standard_description.flavor_text = translation.contents["translated"];

            return pokemon;

        }

    }
}
