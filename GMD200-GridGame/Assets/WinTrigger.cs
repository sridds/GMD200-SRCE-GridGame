using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.SceneManagement.SceneManager;

public class WinTrigger : MonoBehaviour
{
    [SerializeField]
    ItemSO itemCheck;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;

        if (GameManager.Instance.inventory.CheckForItem(itemCheck))
        {
            SceneLoader.loadScene(3);
        }
    }
}
