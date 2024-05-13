using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryFollowPlayer : MonoBehaviour
{
    private Transform _playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        _playerTransform = Util.FindChildWithPath<Transform>("@playerChracter");
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(_playerTransform.position.y);
        transform.position = new Vector2(
            transform.position.x,
            Mathf.Clamp(_playerTransform.position.y, 0f, 10f));// ¾È µÊ..
    }
}
