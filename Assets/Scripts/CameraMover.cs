using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;

public class CameraMover : MonoBehaviour
{
    public int scrollTreshhold = 50;
    public int speed = 5;
    CinemachineVirtualCamera myCam;
    float screenWidth;
    float screenHeight;
    // Start is called before the first frame update
    void Start()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        myCam = GetComponent<CinemachineVirtualCamera>();
    }

    void Update()
    {
        if (!IsPointerOverUIObject()) {
            if (!Controller.Instance.IsInPlayerMode) {
                if (Input.mousePosition.x > screenWidth - scrollTreshhold) {
                    transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
                }
                if (Input.mousePosition.x < 0 + scrollTreshhold) {
                    transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
                }
                if (Input.mousePosition.y > screenHeight - scrollTreshhold) {
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + speed * Time.deltaTime);
                }
                if (Input.mousePosition.y < 0 + scrollTreshhold) {
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - speed * Time.deltaTime);
                }
            }
        }
        if (myCam.Priority != 10) {
            Vector3 playerPos = Controller.Instance.PlayerTransform.position;
            Vector3 pos = new(playerPos.x, 100, playerPos.z - 25);
            transform.position = pos;
        }
    }

    private bool IsPointerOverUIObject() {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }
}
