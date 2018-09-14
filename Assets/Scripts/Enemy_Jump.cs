using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Jump : Enemy {

    Rigidbody2D _rb;
    [SerializeField] float _jumpPower;  // ジャンプ力

    // Use this for initialization
    void Start () {
        _rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update () {
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<Player>().ReceiveDamage(attackPower);
        }
        else if (col.gameObject.tag == "Ground")
        {
            _rb.AddForce(Vector2.up * _jumpPower * 10);
        }

    }
    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<Player>().ReceiveDamage(attackPower);
        }
        else if (col.gameObject.tag == "Ground")
        {
            _rb.AddForce(Vector2.up * _jumpPower * 10);
        }

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<Player>().ReceiveDamage(attackPower);
        }
        else if (col.gameObject.tag == "Ground")
        {
            _rb.AddForce(Vector2.up * _jumpPower * 10);
        }

    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<Player>().ReceiveDamage(attackPower);
        }
        else if (col.gameObject.tag == "Ground")
        {
            _rb.AddForce(Vector2.up * _jumpPower * 10);
        }

    }

}
