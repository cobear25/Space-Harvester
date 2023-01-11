using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameController : MonoBehaviour
{
    public AudioSource audioSource;
    public CinemachineVirtualCamera virtualCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            audioSource.enabled = !audioSource.enabled;
        }

        // camera zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel") * 5f;
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var orthoSize = virtualCamera.m_Lens.OrthographicSize;
        if (orthoSize + scroll > 7.5 && orthoSize + scroll < 15) {
            virtualCamera.m_Lens.OrthographicSize += scroll;
        } 
    }
}
