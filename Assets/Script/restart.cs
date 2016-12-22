using UnityEngine;
using System.Collections;

public class restart : MonoBehaviour {

	
    //点击事件
    void OnMouseUpAsButton() {
        print("点击重置");
        GameManager._instance.destroyAllItem();

    }

}
