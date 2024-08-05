using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class Experience : MonoBehaviour
{
    private ExperienceService _experienceService;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Player>() != null)
        {
            _experienceService.SetExperienceValue(10);  
            _experienceService.ReturnToExperiencePool(this);
        }

    }
    [Inject]
    private void Construct(ExperienceService service)
    {
        _experienceService = service;
    }

}
