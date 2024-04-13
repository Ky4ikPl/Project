using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paint : MonoBehaviour
{

    [SerializeField] private int _textureSizeX = 400;
    [SerializeField] private int _textureSizeY =200;
    [SerializeField] private TextureWrapMode _textureWrapMode;
    [SerializeField] private FilterMode _filterMode;
    [SerializeField] private Texture2D _texture;
    [SerializeField] private Material _material;
    [SerializeField] private Material _clearMaterial;

    [SerializeField] private Camera _camera;
    [SerializeField] private Collider _collider;
    [SerializeField] private Color _color;
    [SerializeField] private int _brushSize = 8;

    private int _oldRayX, _oldRayY;

    void OnValidate()
    {

        if (_texture == null)
        {
            _texture = new Texture2D(_textureSizeX, _textureSizeY);
        }
        if (_texture.width != _textureSizeX)
        {
            _texture.Reinitialize(_textureSizeX, _textureSizeY);
        }
        for (int i = 0; i < _textureSizeX; i++)
        {
            for (int j = 0; j < _textureSizeY; j++)
            {
                _texture.SetPixel(i, j, Color.white);
            }
        }

        _texture.wrapMode = _textureWrapMode;
        _texture.filterMode = _filterMode;
        _material.mainTexture = _texture;
        _clearMaterial= _material;
        _texture.Apply();
    }

    private void Update()
    {

        _brushSize += (int)Input.mouseScrollDelta.y;
        if (Input.GetMouseButtonDown(0))
        {
            CharactersPlace.Instance.ReSetPoints();
        }
        if (Input.GetMouseButton(0))
        {
            Time.timeScale = 0f;
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (_collider.Raycast(ray, out hit, 1000f))
            {

                int rayX = (int)(hit.textureCoord.x * _textureSizeX);
                int rayY = (int)(hit.textureCoord.y * _textureSizeY);

                if (_oldRayX != rayX || _oldRayY != rayY)
                {
                    //DrawQuad(rayX, rayY);
                    DrawCircle(rayX, rayY);
                    _oldRayX = rayX;
                    _oldRayY = rayY;
                    CharactersPlace.Instance.SetPoints(new Vector3(-rayX, -rayY, 0f));
                }
                _texture.Apply();
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            Time.timeScale = 1f;
            for(int i = 0; i<_textureSizeX; i++)
            {
                for(int j= 0; j<_textureSizeY; j++)
                {
                    _texture.SetPixel(i, j, Color.white);
                }
            }
            CharactersPlace.Instance.SetSpline();
            _texture.Apply();
        }
    }

    void DrawQuad(int rayX, int rayY)
    {
        for (int y = 0; y < _brushSize; y++)
        {
            for (int x = 0; x < _brushSize; x++)
            {
                _texture.SetPixel(rayX + x - _brushSize / 2, rayY + y - _brushSize / 2, _color);
            }
        }
    }

    void DrawCircle(int rayX, int rayY)
    {
        for (int y = 0; y < _brushSize; y++)
        {
            for (int x = 0; x < _brushSize; x++)
            {

                float x2 = Mathf.Pow(x - _brushSize / 2, 2);
                float y2 = Mathf.Pow(y - _brushSize / 2, 2);
                float r2 = Mathf.Pow(_brushSize / 2 - 0.5f, 2);

                if (x2 + y2 < r2)
                {
                    int pixelX = rayX + x - _brushSize / 2;
                    int pixelY = rayY + y - _brushSize / 2;

                    if (pixelX >= 0 && pixelX < _textureSizeX && pixelY >= 0 && pixelY < _textureSizeY)
                    {
                        Color oldColor = _texture.GetPixel(pixelX, pixelY);
                        Color resultColor = Color.Lerp(oldColor, _color, _color.a);
                        _texture.SetPixel(pixelX, pixelY, resultColor);
                    }

                }


            }
        }
    }

}