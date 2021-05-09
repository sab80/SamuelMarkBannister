using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTriggerRight : MonoBehaviour
{
    public int damageToAttack;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void DamageUpdate()
    {
        damageToAttack = GameObject.Find("PlayerGameObject").GetComponent<PlayerAttack>().currentAttackDamage;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.isTrigger != true && col.CompareTag("Enemy"))
        {
            Debug.Log("Ontrigger");
            Debug.Log(damageToAttack);
            col.SendMessageUpwards("TakeDamage", damageToAttack);
        }
    }
    // Update is called once per frame
    void Update()
    {
        DamageUpdate();
    }
}
