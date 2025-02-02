using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _CellPrefab;
    [SerializeField] private GameObject _FinishSpotPrefab;

    [SerializeField] private GameObject _FrontParent;
    [SerializeField] private GameObject _BackParent;
    [SerializeField] private GameObject _LeftParent;
    [SerializeField] private GameObject _RightParent;
    [SerializeField] private GameObject _TopParent;
    [SerializeField] private GameObject _BottomParent;

    private List<GameObject> parents;
    private List<GameObject> children;

    public int sizeCube { get; set; }

    public static float coeff { get; set; } = 2;

    public static int length { get; set; }

    private void SpawnSideOfCube(GameObject Parent, MazeCubeGeneratorCell[,,] maze, int numberOfSide)
    {
        switch (numberOfSide)
        {
            case 1:
                for (int x = 1; x < length; x++)
                {
                    for (int y = 1; y < length; y++)
                    {
                        var FrontChild = Instantiate(_CellPrefab, new Vector3(x * coeff, y * coeff, 0), Quaternion.identity);
                        FrontChild.transform.SetParent(Parent.transform);

                        children.Add(FrontChild);

                        Cell cell = FrontChild.GetComponent<Cell>();
                        cell.WallLeft.SetActive(maze[x, y, 0].WallLeft);
                        cell.WallBottom.SetActive(maze[x, y, 0].WallBottom);
                        cell.floor.SetActive(maze[x, y, 0].floor);
                    }
                }
                break;
            case 2:
                for (int x = 1; x < length; x++)
                {
                    for (int y = 1; y < length; y++)
                    {
                        var BackChild = Instantiate(_CellPrefab, new Vector3(x * coeff, y * coeff, length * coeff), Quaternion.Euler(0, 180, 0));
                        BackChild.transform.SetParent(Parent.transform);

                        children.Add(BackChild);

                        Cell cell = BackChild.GetComponent<Cell>();
                        cell.WallLeft.SetActive(maze[x, y, length].WallLeft);
                        cell.WallBottom.SetActive(maze[x, y, length].WallBottom);
                        cell.floor.SetActive(maze[x, y, length].floor);
                    }
                }
                break;
            case 3:
                for (int z = 1; z < length; z++)
                {
                    for (int y = 1; y < length; y++)
                    {
                        var LeftChild = Instantiate(_CellPrefab, new Vector3(0, y * coeff, z * coeff), Quaternion.Euler(0, 90, 0));
                        LeftChild.transform.SetParent(Parent.transform);

                        children.Add(LeftChild);

                        Cell cell = LeftChild.GetComponent<Cell>();
                        cell.WallLeft.SetActive(maze[0, y, z].WallLeft);
                        cell.WallBottom.SetActive(maze[0, y, z].WallBottom);
                        cell.floor.SetActive(maze[0, y, z].floor);
                    }
                }
                break;
            case 4:
                for (int z = 1; z < length; z++)
                {
                    for (int y = 1; y < length; y++)
                    {
                        var RightChild = Instantiate(_CellPrefab, new Vector3(length * coeff, y * coeff, z * coeff), Quaternion.Euler(0, -90, 0));
                        RightChild.transform.SetParent(Parent.transform);

                        children.Add(RightChild);

                        Cell cell = RightChild.GetComponent<Cell>();
                        cell.WallLeft.SetActive(maze[length, y, z].WallLeft);
                        cell.WallBottom.SetActive(maze[length, y, z].WallBottom);
                        cell.floor.SetActive(maze[length, y, z].floor);
                    }
                }
                break;
            case 5:
                for (int z = 1; z < length; z++)
                {
                    for (int x = 1; x < length; x++)
                    {
                        var BottomChild = Instantiate(_CellPrefab, new Vector3(x * coeff, 0, z * coeff), Quaternion.Euler(-90, 0, 0));
                        BottomChild.transform.SetParent(Parent.transform);

                        children.Add(BottomChild);

                        Cell cell = BottomChild.GetComponent<Cell>();
                        cell.WallLeft.SetActive(maze[x, 0, z].WallLeft);
                        cell.WallBottom.SetActive(maze[x, 0, z].WallBottom);
                        cell.floor.SetActive(maze[x, 0, z].floor);
                    }
                }
                break;
            case 6:
                for (int z = 1; z < length; z++)
                {
                    for (int x = 1; x < length; x++)
                    {
                        var TopChild = Instantiate(_CellPrefab, new Vector3(x * coeff, length * coeff, z * coeff), Quaternion.Euler(90, 0, 0));
                        TopChild.transform.SetParent(_TopParent.transform);

                        children.Add(TopChild);

                        Cell cell = TopChild.GetComponent<Cell>();
                        cell.WallLeft.SetActive(maze[x, length, z].WallLeft);
                        cell.WallBottom.SetActive(maze[x, length, z].WallBottom);
                        cell.floor.SetActive(maze[x, length, z].floor);
                    }
                }
                break;
        }
    }

    public void SpawnMaze()
    {
        parents = new List<GameObject>();
        children = new List<GameObject>();

        MazeCubeGenerator mazeGenerator = new MazeCubeGenerator(sizeCube);
        MazeCubeGeneratorCell[,,] maze = mazeGenerator.GenerateMaze();

        length = maze.GetLength(0) - 2;

        // ����� ����� ������ ����� ��������� ���� (��������, ������ � ��)
       
        SpawnSideOfCube(_FrontParent, maze, 1);

        _FrontParent.transform.position = new Vector3(0, 0, 0);

        parents.Add(_FrontParent);

   
        SpawnSideOfCube(_BackParent, maze, 2);

        _BackParent.transform.position = new Vector3(0.63f, 0, -1.01f);

        parents.Add(_BackParent);


        SpawnSideOfCube(_LeftParent, maze, 3);

        _LeftParent.transform.position = new Vector3(0.82f, 0, -0.19f);

        parents.Add(_LeftParent);


        SpawnSideOfCube(_RightParent, maze, 4);

        _RightParent.transform.position = new Vector3(-0.19f, 0, -0.82f);

        parents.Add(_RightParent);

    
        SpawnSideOfCube(_BottomParent, maze, 5);

        _BottomParent.transform.position = new Vector3(0, 0.515f, -0.501f);

        parents.Add(_BottomParent);

     
        SpawnSideOfCube(_TopParent, maze, 6);

        _TopParent.transform.position = new Vector3(0, -0.515f, -0.501f);

        parents.Add(_TopParent);

        SpawnFinish();
    }

    public void OnDestroy()
    {
        foreach (GameObject parent in parents)
        {
            if(parent != null)
            parent.transform.position = Vector3.zero;
        }

        foreach (GameObject child in children)
        {
            Destroy(child.gameObject);
        }
    }

    // �������, ������� �������� �������������� ����� � ������� ����� ������ � ���������
    private void SpawnFinish()
    {
        GameObject sideOfCube = parents[Random.Range(0, parents.Count)];

        List<Transform> cells = new List<Transform>();

        foreach (Transform cell in sideOfCube.transform)
        {
            cells.Add(cell);
        }

        var position = Random.Range(0, cells.Count);

        Transform spawnPlace = cells[position];

        var finish = Instantiate(_FinishSpotPrefab, spawnPlace.position, Quaternion.identity);

        finish.transform.SetParent(spawnPlace);
    }
}
