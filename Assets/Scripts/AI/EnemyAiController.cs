using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAiController : AiController {

    protected override void Start()
    {
        base.Start();
        GetComponent<CapsuleCollider>().enabled = false;
        StartCoroutine(ResetCollider());
    }

    IEnumerator ResetCollider()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<CapsuleCollider>().enabled = true;
    }

    private void Update()
    {
        if (Vector3.Distance(this.transform.position,PlayerManager.S_INSTANCE.player.transform.position) < 6)
        {
            movementController.MoveToTarget(PlayerManager.S_INSTANCE.player.transform.position, AiBehaviorEnum.Attack, 0);
        }
    }

    /// <summary>
    /// attacks player when it touches the trigger capsule
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            movementController.MoveToTarget(other.gameObject.transform.position, AiBehaviorEnum.Attack,0);
        }
    }
}
