using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class UnityAPITest : MonoBehaviour
{
    private void Start()
    {
#if UNITY_EDITOR
        Run();
#endif
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Run();
        }
    }

    [System.Serializable]
    class JsonTestClass
    {
        public int a;
        public int b;
        public int[] array;
    }

    void Receive()
    {

    }

    void ReceiveString(string s)
    {

    }

    void ReceiveInt(int i)
    {

    }

    void Run()
    {
        // 46byte
        Profiler.BeginSample("Object.name");
        string s = gameObject.name;
        Profiler.EndSample();

        // 460byte
        Profiler.BeginSample("Object.name 10");
        for (int i = 0; i < 10; i++)
        {
            s = gameObject.name;
        }
        Profiler.EndSample();

        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // 24byte
        Profiler.BeginSample("GameObject");
        var go = new GameObject();
        Profiler.EndSample();

        // 112byte
        // IL2CPP build 24byte
        Profiler.BeginSample("AddComponent");
        go.AddComponent<EmptyComponent>();
        Profiler.EndSample();

        // 0byte
        Profiler.BeginSample("GetComponent");
        var e = go.GetComponent<EmptyComponent>();
        Profiler.EndSample();

        // 32byte
        {
            Profiler.BeginSample("GetComponents<Collider>");
            var rs = go.GetComponents<Collider>();
            Profiler.EndSample();
        }

        // 32byte
        {
            Profiler.BeginSample("GetComponents<Collider>");
            var rs = go.GetComponents<Collider>();
            Profiler.EndSample();
        }

        // 0.6KB
        // 存在しないComponentを取得しようとするとメモリがとられる？
        // MissingComponentString
        {
            Profiler.BeginSample("GetComponent<Rigidbody>");
            var r = go.GetComponent<Rigidbody>();
            Profiler.EndSample();
        }

        // 0.6KB
        {
            Profiler.BeginSample("GetComponent<ParticleTest>");
            var p = go.GetComponent<ParticleTest>();
            Profiler.EndSample();
        }

        // 0byte
        {
            Profiler.BeginSample("TryGetComponent<ParticleTest>");
            go.TryGetComponent<ParticleTest>(out var p);
            Profiler.EndSample();
        }

        // 59.2KB
        {
            Profiler.BeginSample("GetComponent<ParticleTest> x100");
            for (int i = 0; i < 100; i++)
            {
                var p = go.GetComponent<ParticleTest>();
            }
            Profiler.EndSample();
        }

        // 40 byte
        {
            Profiler.BeginSample("GetComponent<Renderer> x1000");
            for (int i = 0; i < 1000; i++)
            {
                var p = cube.GetComponent<Renderer>();
            }
            Profiler.EndSample();
        }

        // 0byte
        {
            Profiler.BeginSample("GetComponent(typeof(Renderer)) x1000");
            for (int i = 0; i < 1000; i++)
            {
                var p = cube.GetComponent(typeof(Renderer)) as Renderer;
            }
            Profiler.EndSample();
        }

        // 0byte
        {
            Profiler.BeginSample("TryGetComponent<Renderer> x1000");
            for (int i = 0; i < 1000; i++)
            {
                bool ret = cube.TryGetComponent<Renderer>(out var p);
            }
            Profiler.EndSample();
        }

        // 40byte
        {
            Profiler.BeginSample("GetComponent<BoxCollider> x1000");
            for (int i = 0; i < 1000; i++)
            {
                var p = cube.GetComponent<BoxCollider>();
            }
            Profiler.EndSample();
        }

        // 0byte
        {
            Profiler.BeginSample("TryGetComponent<BoxCollider> x1000");
            for (int i = 0; i < 1000; i++)
            {
                bool ret = cube.TryGetComponent<BoxCollider>(out var p);
            }
            Profiler.EndSample();
        }

        // 0byte
        {
            Profiler.BeginSample("TryGetComponent<ParticleTest> x100");
            for (int i = 0; i < 100; i++)
            {
                go.TryGetComponent<ParticleTest>(out var p);
            }
            Profiler.EndSample();
        }

        // 120byte
        Profiler.BeginSample("GetComponents<Rigidbody>(List)");
        List<Rigidbody> rigidBodyList = new List<Rigidbody>();
        go.GetComponents<Rigidbody>(rigidBodyList);
        Profiler.EndSample();

        // 0byte
        Profiler.BeginSample("GetInstanceID");
        int id = go.GetInstanceID();
        Profiler.EndSample();

        // 120byte
        Profiler.BeginSample("GetChild");
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
        }
        Profiler.EndSample();

        // 0byte
        Profiler.BeginSample("GetChild 2");
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
        }
        Profiler.EndSample();

        // 32byte
        Profiler.BeginSample("foreach transform");
        foreach (var child in transform)
        {
        }
        Profiler.EndSample();

        // 32byte
        Profiler.BeginSample("foreach transform 2");
        foreach (var child in transform)
        {
        }
        Profiler.EndSample();


        Transform c;
        Transform cc;
        // 40byte
        Profiler.BeginSample("Find");
        c = transform.Find("child0");
        cc = c.Find("child00");
        Profiler.EndSample();

        Debug.Assert(cc != null);

        // 40byte
        Profiler.BeginSample("Find 2");
        cc = transform.Find("child1/child10");
        Profiler.EndSample();

        Debug.Assert(cc != null);

        // 416byte
        {
            var animator = GetComponent<Animator>();
            int hash = 0;
            Profiler.BeginSample("Animator.parameters");
            foreach (var p in animator.parameters)
            {
                hash = p.nameHash;
            }
            Profiler.EndSample();
        }

        // GetParameterはparametersを内部で呼び出している
        // 2.9KB
        {
            var animator = GetComponent<Animator>();
            Profiler.BeginSample("Animator.GetParameter");
            int hash = 0;
            for (int i = 0; i < animator.parameterCount; i++)
            {
                hash = animator.GetParameter(i).nameHash;
            }
            Profiler.EndSample();
        }

        
        {
            var animator = GetComponent<Animator>();

            // 46byte
            Profiler.BeginSample("Animator.GetLayerName");
            string layerName = animator.GetLayerName(0);
            Profiler.EndSample();

            // 0byte
            Profiler.BeginSample("Animator.GetLayerIndex");
            int layerIndex = animator.GetLayerIndex(layerName);
            Profiler.EndSample();

            // 存在しないレイヤーを指定した場合は-1
            layerIndex = animator.GetLayerIndex("hoge");
            Debug.Log(layerIndex);
        }

        // 0byte
        {
            Profiler.BeginSample("Animator.StringToHash");
            int hash = Animator.StringToHash("Test0");
            Profiler.EndSample();
        }

        // 0byte
        {
            var animator = GetComponent<Animator>();

            Profiler.BeginSample("Animator.GetCurrentAnimatorStateInfo");
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
            Profiler.EndSample();
        }

        {
            var animator = GetComponent<Animator>();

            Profiler.BeginSample("Animator.GetNextAnimatorStateInfo");
            AnimatorStateInfo info = animator.GetNextAnimatorStateInfo(0);
            Profiler.EndSample();
        }

        // 32byte
        {
            var animator = GetComponent<Animator>();

            Profiler.BeginSample("Animator.GetCurrentAnimatorClipInfo");
            AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
            Profiler.EndSample();
        }

        // 0byte
        {
            var animator = GetComponent<Animator>();

            Profiler.BeginSample("Animator.HasState");
            bool hasState = animator.HasState(0, Animator.StringToHash("State0"));
            Profiler.EndSample();
        }

        // 0byte
        {
            var animator = GetComponent<Animator>();

            Profiler.BeginSample("Animator.runtimeAnimatorController");
            var controller = animator.runtimeAnimatorController;
            Profiler.EndSample();
        }

        // 42byte
        {
            Profiler.BeginSample("tag equals");
            bool ret = gameObject.tag == "hoge";
            Profiler.EndSample();
        }

        // 0byte
        {
            Profiler.BeginSample("tag CompareTag");
            bool ret = gameObject.CompareTag("hoge");
            Profiler.EndSample();
        }

        // リフレクション呼び出しがあるがIL2CPPビルドだとGC Allocが少なくなる
        // 4.2KB
        // IL2CPP build 40byte
        {
            Profiler.BeginSample("SupportsTextureFormat 0");
            bool result = SystemInfo.SupportsTextureFormat(TextureFormat.ETC2_RGB);
            Profiler.EndSample();
        }

        // キャッシュされるので2回目の呼び出しはGC Allocが少なくなる
        // 400B
        {
            Profiler.BeginSample("SupportsTextureFormat 1");
            bool result = SystemInfo.SupportsTextureFormat(TextureFormat.ETC2_RGB);
            Profiler.EndSample();
        }

        // SupportsTextureFormatが呼び出されている
        // 1.1KB
        // IL2CPP build 64byte
        {
            Profiler.BeginSample("new Texture2D");
            var tex = new Texture2D(1, 1);
            Destroy(tex);
            Profiler.EndSample();
        }

        // 初回のみ(ScriptingWrapperFor)
        // 40byte
        {
            Profiler.BeginSample("shader");
            var shader = Shader.Find("Standard");
            Profiler.EndSample();
        }

        // 40byte
        // IL2CPP build 302byte
        {
            Profiler.BeginSample("new Material");
            var shader = Shader.Find("Standard");
            var mat = new Material(shader);
            Profiler.EndSample();
        }

        // 0byte
        {
            var shader = Shader.Find("Standard");
            var mat = new Material(shader);
            Profiler.BeginSample("Material.shader");
            var tmp = mat.shader;
            Profiler.EndSample();
        }

        // 40byte
        {
            cube.TryGetComponent<Renderer>(out var p);
            Profiler.BeginSample("Renderer.sharedMaterials");
            var mats = p.sharedMaterials;
            Profiler.EndSample();
        }

        // 40byte
        {
            cube.TryGetComponent<Renderer>(out var p);
            Profiler.BeginSample("Renderer.sharedMaterials 2");
            var mats = p.sharedMaterials;
            Profiler.EndSample();
        }

        // 80byte
        {
            cube.TryGetComponent<Renderer>(out var p);
            Profiler.BeginSample("Renderer.materials");
            var mats = p.materials;
            Profiler.EndSample();
        }

        // 40byte
        {
            cube.TryGetComponent<Renderer>(out var p);
            Profiler.BeginSample("Renderer.materials 2");
            var mats = p.materials;
            Profiler.EndSample();
        }

        // 40byte
        {
            Profiler.BeginSample("LayerToName");
            string layer = LayerMask.LayerToName(0);
            Profiler.EndSample();
        }

        // 0byte
        {
            Profiler.BeginSample("NameToLayer");
            int layer = LayerMask.NameToLayer("Default");
            Profiler.EndSample();
        }

        // 40byte
        {
            Profiler.BeginSample("Camera.main 0");
            var camera = Camera.main;
            Profiler.EndSample();
        }

        // 0byte
        {
            Profiler.BeginSample("Camera.main 1");
            var camera = Camera.main;
            Profiler.EndSample();
        }

        // 40byte
        {
            Profiler.BeginSample("Camera.allCameras");
            var cameras = Camera.allCameras;
            Profiler.EndSample();
        }

        {
            // 80byte
            Profiler.BeginSample("JsonUtility object");
            JsonTestClass json = new JsonTestClass();
            json.a = 1;
            json.b = 2;
            json.array = new int[]{ 1, 2, 3, 4 };
            Profiler.EndSample();

            // 114byte
            Profiler.BeginSample("JsonUtility.ToJson");
            string text = JsonUtility.ToJson(json);
            Profiler.EndSample();

            Debug.Log(text);

            // 80byte
            Profiler.BeginSample("JsonUtility.FromJson");
            var data = JsonUtility.FromJson<JsonTestClass>(text);
            Profiler.EndSample();
        }

        {
            Color32 color = Color.red;
            // RGBA(255, 0, 0, 255)
            Debug.Log(color.ToString());
            // RGBA(FF, 0, 0, FF)
            Debug.Log(color.ToString("X"));
            // RGBA(ff, 0, 0, ff)
            Debug.Log(color.ToString("x"));
        }

        // 0byte
        {
            Profiler.BeginSample("SendMessage");
            SendMessage("Receive");
            Profiler.EndSample();
        }

        // 48byte
        {
            Profiler.BeginSample("SendMessage string");
            string text = "hoge";
            SendMessage("ReceiveString", text);
            Profiler.EndSample();
        }

        // 20byte
        {
            Profiler.BeginSample("SendMessage int");
            SendMessage("ReceiveInt", 0, SendMessageOptions.RequireReceiver);
            Profiler.EndSample();
        }

        // 15.4Kbyte
        {
            Profiler.BeginSample("Application.persistentDataPath");
            for (int i = 0; i < 100; i++)
            {
                string path = Application.persistentDataPath;
            }
            Profiler.EndSample();
        }
    }
}
