using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    public CombatManager combatManager;
    public float speed;

    Vector3 target;
    Vector3 nextTarget;
    public GemColor gem;
    int value;

    bool active;
    bool useToken;
    float potentialShowSpeedModifier = 2f;

    void Update()
    {
        if (!active)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);

        if (Vector3.Distance(transform.position, target) < 0.05f)
        {
            if (nextTarget == Vector3.zero){
                combatManager.SphereArrived(this, gem, value, useToken);
                Destroy(gameObject);
            }else{
                target = nextTarget;
                nextTarget = Vector3.zero;
                speed *= potentialShowSpeedModifier * 1.3f;
            }
        }
    }

    public void Set(Vector3 target, Color color, GemColor gemColor, int value, bool useToken)
    {
        this.gem = gemColor;
        this.value = value;
        this.target = Camera.main.ScreenToWorldPoint(target);
        this.target = new Vector3(this.target.x, this.target.y, 0);
        this.useToken = useToken;
        ParticleSystem.MainModule ma = GetComponent<ParticleSystem>().main;
        ma.startColor = color;
        active = true;
        if (nextTarget != Vector3.zero){
            Vector3 temp = nextTarget;
            nextTarget = this.target;
            this.target = temp;
        }
    }

    public void SetFirstTarget(Vector3 vect){
        nextTarget = vect;
        speed /= potentialShowSpeedModifier;
    }
}
