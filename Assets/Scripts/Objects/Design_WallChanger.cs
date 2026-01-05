using UnityEngine;

public class Design_WallChanger : MonoBehaviour
{
    private MeshRenderer mr;

    public Material[] normalMats;
    public Material[] darkMats;

    public GameObject destroyingParticle = null;

    private bool alreadyChanged = false;

    private void Awake()
    {
        mr = GetComponent<MeshRenderer>();

        if (destroyingParticle != null)
            destroyingParticle.SetActive(false);
    }

    public void ChangeSprite()
    {
        if (alreadyChanged)
        {
            return;
        }

        int index = normalMats.Length == 1
            ? 0
            : Random.Range(0, normalMats.Length);

        var mats = mr.materials;

        if (mats.Length > 0)
            mats[0] = normalMats[index];

        if (mats.Length > 1)
            mats[1] = darkMats[index];

        mr.materials = mats;

        if (destroyingParticle != null)
            destroyingParticle.SetActive(true);

        alreadyChanged = true;
    }
}
