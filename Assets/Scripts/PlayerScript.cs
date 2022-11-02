using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#nullable enable

public class PlayerScript : MonoBehaviour
{
  static PlayerScript instance = default!;

  CharacterController? controller;

  [SerializeField]
  Text? pointsText;
  [SerializeField]
  Text? speedText;

  int points = 0;
  bool isSpeedBoostActive = false;

  List<Coroutine> coroutines = new List<Coroutine>();

  public void AddPoints(int newPoints)
  {
    points += newPoints;

    UpdatePoints();
  }

  public void BuffSpeed()
  {
    foreach (var item in coroutines)
    {
      StopCoroutine(item);
    }

    coroutines.Clear();

    coroutines.Add(StartCoroutine(SpeedBuff()));
  }

  System.Collections.IEnumerator SpeedBuff()
  {
    isSpeedBoostActive = true;

    UpdateSpeed();

    yield return new WaitForSeconds(4f);

    isSpeedBoostActive = false;

    UpdateSpeed();
  }

  void UpdatePoints()
  {
    if (pointsText != null)
    {
      pointsText.text = points.ToString();
    }
  }

  void UpdateSpeed()
  {
    if (speedText != null)
    {
      speedText.text = (isSpeedBoostActive ? 10f : 5f).ToString("0.00");
    }
  }

  void Awake()
  {
    instance = this;

    controller = GetComponent<CharacterController>();
  }

  void OnControllerColliderHit(ControllerColliderHit hit)
  {
    if (hit.collider.tag == "PointsBuff")
    {
      AddPoints(10);

      Destroy(hit.collider.gameObject);
    }
    else if (hit.collider.tag == "SpeedBuff")
    {
      BuffSpeed();

      Destroy(hit.collider.gameObject);
    }
  }

  void Start()
  {
    UpdatePoints();
    UpdateSpeed();
  }

  void Update()
  {
    var x = Input.GetAxis("Horizontal");
    var z = Input.GetAxis("Vertical");

    var input = Vector3.ClampMagnitude(Vector3.forward * z + Vector3.right * x, 1f);

    controller?.SimpleMove(input * (isSpeedBoostActive ? 10f : 5f));
  }
}
