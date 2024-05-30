using System;

public class DoubleChainList
{

    public class Node
    {
        public int Data { get; set; }
        public Node Next { get; set; }
        public Node Previous { get; set; }

        public Node(int data)
        {
            Data = data;
            Next = null;
            Previous = null;
        }
    }

    private Node head; //начало списка
    private Node tail; //конец
    private Node[] indexArray; //массив индексов
    private int count; //кол-во узлов
    public const int interval = 50; //интервал

    public DoubleChainList()
    {
        head = null;
        tail = null;
        count = 0;
        indexArray = new Node[1]; //инициализация массива с начальным значением
        indexArray[0] = head;
    }

    public void Add(int data) //добавление узла в конец списка
    {
        Node newNode = new Node(data);

        if (head == null)
        {
            head = newNode;
            tail = newNode;
            indexArray[0] = newNode; // Обновляем первый элемент массива
            return;
        }

        Node current = head;

        while (current.Next != null)
        {
            current = current.Next;
        }

        current.Next = newNode;
        newNode.Previous = current;
        tail = newNode;

        if ((count + 1) % interval == 0)
        {
            int newIndexArraySize = (count / interval) + 2; // Увеличиваем размер массива на 1
            Array.Resize(ref indexArray, newIndexArraySize);
            indexArray[newIndexArraySize - 1] = newNode; // Обновляем последний элемент массива
        }

        count++;
    }



    public void RemoveAt(int index) //удаление узла по индексу
    {
        if (index < 0 || index >= count)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range");
        }

        Node nodeToRemove = head;
        for (int i = 0; i < index; i++)
        {
            nodeToRemove = nodeToRemove.Next;
        }

        if (nodeToRemove.Previous != null)
        {
            nodeToRemove.Previous.Next = nodeToRemove.Next;
        }
        else
        {
            head = nodeToRemove.Next;
        }

        if (nodeToRemove.Next != null)
        {
            nodeToRemove.Next.Previous = nodeToRemove.Previous;
        }
        else
        {
            tail = nodeToRemove.Previous;
        }

        // Обновляем массив индексов
        int blockIndex = index / interval;
        if (blockIndex < indexArray.Length)
        {
            ShiftIndexArrayLeft(blockIndex);
        }

        count--;
    }


    public void Insert(int data, int index) //вставка узла по индексу
    {
        if (index < 0 || index > count)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range");
        }

        if (index == count)
        {
            Add(data);
            return;
        }

        Node newNode = new Node(data);
        Node nodeAtIndex = Find(index);

        if (nodeAtIndex == null)
        {
            throw new InvalidOperationException("Node not found");
        }

        if (nodeAtIndex.Previous != null)
        {
            nodeAtIndex.Previous.Next = newNode;
            newNode.Previous = nodeAtIndex.Previous;
        }
        else
        {
            head = newNode;
        }

        newNode.Next = nodeAtIndex;
        nodeAtIndex.Previous = newNode;

        // Обновляем массив индексов
        if (index % interval == 0)
        {
            ResizeIndexArray();
            indexArray[index / interval] = newNode;
        }

        ShiftIndexArrayRight(index / interval);

        count++;
    }

    public void Clear() //очистка списка
    {
        head = null;
        tail = null;
        count = 0;
        Array.Clear(indexArray, 0, indexArray.Length);
    }

    private Node Find(int index) //поиск узла по индексу
    {
        if (index < 0 || index >= count)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range");
        }

        int blockIndex = index / interval;

        // Находим нужный интервал
        for (int i = 0; i < indexArray.Length; i++)
        {
            if (indexArray[i] == null)
            {
                break;
            }
            if (indexArray[i].Data > index)
            {
                blockIndex = i - 1;
                break;
            }
        }

        // Если индекс находится в последнем интервале
        if (blockIndex == indexArray.Length - 1)
        {
            blockIndex--;
        }

        int startIndex = blockIndex * interval;
        int endIndex = startIndex + interval - 1;

        Node blockStart = indexArray[blockIndex];
        Node findNode = BinarySearch(blockStart, index, startIndex, endIndex);

        return findNode;
    }

    private Node BinarySearch(Node startNode, int targetIndex, int startIndex, int endIndex) //бинарны поиск
    {
        int left = 0;
        int right = endIndex - startIndex;

        while (left <= right)
        {
            int mid = left + (right - left) / 2;
            int midIndex = startIndex + mid;

            if (midIndex == targetIndex)
            {
                return startNode;
            }
            else if (midIndex < targetIndex)
            {
                if (startNode.Next == null) // Проверяем, не достигли ли конца списка
                    break;
                startNode = startNode.Next;
                left = mid + 1;
            }
            else
            {
                if (startNode.Previous == null) // Проверяем, не достигли ли начала списка
                    break;
                startNode = startNode.Previous;
                right = mid - 1;
            }
        }

        return null;
    }



    public void FindDataofIndex(int index) //поиск данных по индексу
    {
        Node findNode = Find(index);

        Console.WriteLine($"{findNode.Data}");
    }


    private void ResizeIndexArray() //увеличение размера массива индексов
    {
        int newSize = Math.Max(1, count / interval);
        Array.Resize(ref indexArray, newSize);
    }

    private void ShiftIndexArrayLeft(int startIndex) //сдвиг массива индексов влево
    {
        for (int i = startIndex; i < indexArray.Length - 1; i++)
        {
            indexArray[i] = indexArray[i + 1];
        }
        indexArray[indexArray.Length - 1] = null;
    }

    private void ShiftIndexArrayRight(int startIndex) //сдвиг массива индексов вправо
    {
        for (int i = indexArray.Length - 1; i > startIndex; i--)
        {
            indexArray[i] = indexArray[i - 1];
        }
        indexArray[startIndex] = null;
    }



    public void Print()  //вывод списка
    {
        Node current = head;

        while (current != null)
        {
            Console.Write($"{current.Data} ");
            current = current.Next;
        }
    }

    public int Count
    {
        get { return count; }
    }

    public void PrintIntervals() //вывод интервалов
    {
        Console.WriteLine("Intervals:");
        for (int i = 0; i < indexArray.Length; i++)
        {
            if (indexArray[i] != null)
            {
                Console.WriteLine($"Interval {i + 1}: Start Index = {i * interval}, Node Data = {indexArray[i].Data}");
            }
        }
    }
}
