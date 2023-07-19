using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Hex : MonoBehaviour
{
    [SerializeField] MeshCollider myClickCollider;
    [SerializeField] GridCell myCell;
    [SerializeField] Transform obj;
    bool hexMode;
    MeshRenderer mr;
    public List<MeshCollider> objsColls = new List<MeshCollider>();
    public GridCell MyCell {
        get { return myCell; }
        set { myCell = value; }
    }

    void Start()
    {
        Controller.hexMode += HexModeActive;
        Controller.playMode += PlayModeActive;
        mr = GetComponent<MeshRenderer>();
        myClickCollider = GetComponent<MeshCollider>();
        foreach (Transform item in obj) {
            if(item.TryGetComponent(out MeshCollider mc)) objsColls.Add(mc);
        }
        if (!Controller.Instance.IsInPlayerMode) HexModeActive();
        else PlayModeActive();
    }

    public void HexModeActive() {
        hexMode = true;
        DisableObjectsColliders();
        myClickCollider.enabled = true;
    }
    public void PlayModeActive() {
        hexMode = false;
        EnableObjectsColliders();
        myClickCollider.enabled = false;
    }

    void DisableObjectsColliders() {
        foreach (var item in objsColls) {
            item.enabled = false;
        }
    }

    void EnableObjectsColliders() {
        foreach (var item in objsColls) {
            item.enabled = true;
        }
    }

    private void OnMouseDown() {
        if (hexMode) {
            if(Controller.Instance.CurrentOperation == HexGrid.HexOperationType.RotateSingle) Rotate();
            if (Controller.Instance.CurrentOperation == HexGrid.HexOperationType.RotateAround) Controller.Instance.hexGrid.RotateAround(myCell);
            if (Controller.Instance.CurrentOperation == HexGrid.HexOperationType.SwitchPlace) {
                if(!myCell.startingCell) Controller.Instance.hexGrid.SwitchPlace(myCell);
            }
        }
    }

    public void RiseToMove() {
        transform.DOMoveY(15, .5f).SetEase(Ease.InCubic);
    }

    void Rotate() {
        transform.DOMoveY(15, .8f).SetEase(Ease.InCubic).OnComplete(() => {
            Quaternion newRot = transform.rotation * Quaternion.Euler(0, 60, 0);
            transform.DOLocalRotateQuaternion(newRot, .5f).OnComplete(()=> {
                transform.DOMoveY(0, .5f).SetEase(Ease.InCubic);
            });
        });
    }

    public void ChangePosition(GridCell targetCell, bool higher) {
        Vector3 newPos = new(targetCell.transform.position.x, 15, targetCell.transform.position.z);
        StartCoroutine(DelayClaimingNewCell(targetCell));
        transform.DOMoveY(higher?50:15, .8f).SetEase(Ease.InCubic).OnComplete(() => {
            transform.DOMove(newPos, 1).OnComplete(() => {
                transform.DOMoveY(targetCell.transform.position.y, .5f).SetEase(Ease.InCubic);
                Controller.Instance.hexGrid.AddNeighbours(myCell, true);
            });
        });
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.GetComponent<PlayerController>()) {
            collision.transform.parent = transform;
        }
    }

    IEnumerator DelayClaimingNewCell(GridCell targetCell) {
        yield return new WaitForSeconds(.3f);
        myCell.Taken = false;
        myCell.MyHex = null;
        yield return new WaitForSeconds(.3f);
        myCell = targetCell;
        myCell.Taken = true;
        myCell.MyHex = this;
    }
}
