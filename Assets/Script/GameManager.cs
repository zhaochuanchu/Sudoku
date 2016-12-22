using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum Level {
    easy,
    normal,
    hard
}

public class GameManager : MonoBehaviour {

    public static GameManager _instance;//实例


    public Sprite[] sprites;

    public GameObject item;//预设prefab 

    public List<GameObject> list;

    public Stack<Point> allList;//全数独point数组

    public Stack<Point> availableList;//最终显示的数独

    public Stack<int> recordList;//记录选取的合理value值

    public Level level;

    //public //获取所有item的实例

    void Awake() {
        _instance = this;//单例模式
        list = new List<GameObject>();
        recordList = new Stack<int>();
        initialize();
        level = Level.easy;
    }

	// Use this for initialization
	void Start () {

	}


    //初始化所有item
    public void initialize() {//初始化所有item
        allList = getFullSoduku();
        availableList = getSoduku(allList);//输出全数独，根据难度返回可填写的数独
        for (int i=1; i<=9; i++) {
            for(int j = 1; j <= 9; j++) {
                GameObject newGameObject = (GameObject)GameObject.Instantiate(item, transform.position, Quaternion.identity);//实例化预设 返回一个gameObject
                //newGameObject.GetComponent<Item>().point.x = i;       //i->x
                //newGameObject.GetComponent<Item>().point.y=j;         //j->y

                newGameObject.GetComponent<Item>().point = availableList.Peek();
                availableList.Pop();

                list.Add(newGameObject);
            }
        }
    }

    //销毁所有item
    public void destroyAllItem() {
        foreach(GameObject gameObject in list) {
            GameObject.Destroy(gameObject);
        }
        list.Clear();
    }

    //利用回溯算法生成一个全数独
    public Stack<Point> getFullSoduku() {//利用回溯算法生成一个全数独

        List<Point> firstLine = getFirstLine();

        //Stack<Point> listStack = new Stack<Point>(firstLine);//此方法也可以

        Stack<Point> listStack = new Stack<Point>();
        foreach (Point point in firstLine) {
            listStack.Push(point);
        }

        
        //将stack扩展到81位 后面value均置为0 无必要
        /*for (int j = 2; j <= 9; j++) {//y索引从2开始
            for (int i = 1; i <= 9; i++) {//x索引从1开始
                //list.Add(new Point(i, j, 0));//其余的均初始化为0
                Point point = new Point(i, j, 0);
                listStack.Push(point);
            }
        }*/


        foreach (Point point in listStack) {
            //print(point.value);
            recordList.Push(point.value);
        }
        recordList.Push(0);//初始0


        for (int j = 2; j <= 9; j++) {//y索引从2开始

            for (int i = 1; i <= 9; i++) {//x索引从1开始

                Point newPoint = new Point(i, j, 0);

                //如果栈顶是9直接尴尬了
                if (recordList.Peek() != 9) {

                    for (int value = recordList.Peek() + 1; value <= 9; value++) {//从栈顶+1开始循环 找寻合理值


                        Point wrongPoint = new Point(0, 0, 0);//调试用 记录冲突元素

                        newPoint.value = value;
                        listStack.Push(newPoint);

                        bool isAvailable = true;
                        foreach (Point point in listStack) {
                            //如果该value不合理
                            //先push把自己给算上了 shit
                            if ((listStack.Peek().x == point.x || listStack.Peek().y == point.y || listStack.Peek().place == point.place) && listStack.Peek().value == point.value
                                 && !(listStack.Peek().x == point.x && listStack.Peek().y == point.y)) {

                                isAvailable = false;
                                wrongPoint = point;

                            }
                        }

                        if (!isAvailable) {
                            //print("isNotAvailable!");
                            listStack.Pop();
                            if (value == 9) {//如果到9仍然不合理 多pop一次 返回上一层而不是下一层
                                print("返回上一层");
                                print("冲突时其坐标为(" + i + "," + j + ")此时冲突元素为(" + wrongPoint.x + "," + wrongPoint.y + "," + wrongPoint.value + ")");
                                listStack.Pop();
                                recordList.Pop();//把栈顶的push掉
                                i -= 2;         //调整序列x-1
                                if (i <= 0) {
                                    j--;
                                    i += 9;
                                }

                            }

                        }
                        else {
                            recordList.Pop();//把栈顶的数push掉 
                            recordList.Push(value);//更新value
                            recordList.Push(0);
                            break;//合理跳出循环
                        }

                    }

                }
                else {
                    print("返回上一层");
                    listStack.Pop();
                    recordList.Pop();//把栈顶的push掉
                    i -= 2;         //调整序列x-1
                    if (i <= 0) {
                        j--;
                        i += 9;
                    }
                }

            }
        }

        foreach(Point point in listStack) {
            print(point.value);
            //print(point.place);
        }

        firstLine.AddRange(listStack);

        /*foreach(int a in recordList) {
            print(a);
        }*/

        return listStack;
    }

    //生成第一行1-9
    public List<Point> getFirstLine() {//生成第一行1-9
        List<Point> firstLine = new List<Point>();
        System.Random random = new System.Random();//根据触发那刻的系统时间做为种子

        for(int i = 1; i <= 9; i++) {
            Point point = new Point(i,1,0);
            point.value = random.Next(1, 10);
            bool isEqual = true;

            while (isEqual) {
                isEqual = false;
                for (int j = 1; j <= firstLine.Count; j++) {
                    if (point.value == firstLine[j - 1].value) {
                        isEqual = true;
                        point.value = random.Next(1, 10);
                    }
                }
            }
            firstLine.Add(point);
        }
        return firstLine;

    }
    
    //根据难度获取可以填写的数独数列
    public Stack<Point> getSoduku(Stack<Point> fullSoduku) {
        Stack<Point> stackSoduku = fullSoduku;
        float rate = 0;
        switch (level) {
            case Level.easy:
                rate = 0.7f;
                break;
            case Level.normal:
                rate = 0.55f;
                break;
            case Level.hard:
                rate = 0.4f;
                break;
        }
        System.Random random = new System.Random();

        foreach(Point point in stackSoduku) {
            if ((float)random.Next(1, 100) / 100 > rate) {
                point.value = 0;
            }
        }

        return stackSoduku;
    }
  

}


