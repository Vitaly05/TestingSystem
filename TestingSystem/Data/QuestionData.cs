using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TestingSystem.Data
{
    internal class QuestionData
    {
        [JsonIgnore]
        public int id;

        [JsonPropertyName("Questions")]
        public List<string> questions { get; set; }

        [JsonPropertyName("Answers")]
        public List<string> answers { get; set; }

        [JsonPropertyName("QuestionTypes")]
        public List<string> questionTypes { get; set; }

        [JsonPropertyName("IncorrectAnswers")]
        public List<List<string>> incorrectAnswers { get; set; }

        [JsonPropertyName("TimerOn")]
        public bool timerOn { get; set; }

        [JsonPropertyName("Minutes")]
        public int minutes { get; set; }

        [JsonPropertyName("Seconds")]
        public int seconds { get; set; }
    }
}
