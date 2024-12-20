using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody rigid;
    SpriteRenderer spriteRenderer;
    new Collider collider;
    Component movement;
    public float removeHeight = -50f;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider>();
    }

    void Update()
    {
        // 적의 y좌표가 지정된 높이 이하로 떨어지면 제거
        if (transform.position.y <= removeHeight)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    public void OnDamaged()
    {
        //Sprite Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        //Sprite Flip Y
        spriteRenderer.flipY = true;
        //Collider DIsable
        collider.enabled = false;
        //Die Effect Jump
        rigid.AddForce(Vector3.up * 5, ForceMode.Impulse);
        //Destroy
        Invoke("DeActive", 2);
        //활동 스크립트 꺼버리기
        gameObject.GetComponent<MonoBehaviour>().enabled = false;
        gameObject.GetComponent<Animator>().enabled = false;
       

    }
    void DeActive()
    {
        gameObject.SetActive(false);
    }

}
