using UnityEngine;

public class DialogControl : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
        gameObject.SetActive(false);
        }
    }
}
