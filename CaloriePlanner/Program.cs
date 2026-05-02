using CaloriePlanner.Model;
using CaloriePlanner.Services;


var calorieService = new CalorieService();
var nutritionService = new NutritionService();
var excelService = new ExcelService();

var user = GetUserInput();

double bmr = calorieService.CalculateBMR(user);
double tdee = calorieService.CalculateTDEE(bmr, user.ActivityFactor);

Console.WriteLine($"BMR: {bmr}");
Console.WriteLine($"TDEE: {tdee}");

var fruits = new List<Fruit>();

while (true)
{
    Console.Write("Enter fruit name (or 'done'): ");
    string name = Console.ReadLine();

    if (name.ToLower() == "done")
        break;

    double calories = await nutritionService.GetCalories(name);

    Console.Write("Enter grams: ");
    double grams = double.Parse(Console.ReadLine() ?? "0");

    double totalCalories = (calories / 100) * grams;

    Console.WriteLine($"{name} => {totalCalories} kcal for {grams}g");

    // For now, still store per 100g (we'll improve model next)
    fruits.Add(new Fruit
    {
        Name = name,
        CaloriesPer100g = calories,
        Grams = grams,
        TotalCalories = totalCalories
    });
}

Console.Write("Enter your total daily calories (approx): ");
double totalDailyCalories = double.Parse(Console.ReadLine() ?? "0");
double weeklyFatLoss = (tdee - totalDailyCalories) * 7 / 7700;
Console.WriteLine($"Estimated fat loss per week: {weeklyFatLoss:F2} kg");

double totalFruitCalories = 0;

foreach (var f in fruits)
{
    totalFruitCalories += (f.CaloriesPer100g / 100) * f.Grams;
}

string summary = $@"
User:
- Age: {user.Age}
- Weight: {user.Weight} kg
- Height: {user.Height} cm

Calories:
- TDEE: {tdee}
- Total Daily Calories: {totalDailyCalories}
- Fruit Calories: {totalFruitCalories}

Goal: Fat Loss
";

excelService.Export(user, bmr, tdee, fruits);

var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

var aiService = new AiService(apiKey);


string advice = await aiService.GetDietAdvice(summary);

Console.WriteLine("\nAI Suggestions:\n");
Console.WriteLine(advice);

Console.WriteLine("Excel file generated!");
Console.ReadLine();




static User GetUserInput()
{
    Console.Write("Age: ");
    int age = int.Parse(Console.ReadLine());

    Console.Write("Gender (male/female): ");
    string gender = Console.ReadLine();

    Console.Write("Weight (kg): ");
    double weight = double.Parse(Console.ReadLine());

    Console.Write("Height (cm): ");
    double height = double.Parse(Console.ReadLine());

    Console.WriteLine("Activity Level: 1.2 (low), 1.55 (moderate), 1.725 (high)");
    double activity = double.Parse(Console.ReadLine());

    return new User
    {
        Age = age,
        Gender = gender,
        Weight = weight,
        Height = height,
        ActivityFactor = activity
    };
}


 