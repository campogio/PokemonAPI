using Newtonsoft.Json;

namespace PokeAPI.Models
{
    public class PokemonModel
    {

        public PokemonModel() {

            name = String.Empty;

            id = 0;

            is_legendary = false;
        
        }

        public string name {  get; set; }

        public FlavorTextModel standard_description { get; set; }

        public int id { get; set; }

        public HabitatModel habitat { get; set; }

        public bool is_legendary { get; set; }

        [JsonProperty]
        private IEnumerable<FlavorTextModel> flavor_text_entries { get; set; }

        public void setRequestedFlavorText(string langCode)
        {

            FlavorTextModel temp = flavor_text_entries.FirstOrDefault(X => X.language.name.Equals(langCode)) ?? null;

            if (temp == null) 
            {

                FlavorTextModel flavorTextModel = new FlavorTextModel() {

                    flavor_text = "No flavor text found for the requested language."
                
                };

                temp = flavorTextModel;
            
            } 
            
            else 
            { 
                
                fixFlavorText(temp); 
            
            }

            standard_description = temp;

        }

        private void fixFlavorText(FlavorTextModel flavor_text)
        {
            /*  Page breaks are treated just like newlines.
                Soft hyphens followed by newlines vanish.
                Letter-hyphen-newline becomes letter-hyphen, to preserve real
                hyphenation.
                Any other newline becomes a space. 
                https://github.com/veekun/pokedex/issues/218#issuecomment-339841781
             */

            flavor_text.flavor_text = flavor_text.flavor_text.Replace('\f', '\n').Replace("\u00ad\n", "").Replace("\u00ad", "")
                .Replace(" -\n", " - ").Replace("-\n", "-").Replace("\n", " ");


        }

    }
}
