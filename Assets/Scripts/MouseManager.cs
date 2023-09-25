using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Events;
using System;

//[System.Serializable]

//public class EventVector3 : UnityEvent<Vector3> { }

public class MouseManager : Singleton<MouseManager>
{
    //public Camera camera = Camera.main;
    //public EventVector3 OnMouseClicked;
    [SerializeField]
    private List<Texture2D> cursorSprite;     //0:arrow   1:move   2:attack  3:teleport  4: finger

    private int cursorWidth, cursorHeight;
    private Vector2 cursorPos;
    public event Action<Vector3> OnMapClicked;
    public event Action<GameObject> OnEnemyClicked;


    RaycastHit hit;
    Ray ray;
    void Start()
    {
        cursorWidth = cursorSprite[0].width;   cursorHeight = cursorSprite[0].height;
        cursorPos = new Vector2(cursorWidth / 2, cursorHeight / 2);
    }

    void Update()
    {
        SetCursor();
        MouseControl();
    }

    void SetCursor()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            switch (hit.collider.gameObject.tag)
            {
                case "Ground":
                    Cursor.SetCursor(cursorSprite[1], cursorPos, CursorMode.Auto);
                    break;
                case "Enemy":
                    Cursor.SetCursor(cursorSprite[2], cursorPos, CursorMode.Auto);
                    break;
                case "Portal":
                    Cursor.SetCursor(cursorSprite[3], cursorPos, CursorMode.Auto);
                    break;
            }
        }
    }

    void MouseControl()
    {
        if(Input.GetMouseButtonDown(0) && hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Ground")|| hit.collider.gameObject.CompareTag("Portal"))
            {
                //OnMouseClicked?.Invoke(hit.transform.position);
                OnMapClicked?.Invoke(hit.point);
            }
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                OnEnemyClicked?.Invoke(hit.collider.gameObject);
            }

        }
    }
}
