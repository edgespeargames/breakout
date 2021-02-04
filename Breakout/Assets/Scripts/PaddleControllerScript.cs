using UnityEngine;
public class PaddleControllerScript : MonoBehaviour, IPowerUp
{
    public Animator anim;

    public Vector2 originalTransform;
    private void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
        originalTransform = transform.position;
    }

    public void NoPowerUp()
    {
        anim.SetBool("Fire", false);
    }
    public void FirePowerUp()
    {
        anim.SetBool("Fire", true);
    }
    public void MultiplierPowerUp(float multiplier)
    {
    }
    public void LivesTrigger(int lives)
    {
        switch (lives)
        {
            case 1:
                anim.SetTrigger("OneLife");
                break;
            case 2:
                anim.SetTrigger("TwoLives");
                break;
            case 3:
                anim.SetTrigger("ThreeLives");
                break;
            default:
                //anim.SetTrigger("ThreeLives");
                break;
        }
    }
    public void ResetPosition()
    {
        transform.position = originalTransform;
        transform.rotation = Quaternion.identity;
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Fire"))
        {
            SceneControllerScript.Instance.RemovePowerUp(col.gameObject);
            Destroy(col.gameObject);
            SceneControllerScript.Instance.BeginPowerUp(PowerUpEnum.Fire);
        }
        if (col.gameObject.CompareTag("Life"))
        {
            SceneControllerScript.Instance.RemovePowerUp(col.gameObject);
            Destroy(col.gameObject);
            SceneControllerScript.Instance.AddLife();
        }
        if (col.gameObject.CompareTag("x2"))
        {
            SceneControllerScript.Instance.RemovePowerUp(col.gameObject);
            Destroy(col.gameObject);
            SceneControllerScript.Instance.SetMultiplier();
            Brick.SetMultiplier(2);
        }
    }
}
