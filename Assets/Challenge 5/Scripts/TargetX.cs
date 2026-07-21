using System.Collections;
using UnityEngine;

public class TargetX : MonoBehaviour
{
    private GameManagerX gameManagerX;
    public int PointValue;
    public GameObject ExplosionFx;

    public float TimeOnScreen = 1.0f;

    void Start()
    {
        gameManagerX = GameObject.Find("Game Manager").GetComponent<GameManagerX>();

        transform.position = GameManagerX.RandomSpawnPosition(); 
        StartCoroutine(RemoveObjectRoutine()); // begin timer before target leaves screen
    }

    private void OnMouseDown()
    {
        // When target is clicked, destroy it, update score, and generate explosion
        if (gameManagerX.IsGameActive)
        {
            Destroy(gameObject);
            gameManagerX.UpdateScore(PointValue);
            Explode();
        }
    }

    // If target that is NOT the bad object collides with sensor, trigger game over
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);

        if (other.gameObject.CompareTag("Sensor") && !gameObject.CompareTag("Bad"))
        {
            gameManagerX.GameOver();
        }
    }

    // Display explosion particle at object's position
    void Explode ()
    {
        Instantiate(ExplosionFx, transform.position, ExplosionFx.transform.rotation);
    }

    // After a delay, Moves the object behind background so it collides with the Sensor object
    IEnumerator RemoveObjectRoutine ()
    {
        yield return new WaitForSeconds(TimeOnScreen);
        if (gameManagerX.IsGameActive)
        {
            transform.Translate(Vector3.forward * 5, Space.World);
        }
    }

}
