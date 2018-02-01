using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

	[SerializeField] private int attackStrength;
	public Rigidbody2D rb2d;
	private Animator anim;
	private float velocity;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		anim.Play("Fireball");
	}
	
	// Update is called once per frame
	void Update () {
		// will need to destroy either on enemy hit, or when certain distance from player
		transform.localPosition = Vector2.right;
	}

}
