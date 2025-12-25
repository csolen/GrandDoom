using UnityEngine;

public class Design_WallChanger : MonoBehaviour
{
    private MeshRenderer mr;

    public Material newMatNormal;
    public Material newmatDarker;

    public GameObject destroyingParticle = null;

    private void Awake()
    {
        mr = GetComponent<MeshRenderer>();

        if (destroyingParticle != null)
        {
            destroyingParticle.SetActive(false);
        }
    }

    public void ChangeSprite()
    {
        var mats = mr.materials;

        if (mats.Length > 0 && mats[0] != newMatNormal)
        {
            mats[0] = newMatNormal;
        }

        if (mats.Length > 1 && mats[1] != newmatDarker)
        {
            mats[1] = newmatDarker;
        }

        mr.materials = mats;

        if (destroyingParticle != null)
            destroyingParticle.SetActive(true);
    }

}
