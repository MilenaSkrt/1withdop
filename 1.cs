// Метод для поиска индекса элемента в IndexList перебором
public int FindByBruteForce(int index)
{
    for (int i = 0; i < count; i++)
    {
        if (buffer[i].index == index) { return i; }
    }
    throw new IndexOutOfRangeException();
}
