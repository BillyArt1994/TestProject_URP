using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassObj : MonoBehaviour
{
    private Material mat;
    private Vector3 followPos = Vector3.zero;//�Ӷ���λ��
    private Vector3 massVelocity = Vector3.zero;//�Ӷ����ٶ�
    public float stiffness = 60f;//����ϵ��
    public float damping = 2f;//����ϵ��

    private float max, min;//ģ����ģ�Ϳռ����, ��͵��yֵ

    private void Start()
    {
        mat = GetComponent<MeshRenderer>().sharedMaterial;
        followPos = transform.position;//�������һ���Ӷ���

        //�������ĵ�����ռ����
        max = GetComponent<MeshFilter>().sharedMesh.bounds.max.y;
        min = GetComponent<MeshFilter>().sharedMesh.bounds.min.y;

        mat.SetFloat("_MeshH", max - min);//ģ���ܸ߶�
    }
    private void Update()
    {
        //����һЩ����, ���ٶ�, �ٶ�, ·��, �˶� ����ֵ����
        Vector3 force = GetMainForce();//����
        force += GetDampingForce();//����
        massVelocity += force * Time.deltaTime;//���̶�����Ϊ1, ��force��ֵ���ڼ��ٶ���ֵ
        followPos += massVelocity * Time.deltaTime;//�Ӷ�����ƶ�

        //Ϊshader��������
        SetMatData();
    }
    private Vector3 GetMainForce()
    {
        //���˶���
        Vector3 forceDir = transform.position - followPos;
        return forceDir * stiffness;
    }
    private Vector3 GetDampingForce()
    {
        return -massVelocity * damping;//��������
    }
    private void SetMatData()
    {
        mat.SetVector("_MainPos", transform.position);//������
        mat.SetVector("_FollowPos", followPos);//�Ӷ���
        mat.SetFloat("_W_Bottom", transform.position.y + min);//ģ����͵�yֵ
    }
}