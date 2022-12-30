using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public interface IValueAnimator<T>
{
    public Task AnimateValue(T value);
}
