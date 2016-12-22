using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

    private SpriteRenderer render;

    public Point point;//引用类型 不是值类型 所以必须new一个


    void Awake() {
        point = new Point(0, 0, 1);
        //sprite = this.GetComponent<SpriteRenderer>().sprite;这种方法取到的不是引用
        render = this.GetComponent<SpriteRenderer>();
        
    }

    void Start() {
        Vector3 position = this.transform.position;
        position.x += (point.x * 0.6f);
        position.y += (point.y * 0.6f)-6;
        this.transform.position = position;
        render.sprite = GameManager._instance.sprites[point.value];
    }
}


