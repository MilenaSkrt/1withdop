// Использование пространства имен System
namespace laba1
{
    // Определение класса ChainList
    public class ChainList
    {
        // Ссылка на голову списка
        public Node head;

        // Метод добавления элемента в список
        public void Add(int data)
        {
            Node newNode = new Node(data);

            // Если список пустой
            if (head == null)
            {
                head = newNode;
                Node.count++;
                return;
            }

            Node current = head;

            // Поиск конца списка
            while (current.Next != null)
            {
                current = current.Next;
            }

            current.Next = newNode;
            Node.count++;
        }

        // Метод поиска элемента по индексу
        public Node Find(int index)
        {
            if (index < 0 || index >= Node.count)
            {
                return null;
            }

            int currentIndex = 0;
            Node current = head;

            // Поиск элемента по индексу
            while (current != null)
            {
                if (currentIndex == index)
                {
                    return current;
                }
                current = current.Next;
                currentIndex++;
            }

            return null;
        }

        // Метод удаления элемента по индексу
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= Node.count) return;

            if (index == 0)
            {
                head = head.Next;
                Node.count--;
                return;
            }

            int currentIndex = 0;
            Node current = head;

            // Удаление элемента по индексу
            while (current != null)
            {
                if (currentIndex + 1 == index)
                {
                    current.Next = current.Next.Next;
                    Node.count--;
                    return;
                }
                currentIndex++;
                current = current.Next;
            }
        }

        // Метод вставки элемента по индексу
        public void Insert(int data, int index)
        {
            if (index < 0 || index > Node.count) return;

            Node newNode = new Node(data);

            if (index == 0)
            {
                newNode.Next = head;
                head = newNode;
                Node.count++;
                return;
            }

            Node current = head;
            int currentIndex = 0;

            // Вставка элемента по индексу
            while (current != null)
            {
                if (currentIndex + 1 == index)
                {
                    newNode.Next = current.Next;
                    current.Next = newNode;
                    Node.count++;
                    return;
                }
                currentIndex++;
                current = current.Next;
            }
        }

        // Индексатор для доступа к элементам списка
        public int this[int index]
        {
            get
            {
                if (index < 0 || index >= Node.count) return 0;

                Node node = Find(index);
                return node.Data;
            }
            set
            {
                if (index >= 0 && index < Node.count)
                {
                    Node node = Find(index);
                    if (node != null)
                        node.Data = value;
                }
            }
        }

        // Метод печати элементов списка
        public void Print()
        {
            Node current = head;

            // Печать элементов списка
            while (current != null)
            {
                Console.Write(current.Data + " ");
                current = current.Next;
            }
            Console.WriteLine();
        }

        // Метод очистки списка
