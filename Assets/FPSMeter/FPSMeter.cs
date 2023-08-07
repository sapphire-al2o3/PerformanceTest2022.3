using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class FPSMeter : MonoBehaviour
{
    [SerializeField]
    float targetFrameRate = 60.0f;
    [SerializeField]
    Color color = new Color(0.0f, 1.0f, 0.0f);
    [SerializeField]
    Color overColor;

    [SerializeField]
    float overFrameRate = 30.0f;

    float frameDeltaTime = 0.0f;
    float prevTime = 0.0f;

    [SerializeField]
    float height = 0.05f;

    Vector4 size;
    Material mat;

    [SerializeField]
    float interval = 0.2f;
    float elapsed = 0.0f;
    int frame = 0;

    int sizeID;
    int colorID;
    Mesh mesh;

    [SerializeField]
    Camera targetCamera;
    CommandBuffer commandBuffer;

    [SerializeField]
    Anchor anchor = Anchor.Top;

    public enum Anchor
    {
        Top,
        Bottom,
        Left,
        Right
    }


	void OnDestroy()
    {
        if (mesh != null)
        {
            Destroy(mesh);
        }
        if (mat != null)
        {
            Destroy(mat);
        }
    }

    void Start()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        mesh = new Mesh();
        int[] indices = new int[6];
        Vector2[] uvs = new Vector2[4];
        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(0, 0, 0);
        vertices[1] = new Vector3(0, 0, 0);
        vertices[2] = new Vector3(0, 0, 0);
        vertices[3] = new Vector3(0, 0, 0);

        uvs[0] = new Vector2(0, 0);
        uvs[1] = new Vector2(1, 0);
        uvs[2] = new Vector2(0, 1);
        uvs[3] = new Vector2(1, 1);

        indices[0] = 0;
        indices[1] = 1;
        indices[2] = 2;
        indices[3] = 2;
        indices[4] = 1;
        indices[5] = 3;

        mesh.vertices = vertices;
        mesh.triangles = indices;
        mesh.uv = uvs;

        sizeID = Shader.PropertyToID("_Size");
        mat = new Material(Shader.Find("Unlit/MeterShader"));
        size.x = 1.0f;
        size.y = height;
        mat.SetVector(sizeID, size);
        colorID = Shader.PropertyToID("_Color");

        commandBuffer = new CommandBuffer();
        commandBuffer.DrawMesh(mesh, Matrix4x4.identity, mat);
        targetCamera.AddCommandBuffer(CameraEvent.AfterForwardAlpha, commandBuffer);

    }

    void OnEnable()
    {
        if (targetCamera != null && commandBuffer != null)
        {
            targetCamera.AddCommandBuffer(CameraEvent.AfterForwardAlpha, commandBuffer);
        }
    }

    void OnDisable()
    {
        if (targetCamera != null)
        {
            targetCamera.RemoveCommandBuffer(CameraEvent.AfterForwardAlpha, commandBuffer);
        }
    }

    void Update()
    {
        float currentTime = Time.realtimeSinceStartup;
        frameDeltaTime = currentTime - prevTime;
        prevTime = currentTime;
        elapsed += frameDeltaTime;
        frame++;

        if (elapsed < interval)
        {
            return;
        }

        float time = elapsed / frame;

        if (anchor == Anchor.Top)
        {
            size.x = time * targetFrameRate * 0.5f;
            size.y = height;
            size.z = 0.0f;
            size.w = 0.0f;
        }
        else if (anchor == Anchor.Bottom)
        {
            size.x = time * targetFrameRate * 0.5f;
            size.y = height;
            size.z = 0.0f;
            size.w = 1.0f - height;
        }
        else if (anchor == Anchor.Left)
        {
            size.x = height;
            size.y = time * targetFrameRate * 0.5f;
            size.z = 0.0f;
            size.w = 0.0f;
        }
        else if (anchor == Anchor.Right)
        {
            size.x = height;
            size.y = time * targetFrameRate * 0.5f;
            size.z = 1.0f - height;
            size.w = 0.0f;
        }
        mat.SetVector(sizeID, size);

        //Debug.Log(frame / elapsed);

        mat.SetColor(colorID, time > 1.0f / overFrameRate ? overColor : color);

        frame = 0;
        elapsed = 0.0f;
    }
}
