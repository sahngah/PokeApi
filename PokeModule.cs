using System;
using Nancy;
using System.Collections.Generic;
using ApiCaller;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace PokeInfo
{
    public class PokeModule : NancyModule
    {
        public PokeModule()
        {
            List<Dictionary<string, object>> Action = new List<Dictionary<string, object>>();
            string Name = "";
            int Height = 0;
            int Weight = 0;
            int Id = 0;
            string PriType = "";
            string SecType = "";

            Get("/", async args => 
            {
                await WebRequest.SendRequest("http://pokeapi.co/api/v2/pokemon/12", new Action<Dictionary<string, object>>( JsonResponse => 
                { 
                    Name = (string)JsonResponse["name"];
                    Height = Convert.ToInt32(JsonResponse["height"]);
                    Weight = Convert.ToInt32(JsonResponse["weight"]);
                    Id = Convert.ToInt32(JsonResponse["id"]);
                    string typeStr = JsonResponse["types"].ToString();
                    JArray types = (JArray)JsonConvert.DeserializeObject(typeStr);
                    PriType = types[0]["type"]["name"].ToString();
                    SecType = types.Count == 1 ? "" : types[1]["type"]["name"].ToString();
                }
                ));                

                Console.WriteLine(Name);
                ViewBag.name = Name; 
                ViewBag.height = Height;
                ViewBag.weight = Weight;
                ViewBag.pritype = PriType;
                ViewBag.sectype = SecType;
                ViewBag.img = $"<img src='https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/{Id}.png'>";
                return View["Poke"];
            });
        }
    }
}