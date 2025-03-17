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

        //기본값 세팅
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

        //바운딩 계산하기
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
        // 레이 충돌 및 바운드를 통해 현재 고스트프리뷰 아이템이 위치할 장소 계산.
        // 레이 충돌 => 그 지점에서 찍기
        // 바운드 => 중심점이 지표면으로부터 이격돼야할 길이
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
        //T,Y키를 통한 회전값 변경
        if (Input.GetKey(KeyCode.T))
            rotation *= Quaternion.Euler(0, -1, 0);
        else if (Input.GetKey(KeyCode.Y))
            rotation *= Quaternion.Euler(0, 1, 0);
    }

    private void SetTransform()
    {
        //계산된 위치값, 회전값 결과를 실제 고스트 뷰에 반영하기
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
        // 현재 위치에 설치 가능한지
        // 다른 오브젝트와의 충돌이 있는지 (바운드 사용해서)

        bool isOtheColliderOK;
        Collider[] temp = Physics.OverlapBox(bounds.center, bounds.extents, Quaternion.identity, ~(1 << 7), QueryTriggerInteraction.Ignore);

        if (temp.Length > 0) 
        {
            Debug.Log("땅 외의 콜라이더와 충돌이 있어요");
            isOtheColliderOK = false;
        }
        else
        {
            Debug.Log("땅 외의 콜라이더와는 충돌이 없어요");
            isOtheColliderOK = true;
        }

        // 땅과의 거리(너무 침범 당하거나, 너무 멀거나 방지)
        bool isGroundOK = GroundCheck();

        // 결과 기입
        newPlaceState = (isGroundOK && isOtheColliderOK) ? PlacingValidity.Valid : PlacingValidity.Invalid;

        // 설치 가능 상태가 바뀌었다면 아래를 실행
        if (newPlaceState != placeValidity)
        {
            placeValidity = newPlaceState;
            UpdateMaterial();
        }
    }

    private bool GroundCheck()
    {
        // 땅과의 거리(너무 침범 당하거나, 너무 멀거나 방지)

        // 바운드에 몇가지 점 선정
        Vector3 bottomCenter = new Vector3(bounds.center.x, bounds.min.y, bounds.center.z);
        Vector3[] CheckPoints =
        {
            bottomCenter,
            bottomCenter + new Vector3(bounds.extents.x, 0, bounds.extents.z),
            bottomCenter + new Vector3(-bounds.extents.x, 0, bounds.extents.z),
            bottomCenter + new Vector3(bounds.extents.x, 0, -bounds.extents.z),
            bottomCenter + new Vector3(-bounds.extents.x, 0, -bounds.extents.z)
        };

        // 아래로 1만큼 레이캐스트
        int hitGround = 0;
        foreach (Vector3 point in CheckPoints)
        {
            if (Physics.Raycast(point, Vector3.down, floatingHeight, 1 << 7))
            {
                hitGround++;
            }
        }

        // 조건 하나씩 검사 (세가지점 맞으면 오케이)
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
        // 설치하기
        item2Place.ToPlacedItem(position, rotation);

        // 종료 함수 호출
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

