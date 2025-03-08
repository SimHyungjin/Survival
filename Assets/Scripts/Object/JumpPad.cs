using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CharacterManager.Instance.Player.controller.rb.AddForce(Vector3.up*10, ForceMode.Impulse);
            CharacterManager.Instance.Player.controller.Jump();
        }
    }
}
