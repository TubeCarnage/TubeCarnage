using UnityEngine;
using System.Collections;

public abstract class BasePowerup : MonoBehaviour
{
    [Header("Base")]
    public float Cooldown = 10.0f;
    public float Duration = 5.0f;

    //EVENTS
    protected abstract void OnActivate();
    protected abstract void OnReset();
    protected abstract void OnEffectBegin();
    protected abstract void OnEffectEnd();

    protected Player Target;
    protected bool PendingEffect = false;
    protected BoxCollider Trigger;

    public void _Start()
    {
        Trigger = GetComponent<BoxCollider>();
    }

    public void _OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponentInParent<Player>();
        if (player)
        {
            //Set target player
            Target = player;
            OnActivate();

            //Disable trigger
            Trigger.enabled = false;
            Invoke("ResetCooldown", Cooldown);
        }
    }

    private void ResetCooldown()
    {
        Trigger.enabled = true;
        OnReset();
    }

    private void EndEffect()
    {
        OnEffectEnd();
        Target = null;
        PendingEffect = false;
    }

    public void _Update()
    {
        if (Target && !PendingEffect && Input.GetButton("PowerUp"))
        {
            OnEffectBegin();
            PendingEffect = true;
            Invoke("EndEffect", Duration);
        }
    }
}