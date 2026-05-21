using System;
using System.Collections.Generic;

public class BinarySearchTree
{
    private class Node
    {
        public int Key;
        public Node Left, Right;

        public Node(int key)
        {
            Key = key;
            Left = null;
            Right = null;
        }
    }

    private Node root;

    // Конструкторы
    public BinarySearchTree()
    {
        root = null;
    }

    public BinarySearchTree(int rootKey)
    {
        root = new Node(rootKey);
    }

    public BinarySearchTree(BinarySearchTree other)
    {
        root = Copy(other.root);
    }

    private Node Copy(Node node)
    {
        if (node == null) return null;
        Node newNode = new Node(node.Key);
        newNode.Left = Copy(node.Left);
        newNode.Right = Copy(node.Right);
        return newNode;
    }

    // Деструктор (финализатор)
    ~BinarySearchTree()
    {
        Clear(root);
    }

    private void Clear(Node node)
    {
        if (node == null) return;
        Clear(node.Left);
        Clear(node.Right);
    }

    // Добавление узла
    public void Insert(int key)
    {
        root = InsertRec(root, key);
    }

    private Node InsertRec(Node node, int key)
    {
        if (node == null) return new Node(key);
        if (key < node.Key) node.Left = InsertRec(node.Left, key);
        else if (key > node.Key) node.Right = InsertRec(node.Right, key);
        return node;
    }

    // Удаление узла
    public bool Remove(int key)
    {
        if (!Search(key)) return false;
        root = RemoveRec(root, key);
        return true;
    }

    private Node RemoveRec(Node node, int key)
    {
        if (node == null) return null;
        if (key < node.Key) node.Left = RemoveRec(node.Left, key);
        else if (key > node.Key) node.Right = RemoveRec(node.Right, key);
        else
        {
            if (node.Left == null) return node.Right;
            if (node.Right == null) return node.Left;
            Node minNode = FindMin(node.Right);
            node.Key = minNode.Key;
            node.Right = RemoveRec(node.Right, minNode.Key);
        }
        return node;
    }

    private Node FindMin(Node node)
    {
        while (node.Left != null) node = node.Left;
        return node;
    }

    // Поиск
    public bool Search(int key)
    {
        return SearchRec(root, key);
    }

    private bool SearchRec(Node node, int key)
    {
        if (node == null) return false;
        if (key == node.Key) return true;
        if (key < node.Key) return SearchRec(node.Left, key);
        return SearchRec(node.Right, key);
    }

    // Высота дерева
    public int GetHeight()
    {
        return GetHeightRec(root);
    }

    private int GetHeightRec(Node node)
    {
        if (node == null) return 0;
        return Math.Max(GetHeightRec(node.Left), GetHeightRec(node.Right)) + 1;
    }

    // Объединение двух деревьев
    public void Merge(BinarySearchTree otherTree)
    {
        if (otherTree == null || otherTree.root == null) return;
        List<int> allValues = new List<int>();
        InOrderCollect(otherTree.root, allValues);
        foreach (int value in allValues) Insert(value);
    }

    private void InOrderCollect(Node node, List<int> list)
    {
        if (node == null) return;
        InOrderCollect(node.Left, list);
        list.Add(node.Key);
        InOrderCollect(node.Right, list);
    }

    // Подсчёт узлов на уровне
    public int CountNodesAtLevel(int level)
    {
        if (level < 0) return 0;
        return CountNodesAtLevelRec(root, level, 0);
    }
    private int CountNodesAtLevelRec(Node node, int targetLevel, int currentLevel)
    {
        if (node == null) return 0;
        if (currentLevel == targetLevel) return 1;
        return CountNodesAtLevelRec(node.Left, targetLevel, currentLevel + 1) +
               CountNodesAtLevelRec(node.Right, targetLevel, currentLevel + 1);
    }
    // Подобие двух деревьев
    public bool IsSimilar(BinarySearchTree other)
    {
        return IsSimilarRec(root, other?.root);
    }
    private bool IsSimilarRec(Node node1, Node node2)
    {
        if (node1 == null && node2 == null) return true;
        if (node1 == null || node2 == null) return false;
        return IsSimilarRec(node1.Left, node2.Left) &&
               IsSimilarRec(node1.Right, node2.Right);
    }
    // Вывод дерева (для тестирования)
    public void PrintInOrder()
    {
        PrintInOrderRec(root);
        Console.WriteLine();
    }

    private void PrintInOrderRec(Node node)
    {
        if (node == null) return;
        PrintInOrderRec(node.Left);
        Console.Write(node.Key + " ");
        PrintInOrderRec(node.Right);
    }
}

// Программа для тестирования
class Program
{
    static void Main()
    {
        Console.WriteLine("=== БИНАРНОЕ ДЕРЕВО СОРТИРОВКИ ===\n");

        BinarySearchTree tree = new BinarySearchTree();

        // Вставка элементов
        int[] values = { 50, 30, 80, 20, 40, 70, 90 };
        foreach (int v in values) tree.Insert(v);

        Console.Write("Дерево (симметричный обход): ");
        tree.PrintInOrder();

        Console.WriteLine($"Высота дерева: {tree.GetHeight()}");
        Console.WriteLine($"Узлов на уровне 1: {tree.CountNodesAtLevel(1)}");
        Console.WriteLine($"Узлов на уровне 2: {tree.CountNodesAtLevel(2)}");

        Console.WriteLine($"\nПоиск 40: {(tree.Search(40) ? "найден" : "не найден")}");
        Console.WriteLine($"Поиск 100: {(tree.Search(100) ? "найден" : "не найден")}");

        Console.WriteLine("\nУдаление узла 30...");
        tree.Remove(30);
        Console.Write("Дерево после удаления 30: ");
        tree.PrintInOrder();

        // Проверка подобия
        BinarySearchTree tree2 = new BinarySearchTree();
        tree2.Insert(10); tree2.Insert(5); tree2.Insert(15);

        BinarySearchTree tree3 = new BinarySearchTree();
        tree3.Insert(100); tree3.Insert(50); tree3.Insert(150);

        Console.WriteLine($"\nПодобны ли деревья 2 и 3? {tree2.IsSimilar(tree3)}");

        // Конструктор копирования
        BinarySearchTree tree4 = new BinarySearchTree(tree);
        Console.Write("Дерево 4 (копия дерева 1): ");
        tree4.PrintInOrder();

        Console.WriteLine("\n\nНажмите любую клавишу для выхода...");
        Console.ReadKey();
    }
}