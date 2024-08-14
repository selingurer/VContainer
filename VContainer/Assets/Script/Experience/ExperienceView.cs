using DG.Tweening;
using System;
using UnityEngine;

public class ExperienceView : MonoBehaviour
{
    public Action<ExperienceView> ReturnToPoolExperienceAction;
    public Action<int> ExperienceClaim;
    private int _experienceValue = 10;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerView>() != null)
        {
            ExperienceClaim?.Invoke(_experienceValue);
            Sequence mySequence = DOTween.Sequence();
            this.GetComponent<BoxCollider2D>().enabled = false;
            mySequence.Append(this.gameObject.transform.DOScale(0.6f, 0.2f)).Append(this.gameObject.transform.DOMoveY(transform.position.y + 10, 0.5f))
                .Join(this.GetComponent<SpriteRenderer>().DOFade(0, 0.5f)).OnComplete(() =>
                {
                    this.GetComponent<BoxCollider2D>().enabled = true;
                    this.GetComponent<SpriteRenderer>().DOFade(1, 0);
                    this.gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    ReturnToPoolExperienceAction?.Invoke(this);
                });
        }
    }

}
