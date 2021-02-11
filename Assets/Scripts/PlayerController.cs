using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public Transform facing_dir;
    public float speed = 20.0f;

    private CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float z = Input.GetAxis("Vertical");
        float x = Input.GetAxis("Horizontal");

        Vector3 new_player_pos = facing_dir.transform.right * x + facing_dir.transform.forward * z;
        controller.Move(new_player_pos * Time.deltaTime * speed);
    }
}
