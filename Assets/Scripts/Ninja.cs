using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Ninja : MonoBehaviour {

    [SerializeField] private float maxSpeed = 10f;
	[SerializeField] private float jumpForce = 500f;
	[SerializeField] private float healthPoints = 100f;
	[SerializeField] private GameObject fireball;
	[SerializeField] private Transform groundCheck;

	private float expPoints = 0f;
    private bool grounded = false;
	private bool facingRight = true;
	private float groundRadius = 0.2f;
	private Rigidbody2D rb2d;
    private Animator anim;

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
	}

	void FixedUpdate(){
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
		anim.SetBool("isGrounded", grounded);

		anim.SetFloat("vSpeed", rb2d.velocity.y);

		if(!grounded) return;

		float move = Input.GetAxis("Horizontal");
		
		anim.SetFloat("xSpeed", Mathf.Abs(move));
		rb2d.velocity = new Vector2(move * maxSpeed, rb2d.velocity.y);
		
		if(move > 0 && !facingRight){ // if moving right and not facing right
			Flip();
		}else if(move < 0 && facingRight){	// else if move left and not facing left
			Flip();
		}
	}

	void Update(){
		if(grounded && Input.GetKeyDown(KeyCode.UpArrow)){ // not best method
			anim.SetBool("isGrounded", false);
			rb2d.AddForce(new Vector2(0, jumpForce));
		}

		if(groundCheck && Input.GetKeyDown(KeyCode.Space)){
			anim.SetTrigger("isAttacking");
		}
		if(groundCheck && Input.GetKeyDown(KeyCode.Alpha1)){
			Attack();
		}
	}

	void Flip(){
		facingRight = !facingRight; // now facing oposite direction
		Vector3 theScale = transform.localScale; // get the local scale
		theScale.x *= -1; // flip the x axis
		transform.localScale = theScale; // then apply it back to local scale
	}

	private void Attack(){
		GameObject newFireball = Instantiate(fireball) as GameObject;
		newFireball.transform.position = transform.position;
		StartCoroutine(LaunchFireBall(newFireball));
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

}