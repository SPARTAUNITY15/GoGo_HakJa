using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

public class Preview_Item : MonoBehaviour
{
    ItemPlaceController controller;
    public ItemData item2Place;

    Renderer[] renderers;
    List<Material> materials = new();
    Bounds bounds;

    Vector3 position;
    Quaternion rotation;

    //int colliderCount;
    float floatingHeight;
    PlacingValidity placeValidity;



    public void Init(ItemData item2Place, ItemPlaceController controller, float floatingHeight)
    {
        this.item2Place = item2Place;
        this.controller = controller;
        this.floatingHeight = floatingHeight;

        //�⺻�� ����
        rotation = Quaternion.identity;
        foreach (var colli in GetComponentsInChildren<Collider>())
        {
            colli.enabled = false;
        }
        renderers = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            foreach (Material material in renderer.materials)
            {
                materials.Add(material);
            }
        }

        //�ٿ�� ����ϱ�
        CalcBounds();
    }

    private void CalcBounds()
    {
        //Collider[] renderers = gameObject.GetComponentsInChildren<Collider>();
        //bounds = renderers[0].bounds;
        //for (int i = 1; i < renderers.Length; i++)
        //{
        //    bounds.Encapsulate(renderers[i].bounds);
        //}

        bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }
    }

    void Update()
    {
        CalcPosition();
        CalcRot();
        CalcBounds();
        SetTransform();

        CheckPlaceAvailable();

        if (Input.GetKeyDown(KeyCode.Return) & placeValidity == PlacingValidity.Valid)
            ConfirmPlace();
    }

    private void CalcPosition()
    {
        // ���� �浹 �� �ٿ�带 ���� ���� ��Ʈ������ �������� ��ġ�� ��� ���.
        // ���� �浹 => �� �������� ���
        // �ٿ�� => �߽����� ��ǥ�����κ��� �̰ݵž��� ����
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.4f));
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, 1 << 7))
        {
            //position = new Vector3(hitInfo.point.x, hitInfo.point.y + bounds.extents.y, hitInfo.point.z);
            position = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
        }
        Debug.Log($"{hitInfo.point.y} + {bounds.extents.y}");

    }

    private void CalcRot()
    {
        //T,YŰ�� ���� ȸ���� ����
        if (Input.GetKey(KeyCode.T))
            rotation *= Quaternion.Euler(0, -1, 0);
        else if (Input.GetKey(KeyCode.Y))
            rotation *= Quaternion.Euler(0, 1, 0);
    }

    private void SetTransform()
    {
        //���� ��ġ��, ȸ���� ����� ���� ��Ʈ �信 �ݿ��ϱ�
        transform.localPosition = position;
        transform.rotation = rotation;

    }

    private enum PlacingValidity
    {
        Valid,
        Invalid
    }

    private void OnDrawGizmos()
    {
        if (bounds != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
    }

    private void CheckPlaceAvailable()
    {
        PlacingValidity newPlaceState;
        // ���� ��ġ�� ��ġ ��������
        // �ٸ� ������Ʈ���� �浹�� �ִ��� (�ٿ�� ����ؼ�)

        bool isOtheColliderOK;
        Collider[] temp = Physics.OverlapBox(bounds.center, bounds.extents, Quaternion.identity, ~(1 << 7), QueryTriggerInteraction.Ignore);

        if (temp.Length > 0) 
        {
            Debug.Log("�� ���� �ݶ��̴��� �浹�� �־��");
            isOtheColliderOK = false;
        }
        else
        {
            Debug.Log("�� ���� �ݶ��̴��ʹ� �浹�� �����");
            isOtheColliderOK = true;
        }

        // ������ �Ÿ�(�ʹ� ħ�� ���ϰų�, �ʹ� �ְų� ����)
        bool isGroundOK = GroundCheck();

        // ��� ����
        newPlaceState = (isGroundOK && isOtheColliderOK) ? PlacingValidity.Valid : PlacingValidity.Invalid;

        // ��ġ ���� ���°� �ٲ���ٸ� �Ʒ��� ����
        if (newPlaceState != placeValidity)
        {
            placeValidity = newPlaceState;
            UpdateMaterial();
        }
    }

    private bool GroundCheck()
    {
        // ������ �Ÿ�(�ʹ� ħ�� ���ϰų�, �ʹ� �ְų� ����)

        // �ٿ�忡 ��� �� ����
        Vector3 bottomCenter = new Vector3(bounds.center.x, bounds.min.y, bounds.center.z);
        Vector3[] CheckPoints =
        {
            bottomCenter,
            bottomCenter + new Vector3(bounds.extents.x, 0, bounds.extents.z),
            bottomCenter + new Vector3(-bounds.extents.x, 0, bounds.extents.z),
            bottomCenter + new Vector3(bounds.extents.x, 0, -bounds.extents.z),
            bottomCenter + new Vector3(-bounds.extents.x, 0, -bounds.extents.z)
        };

        // �Ʒ��� 1��ŭ ����ĳ��Ʈ
        int hitGround = 0;
        foreach (Vector3 point in CheckPoints)
        {
            if (Physics.Raycast(point, Vector3.down, floatingHeight, 1 << 7))
            {
                hitGround++;
            }
        }

        // ���� �ϳ��� �˻� (�������� ������ ������)
        return hitGround > 2;
    }

    private void UpdateMaterial()
    {
        foreach (var material in materials)
        {
            if (placeValidity == PlacingValidity.Valid)
                material.color = Color.green;
            if (placeValidity == PlacingValidity.Invalid)
                material.color = Color.red;
        }
    }


    private void ConfirmPlace()
    {
        // ��ġ�ϱ�
        item2Place.ToPlacedItem(position, rotation);

        // ���� �Լ� ȣ��
        controller.QuitPlacing();
    }

    private void ForceQuit()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            controller.QuitPlacing();
        }
    }
}

