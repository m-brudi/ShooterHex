using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Hex : MonoBehaviour
{
    [SerializeField] GameObject ground;
    [SerializeField] MeshCollider groundCollider;
    [SerializeField] GridCell myCell;
    [SerializeField] Transform objects;
    [SerializeField] List<GameObject> enemies;
    [SerializeField] Transform[] enemiesSpawnPoints;
    [SerializeField] Transform decorations;
    [SerializeField] Transform enemiesParent;
    [SerializeField] GameObject enemyPrefab;
    bool hexMode;
    MeshCollider myClickCollider;
    //Quaternion enemiesRot;
    List<MeshCollider> objsColls = new List<MeshCollider>();
    bool actionDone = true;

    public GridCell MyCell {
        get { return myCell; }
        set { myCell = value; }
    }

    public void MakeMeStartHex() {
        //not sure but might come in handy later
    }

    public void Setup(GridCell cell) {
        Controller.hexMode += HexModeActive;
        Controller.playMode += PlayModeActive;
        myClickCollider = GetComponent<MeshCollider>();
        foreach (Transform item in objects) {
            if (item.TryGetComponent(out MeshCollider mc)) objsColls.Add(mc);
        }
        if (!Controller.Instance.IsInPlayerMode) HexModeActive();
        else PlayModeActive();
        myCell = cell;
        ground.GetComponent<MeshRenderer>().material.DOFade(1, 0);
    }

    public void SetupOnlyForShow() {
        myClickCollider = GetComponent<MeshCollider>();
        foreach (Transform item in objects) {
            if (item.TryGetComponent(out MeshCollider mc)) objsColls.Add(mc);
        }
        HexModeActive();
        ground.GetComponent<MeshRenderer>().material.DOFade(.5f, 0);

    }

    public void HexModeActive() {
        hexMode = true;
        DisableObjectsColliders();
        DeleteEnemies();
        if(myClickCollider)myClickCollider.enabled = true;
        if(groundCollider)groundCollider.enabled = false;
    }
    public void PlayModeActive() {
        hexMode = false;
        EnableObjectsColliders();
        SpawnEnemies();
        if (myClickCollider) myClickCollider.enabled = false;
        if (groundCollider) groundCollider.enabled = true;
    }

    void DeleteEnemies() {
        foreach (var item in enemies) {
            Destroy(item.gameObject);
        }
        enemies.Clear();
    }
    void SpawnEnemies() {
        List<Transform> spawnPoints = new List<Transform>(enemiesSpawnPoints);
        foreach (var item in spawnPoints) {
            float chance = Random.value;
            if(chance > .3f) {
                GameObject e = Instantiate(enemyPrefab, item.transform.position, Quaternion.identity, enemiesParent);
                enemies.Add(e);
            }
        }
    }

    void DisableObjectsColliders() {
        foreach(Transform child in decorations) {   
            child.gameObject.SetActive(false);
        }
        foreach (var item in objsColls) {
            item.enabled = false;
        }
    }

    void EnableObjectsColliders() {
        foreach (Transform child in decorations) {
            child.gameObject.SetActive(true);
        }
        foreach (var item in objsColls) {
            item.enabled = true;
        }
    }

    private void OnMouseDown() {
        if (hexMode && Controller.Instance.CanManipulateHex) {
            if (Controller.Instance.CurrentOperation == HexGrid.HexOperationType.RotateSingle) {
                if (!myCell.startingCell) Rotate();
            }
            if (Controller.Instance.CurrentOperation == HexGrid.HexOperationType.RotateAround) {
                Controller.Instance.hexGrid.RotateAround(myCell);
                Controller.Instance.Coins -= Controller.Instance.costOfOperation;
            }
            if (Controller.Instance.CurrentOperation == HexGrid.HexOperationType.SwitchPlace) {
                if (!myCell.startingCell) {
                    Controller.Instance.hexGrid.SwitchPlace(myCell);
                    Controller.Instance.Coins -= Controller.Instance.costOfOperation;
                }
            }
        }
    }

    public void RiseToMove() {
        transform.DOMoveY(15, .5f).SetEase(Ease.InCubic);
    }

    void Rotate() {
        if (actionDone) {
            Controller.Instance.Coins -= Controller.Instance.costOfOperation;
            actionDone = false;
            transform.DOMoveY(15, .8f).SetEase(Ease.InCubic).OnComplete(() => {
                Quaternion newRot = transform.rotation * Quaternion.Euler(0, 60, 0);
                transform.DOLocalRotateQuaternion(newRot, .5f).OnComplete(() => {
                    transform.DOMoveY(-6, .5f).SetEase(Ease.InCubic);
                    actionDone = true;
                });
            });
        }
    }

    public void ChangePosition(GridCell targetCell, bool higher) {
        if (actionDone) {
            actionDone = false;
            Vector3 newPos = new(targetCell.transform.position.x, 15, targetCell.transform.position.z);
            StartCoroutine(DelayClaimingNewCell(targetCell));
            transform.DOMoveY(higher ? 50 : 15, .8f).SetEase(Ease.InCubic).OnComplete(() => {
                transform.DOMove(newPos, 1).OnComplete(() => {
                    transform.DOMoveY(-6, .5f).SetEase(Ease.InCubic);
                    Controller.Instance.hexGrid.AddNeighbours(myCell, true);
                    actionDone = true;
                });
            });
        }
    }

    void OnCollisionEnter(Collision collision) {
        if(!hexMode) {
            if (collision.gameObject.GetComponent<PlayerController>()) {
                collision.transform.parent = transform;
            }
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
