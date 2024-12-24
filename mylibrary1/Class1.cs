using System;
using System.Data;

namespace mylibrary1
{
    public class Class1
    {
        /// <summary>
        /// Вычисляет математическое выражение из строки.
        /// </summary>
        /// <param name="expression">Математическое выражение</param>
        /// <returns>Результат вычисления</returns>
        public double Evaluate(string expression)
        {
            // Проверка пустой строки
            if (string.IsNullOrWhiteSpace(expression))
                throw new ArgumentException("Выражение не может быть пустым или с пробелами.");

            // Обработаем корень
            expression = ProcessSquareRoot(expression);
            // Обработаем возведение в степень
            expression = ProcessExponentiation(expression);

            // Создаем объект DataTable для выполнения выражения
            var dataTable = new DataTable();
            try
            {
                // Вычисляем выражение и возвращаем результат
                var result = dataTable.Compute(expression, string.Empty);
                return Convert.ToDouble(result);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Ошибка при вычислении выражения: " + ex.Message);
            }
        }

        private string ProcessSquareRoot(string expression)
        {
            // Функция для обработки выражения с операцией извлечения квадратного корня
            // Паттерн для нахождения "sqrt(a)"
            while (expression.Contains("sqrt("))
            {
                int startIndex = expression.IndexOf("sqrt(");
                int closingParenthesisIndex = FindClosingParenthesis(expression, startIndex + 5);

                if (closingParenthesisIndex == -1)
                    throw new InvalidOperationException("Некорректное выражение корня.");

                // Извлекаем часть выражения внутри sqrt
                string innerExpression = expression.Substring(startIndex + 5, closingParenthesisIndex - (startIndex + 5));
                double innerValue = Evaluate(innerExpression);

                // Заменить "sqrt(a)" на "a^(1/2)"
                double sqrtResult = Math.Pow(innerValue, 0.5);
                expression = expression.Substring(0, startIndex) + sqrtResult + expression.Substring(closingParenthesisIndex + 1);
            }

            return expression;
        }

        private int FindClosingParenthesis(string expression, int startIndex)
        {
            int depth = 1;
            for (int i = startIndex; i < expression.Length; i++)
            {
                if (expression[i] == '(') depth++;
                if (expression[i] == ')') depth--;
                if (depth == 0) return i; // Найден закрывающий символ
            }
            return -1; // Не найден
        }

        private string ProcessExponentiation(string expression)
        {
            // Функция для обработки выражения с операцией возведения в степень
            // Паттерн для нахождения "a^b"
            while (expression.Contains("^"))
            {
                int index = expression.IndexOf("^");
                int start = index - 1;
                int end = index + 1;

                // Находим границы числа слева от ^
                while (start >= 0 && IsPartOfNumber(expression[start])) start--;
                start++;

                // Находим границы числа справа от ^
                while (end < expression.Length && IsPartOfNumber(expression[end])) end++;

                double baseValue = Convert.ToDouble(expression.Substring(start, index - start));
                double exponentValue = Convert.ToDouble(expression.Substring(index + 1, end - index - 1));
                double powerResult = Math.Pow(baseValue, exponentValue);

                // Заменяем "a^b" на результат в выражении
                expression = expression.Substring(0, start) + powerResult + expression.Substring(end);
            }

            return expression;
        }

        /// <summary>
        /// Решает квадратное уравнение ax^2 + bx + c = 0.
        /// </summary>
        /// <param name="a">Коэффициент a</param>
        /// <param name="b">Коэффициент b</param>
        /// <param name="c">Коэффициент c</param>
        /// <returns>Массив корней уравнения</returns>
        public double[] SolveQuadraticEquation(double a, double b, double c)
        {
            if (a == 0)
                throw new ArgumentException("Коэффициент a не может быть равен нулю для квадратного уравнения.");

            double D = b * b - 4 * a * c;

            if (D > 0)
            {
                double x1 = (-b + Math.Sqrt(D)) / (2 * a);
                double x2 = (-b - Math.Sqrt(D)) / (2 * a);
                return new double[] { x1, x2 };
            }
            else if (D == 0)
            {
                double x = -b / (2 * a);
                return new double[] { x };
            }
            else
            {
                return new double[0]; // Нет действительных корней
            }
        }

        private bool IsPartOfNumber(char c)
        {
            // Проверка, является ли символ частью числа (например, цифры, точка)
            return char.IsDigit(c) || c == '.' || c == '-' || c == '+';
        }
    }
}





