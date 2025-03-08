using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.Port;

public class GatherResource : MonoBehaviour
{
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    public void OnHit()
    {
        Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, CharacterManager.Instance.Player.equipment.curEquip.attackDistance))
        {
            if (hit.collider.TryGetComponent(out Resource resource))
            {
                for (int i = 0; i < CharacterManager.Instance.Player.equipment.curEquip.doesGatherResourcesNum; i++)
                {
                    resource.capacy--;
                    if (resource.capacy <= 0)
                    {
                        Destroy(resource.gameObject);
                        break;
                    }
                    Vector3 spawnPosition = resource.gameObject.transform.position + Vector3.up;
                    Quaternion spawnRotation = Quaternion.LookRotation(hit.normal);
                    GameObject spawnItem = Instantiate(resource.itemToGive.dropPrefab, spawnPosition, spawnRotation);

                    Rigidbody rb = spawnItem.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        Vector3 explosionForce = hit.normal * 10f;
                        rb.AddForce(explosionForce, ForceMode.Impulse);
                    }
                }
            }
        }
    }
}
