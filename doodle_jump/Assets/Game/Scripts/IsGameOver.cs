using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IsGameOver : MonoBehaviour
{
    private GameObject _isGameOver;
    private BoxCollider2D _boxCollider;
    private GameObject _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _isGameOver = Resources.Load<GameObject>("Prefabs/isGameOver");
        _isGameOver = Instantiate(_isGameOver, _player.transform.position, Quaternion.identity);
        _isGameOver.AddComponent<BoxCollider2D>();
        _isGameOver.AddComponent<AttachGameOver>();
        _boxCollider = _isGameOver.GetComponent<BoxCollider2D>();
        _boxCollider.size = new Vector2(4.5f, 1);
        _boxCollider.isTrigger = true;
        StartCoroutine(UpdateGameOverSpot());
    }

    IEnumerator UpdateGameOverSpot()
    {
        _isGameOver.transform.position = _player.transform.position - new Vector3(0, 5, 0);
        yield return new WaitForSeconds(3);
        StartCoroutine(UpdateGameOverSpot());
    }
}
