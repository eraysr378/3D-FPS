using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDamageText : MonoBehaviour
{
    private enum ShotType
    {
        Headshot,
        Bodyshot,
        Legshot
    }
    [SerializeField] private ShotType shotType;
    [SerializeField] private float duration;
    private Animator animator;
    private float timer;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        switch (shotType)
        {
            case ShotType.Headshot:
                animator.SetBool("IsHeadshot", true);
                break;
            case ShotType.Bodyshot:
                animator.SetBool("IsBodyshot", true);
                break;
            case ShotType.Legshot:
                animator.SetBool("IsLegshot", true);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > duration)
        {
            Destroy(gameObject);
        }
    }
}
