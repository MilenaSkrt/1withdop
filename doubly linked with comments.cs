// Метод для поиска узла по индексу в списке
public Node Find(int index)
{
    // Проверка на выход за границы списка и наличие головы
    if (index >= count || head == null) return null;
    // Если индекс равен 0 и список не пуст, то вернуть голову списка
    else if (index == 0 && count != 0) return head;
    // В противном случае
    else
    {
        // Нахождение интервального индекса
        int intervalIndex = index / Interval;
        int leftEnd = lst.GetIndex(intervalIndex);
        int rightEnd = lst.GetIndex(intervalIndex + 1);
        Node target = lst.GetNode(leftEnd);

        // Проверка, что есть следующий интервал в списке
        if (intervalIndex + 1 < lst.Count)
        {
            // Нахождение середины интервала
            int mid = (leftEnd + (leftEnd + Interval)) / 2;
            
            // Корректировка середины для первого интервала
            if (intervalIndex == 0) 
                mid = Interval - 1 / 2;

            // Если индекс больше, чем середина
            if (index > mid)
            {
                // Перемещение по узлам от конца к началу
                target = lst.GetNode(rightEnd);
                for (int i = rightEnd; i > index; i--)
                {
                    target = target.Previous;
                }
                return target;
            }
        }

        // Перемещение по узлам от начала к указанному индексу
        for (int i = leftEnd; i < index; i++)
        {
            target = target.Next;
        }
        return target;
    }
}

// Создаем новый узел
Node newNode = new Node(data);

// Если список пустой, присваиваем новый узел голове списка и добавляем его в линейный список
if (head == null)
{
    head = newNode;
    lst.Append(head, count);
    count++;
}
else
{
    // Находим последний узел в списке
    Node last = Find(count - 1);
    Node current = newNode;
    
    // Устанавливаем ссылки между последним и новым узлом
    last.Next = current;
    current.Previous = last;
    count++;

    // Если количество элементов кратно интервалу, добавляем текущий элемент в линейный список
    if (count % Interval == 0)
    {
        lst.Append(current, count - 1);
    }
    
    // Если предыдущее количество элементов было кратно интервалу, устанавливаем узел предыдущего элемента
    if ((count - 1) % Interval == 0)
    {
        lst.SetNode(current.Previous, (count - 2));
    }
}

// Публичный метод для вставки элемента в указанный индекс
public void Insert(int data, int index)
{
    // Проверка на корректность индекса
    if (index < 0 || index > count) { return; }

    // Если индекс совпадает с количеством элементов в списке, то вызывается метод Add
    if (index == count) { Add(data); }

    // Если вставка не в конец списка
    else
    {
        // Создание нового узла
        Node newNode = new Node(data);

        // Если вставка в начало списка
        if (index == 0)
        {
            // Обновление связей узлов
            newNode.Next = head;
            head.Previous = newNode;
            head = newNode;
            lst[0] = head;
            UpdateIndexNodesAfterInsert(index);
            count++;
        }
        else
        {
            // Нахождение предыдущего и текущего узлов для вставки
            Node previous = Find(index - 1);
            Node current = Find(index);
            // Обновление связей узлов
            newNode.Next = current;
            newNode.Previous = previous;
            previous.Next = newNode;
            current.Previous = newNode;
            
            // Обновление индексного списка в зависимости от условий
            if ((index + 2) % Interval == 0 && count >= index + 2)
            {
                lst.SetNode(current.Next, index + 1);
            }
            if (index % Interval == 0)
            {
                lst.SetNode(previous, index - 1);
            }
            if ((index + 1) % Interval == 0)
            {
                // Если вставка в позицию, соответствующую индексу в индексном списке
                if (count == (index + 1))
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

// Метод для удаления элемента по указанному индексу
public void RemoveAt(int index)
{
    // Проверка на корректность индекса
    if (index < 0 || index >= count)
    {
        return;
    }

    // Удаление элемента в начале списка
    if (index == 0)
    {
        // Если в списке только один элемент
        if (count == 1)
        {
            head = null;
            lst.Delete(0);
            count--;
        }
        // Если в списке больше одного элемента
        else
        {
            head = head.Next;
            head.Previous = null;
            lst[0] = head;
            UpdateIndexNodesAfterRemove(index);
            count--;
        }
    }
    // Удаление элемента в середине или конце списка
    else
    {
        // Находим целевой узел для удаления и его предыдущий узел
        Node target = Find(index);
        Node previous = target.Previous;
        previous.Next = target.Next;

        // Обновляем связи между узлами
        if (previous.Next != null)
        {
            Node newCurrent = previous.Next;
            newCurrent.Previous = previous;

            // Обновляем список узлов при необходимости
            if ((index + 2) % Interval == 0 && count >= index + 2)
            {
                lst.SetNode(newCurrent, index + 1);
            }

            // Обновляем список узлов при необходимости
            if ((index + 1) % Interval == 0)
            {
                lst.SetNode(previous, index);
            }
        }

        // Обновляем список узлов при необходимости
        if (index % Interval == 0)
        {
            lst.SetNode(previous, index - 1);
        }

        // Удаляем последний элемент в списке, если необходимо
        if ((index + 1) % Interval == 0 && count == (index + 1))
        {
            lst.Delete(lst.Find(index));
            count--;
            return;
        }

        // Обновляем связи узлов после удаления
        UpdateIndexNodesAfterRemove(index);
        count--;
    }
}

// Обновление индексов узлов после удаления элемента
private void UpdateIndexNodesAfterRemove(int index)
{
    // Вычисление сдвига индекса
    int shiftIndex = (index / Interval) + 1;

    // Если количество элементов делится на интервал без остатка, удаляем последний элемент
    if (count % Interval == 0)
    {
        lst.Delete(lst.Count - 1);
    }

    // Обновление индексов после удаления элемента
    for (int i = shiftIndex; i < lst.Count; i++)
    {
        lst[i] = lst[i].Next;
    }
}

// Обновление индексов узлов после вставки элемента
private void UpdateIndexNodesAfterInsert(int index)
{
    // Вычисление сдвига индекса
    int shiftIndex = (index / Interval) + 1;
    
    // Если после вставки элемента количество элементов станет кратным интервалу, добавляем новый элемент
    if ((count + 1) % Interval == 0)
    {
        lst.Append(Find(count), count);
    }

    // Обновление индексов после вставки элемента
    for (int i = shiftIndex; i < lst.Count; i++)
    {
        lst[i] = lst[i].Previous;
    }
}
