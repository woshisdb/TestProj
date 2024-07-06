using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularQueue<T>
{
    private T[] _queue;
    private int _head;
    private int _tail;
    private int _count;
    private int _capacity;

    public CircularQueue(int capacity)
    {
        _capacity = capacity;
        _queue = new T[_capacity];
        _head = 0;
        _tail = 0;
        _count = 0;
    }

    public int Count => _count;

    public bool IsEmpty => _count == 0;

    public bool IsFull => _count == _capacity;

    public void Enqueue(T item)
    {
        _queue[_tail] = item;
        _tail = (_tail + 1) % _capacity;
        _count++;
    }

    public T Dequeue()
    {
        T item = _queue[_head];
        _head = (_head + 1) % _capacity;
        _count--;
        return item;
    }

    public T Peek()
    {
        return _queue[_head];
    }
    /// <summary>
    /// 计算平均值
    /// </summary>
    /// <param name="buffer"></param>
    /// <returns></returns>
    public static double CalculateAverage(CircularQueue<int> buffer)
    {
        double sum = 0;
        for (int i = 0; i < buffer.Count; i++)
        {
            sum += buffer._queue[(buffer._head + i) % buffer._capacity];
        }
        return sum / buffer.Count;
    }
    /// <summary>
    /// 预测下个值
    /// </summary>
    /// <param name="buffer"></param>
    /// <returns></returns>
    public static double PredictNextValue(CircularQueue<int> buffer)
    {

        double sumX = 0;
        double sumY = 0;
        double sumXY = 0;
        double sumX2 = 0;
        int n = buffer.Count;

        for (int i = 0; i < n; i++)
        {
            int x = i;
            int y = buffer._queue[(buffer._head + i) % buffer._capacity];
            sumX += x;
            sumY += y;
            sumXY += x * y;
            sumX2 += x * x;
        }

        double slope = ((n * sumXY) - (sumX * sumY)) / ((n * sumX2) - (sumX * sumX));
        double intercept = ((sumY * sumX2) - (sumX * sumXY)) / ((n * sumX2) - (sumX * sumX));

        return slope * n + intercept;
    }
}

