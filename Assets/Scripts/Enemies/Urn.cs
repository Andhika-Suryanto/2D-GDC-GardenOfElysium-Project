using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Urn : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject urnProjectilePrefab;

    private Animator myAnimator;
    private SpriteRenderer spriteRenderer;

    readonly int ATTACK_HASH = Animator.StringToHash("Attack");

    private void Awake() {
        myAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Attack() {
        myAnimator.SetTrigger(ATTACK_HASH);

        if (transform.position.x - PlayerController.Instance.transform.position.x < 0) {
            spriteRenderer.flipX = false;
        } else {
            spriteRenderer.flipX = true;
        }
    }

    public void SpawnProjectileAnimEvent() {
        Instantiate(urnProjectilePrefab, transform.position, Quaternion.identity);
    }
}
