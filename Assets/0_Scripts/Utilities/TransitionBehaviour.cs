using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public abstract class TransitionBehaviour : MonoBehaviour
{
    public async Task EnterTransition()
    {
        InitAnimation();
        await PlayStart();
    }

    public async Task ExitTransition()
    {
        await PlayEnd();
        EndAnimation();
    }

    protected abstract void InitAnimation();
    protected abstract Task PlayStart();

    protected abstract Task PlayEnd();
    protected abstract void EndAnimation();
}
