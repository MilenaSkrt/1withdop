using System;
using System.Collections.Generic;

namespace laba1
{
    public class DoublyLinkedList
    {
        public class Node
        {
            public int Data { get; set; }
            public Node Previous { get; set; }
            public Node Next { get; set; }

            public Node(int data)
            {
                Data = data;
                Previous = null;
                Next = null;
            }
        }
        //поля класса
        private Node head = null;
        private int count = 0;

        private IndexList lst = new IndexList();

        public const int Interval = 50;

        public Node Find(int index) //метод для поиска элементов по индексу
        {
            if (index >= count || head == null) return null;  // Проверка на выход за границы списка и наличие головы
            else if (index == 0 && count != 0) return head; // Если индекс равен 0 и список не пуст, то вернуть голову списка
            else
            {     // Нахождение интервального индекса
                int intervalIndex = index / Interval;
                int leftEnd = lst.GetIndex(intervalIndex);
                int rightEnd = lst.GetIndex(intervalIndex + 1);
                Node target = lst.GetNode(leftEnd);

                if (intervalIndex + 1 < lst.Count)  // Проверка, что есть следующий интервал в списке
                {
                    int mid = (leftEnd + (leftEnd + Interval)) / 2; // Нахождение середины интервала

                    if (intervalIndex == 0)   // Корректировка середины для первого интервала
                        mid = Interval - 1 / 2;

                    if (index > mid)  // Если индекс больше, чем середина
                    {
                        target = lst.GetNode(rightEnd);  // Перемещение по узлам от конца к началу
                        for (int i = rightEnd; i > index; i--)
                        {
                            target = target.Previous;
                        }
                        return target;
                    }
                }

                for (int i = leftEnd; i < index; i++) // Перемещение по узлам от начала к указанному индексу
                {
                    target = target.Next;
                }
                return target;
            }
        }

        public void Add(int data) //метод для добавления новго элемента
        {
            Node newNode = new Node(data); //создаем новый узел

            if (head == null) // Если список пустой, присваиваем новый узел голове списка и добавляем его в линейный список
            {
                head = newNode;
                lst.Append(head, count);
                count++;
            }
            else
            {
                Node last = Find(count - 1);  // Находим последний узел в списке
                Node current = newNode;
                last.Next = current;   // Устанавливаем ссылки между последним и новым узлом
                current.Previous = last;
                count++;

                if (count % Interval == 0) // Если количество элементов кратно интервалу, добавляем текущий элемент в линейный список
                {
                    lst.Append(current, count - 1);
                } 
                if ((count - 1) % Interval == 0)  // Если предыдущее количество элементов было кратно интервалу, устанавливаем узел предыдущего элемента
                {
                    lst.SetNode(current.Previous, (count - 2));
                }
            }
        }

        public void Insert(int data, int index) //метод для вставки элемента по индексу
        {
            if (index < 0 || index > count) { return; }  // Проверка на корректность индекса

            if (index == count) { Add(data); }  // Если индекс совпадает с количеством элементов в списке, то вызывается метод Add

            else // Если вставка не в конец списка
            {
                Node newNode = new Node(data);  // Создание нового узла

                if (index == 0)   // Если вставка в начало списка
                { 
                    newNode.Next = head;    // Обновление связей узлов
                    head.Previous = newNode;
                    head = newNode;
                    lst[0] = head;
                    UpdateIndexNodesAfterInsert(index);
                    count++;
                }
                else
                {     // Нахождение предыдущего и текущего узлов для вставки
                    Node previous = Find(index - 1);
                    Node current = Find(index);
                    newNode.Next = current;
                    newNode.Previous = previous;
                    previous.Next = newNode;
                    current.Previous = newNode;
                    // Обновление индексного списка в зависимости от условий
                    // Если вставка на 48 то обновляется узел на 49  
                    if ((index + 2) % Interval == 0 && count >= index + 2)
                    {
                        lst.SetNode(current.Next, index + 1);
                    }
                    // Если вставка на 50 то обновляется узел на 49
                    if (index % Interval == 0)
                    {
                        lst.SetNode(previous, index - 1);
                    }
                    // Если вставка на 49 (и если это последнее), новый узел на 50 добавляется в индексный список
                    if ((index + 1) % Interval == 0)
                    {
                        if (count == (index + 1))  // Если вставка в позицию, соответствующую индексу в индексном списке
                        {
                            lst.SetNode(newNode, index);
                            count++;
                            return;
                        }
                        lst.SetNode(current, index);
                    }
                    UpdateIndexNodesAfterInsert(index);
                    count++;
                }
            }
        }

        public void RemoveAt(int index) //метод для удаления элементов по индексу
        {
            if (index < 0 || index >= count)  // Проверка на корректность индекса
            {
                return;
            }
             
            if (index == 0)  // Удаление элемента в начале списка
            {
                if (count == 1)  // Если в списке только один элемент
                {
                    head = null;
                    lst.Delete(0);
                    count--;
                }
                else   // Если в списке больше одного элемента
                {
                    head = head.Next;
                    head.Previous = null;
                    lst[0] = head;
                    UpdateIndexNodesAfterRemove(index);
                    count--;
                }
            }
            else   // Удаление элемента в середине или конце списка
            {
                Node target = Find(index);    // Находим целевой узел для удаления и его предыдущий узел
                Node previous = target.Previous;
                previous.Next = target.Next;

                if (previous.Next != null)  // Обновляем связи между узлами
                {
                    Node newCurrent = previous.Next;
                    newCurrent.Previous = previous;

                    if ((index + 2) % Interval == 0 && count >= index + 2)  // Обновляем список узлов при необходимости
                    {
                        lst.SetNode(newCurrent, index + 1); 
                    }

                    if ((index + 1) % Interval == 0)  // Обновляем список узлов при необходимости
                    {
                        lst.SetNode(previous, index);
                    }
                }

                if (index % Interval == 0)  // Обновляем список узлов при необходимости
                {
                    lst.SetNode(previous, index - 1);
                }

                if ((index + 1) % Interval == 0 && count == (index + 1))  // Удаляем последний элемент в списке, если необходимо
                {
                    lst.Delete(lst.Find(index));
                    count--;
                    return;
                }
                UpdateIndexNodesAfterRemove(index);  // Обновляем связи узлов после удаления
                count--;
            }
        }

        private void UpdateIndexNodesAfterRemove(int index) //метод для обновления узлов после удаления элементов
        {
            int shiftIndex = (index / Interval) + 1; // Вычисление сдвига индекса

            if (count % Interval == 0)  // Если количество элементов делится на интервал без остатка, удаляем последний элемент
            {
                lst.Delete(lst.Count - 1);
            }

            for (int i = shiftIndex; i < lst.Count; i++) // Обновление индексов после удаления элемента
            {
                lst[i] = lst[i].Next;
            }
        }

        private void UpdateIndexNodesAfterInsert(int index) //метод для обновления узлов после вставки
        {
            int shiftIndex = (index / Interval) + 1;  // Вычисление сдвига индекса

            if ((count + 1) % Interval == 0)  // Если после вставки элемента количество элементов станет кратным интервалу, добавляем новый элемент
            {
                lst.Append(Find(count), count);
            }

            for (int i = shiftIndex; i < lst.Count; i++)  // Обновление индексов после вставки элемента
            {
                lst[i] = lst[i].Previous;
            }
        }

        public int Count { get { return count; } } //свойство для получения кол-ва элементов в списке

        public int this[int index] //индексатор для доступа к элементам по индексу
        {
            get
            {
                if (index >= count || index < 0) return 0;
                Node findNode = Find(index);
                return findNode.Data;


            }
            set
            {
                if (index >= count || index < 0) return;
                Node findNode = Find(index);
                findNode.Data = value;

            }
        }

        public void Clear() //метод для очистки списка
        {
            head = null;
            lst.Clear();
            count = 0;
        }

        public void Print() //метод для печати эл-ов списка
        {
            Node current = head;

            while (current != null)
            {
                Console.Write($"{current.Data} ");
                current = current.Next;
            }
            Console.WriteLine();
        }
    }
}
