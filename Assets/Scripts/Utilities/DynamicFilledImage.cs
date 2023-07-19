using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DynamicFilledImage : MonoBehaviour
{
    public Image image;
    Sequence fillSq;
    public float FillAmount { get { return image.fillAmount; } set { image.fillAmount = value; } }
    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void SetFill(float fill)
    {
        if (image == null)
            image = GetComponent<Image>();

        float target = fill;
        fillSq?.Kill();
        fillSq = DOTween.Sequence();
        fillSq.Append(DOTween.To(
            () => image.fillAmount,
            x =>
            {
                image.fillAmount = x;
            },
            target,
            1f));
    }
    public void SetFill(float fill, bool linear) {
        if (image == null)
            image = GetComponent<Image>();

        float target = fill;
        fillSq?.Kill();
        fillSq = DOTween.Sequence();
        fillSq.Append(DOTween.To(
            () => image.fillAmount,
            x => {
                image.fillAmount = x;
            },
            target,
            1f).SetEase(Ease.InCubic));
    }
    public void SetFill(float fill, System.Action callback) {
        if (image == null)
            image = GetComponent<Image>();

        float target = fill;
        fillSq?.Kill();
        fillSq = DOTween.Sequence();
        fillSq.Append(DOTween.To(
            () => image.fillAmount,
            x => {
                image.fillAmount = x;
            },
            target,
            (1 + 1*fill)).OnComplete(()=>callback()));
    }
    public void SetFill(float fill, float time) {
        if (image == null)
            image = GetComponent<Image>();

        float target = fill;
        fillSq?.Kill();
        fillSq = DOTween.Sequence();
        fillSq.Append(DOTween.To(
            () => image.fillAmount,
            x => {
                image.fillAmount = x;
            },
            target,
            time).SetEase(Ease.InCubic));
    }

    public void SetFill(float fill, float time, bool linear) {
        if (image == null)
            image = GetComponent<Image>();

        float target = fill;
        fillSq?.Kill();
        fillSq = DOTween.Sequence();
        fillSq.Append(DOTween.To(
            () => image.fillAmount,
            x => {
                image.fillAmount = x;
            },
            target,
            time).SetEase(Ease.Linear));
    }
    public void SetFill(float fill, float time, System.Action callback) {
        if (image == null)
            image = GetComponent<Image>();

        float target = fill;
        fillSq?.Kill();
        fillSq = DOTween.Sequence();
        fillSq.Append(DOTween.To(
            () => image.fillAmount,
            x => {
                image.fillAmount = x;
            },
            target,
            time).SetEase(Ease.Linear).OnComplete(() => callback()));
    }

    public void KillFill() {
        fillSq?.Kill();
    }
}
