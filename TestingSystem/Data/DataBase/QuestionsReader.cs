using System.Linq;
using System.Text.Json;
using TestingSystem.Models;

namespace TestingSystem.Data.DataBase
{
    internal class QuestionsReader
    {
        private Test selectedTest;
        public Test SelectedTest
        {
            set { selectedTest = value; }
        }

        public QuestionData GetQuestionData()
        {
            using (var db = new ApplicationContext())
            {
                var question = db.Questions.FirstOrDefault(p => p.test_id == selectedTest.id);
                QuestionData questionData = JsonSerializer.Deserialize<QuestionData>(question.data);
                questionData.id = question.id;
                return questionData;
            }
        }
    }
}
