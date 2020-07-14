using UnityEngine;
using UnityEngine.UI;


namespace Framework.CharacterWriter
{
    public class Draw : MonoBehaviour
    {
        //预设
        [SerializeField]
        private RawImage rawImage;
        [SerializeField]
        private Material brushMat;
        [SerializeField]
        private Material clearMat;

        //传入
        private Camera m_uiCamera;
        private RenderMode m_renderMode;

        private float m_rawImageSizeX;
        private float m_rawImageSizeY;
        private Vector3 m_mousePos;
        private Vector3 m_lastMousePos;
        private RenderTexture m_renderTex;
        private RenderTexture m_lastRenderTex;

        public void Init(Canvas canvas, Camera uiCamera)
        {
            m_uiCamera = uiCamera;
            m_renderMode = canvas.renderMode;

            m_rawImageSizeX = rawImage.GetComponent<RectTransform>().sizeDelta.x;
            m_rawImageSizeY = rawImage.GetComponent<RectTransform>().sizeDelta.y;

            m_renderTex = RenderTexture.GetTemporary(512, 512);
            m_lastRenderTex = RenderTexture.GetTemporary(512, 512);
            rawImage.texture = m_renderTex;

            brushMat.SetColor("_Color", Color.black);
            brushMat.SetFloat("_Size", 100);
        }

        public void Release()
        {
            if (m_renderTex != null) m_renderTex.Release();
            if (m_lastRenderTex != null) m_lastRenderTex.Release();
        }

        public void SetProperty(Color brushColor, int size)
        {
            brushMat.SetColor("_Color", brushColor);
            brushMat.SetFloat("_Size", size);
        }
        public void SetProperty(Color brushColor)
        {
            brushMat.SetColor("_Color", brushColor);
        }
        public void SetProperty(int size)
        {
            brushMat.SetFloat("_Size", size);
        }

        public void Clear()
        {
            Graphics.Blit(null, m_renderTex, clearMat);
            Graphics.Blit(null, m_lastRenderTex, clearMat);
        }

        public void StartWrite(Vector3 pos)
        {
            m_mousePos = pos;
            m_lastMousePos = pos;
        }

        public void Writing(Vector3 pos)
        {
            m_mousePos = pos;
            Paint();
            m_lastMousePos = pos;
        }

        void Paint()
        {
            var uv = GetUV(m_mousePos);
            var last = GetUV(m_lastMousePos);

            brushMat.SetTexture("_Tex", m_renderTex);
            brushMat.SetVector("_UV", uv);
            brushMat.SetVector("_LastUV", last);

            Graphics.Blit(m_renderTex, m_renderTex, brushMat);
        }

        Vector2 GetUV(Vector2 brushPos)
        {
            //获取图片在屏幕中的像素位置
            Vector2 rawImagePos = Vector2.zero;

            //判断所在画布的渲染方式，不同渲染方式的位置计算方式不同
            switch (m_renderMode)
            {
                case RenderMode.ScreenSpaceOverlay:
                    rawImagePos = rawImage.rectTransform.position;
                    break;
                default:
                    rawImagePos = m_uiCamera.WorldToScreenPoint(rawImage.rectTransform.position);
                    break;
            }

            //换算鼠标在图片中心点的像素位置
            Vector2 pos = brushPos - rawImagePos;

            //换算鼠标在图片中UV坐标
            Vector2 uv = new Vector2(pos.x / m_rawImageSizeX + 0.5f, pos.y / m_rawImageSizeY + 0.5f);

            return uv;
        }
    }
}