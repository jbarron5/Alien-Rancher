using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabDrop : MonoBehaviour
{
    // Reference to the character camera.

    public Camera characterCamera;
    public Transform slot;
    private PickableItem pickedItem;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Fire Pressed");
            if (pickedItem)
            {
                DropItem(pickedItem);
            }
            else
            {
                Debug.Log("Try Pick");
                var ray = characterCamera.ViewportPointToRay(Vector3.one * 0.5f);
                RaycastHit hit;
                
                if (Physics.Raycast(ray, out hit, 5f))
                {
                    Debug.Log("hit: " + hit);
                    var pickable = hit.transform.GetComponent<PickableItem>();
                    
                    if (pickable)
                    {
                        PickItem(pickable);
                    }
                }
            }
        }
    }

    private void PickItem(PickableItem item)
    {
        pickedItem = item;
        item.Rb.isKinematic = true;
        item.Rb.velocity = Vector3.zero;
        item.Rb.angularVelocity = Vector3.zero;
        item.transform.SetParent(slot);
        item.transform.localPosition = Vector3.zero;
        item.transform.localEulerAngles = Vector3.zero;
    }

    private void DropItem(PickableItem item)
    {
        pickedItem = null;
        item.transform.SetParent(null);
        item.Rb.isKinematic = false;
        item.Rb.AddForce(item.transform.forward * 2, ForceMode.VelocityChange);
    }
}