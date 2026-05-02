using OpenAI;
using OpenAI.Chat;

namespace CaloriePlanner.Services;

public class AiService
{
    private readonly OpenAIClient _client;

    public AiService(string apiKey)
    {
        _client = new OpenAIClient(apiKey);
    }

    public async Task<string> GetDietAdvice(string summary)
    {
        var chat = _client.GetChatClient("gpt-4o-mini");

        var response = await chat.CompleteChatAsync(
@"You are a strict fitness coach.

Given:
- User TDEE
- Calories consumed

Do the following:
1. Calculate if the user is in calorie deficit or surplus
2. Say how big the deficit/surplus is
3. Recommend exact daily calories for fat loss
4. Give 3 short, practical suggestions

Rules:
- Be direct
- No long explanations
- No theory
"
+ summary
);

        return response.Value.Content[0].Text;
    }
}