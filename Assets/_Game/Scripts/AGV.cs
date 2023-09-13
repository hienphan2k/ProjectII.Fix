using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
using Photon.Pun;
using Sirenix.OdinInspector;
using System;
using System.Linq;

public class AGV : MonoBehaviour, ICheckCollision
{
    private Transform cachedTransform;
    private bool stationary = true;
    public Junction currentJunction;
    public float velocity = 5f;
    public bool blocked = false;

    public float time_start, time_finish; // time dừng và time rời tại một junction

    private void Awake()
    {
        cachedTransform = transform;
    }

    // dijkstra---------------------------
    public List<Junction> junctions {get; set;} // list cac junctions
    int n;
    private float[] d = new float[100]; // khoang cach tu diem xuat phat den diem hien tai
    private float[] h = new float[100]; // khoang cach chim bay tu diem dich den diem hien tai
    private int[] p = new int[100]; // duong da di duoc tu truoc do
    List<int>[] edge = new List<int>[100]; // danh sach canh ke (luu theo index junction)
    public int isCollided = 0;
    List<int> path = new List<int>(); // danh sach cac junction agv can phai di
    public void GetGraph(List<Junction> junctions)
    {
        this.n = junctions.Count; // so luong junction
        this.junctions = junctions.GetRange(0, n);

        for (int i = 0; i < n+2; i++) 
        {
            edge[i] = new List<int>();
        }   

        for(int i=0; i<n; i++)
        {
            Junction u = this.junctions[i];
            int idu = this.junctions.IndexOf(u);
            foreach(Junction v in u.Junctions) //loop qua cac junction v ke voi u
            {
                int idv = this.junctions.IndexOf(v);
                edge[idu].Add(idv);
            }
        }
    }
    //----------------------------------

    void Update()
    {
        MoveWithPath();
    }
    
    // di chuyen voi mot day duong di
    public void MoveWithPath()
    {
        // neu di chuyen het path thi khong con duong di nua => stop
        if (this.path.Count == 0)
            return;
        int next = GetNextJunc(this.junctions[path.ElementAt(0)]);
        if(next == 1)
        {
            this.path.RemoveAt(0);
            if(this.path.Count>0)
            {
                this.Deliver(this.path[this.path.Count-1]);
            }
        }
    }
    //lay day duong di den diem t 
    public void Deliver(int t)
    {
        this.path.Clear();
        int s = this.junctions.IndexOf(currentJunction);
        //Debug.Log("xxx " + s +" "+ t);
        // Dijkstra(s, t);
        // A_star(s, t);
        // Arrival_dijkstra(s, t);
        Harmfulness_dijkstra(s, t);
        this.path = TraceBack(s, t);
    }
    // truy vet duong di sau khi dijkstra
    public List<int> TraceBack(int s, int t)
    {
        List<int> path = new List<int>();
        //Debug.Log("start "+s);
        while(t!=s)
        {
            // Debug.Log("Current "+t);
            path.Add(t);
            t = p[t];
           
        }
        path.Reverse();
        return path;
    }
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ICheckCollision>(out _) )
        {
            var heading = other.transform.position - cachedTransform.position;
            // heading.Normalize();
            var dot = Vector3.Dot(heading, cachedTransform.forward);
            // var angleInRadians = Mathf.Acos(dot);
            // var angleInDegrees = angleInRadians * Mathf.Rad2Deg;
            if ( dot > 0f )
            {
                this.isCollided++;
                
            }
            // Debug.Log(this.currentJunction.name + "  " + dot + "  " + this.isCollided);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<ICheckCollision>(out _))
        {
            var heading = other.transform.position - cachedTransform.position;
            // heading.Normalize();
            var dot = Vector3.Dot(heading, cachedTransform.forward);
            // var angleInRadians = Mathf.Acos(dot);
            // var angleInDegrees = angleInRadians * Mathf.Rad2Deg;
            if ( dot > 0f )
                this.isCollided--;
            //Debug.Log(this.gameObject.name + " " + other.gameObject.name);
        }
    }

    //di chuyen den junction tiep theo
    // tra ve 1 neu nhu da den vi tri tiep theo
    public int GetNextJunc(Junction nextJunction)
    {
        if(this.isCollided > 0 || this.blocked)
        {
            return 0;
        }
        //Xoay
        Vector3 direction = nextJunction.transform.position - cachedTransform.position;
        direction.y = 0; 
        direction.Normalize();
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        // Debug.Log(targetRotation.x+ " "+ targetRotation.y+" "+targetRotation.z+" "+targetRotation.w);
        cachedTransform.rotation = Quaternion.Slerp(cachedTransform.rotation, targetRotation, velocity * Time.deltaTime);
        //di chuyen
        cachedTransform.position = Vector3.MoveTowards(cachedTransform.position, nextJunction.transform.position, velocity * Time.deltaTime);
        if( (cachedTransform.position - nextJunction.transform.position).sqrMagnitude < 1e-5)
        {
            currentJunction = nextJunction;
            return 1;
            //if(gameObject.GetComponent<PhotonView>().ViewID == 1001)
            //{
            //    Debug.Log(currentJunction + " " + this.junctions.IndexOf(currentJunction));
            //}
        }
        return 0;
    }
    //---------------------------------------------------------
    void Dijkstra(int s, int t)
    {
        for(int i=0;i<n;i++)
        {
            p[i] = -1;
            d[i] = float.MaxValue;
        }
        d[s] = 0;
        var qu = new SortedSet<KeyValuePair<float, int>>(new PairComparer());
        qu.Add(new KeyValuePair<float, int>(d[s], s));
        while (qu.Count > 0)
        {
            var tmp = qu.FirstOrDefault();
            float du = tmp.Key;
            int u = tmp.Value;
            qu.Remove(tmp);
            //Debug.Log(qu.Count + " " + u + " " + d[u] + " "+ du);
            if (du > d[u])
                continue;
            
            foreach (int v in edge[u])
            {
                float w = Vector3.Distance(junctions[u].transform.position, junctions[v].transform.position);
                if (d[u] + w < d[v])
                {
                    d[v] = d[u] + w;
                    p[v] = u;
                    qu.Add(new KeyValuePair<float, int>(d[v], v));
                }
            }
        }
    }

    void A_star(int s, int t)
    {
        int[] cx = new int[100]; 
        for(int i=0;i<n;i++)
        {
            p[i] = -1;
            d[i] = float.MaxValue;
            h[i] = Vector3.Distance(junctions[i].transform.position, junctions[t].transform.position);
        }
        d[s] = 0;
        var qu = new SortedSet<KeyValuePair<float, int>>(new PairComparer());
        qu.Add(new KeyValuePair<float, int>(d[s] + h[s], s));
        while (qu.Count > 0)
        {
            var tmp = qu.FirstOrDefault();
            float fu = tmp.Key;
            int u = tmp.Value;
            qu.Remove(tmp);
            // Debug.Log(qu.Count + " " + u + " " + d[u] + " " + fu);
            if (cx[u] == 1)
                continue;
            cx[u] = 1;
            foreach (int v in edge[u])
            {
                float w = Vector3.Distance(junctions[u].transform.position, junctions[v].transform.position);
                if (d[u] + w < d[v])
                {
                    d[v] = d[u] + w;
                    p[v] = u;
                    qu.Add(new KeyValuePair<float, int>(d[v] + h[v], v));
                }
            }
        }
    }

    void Arrival_dijkstra(int s, int t)
    {
        int[] cx = new int[100]; 
        float[] weight = new float[100]; // luu weight cua junction
        for(int i=0;i<n;i++)
        {
            p[i] = -1;
            d[i] = float.MaxValue;
            weight[i] = junctions[i].weight; // lay weight cua junction
        }
        d[s] = 0;
        var qu = new SortedSet<KeyValuePair<float, int>>(new PairComparer());
        qu.Add(new KeyValuePair<float, int>(d[s], s));
        while (qu.Count > 0)
        {
            var tmp = qu.FirstOrDefault();
            float fu = tmp.Key;
            int u = tmp.Value;
            qu.Remove(tmp);
            // Debug.Log(qu.Count + " " + u + " " + d[u] + " " + fu);
            if (cx[u] == 1)
                continue;
            cx[u] = 1;
            foreach (int v in edge[u])
            {
                float w = Vector3.Distance(junctions[u].transform.position, junctions[v].transform.position);
                float c = w / this.velocity; // trong so canh
                float C = weight[v] + c; // heuristic
                if (d[u] + C < d[v])
                {
                    d[v] = d[u] + C;
                    p[v] = u;
                    qu.Add(new KeyValuePair<float, int>(d[v], v));
                }
            }
        }
    }

    void Harmfulness_dijkstra(int s, int t)
    {
        int[] cx = new int[100]; 
        float[] weight = new float[100]; // luu weight cua junction
        for(int i=0;i<n;i++)
        {
            p[i] = -1;
            d[i] = float.MaxValue;
            weight[i] = junctions[i].weight; // lay weight cua junction
        }
        d[s] = 0;
        var qu = new SortedSet<KeyValuePair<float, int>>(new PairComparer());
        qu.Add(new KeyValuePair<float, int>(d[s], s));
        while (qu.Count > 0)
        {
            var tmp = qu.FirstOrDefault();
            float fu = tmp.Key;
            int u = tmp.Value;
            qu.Remove(tmp);
            // Debug.Log(qu.Count + " " + u + " " + d[u] + " " + fu);
            if (cx[u] == 1)
                continue;
            cx[u] = 1;
            foreach (int v in edge[u])
            {
                float no_people_agv = Utils.CountObstacles(junctions[u].GetComponent<Collider>(), junctions[v].GetComponent<Collider>());
                float area = Utils.GetOverlapBoxAreaXZ(junctions[u].GetComponent<Collider>(), junctions[v].GetComponent<Collider>());
                float density = no_people_agv/area;
                float w = Vector3.Distance(junctions[u].transform.position, junctions[v].transform.position);
                float c = w / this.velocity; // trong so canh
                float C = weight[v] + c + density * 10; // heuristic
                if (d[u] + C < d[v])
                {
                    d[v] = d[u] + C;
                    p[v] = u;
                    qu.Add(new KeyValuePair<float, int>(d[v], v));
                }
            }
        }
    }
    class PairComparer : IComparer<KeyValuePair<float, int>>
    {
        public int Compare(KeyValuePair<float, int> x, KeyValuePair<float, int> y)
        {
        // So sánh thứ tự của x và y theo tiêu chí của bạn
        // Ví dụ, so sánh trường đầu tiên của pair:
        return x.Key.CompareTo(y.Key);
        }
    }

}
public class Utils
{
    
    public static int CountObstacles(Collider sphereColliderA, Collider sphereColliderB) // counts number of object/people on the way
    {
        Collider[] obstacles = Physics.OverlapBox(GetCenterPoint(sphereColliderA, sphereColliderB), GetHalfExtents(sphereColliderA, sphereColliderB), Quaternion.identity);

        int obstacleCount = 0;
        foreach (Collider obstacle in obstacles)
        {
            if (obstacle != sphereColliderA && obstacle != sphereColliderB && obstacle.TryGetComponent<ICheckCollision>(out _))
            {
                GameObject obj = obstacle.gameObject;
                // Debug.Log("----------" + obj.name);
                obstacleCount++;
            }
        }

        // Debug.Log("Number of obstacles between the boxes: " + obstacleCount);
        return obstacleCount;
    }
    public static Vector3 GetCenterPoint(Collider sphereColliderA, Collider sphereColliderB)
    {
        return (sphereColliderA.bounds.center + sphereColliderB.bounds.center) * 0.5f;
    }

    public static Vector3 GetHalfExtents(Collider sphereColliderA, Collider sphereColliderB)
    {
        Vector3 size = new Vector3(
            Mathf.Abs(sphereColliderA.bounds.center.x - sphereColliderB.bounds.center.x) + ((SphereCollider)sphereColliderA).radius *2,
            Mathf.Abs(sphereColliderA.bounds.center.y - sphereColliderB.bounds.center.y) + ((SphereCollider)sphereColliderA).radius *2,
            Mathf.Abs(sphereColliderA.bounds.center.z - sphereColliderB.bounds.center.z) + ((SphereCollider)sphereColliderA).radius *2
        );
        // Debug.Log(sphereColliderA.bounds.center.x+"---------------------________________"+sphereColliderB.bounds.center.x);
        return size*0.5f;
    }
    public static float GetOverlapBoxAreaXZ(Collider sphereColliderA, Collider sphereColliderB)
    {
        var halfExtents = GetHalfExtents(sphereColliderA, sphereColliderB);
        float length = halfExtents.x * 2;
        float width = halfExtents.z * 2;
        float area = length * width;
        return area;
    }
}