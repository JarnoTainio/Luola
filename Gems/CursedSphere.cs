using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursedSphere : MonoBehaviour
{
  public CombatManager combatManager;
  public float speed;

  Vector3 target;
  GemEffect curse;
  int value;

  bool active;

  void Update()
  {
    if (!active)
    {
      return;
    }

    transform.position = Vector2.MoveTowards(transform.position, target, Time.deltaTime * speed);
    if (Vector2.Distance(transform.position, target) < 0.05f)
    {
      combatManager.SphereArrived(this, curse, value);
      Destroy(gameObject);
    }
  }


  public void SetTarget(Vector3 target, GemEffect curse = GemEffect.None, bool adjustTarget = false)
  {
    this.curse = curse;
    this.target = adjustTarget ? Camera.main.ScreenToWorldPoint(target) : target;
    value = 2;
    //this.target = Camera.main.ScreenToWorldPoint(target);
    //ParticleSystem.MainModule ma = GetComponent<ParticleSystem>().main;
    active = true;
  }
}
