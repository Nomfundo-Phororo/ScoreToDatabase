using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Globalization;

namespace ScoreToDatabase
{
    public class GradeBook
    {
        public List<double> grades;
        public string Name;
        public GradeBook()
        {
            grades = new List<double>();
        }
        public GradeBook(string Name)
        {
            this.Name = Name;
        }

        public void Addgrade(double grade)
        {
            grades.Add(grade);
        }

        public Statistics GetStatistics()
        {
            Statistics statistics = new Statistics();
            if (grades.Count != 0)
            {

                statistics.Average = Math.Round(grades.Average(), 1);
                statistics.Highgrade = grades.Max();
                statistics.Lowgrade = grades.Min();

                return statistics;
            }
            else
            {
                statistics.Average = 0;
                statistics.Highgrade = 0;
                statistics.Lowgrade = 0;
                return statistics;
            }

        }
        public void GetUserStatistics(Statistics result)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("The average is: {0}", Math.Round(result.Average, 2));
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("The highest is: {0}", Math.Round(result.Highgrade, 2));
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("The lowest grade is: {0}", Math.Round(result.Lowgrade, 2));
        }
        public void Greetings(GradeBook gradeBook)
        {
            Console.Write("Enter students name:");
            gradeBook.Name = Console.ReadLine();
            Console.WriteLine("----------------------------------------------------------------------------------");
            Console.WriteLine("                           Welcome {0}", gradeBook.Name);
            Console.WriteLine("----------------------------------------------------------------------------------");
            Console.WriteLine("-------------------------------RULES----------------------------------------------");
            Console.WriteLine("1.Enter decimal numbers");
            Console.WriteLine("2.Scores must be less that 100 ");
            Console.WriteLine("3.To stop entering scores and see stats, enter '111' ");
        }

        public int UserAddingGrades(GradeBook gradeBook, int i)
        {
            do
            {
                
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(" Please enter score {0}", i);

                string answer = Console.ReadLine();

                double score;


                bool success = double.TryParse(answer, NumberStyles.Number, CultureInfo.InvariantCulture,out score);

               

                if (success)
                {
                    if (score == 111)
                    {
                        break;
                    }
                    else if (score <= 100 && score > -1)
                    {
                        gradeBook.Addgrade(score);
                        SendToDB(gradeBook, score);

                        i++;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Must be less than 100 and greater than 0");
                        continue;

                    }

                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error!! Score must be a Decimal........ You were warned :(");
                    continue;
                }



            }
            while (!gradeBook.grades.Contains(111));
            return i;
        }

        private static void SendToDB(GradeBook gradeBook, double score)
        {

            string txt = score.ToString(CultureInfo.InvariantCulture);
            SqlConnection connection = new SqlConnection("Data Source=LAPTOP-MD52G2CP;Initial Catalog=AppFactory;Integrated Security=True");
            connection.Open();

            StringBuilder query = new StringBuilder();
            query.Append("Insert into AppFactScores (Scores,Student_Name)values(");
            query.Append(txt);
            query.Append(", '" + gradeBook.Name + "')");
          SqlCommand SqlCommand = new SqlCommand(query.ToString(), connection);
            SqlCommand.ExecuteNonQuery();

   


            connection.Close();
        }
    }
}
