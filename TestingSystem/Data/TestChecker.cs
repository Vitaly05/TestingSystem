using System;
using System.Collections.Generic;
using System.Linq;

namespace TestingSystem.Data
{
    internal class TestChecker
    {
        private List<string> userAnswers;
        private List<string> correctAnswers;
        private int testMaxMark;


        public TestChecker(List<string> userAnswers, List<string> correctAnswers, int testMaxMark)
        {
            this.userAnswers = userAnswers;
            this.correctAnswers = correctAnswers;
            this.testMaxMark = testMaxMark;
        }

        

        public double GetTestMark()
        {
            return calculateMark(getCorrectAnswersCount());
        }

        private int getCorrectAnswersCount()
        {
            int correctUserAnswersCount = 0;

            for (int iii = 0; iii < userAnswers.Count; iii++)
            {
                
                if (userAnswers.ElementAt(iii).ToLower()
                    == correctAnswers.ElementAt(iii).ToLower())
                    ++correctUserAnswersCount;
            }

            return correctUserAnswersCount;
        }

        private double calculateMark(int correctUserAnswersCount)
        {
            double mark = ((double)testMaxMark / correctAnswers.Count) * correctUserAnswersCount;
            return Math.Round(mark, 1);
        }
    }
}
