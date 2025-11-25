using UnityEngine;

public class Ladder : MonoBehaviour
{
    private readonly float levelStep = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (PlayerController.instance == null) return;

        Transform playerTf = other.transform;

        float ladderZ = transform.position.z;
        float playerZ = playerTf.position.z;

        float diff = ladderZ - playerZ;

        float direction;

        if (Mathf.Abs(diff) < 0.01f)
        {
            direction = -1f;
        }
        else
        {
            direction = Mathf.Sign(diff);
        }

        float targetZ = playerZ + direction * levelStep;

        PlayerController.instance.EnterLadder(this, targetZ);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (PlayerController.instance == null) return;

        PlayerController.instance.ExitLadder(this);
    }
}
