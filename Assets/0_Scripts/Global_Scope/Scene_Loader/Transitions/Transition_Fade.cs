using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Transition_Fade : SceneTransitionPlayer
{
    [Header("References")]
    [SerializeField] private Image _panel;

    [Header("Settings")]
    [SerializeField] private Color _startColor;
    [SerializeField] private Color _endColor;
    [SerializeField] private float _fadeDuration = 1f;

    private Color _startColorTransparent => new Color(_startColor.r, _startColor.g, _startColor.b, 0f);
    private Color _endColorOpaque => new Color(_endColor.r, _endColor.g, _endColor.b, 1f);

    protected override void InitAnimation()
    {
        if (_panel == null) throw new Exception("Panel is not defined. Animation will not play.");
        _panel.color = _startColorTransparent;
    }

    protected override async Task PlayStart()
    {
        Tween fadeIn = _panel.DOBlendableColor(_endColorOpaque, _fadeDuration);
        await fadeIn.AsyncWaitForCompletion();
        fadeIn.Kill();
    }


    protected override async Task PlayEnd()
    {
        Tween fadeOut = _panel.DOBlendableColor(_startColorTransparent, _fadeDuration);
        await fadeOut.AsyncWaitForCompletion();
    }

    protected override void EndAnimation()
    {
        Destroy(this.gameObject);
    }
}
