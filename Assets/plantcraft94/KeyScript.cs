using UnityEngine;

public class KeyScript : MonoBehaviour
{
    PlayerResource playerResource;
    private void Awake()
    {
        playerResource = FindAnyObjectByType<PlayerResource>();
    }
    private void OnDisable()
    {
        playerResource.AddKey();
    }
}
