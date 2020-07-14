using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.CharacterWriter
{
    public class DrawDriver : MonoBehaviour
    {
        public Draw drawComponent;
        public RectTransform canvasRect;
        public Color brushColor = Color.black;
        public Camera MyCamera;
        public int size = 200;
        private readonly Color[] myColor = new Color[] { Color.black, Color.red, Color.green, Color.yellow };

        private void Awake()
        {
            drawComponent.Init(canvasRect.GetComponent<Canvas>(), MyCamera);
            drawComponent.SetProperty(brushColor, size);
        }

        private void Update()
        {
            //划线
            if (Input.GetMouseButtonDown(0))
            {
                DrawStart();
            }
            if (Input.GetMouseButton(0))
            {
                DrawLine();
            }
            if (Input.GetMouseButtonUp(0))
            {
                DrawEnd();
            }
        }

        public void ChangeColor(int colorIndex)
        {
            if (colorIndex >= 0 && colorIndex < myColor.Length)
                drawComponent.SetProperty(myColor[colorIndex]);
            else
                Debug.LogError("input color Error");
        }

        public void ChangeSize(int s)
        {
            drawComponent.SetProperty(s);
        }

        public void Clear()
        {
            drawComponent.Clear();
        }

        private void DrawStart()
        {
            if (MyCamera == null) return;
            //Debug.Log("落笔");
            drawComponent.StartWrite(Input.mousePosition);
        }

        private void DrawLine()
        {
            if (MyCamera == null) return;

            drawComponent.Writing(Input.mousePosition);
        }

        private void DrawEnd()
        {
            //Debug.Log("抬笔");
        }
    }
}
