﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerManager : Singleton<PlayerManager> {

    [SerializeField] private float maxSpeed = 10f;
	[SerializeField] private float jumpForce = 500f;
	[SerializeField] private float healthPoints = 100f;
	[SerializeField] private float maxHealthPoints = 100f;
	[SerializeField] private GameObject fireball;
	[SerializeField] private Transform groundCheck;

	private int playerLevel = 1;
	private float expPoints = 0f;
    private float expPointsNeeded = 0;
	private int availableStatPoints = 0;
	private int availablePerkPoint = 0; 
	private bool grounded = false;
	private bool facingRight = true;
	private float groundRadius = 0.2f;
	private float move;
	private Rigidbody2D rb2d;
    private Animator anim;
	private float fallMultiplier = 2.5f;
	private float lowJumpMultiplier = 2f;

	public LayerMask whatIsGround;

	void Awake(){
		// assertions
		Assert.AreNotEqual(0, maxSpeed);
		Assert.AreNotEqual(0, jumpForce);
		Assert.AreNotEqual(0, healthPoints);
		Assert.IsNotNull(fireball);
		Assert.IsNotNull(groundCheck);
	}

    void Start(){
		rb2d = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		expPointsNeeded = Mathf.Round((4 * Mathf.Pow(playerLevel+1,3)) / 5); 
	}

	void FixedUpdate(){
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
		anim.SetBool("isGrounded", grounded);

		anim.SetFloat("vSpeed", rb2d.velocity.y);

        // I might need to move this to the InputManager after I create movement UI button
        if(!grounded) return;

		// fall speed
		if(rb2d.velocity.y < 0){
			rb2d.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
		}else if(rb2d.velocity.y > 0 && !Input.GetKeyDown(InputManager.Instance.jump)){
			rb2d.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
		}

		move = Input.GetAxis("Horizontal");
		
		anim.SetFloat("xSpeed", Mathf.Abs(move));
		rb2d.velocity = new Vector2(move * maxSpeed, rb2d.velocity.y);
		
		if(move > 0 && !facingRight){ // if moving right and not facing right
			Flip();
		}else if(move < 0 && facingRight){	// else if move left and not facing left
			Flip();
		}

	}

	void Update(){

		if(groundCheck && Input.GetKeyDown(KeyCode.Alpha1)){
			Ranged();
		}
	}


    public void Run(){
    }
    void Flip(){
		facingRight = !facingRight; // now facing oposite direction
		Vector3 theScale = transform.localScale; // get the local scale
		theScale.x *= -1; // flip the x axis
		transform.localScale = theScale; // then apply it back to local scale
	}

	public void Punch(){
        if(groundCheck){
			anim.SetTrigger("isPunching");
		}
	}

	public void Kick(){
		if(grounded){
			anim.SetTrigger("isKicking");
		}else{
			anim.SetTrigger("isJumpKicking");
		}
	}

	public void Ranged(){
		GameObject newFireball = Instantiate(fireball) as GameObject;
		newFireball.transform.position = transform.position;
		StartCoroutine(LaunchFireBall(newFireball));
		anim.SetTrigger("isRanged");
	}

	public void Jump(){
		if(grounded){
			anim.SetBool("isGrounded", false);
			rb2d.AddForce(new Vector2(0, jumpForce));
		}
	}

	public void Crouch(){
		if(move != 0 && grounded){
			anim.SetTrigger("isSlidding");
			rb2d.AddForce(new Vector2((500f * move), 0));
		}
	}

	IEnumerator LaunchFireBall(GameObject fireball){
		Rigidbody2D fireballrb2d = fireball.GetComponent<Rigidbody2D>();
		fireballrb2d.AddForce(new Vector2(10, 0));
		yield return null;
	}

	public void PlayerHurt(int hitPoints){
		if(healthPoints - hitPoints > 0){
			healthPoints -= hitPoints;
			// anim.Play("Hurt");
		}
		/*else{
			anim.SetTrigger(didDie);
			Die();
		} */
	}

	public void Die(){

	}

	public void AddExperiencePoints(float exp){
		expPoints += exp;	
		CheckLevelUp(expPoints);
	}

	void CheckLevelUp(float currentExp){
		if(currentExp >= expPointsNeeded){
			playerLevel += 1;
			availablePerkPoint += 1;
			expPointsNeeded = Mathf.Round((4 * Mathf.Pow(playerLevel+1,3)) / 5);
			// increase difficulty: Math.round(((4+difficulty) * Math.pow(level+1,3)) / (5-difficulty)); difficulty = 0-2
			CheckLevelUp(currentExp);
		}
	}

}