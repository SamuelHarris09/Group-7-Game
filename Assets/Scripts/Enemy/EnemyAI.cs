using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] GameObject Hitbox;
    [SerializeField] Transform player;
    [SerializeField] bool particles;
    [SerializeField] ParticleSystem backWheelParticles;
    [SerializeField] ParticleSystem frontWheelParticles;

    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float agroRange = 5f;

    //[SerializeField] Animator anim;

    private void Start()
    {
        if (particles == true) 
        { 
            backWheelParticles.Play();
            frontWheelParticles.Play();
        }
    }

    private void Update()
    {
        float distToPlayer = Vector2.Distance(transform.position, player.position);

        if (distToPlayer < agroRange)
        {
            ChasePlayer();

            //anim.SetBool("isAttacking", true);
        }
        else
        {
            //anim.SetBool("isAttacking", false);
        }

        transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
    }

    void ChasePlayer()
    {
        //rotate towards player
        if(transform.position.x < player.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        int layerIndexC = LayerMask.NameToLayer("Wall");

        if (other.gameObject.layer == layerIndexC)
        {
            transform.Rotate(0, 180, 0);
            Debug.Log("Hit wall");
        }
    }
}