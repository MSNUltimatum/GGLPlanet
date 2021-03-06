﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ProjectileLife : NetworkBehaviour
{
    public float lifeSpan = 1.2f;
    public float Speed = 0.05f;
    public GameObject owner;

    List<int> alreadyHit = new List<int>();

    [SerializeField]
    private GameObject hitEffect = null;

    // Update is called once per frame
    void FixedUpdate()
    {
        lifeSpan -= Time.deltaTime;
        transform.Translate(Vector2.right * Speed);
        if (lifeSpan < 0)
        {
            NetworkServer.Destroy(gameObject);
            return;
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Enemy")
        {
            if (!alreadyHit.Contains(coll.gameObject.GetInstanceID()))
            {
                alreadyHit.Add(coll.gameObject.GetInstanceID());
                AttackInformation attack = new AttackInformation(owner, 1);
                coll.gameObject.GetComponent<StatsManager>().DealDamage(attack);
                var hitEff = GameObject.Instantiate(hitEffect, transform.position - transform.forward * 0.5f, transform.rotation);
                Destroy(hitEff, 1);
            }
        }
        
        if (coll.gameObject.tag == "Environment") {
            NetworkServer.Destroy(gameObject);
        }
    }
}
