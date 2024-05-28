// Импорт пространства имен System
namespace laba1
{
    // Объявление класса Programm
    class Programm
    {
        // Основной метод Main
        public static void Main(string[] args)
        {

            // Создание трех различных списков
            DoublyLinkedList doublyLinked = new DoublyLinkedList();
            ChainList chainList = new ChainList();
            ArrayList arrayList = new ArrayList();

            // Создание объекта Random для генерации случайных чисел
            Random random = new Random();

            // Цикл с 10000 итераций
            for (int i = 0; i < 10000; i++)
            {
                // Генерация случайных операций, индексов и данных
                int operation = random.Next(1, 5);
                int index = random.Next(50);
                int Data = random.Next(50);

                // Осуществление операций в зависимости от случайной операции
                switch (operation){

                    // Добавление элементов
                    case 1:
                        arrayList.Add(Data);
                        chainList.Add(Data);
                        doublyLinked.Add(Data);
                        break;

                    // Удаление элементов
                    case 2:
                        arrayList.Delete(index);
                        chainList.RemoveAt(index);
                        doublyLinked.RemoveAt(index);
                        break;

                    /**
                     * // Вставка элементов
                     * case 3:
                     *     arrayList.Insert(index, Data);
                     *     chainList.Insert(index, Data);
                     *     doublyLinked.Insert(index, Data);
                     *     break;
                     */

                    /**
                     * // Очистка списков
                     * case 4:
                     *     arrayList.Clear();
                     *     chainList.Clear();
                     *     doublyLinked.Clear();
                     *     break;
                     */

                    // Изменение элементов по индексу
                    case 5:
                        arrayList[i] = Data;
                        chainList[i] = Data;
                        doublyLinked[i] = Data;
                        break;
                }

            }

            // Вывод содержимого списков
            Console.WriteLine("ArrayList:");
            arrayList.Print();
            Console.WriteLine("ChainList:");
            chainList.Print();
            Console.WriteLine("DoublyLinkedList:");
            doublyLinked.Print();

            // Проверка равенства списков
            AreListsEqual(arrayList, chainList, doublyLinked);

            // Метод для проверки равенства списков
            void AreListsEqual(ArrayList arrayList, ChainList chainList, DoublyLinkedList doublyLinkedList)
            {
                // Изначально считаем списки равными
                bool areEqual = true;

                // Если длины списков не совпадают, списки не равны
                if (arrayList.Count != chainList.Count || arrayList.Count != doublyLinkedList.Count)
                {
                    areEqual = false;
                }
                else
                {
                    // Проверка элементов списков на равенство
                    for (int i = 0; i < arrayList.Count; i++)
                    {
                        if (arrayList[i] != chainList[i] || arrayList[i] != doublyLinkedList[i])
                        {
                            areEqual = false;
                            break;
                        }
                    }
                }

                // Вывод результата проверки равенства списков
                Console.WriteLine($"Равны ли списки?: {areEqual}");
            }
        }
    }
}
