namespace NaviriaAPI.Helpers
{
    public static class PromptTemplates
    {
        private const string _taskCreationPrompt = """
                                                 You are a task creation assistant.
                                                 Based on the user's request, generate a JSON object for TaskCreateDto.
                                                 If some information is missing, simply omit those fields.
                                                 Output ONLY VALID JSON. Do NOT add any explanations, comments, greetings, motivational text, or any other lines.
                                                 
                                                 ❗️Your response MUST contain ONLY syntactically correct JSON.
                                                 ❗️Any text outside of JSON is an error.
                                                 ❗️Do NOT write anything except JSON.
                                                 
                                                 Example structure:
                                                 {
                                                   "UserId": "...",
                                                   "FolderId": "...",
                                                   "Title": "...",
                                                   "Description": "...",
                                                   "Tags": [{ "Name": "..." }],
                                                   "IsDeadlineOn": true,
                                                   "Deadline": "2025-05-20",
                                                   "IsShownProgressOnPage": true,
                                                   "IsNotificationsOn": false,
                                                   "NotificationDate": null,
                                                   "Priority": 1,
                                                   "Type" : "with_subtasks",
                                                   "Subtasks": [
                                                     {
                                                       "Type": "standard",
                                                       "Title": "Прочитати главу 1",
                                                       "Description": "Ознайомитися з матеріалом",
                                                       "IsCompleted": false
                                                     },
                                                     {
                                                       "Type": "repeatable",
                                                       "Title": "Біг 5 км",
                                                       "Description": "Повторювати в понеділок і п’ятницю",
                                                       "RepeatDays": ["Monday", "Friday"]
                                                     },
                                                     {
                                                       "Type": "scale",
                                                       "Title": "Підвищити вагу у вправах",
                                                       "Description": "Прогрес по кг",
                                                       "Unit": "kg",
                                                       "CurrentValue": 50,
                                                       "TargetValue": 70
                                                     }
                                                   ]
                                                 }

                                                 DO NOT add any explanations, comments, greetings, or text outside of JSON.
                                                 Respond with ONLY JSON.
                                                 """;

        public static string TaskCreationPrompt => _taskCreationPrompt;

        public static string WithUserMessage(string template, string userMessage)
        {
          return $"{template}\n\nHere is user's request:\n{userMessage}";
        }
    }
}