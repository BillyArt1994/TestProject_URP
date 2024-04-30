using UnityEngine;

public class UniqueShadowHelper : MonoBehaviour
{
    private void OnEnable()
    {
        if (UniqueShadow.Instance != null)
        {
            UniqueShadow.Instance.Target = transform;
        }
    }

    private void OnDisable()
    {
        if (UniqueShadow.Instance != null)
        {
            if (UniqueShadow.Instance.target == transform)
            {
                UniqueShadow.Instance.Target = null;
            }
        }
    }
}