using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using System.Threading.Tasks;
using DG.Tweening;

public class InstanceVolumeAnimator : IValueAnimator<EventInstance>
{
    private float _duration;
    private float _startvolume;
    private float _currentVolume;
    private float _targetVolume;

    private Tween _volumeTween;
    public InstanceVolumeAnimator(float targetValue, float duration, float overrideStart = -1f)
    {
        _startvolume = overrideStart;
        _targetVolume = targetValue;
        _duration = duration;
        _volumeTween = null;
    }

    public async Task AnimateValue(EventInstance value)
    {
        if (_volumeTween == null)
        {
            if (_startvolume != -1f) value.setVolume(_startvolume);

            value.getVolume(out _currentVolume);

            _volumeTween = DOTween.To(() => _currentVolume, x => _currentVolume = x, _targetVolume, _duration);
        }

        while (_currentVolume != _targetVolume)
        {
            value.setVolume(_currentVolume);
            await Task.Yield();
        }
        _volumeTween.Kill();
    }
}
