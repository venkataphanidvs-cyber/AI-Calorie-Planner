using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace CaloriePlanner.Services
{
    public class NutritionService
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string ApiKey = "AyIjfp2aZrVq3enzERx8AqScGtuWszjcrrDfh7EK";

        public async Task<double> GetCalories(string fruitName)
        {
            string url = $"https://api.nal.usda.gov/fdc/v1/foods/search?query={fruitName}&api_key={ApiKey}";

            var response = await _httpClient.GetStringAsync(url);
            var json = JsonDocument.Parse(response);

            var foods = json.RootElement.GetProperty("foods");

            foreach (var food in foods.EnumerateArray())
            {
                string description = food.GetProperty("description").GetString();

                if (description.ToLower().Contains("raw"))
                {
                    foreach (var nutrient in food.GetProperty("foodNutrients").EnumerateArray())
                    {
                        if (nutrient.GetProperty("nutrientName").GetString() == "Energy")
                        {
                            return nutrient.GetProperty("value").GetDouble();
                        }
                    }
                }
            }

            return 0;
        }
    }
}
