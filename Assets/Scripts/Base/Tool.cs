using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;
/// <summary>
/// 均值数组
/// </summary>
public class AverageList
{
    private LinkedList<int> _list;
    private int _capacity;
    private int[] _prefixSum;
    private int _size;
    private int _currentIndex;

    public AverageList(int P)
    {
        _capacity = P;
        _list = new LinkedList<int>();
        _prefixSum = new int[P];
        _size = 0;
        _currentIndex = 0;
    }

    public void AddFront(int value)
    {
        if (_size >= _capacity)
        {
            // Remove the oldest element from the back
            int removedValue = _list.Last.Value;
            _list.RemoveLast();
            UpdatePrefixSumOnRemoval(removedValue);
        }

        // Add the new element to the front
        _list.AddFirst(value);
        UpdatePrefixSumOnAddition(value);
    }

    public void RemoveEnd()
    {
        if (_size > 0)
        {
            // Remove the oldest element from the end
            int removedValue = _list.Last.Value;
            _list.RemoveLast();
            UpdatePrefixSumOnRemoval(removedValue);
        }
    }

    public double CalculateMean()
    {
        if (_size == 0)
            return 0.0;

        // Calculate the sum of the first P elements
        int sum = _prefixSum[(_currentIndex - 1 + _capacity) % _capacity];
        return (double)sum / Math.Min(_size, _capacity);
    }

    public int? PeekFront()
    {
        if (_list.Count == 0)
            return null; // or throw an exception if you prefer

        return _list.First.Value;
    }

    public void UpdateFront(int newValue)
    {
        if (_list.Count == 0)
            throw new InvalidOperationException("The queue is empty.");

        // Update the front element
        int oldValue = _list.First.Value;
        _list.First.Value = newValue;

        // Update the prefix sums
        UpdatePrefixSumOnUpdateFront(oldValue, newValue);
    }

    private void UpdatePrefixSumOnAddition(int value)
    {
        if (_size < _capacity)
        {
            // Only update the prefix sum array if we haven't filled it yet
            if (_currentIndex > 0)
                _prefixSum[_currentIndex] = _prefixSum[_currentIndex - 1] + value;
            else
                _prefixSum[_currentIndex] = value;
        }
        else
        {
            // Circular buffer behavior
            _prefixSum[_currentIndex] = _prefixSum[(_currentIndex - 1 + _capacity) % _capacity] + value;
        }

        _currentIndex = (_currentIndex + 1) % _capacity;
        _size++;
    }

    private void UpdatePrefixSumOnRemoval(int removedValue)
    {
        if (_size == 0)
            return;

        if (_size <= _capacity)
        {
            if (_currentIndex > 0)
                _prefixSum[_currentIndex - 1] -= removedValue;
        }
        else
        {
            int removedIndex = (_currentIndex - 1 + _capacity) % _capacity;
            _prefixSum[removedIndex] -= removedValue;
        }

        _size--;
    }

    private void UpdatePrefixSumOnUpdateFront(int oldValue, int newValue)
    {
        // Update prefix sum array
        if (_size <= _capacity)
        {
            // Update only the relevant prefix sums
            if (_currentIndex > 0)
                _prefixSum[_currentIndex - 1] += newValue - oldValue;
        }
        else
        {
            int frontIndex = (_currentIndex - _size + _capacity) % _capacity;
            _prefixSum[frontIndex] += newValue - oldValue;
        }
    }
}
public class Tool
{
    public static T DeepClone<T>(T obj)
    {
        // 序列化对象
        var serializedData = SerializationUtility.SerializeValue(obj, DataFormat.Binary);

        // 反序列化对象
        T clonedObject = SerializationUtility.DeserializeValue<T>(serializedData, DataFormat.Binary);

        return clonedObject;
    }
    public static bool IsSameClass(object obj1, object obj2)
    {
        return obj1.GetType() == obj2.GetType();
    }
}
/// <summary>
/// Int的引用类型
/// </summary>
public class Int
{
    public int val;
    public Int(int val=0)
    {
        this.val = val;
    }
    // 隐式转换：从 int 到 IntWrapper
    public static implicit operator Int(int intValue)
    {
        return new Int(intValue);
    }

    // 隐式转换：从 IntWrapper 到 int
    public static implicit operator int(Int intWrapper)
    {
        return intWrapper.val;
    }
}
