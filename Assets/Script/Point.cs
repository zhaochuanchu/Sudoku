using UnityEngine;
using System.Collections;

public class Point{//包含一个item的抽象信息

    public int x;//x位置索引

    public int y;//y位置索引

    public int value;//值1~9

    public int place;//所在宫1~9

    public Point(int index_x, int index_y, int value) {
        this.x = index_x;
        this.y = index_y;
        this.value = value;
        this.place = (this.x - 1) / 3 + 1 + (this.y - 1) / 3 * 3;
    }


}
