using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using UnityEngine;
using VContainer;

public class Experience : MonoBehaviour
{
    private ExperienceService _experienceService;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            _experienceService.SetExperienceValue(10);
            Sequence mySequence = DOTween.Sequence();
            this.GetComponent<BoxCollider2D>().enabled = false;
            mySequence.Append(this.gameObject.transform.DOScale(0.6f, 0.2f)).Append(this.gameObject.transform.DOMoveY(transform.position.y + 10, 0.5f))
                .Join(this.GetComponent<SpriteRenderer>().DOFade(0, 0.5f)).OnComplete(() =>
                {
                    this.GetComponent<BoxCollider2D>().enabled = true;
                    this.GetComponent<SpriteRenderer>().DOFade(1, 0);
                    this.gameObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    _experienceService.ReturnToExperiencePool(this);
                });
        }
    }


    [Inject]
    private void Construct(ExperienceService service)
    {
        _experienceService = service;
    }

}
