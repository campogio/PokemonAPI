using Swashbuckle.AspNetCore.SwaggerGen;

namespace PokeAPI.Models
{
    public class TranslationModel
    {

        public Dictionary<string,string> success {  get; set; }

        //Contents could get its own model, but for the sake of simplicity it is kept as a dictionary
        public Dictionary<string,string> contents { get; set; }

    }
}
