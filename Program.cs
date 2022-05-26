using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyBirthdayApp1
{
    class Program
    {
        static birthdayEntities be = new birthdayEntities();
        static List<string[]> st = new List<string[]>();
        static int maxLength = 0;

        //Множитель символов
        static string Space(string st, int count) 
        {
            string str = "";
            while (str.Length != count) str += st;
            return str;
        }

        //Отображение данных из БД
        static void ViewData() 
        {
            
            st.Clear();
            var view = (be.view.Select(x => new { x.number, x.fio, x.day })).ToList();
            List<int> countSpace = new List<int>();
            

            foreach (var v in view)
            {
                st.Add(new string[] { v.number.ToString(), v.fio, v.day.ToShortDateString() });
                countSpace.Add(v.fio.Length);

                if (v.fio.Length > maxLength) maxLength = v.fio.Length;
            }

            Console.WriteLine("Список дней рождений:");

            for (int i = 0; i < st.Count(); i++)
            {
                //Делаем поле fio одной длинны для красивой табуляции
                st[i][1] += Space(" ", maxLength-st[i][1].Length);
                Console.WriteLine(st[i][0]+"\t"+st[i][1]+"\t"+st[i][2]);
            }
        }

        //Добавление записи в БД
        static void Add() 
        {
            int num = 0;
            string f;
            DateTime d = new DateTime();
            
            string answerDay;
            string answerMonth;
            string answerYear;

            while (true)
            {
                Console.WriteLine("Для отмены введите \'.\'");
                Console.Write("Введите ФИО - ");
                f = Console.ReadLine();

                if (f == ".") return;
                if (f != "" && f != "\n") break;
                Console.WriteLine("Имя не может быть пустым.\nПопробуйте ещё раз.\n");
            }

            while (true) {
                Console.WriteLine("Для отмены введите \'.\'");
                Console.Write("Введите День - ");
                answerDay = Console.ReadLine();

                if (answerDay == ".") return;

                if (int.TryParse(answerDay, out int number) && answerDay != "" && answerDay != "\n")
                {
                    if (int.Parse(answerDay) > 0 && int.Parse(answerDay) <= 31) {
                        break;
                    }
                    
                }
                Console.WriteLine("Число дней должно быть 1 - 31.\nПопробуйте ещё раз.\n");

            }

            while (true)
            {
                Console.WriteLine("Для отмены введите \'.\'");
                Console.Write("Введите Месяц - ");
                answerMonth = Console.ReadLine();

                if (answerMonth == ".") return;

                if (int.TryParse(answerMonth, out int number) && answerMonth != "" && answerMonth != "\n")
                {
                    if (int.Parse(answerMonth) > 0 && int.Parse(answerMonth) <= 12)
                    {
                        break;
                    }

                }
                Console.WriteLine("Число месяцев должно быть 1 - 12.\nПопробуйте ещё раз.\n");

            }

            while (true)
            {
                Console.WriteLine("Для отмены введите \'.\'");
                Console.Write("Введите Год - ");
                answerYear = Console.ReadLine();

                if (answerYear == ".") return;

                if (int.TryParse(answerYear, out int number) && answerYear != "" && answerYear != "\n")
                {
                    if (int.Parse(answerYear) > 1900 && int.Parse(answerYear) <= DateTime.Now.Year)
                    {
                        break;
                    }

                }
                Console.WriteLine("Годы могут быть 1900 - "+DateTime.Now.Year+".\nПопробуйте ещё раз.\n");

            }
            
            d = DateTime.Parse(answerDay + "/" + answerMonth + "/" + answerYear + " 00:00:00");
            view v = new view
            {
                number = num,
                fio = f,
                day = d
            };

            be.view.Add(v);
            try
            {
                be.SaveChanges();
                Console.WriteLine("\nДобавление прошло успешно.\n");
            }
            catch (Exception ex) { Console.WriteLine("Не удалось добавить запись. \nПопробуйте ещё раз.\n"); }
        }

        //Удаление записи из БД
        static void Delete() 
        {
            string answer;

            while (true)
            {
                Console.WriteLine("Для отмены введите \'.\'");
                Console.Write("Введите номер записи - ");
                answer = Console.ReadLine();
                int num;

                if (answer == ".") return;
                if (int.TryParse(answer, out int number) && answer != "" && answer != "\n") {
                    num = int.Parse(answer);
                    var query = be.view.Where(x => x.number == num).FirstOrDefault();
                    if (query != null) {
                        be.view.Remove(query);
                        be.SaveChanges();
                        Console.WriteLine("\nУдаление прошло успешно.\n");
                        break; 
                    }
                }
                Console.WriteLine("Такого номера нет.\nПопробуйте ещё раз.\n");
            }

        }

        //Редактирование записи в БД
        static void Edit() 
        {
            int num;
            string f;
            DateTime d = new DateTime();

            string answerDay;
            string answerMonth;
            string answerYear;
            string answer;

            while (true)
            {
                Console.WriteLine("Для отмены введите \'.\'");
                Console.Write("Введите номер записи - ");
                answer = Console.ReadLine();
                if (answer == ".") return;
                if (answer != "" && answer != "\n" && int.TryParse(answer, out int number))
                {
                    num = int.Parse(answer);
                    var test = be.view.Where(x => x.number == num).FirstOrDefault();
                    if (test != null) break;
                }
                Console.WriteLine("Такой записи не существует.\nПопробуйте ещё раз.\n");
            }

            while (true)
            {
                Console.WriteLine("Для отмены введите \'.\'");
                Console.WriteLine("Ввыберите действие:\n"+"1 - Редактировать ФИО\n"+"2 - Редактировать Дату\n"+"3 - Редактировать ФИО и Дату");
                answer = Console.ReadLine();
                if (answer == ".") return;
                if (answer != "" && answer != "\n" && int.TryParse(answer, out int number)) { 
                    if (int.Parse(answer) == 1 || int.Parse(answer) == 2 || int.Parse(answer) == 3) break; 
                }
                Console.WriteLine("Такого действия не существует.\nПопробуйте ещё раз.\n");
            }

            if (int.Parse(answer) == 1 || int.Parse(answer) == 3)
            {
                while (true)
                {
                    Console.WriteLine("Для отмены введите \'.\'");
                    Console.Write("Введите ФИО - ");
                    f = Console.ReadLine();

                    if (f == ".") return;
                    if (f != "" && f != "\n") break;
                    Console.WriteLine("Имя не может быть пустым.\nПопробуйте ещё раз.\n");
                }
                var query = be.view.Where(x => x.number == num).FirstOrDefault();
                query.fio = f;
                be.SaveChanges();
            }

            if (int.Parse(answer) == 2 || int.Parse(answer) == 3)
            {
                while (true)
                {
                    Console.WriteLine("Для отмены введите \'.\'");
                    Console.Write("Введите День - ");
                    answerDay = Console.ReadLine();

                    if (answerDay == ".") return;

                    if (int.TryParse(answerDay, out int number) && answerDay != "" && answerDay != "\n")
                    {
                        if (int.Parse(answerDay) > 0 && int.Parse(answerDay) <= 31)
                        {
                            break;
                        }

                    }
                    Console.WriteLine("Число дней должно быть 1 - 31.\nПопробуйте ещё раз.\n");

                }

                while (true)
                {
                    Console.WriteLine("Для отмены введите \'.\'");
                    Console.Write("Введите Месяц - ");
                    answerMonth = Console.ReadLine();

                    if (answerMonth == ".") return;

                    if (int.TryParse(answerMonth, out int number) && answerMonth != "" && answerMonth != "\n")
                    {
                        if (int.Parse(answerMonth) > 0 && int.Parse(answerMonth) <= 12)
                        {
                            break;
                        }

                    }
                    Console.WriteLine("Число месяцев должно быть 1 - 12.\nПопробуйте ещё раз.\n");

                }

                while (true)
                {
                    Console.WriteLine("Для отмены введите \'.\'");
                    Console.Write("Введите Год - ");
                    answerYear = Console.ReadLine();

                    if (answerYear == ".") return;

                    if (int.TryParse(answerYear, out int number) && answerYear != "" && answerYear != "\n")
                    {
                        if (int.Parse(answerYear) > 1900 && int.Parse(answerYear) <= DateTime.Now.Year)
                        {
                            break;
                        }

                    }
                    Console.WriteLine("Число лет должно быть 1900 - " + DateTime.Now.Year + ".\nПопробуйте ещё раз.\n");

                }

                d = DateTime.Parse(answerDay + "/" + answerMonth + "/" + answerYear + " 00:00:00");
                var query = be.view.Where(x => x.number == num).FirstOrDefault();
                query.day = d;
                be.SaveChanges();
            }
            Console.WriteLine("\nИзменения сохранены успешно.\n");
        }

        //Вывод ближайших дней рождения по порядку
        static void Nearest() 
        {
            int nowDay = DateTime.Now.Day;
            int nowMonth = DateTime.Now.Month;
            var query = be.view.Where(x => (x.day.Day >= nowDay && x.day.Month == nowMonth) || (x.day.Month > nowMonth)).OrderBy(p => p.day.Month).ToList();

            Console.WriteLine("Ближайшие Дни рождения:");
            foreach (var q in query) 
            {
                Console.WriteLine("("+q.number+")\t" + q.fio+Space(" ", maxLength - q.fio.Length) + "\t" + q.day.ToShortDateString());
            }
            Console.WriteLine();
        
        }

        //Меню консольного приложения
        static void Menu() {
            string strMenu = "\n Приложение \'Поздравлятор\'\n Выберите действие:\n 1 - Отобразить данные \n 2 - Добавить запись \n 3 - Удалить запись \n 4 - Редактировать запись \n 5 - Ближайший день рождения \n 6 - Очистка консоли \n 7 - Закрыть консоль";
            while (true)
            {
                Console.WriteLine(strMenu);
                string answer = Console.ReadLine();
                if (int.TryParse(answer, out var number)) {
                    int check = int.Parse(answer);

                    switch (check) {
                        case 1:
                            ViewData();
                            break;
                        case 2:
                            Add();
                            break;
                        case 3:
                            Delete();
                            break;
                        case 4:
                            Edit();
                            break;
                        case 5:
                            Nearest();
                            break;
                        case 6:
                            Console.Clear();
                            break;
                        case 7:
                            Environment.Exit(0);
                            break;

                        default:
                            EX("Ошибка - программа не смогла найти такое действие.");
                            break;
                    }
                }
            }
        }

        //Вывод сообщения об ошибке
        static void EX(string st) {
            Console.WriteLine(st+"\nПопробуйте ещё раз.");
        }

        static void Main(string[] args)
        {
            ViewData();
            Menu();
        }
    }
}
