/*
* Создайте консольное приложение, в котором будет происходить сортировка списка фамилий из пяти человек.
* Сортировка должна происходить при помощи события.
* Сортировка происходит при введении пользователем либо числа 1 (сортировка А-Я), либо числа 2 (сортировка Я-А).
* Дополнительно реализуйте проверку введённых данных от пользователя через конструкцию TryCatchFinally
* с использованием собственного типа исключения.
*/

namespace Modul_9_Task2;

class Program
{
    public delegate void ShowArrayDelegate(string[] array);

    static void Main(string[] args)
    {
        ShowLastNames();
    }

    static void ShowLastNames()
    {
        NumberReader numberReader = new NumberReader();
        numberReader.NumberEnteredEvent += Show;
        bool status = true;
        while (status)
        {
            try
            {
                numberReader.Read();
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Фамилия не может состоять менее, чем из двух букв");
                ShowLastNames();
            }
            catch (ArrayTypeMismatchException)
            {
                Console.WriteLine("Фамилия должна состоять только из букв");
                ShowLastNames();
            }
            catch (FormatException)
            {
                Console.WriteLine("Введено некорректное значение");
                ShowLastNames();
            }
            finally
            {
                status = false;
            }
        }
    }

    static void Show(int number)
    {
        string[] array = LastNames();
        switch (number)
        {
            case 1:
                Console.WriteLine("Выбрана сортировка от А до Я (A-Z)");
                SortAscend(array);
                break;
            case 2:
                Console.WriteLine("Выбрана сортировка от Я до А (Z-A)");
                SortDescend(array);
                break;
        }
    }

    static string[] LastNames()
    {
        string[] array = new string[5];
        for (int i = 0; i < array.Length; i++)
        {
            Console.Write($"Введите фамилию номер {i + 1}: ");
            array[i] = Console.ReadLine();
            if (array[i].Length < 2)
            {
                throw new ArgumentOutOfRangeException();
            }

            foreach (var letter in array[i])
            {
                if (!Char.IsLetter(letter)) throw new ArrayTypeMismatchException();
            }
        }

        return array;
    }

    static void SortAscend(string[] array)
    {
        bool flag = true;
        while (flag)
        {
            flag = false;
            for (int i = 0; i < array.Length - 1; i++)
                if (array[i].CompareTo(array[i + 1]) > 0)
                {
                    string temp = array[i];
                    array[i] = array[i + 1];
                    array[i + 1] = temp;
                    flag = true;
                }
        }

        foreach (string lastname in array)
            Console.WriteLine(lastname);
    }

    static void SortDescend(string[] array)
    {
        bool flag = true;
        while (flag)
        {
            flag = false;
            for (int i = 0; i < array.Length - 1; i++)
                if (array[i].CompareTo(array[i + 1]) < 0)
                {
                    string temp = array[i];
                    array[i] = array[i + 1];
                    array[i + 1] = temp;
                    flag = true;
                }
        }

        foreach (string lastname in array)
            Console.WriteLine(lastname);
    }
}

class NumberReader
{
    public delegate void NumberEnteredDelegate(int number);

    public event NumberEnteredDelegate NumberEnteredEvent;

    public void Read()
    {
        Console.WriteLine(
            "Для того, чтобы отсортировать список в формате А-Я (A-Z), введите \"1\",\nдля сортировки в формате Я-А (Z-A) введите \"2\"");
        int number = Convert.ToInt32(Console.ReadLine());
        if (number != 1 && number != 2) throw new FormatException();
        NumberEntered(number);
    }

    protected virtual void NumberEntered(int number)
    {
        NumberEnteredEvent?.Invoke(number);
    }

}