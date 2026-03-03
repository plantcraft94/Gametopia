using UnityEngine;

public class PlayerResource : MonoBehaviour
{
    public int KeyAmount;
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("KeyItem"))
        {
            collision.gameObject.SetActive(false);
        }
    }
    public void AddKey()
    {
        KeyAmount += 1;
    }
    public void UseKey()
    {
        KeyAmount -= 1;
    }
    public void ResetResource()
    {
        KeyAmount = 0;
    }
}
