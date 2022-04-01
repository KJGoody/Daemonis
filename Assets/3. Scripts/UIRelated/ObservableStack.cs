using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 델리게이트
public delegate void UpdateStackEvent();

public class ObservableStack<T> : Stack<T>
{
    // 이벤트
    public event UpdateStackEvent OnPush;
    public event UpdateStackEvent OnPop;
    public event UpdateStackEvent OnClear;


    // 아이템을 Stack 배열에 넣을때
    public new void Push(T item)
    {
        // 원래 기능을 작동시키고
        base.Push(item);

        if (OnPush != null)
        {
            // OnPush 에 등록된 함수를 호출한다.
            OnPush();
        }
    }

    // 아이템을 Stack 배열에서 꺼낼때
    public new T Pop()
    {
        // 원래 기능을 작동시키고
        T item = base.Pop();

        // OnPop 에 등록된 함수를 호출한다.
        if (OnPop != null)
        {
            OnPop();
        }

        return item;
    }

    // Stack 배열을 초기화
    public new void Clear()
    {
        // 원래 기능을 작동시키고
        base.Clear();

        if (OnClear != null)
        {
            // OnClear 에 등록된 함수를 호출한다.
            OnClear();
        }
    }
}