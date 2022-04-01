using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��������Ʈ
public delegate void UpdateStackEvent();

public class ObservableStack<T> : Stack<T>
{
    // �̺�Ʈ
    public event UpdateStackEvent OnPush;
    public event UpdateStackEvent OnPop;
    public event UpdateStackEvent OnClear;


    // �������� Stack �迭�� ������
    public new void Push(T item)
    {
        // ���� ����� �۵���Ű��
        base.Push(item);

        if (OnPush != null)
        {
            // OnPush �� ��ϵ� �Լ��� ȣ���Ѵ�.
            OnPush();
        }
    }

    // �������� Stack �迭���� ������
    public new T Pop()
    {
        // ���� ����� �۵���Ű��
        T item = base.Pop();

        // OnPop �� ��ϵ� �Լ��� ȣ���Ѵ�.
        if (OnPop != null)
        {
            OnPop();
        }

        return item;
    }

    // Stack �迭�� �ʱ�ȭ
    public new void Clear()
    {
        // ���� ����� �۵���Ű��
        base.Clear();

        if (OnClear != null)
        {
            // OnClear �� ��ϵ� �Լ��� ȣ���Ѵ�.
            OnClear();
        }
    }
}