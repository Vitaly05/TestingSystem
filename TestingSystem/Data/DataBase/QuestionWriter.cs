using System.Text.Json;

using TestingSystem.Models;

namespace TestingSystem.Data.DataBase
{
    internal class QuestionWriter
    {
        private string data;
        public string Data
        {
            set { data = value; }
        }

        public int questionId;


        private Test testToWhichAddedQuestions;
        public Test SelectedTest
        {
            set { testToWhichAddedQuestions = value; }
        }




        public void SaveQuestions()
        {
            using (var db = new ApplicationContext())
            {
                Question addedQuestion = new Question
                {
                    test_id = testToWhichAddedQuestions.id,
                    data = data
                };

                db.Questions.Add(addedQuestion);
                db.SaveChanges();
            }
        }

        public void UpdateQuestions()
        {
            using (var db = new ApplicationContext())
            {
                Question updatedQuestion = new Question
                {
                    id = questionId,
                    test_id = testToWhichAddedQuestions.id,
                    data = data
                };

                db.Questions.Update(updatedQuestion);
                db.SaveChanges();
            }
        }

        static public string DataToString(QuestionData questionData)
        {
            return JsonSerializer.Serialize(questionData);
        }
    }
}
