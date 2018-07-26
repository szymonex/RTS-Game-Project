﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //w tej bibliotece mamy nawigacje

public class Unit : MonoBehaviour {

    const string ANIMATOR_SPEED = "Speed",
        ANIMATOR_ALIVE = "Alive",
        ANIMATOR_ATTACK = "Attack";

    public Transform target;

    NavMeshAgent nav;
    Animator animator;

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {

        if (target)
        {
            nav.SetDestination(target.position);
        }
        Animate();

	}

    protected virtual void Animate()
    {
        var speedVector = nav.velocity;
        speedVector.y = 0; //po to zeby wyzerowac y ktory czesto jest dosc przypadkowy i moze namieszac w animacji

        float speed = speedVector.magnitude;

        animator.SetFloat(ANIMATOR_SPEED, speed);
    }

    
}
