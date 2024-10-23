using System;
using System.Collections;
using UnityEngine;

public class ListaSimple<T>
{
    private T[] array;
    private int length;
    private int capacity;

    public ListaSimple()
    {
        capacity = 4;
        array = new T[capacity];
        length = 0;
    }

    public void InsertAtEnd(T item)
    {
        if (length == capacity)
        {
            T[] newArray = new T[capacity * 2];
            for (int i = 0; i < length; i++)
            {
                newArray[i] = array[i];
            }
            array = newArray;
            capacity = capacity * 2;
        }
        array[length] = item;
        length++;
    }

    public T GetNodeAtPosition(int index)
    {
        if (index < 0 || index >= length)
        {
            throw new IndexOutOfRangeException("El índice está fuera de los límites.");
        }
        return array[index];
    }

    public int Length
    {
        get { return length; }
    }
}
