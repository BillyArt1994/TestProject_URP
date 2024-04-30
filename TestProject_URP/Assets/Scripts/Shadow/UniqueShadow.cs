using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering.Universal;

[AddComponentMenu("CRenderable/UniqueShadow"), ExecuteInEditMode]
[RequireComponent(typeof(Camera), typeof(UniversalAdditionalCameraData))]
public class UniqueShadow : MonoBehaviour
{
    //public UnityEngine.Rendering.Universal.ShadowResolution shadowMapSize = UnityEngine.Rendering.Universal.ShadowResolution._1024;
    public LayerMask inclusionMask = ~0;
    public float cullingDistance = 100f;
    public float fallbackFilterWidth = 6f;

    [Header("Focus")] public bool autoFocus = true;
    public float autoFocusRadiusBias;
    public Transform target;
    public Vector3 offset;
    public float radius = 1f;
    public float depthBias = 0.01f;
    public float sceneCaptureDistance = 4f;


    public Transform Target
    {
        get => target;
        set
        {
            target = value;
        }
    }

    public Vector3 position
    {
        get
        {
            return target.position
                   + target.right * offset.x
                   + target.up * offset.y
                   + target.forward * offset.z;
        }
    }

    [Space] public Light m_lightSource;

    private Light GetLight()
    {
        var l = m_lightSource ? m_lightSource : RenderSettings.sun;
        return l;
    }

    UniqueShadowCaster m_ShadowCaster;

    public static UniqueShadow Instance;

    void Awake()
    {
        m_ShadowCaster = UniqueShadowCaster.s_Instacne;

        m_Camera = GetComponent<Camera>();

        m_Camera.orthographic = true;

        m_ShadowCaster.Init(1024, 1024, m_Camera.cullingMask);

        Instance = this;

        ShadowCamera.enabled = false;

        //if (target == null)
        //{
        //    target = this.transform;
        //}

        SetFocus();
    }

    private bool dirty = true;
    private Plane[] cullPlanes = new Plane[6];
    Bounds bounds = new Bounds();

    void UpdateFocusRadius()
    {
        if (target == null)
        {
            return;
        }

        GeometryUtility.CalculateFrustumPlanes(Camera.main, cullPlanes);


        var renders = target.GetComponentsInChildren<SkinnedMeshRenderer>();
        bool init = false;
        for (int i = 0; i < renders.Length; i++)
        {
            var aabb = renders[i].bounds;
            if (GeometryUtility.TestPlanesAABB(cullPlanes, aabb))
            {
                if (!init)
                {
                    bounds = aabb;
                    init = true;
                }
                else
                    bounds.Encapsulate(aabb);
            }
        }

        if (!init)
            return;

        Vector3 extents = bounds.extents;
        if (extents.x > 0.1f || extents.y > 0.1f)
        {
            Vector3 offset = bounds.center - target.position;
            float radius = autoFocusRadiusBias + Mathf.Max(extents.x, extents.y);

            if ((this.offset - offset).magnitude > 0.15 || Mathf.Abs(this.radius - radius) > 0.1)
            {
                this.offset = offset;
                this.radius = radius;
                dirty = true;
            }
        }
    }

    Camera m_Camera;

    Camera ShadowCamera
    {
        get
        {
            if (m_Camera == null)
                m_Camera = GetComponent<Camera>();
            return m_Camera;
        }
    }

    void SetFocus()
    {
        m_Camera.cullingMask = inclusionMask;


        if (autoFocus)
            UpdateFocusRadius();

        if (dirty)
        {
            radius = Mathf.Max(radius, Mathf.Epsilon);
            m_Camera.orthographicSize = radius;

            m_Camera.nearClipPlane = 0f;
            m_Camera.farClipPlane = radius * 2f + sceneCaptureDistance;
            m_Camera.projectionMatrix = Matrix4x4.Ortho(-radius, radius, -radius, radius, m_Camera.nearClipPlane,
                m_Camera.farClipPlane);

            m_ShadowCaster.Projection(radius, sceneCaptureDistance, depthBias);

            Light light = GetLight();
            CheckVisibility(light);
            dirty = false;
        }
    }

    public bool Distance(Vector3 target, float cullingDistance)
    {
        Debug.Assert(m_Camera);
        return (target - m_Camera.transform.position).sqrMagnitude < (cullingDistance * cullingDistance);
    }
    //public bool TestAABB(Bounds b)
    //{
    //    Debug.Assert(m_Camera);
    //    return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(m_Camera), b);
    //}

    bool CheckVisibility(Light light)
    {
        if (target == null || light == null)
            return false;

        if (autoFocus)
            UpdateFocusRadius();

        var pos = position - light.transform.forward * (radius + sceneCaptureDistance);
        var rot = light.transform.rotation;

        transform.position = pos;
        transform.rotation = rot;
        m_ShadowCaster.Transform(pos, rot);
        return true;
        //var targetPos = position;
        //var bounds = new Bounds( targetPos, Vector3.one * radius * 2f );

        //return Distance( targetPos, cullingDistance ) && TestAABB( bounds );

    }


    void OnEnable()
    {
        if (target == null)
        {
            //target = this.transform;
        }

        m_ShadowCaster.Init(1024, 1024, m_Camera.cullingMask);
        UpdateFocusRadius();
        dirty = true;
    }

    private void LateUpdate()
    {
        m_ShadowCaster.SetOn(target != null);
        if (autoFocus)
            SetFocus();
    }

    void OnDisable()
    {
        m_ShadowCaster.Clear();
    }

    void OnValidate()
    {
        if (!Application.isPlaying || !m_Camera)
            return;

        m_ShadowCaster.Init(1024, 1024, m_Camera.cullingMask);

        SetFocus();
    }


    void OnDrawGizmosSelected()
    {
        if (target == null)
            return;

        Gizmos.color = m_Camera ? autoFocus ? Color.cyan : Color.green : Color.red;

        Gizmos.DrawWireSphere(position, radius + (autoFocus ? autoFocusRadiusBias : 0f));

        if (m_Camera)
        {
            Gizmos.color = Color.gray;
            Gizmos.matrix = m_Camera.cameraToWorldMatrix;

            Vector3 center = new Vector3(0, 0, -radius);
            Vector3 size = Vector3.one * radius * 2;
            center.z += -sceneCaptureDistance * 0.5f;
            size.z += sceneCaptureDistance;
            Gizmos.DrawWireCube(center, size);
        }
    }

    #region AssetMenu

#if UNITY_EDITOR
    [MenuItem("GameObject/CRenderable/UniqueShadow", false, 10)]
    static void CreatePlanarReflectionObject(MenuCommand menuCommand)
    {
        GameObject go = new GameObject("New UniqueShadow", typeof(UniqueShadow));

        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);

        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }
#endif

    #endregion
}