using UnityEngine;

#nullable enable

public class PlayerScript : MonoBehaviour
{
  CharacterController? controller;

  void Awake()
  {
    controller = GetComponent<CharacterController>();
  }

  void Update()
  {
    var x = Input.GetAxis("Horizontal");
    var z = Input.GetAxis("Vertical");

    var input = Vector3.ClampMagnitude(Vector3.forward * z + Vector3.right * x, 1f);

    controller?.SimpleMove(input * 5f);
  }
}
