public enum Side  // Расположения узла относительно родителя 
{
    Left,
    Right
}

public class BinaryTreeNode<T> where T : IComparable   // Узел бинарного дерева <typeparam name="T"></typeparam>
{
    public BinaryTreeNode(T data)   // Конструктор класса <param name="data">Данные</param>
    {
        Data = data;
    }

    public T Data { get; set; }   // Данные которые хранятся в узле

    public BinaryTreeNode<T> LeftNode { get; set; }   // Левая ветка

    public BinaryTreeNode<T> RightNode { get; set; }  // Правая ветка

    public BinaryTreeNode<T> ParentNode { get; set; }   // Родитель

    public Side? NodeSide =>      // Расположение узла относительно его родителя

        ParentNode == null
        ? (Side?)null
        : ParentNode.LeftNode == this
            ? Side.Left
            : Side.Right;

    public override string ToString() => Data.ToString();  // Преобразование экземпляра класса в строку. Данные узла дерева

}
public class BinaryTree<T> where T : IComparable // Бинарное дерево <typeparam name="T">Тип данных хранящихся в узлах</typeparam>
{
    public BinaryTreeNode<T> RootNode { get; set; } // Корень бинарного дерева
    public BinaryTree<T> LeftNode { get; private set; }


    // Добавление нового узла в бинарное дерево
    // <param name="node">Новый узел</param>
    // <param name="currentNode">Текущий узел</param>
    // <returns>Узел</returns>

    public BinaryTreeNode<T> Add(BinaryTreeNode<T> node, BinaryTreeNode<T> currentNode = null)
    {
        if (RootNode == null)
        {
            node.ParentNode = null;
            return RootNode = node;
        }

        currentNode = currentNode ?? RootNode;
        node.ParentNode = currentNode;
        int result;
        return (result = node.Data.CompareTo(currentNode.Data)) == 0
            ? currentNode
            : result < 0
                ? currentNode.LeftNode == null
                    ? (currentNode.LeftNode = node)
                    : Add(node, currentNode.LeftNode)
                : currentNode.RightNode == null
                    ? (currentNode.RightNode = node)
                    : Add(node, currentNode.RightNode);
    }

    public BinaryTreeNode<T> Add(T data)
    {
        return Add(new BinaryTreeNode<T>(data));
    }

    public int Depth(BinaryTree<T> curentNode, int depth)
    {
        if (curentNode == null) return depth;
        depth++;
        return Depth(curentNode.LeftNode, depth);
    }

    public void Count(BinaryTree<T> root)
    {
        int depth = Depth(root, 0);

        int numOfUpperNodes = (int)(Math.Pow(2, depth - 1) - 1);

        int left = 0;
        int right = numOfUpperNodes; // On the level below there is numberOfUpperNodes + 1 (index is -1)
    }


    // Поиск узла по значению
    // <param name="data">Искомое значение</param>
    // <param name="startWithNode">Узел начала поиска</param>
    // <returns>Найденный узел</returns>

    public BinaryTreeNode<T>? FindNode(T data, BinaryTreeNode<T> startWithNode = null)
    {
        startWithNode = startWithNode ?? RootNode;
        int result;
        return (result = data.CompareTo(startWithNode.Data)) == 0
            ? startWithNode
            : result < 0
                ? startWithNode.LeftNode == null
                    ? null
                    : FindNode(data, startWithNode.LeftNode)
                : startWithNode.RightNode == null
                    ? null
                    : FindNode(data, startWithNode.RightNode);
    }



    // Удаление узла бинарного дерева
    // <param name="node">Узел для удаления</param>
    public void Remove(BinaryTreeNode<T> node)
    {
        if (node == null)
        {
            return;
        }

        var currentNodeSide = node.NodeSide;
        //если у узла нет подузлов, можно его удалить
        if (node.LeftNode == null && node.RightNode == null)
        {
            if (currentNodeSide == Side.Left)
            {
                node.ParentNode.LeftNode = null;
            }
            else
            {
                node.ParentNode.RightNode = null;
            }
        }
        //если нет левого, то правый ставим на место удаляемого 
        else if (node.LeftNode == null)
        {
            if (currentNodeSide == Side.Left)
            {
                node.ParentNode.LeftNode = node.RightNode;
            }
            else
            {
                node.ParentNode.RightNode = node.RightNode;
            }

            node.RightNode.ParentNode = node.ParentNode;
        }
        //если нет правого, то левый ставим на место удаляемого 
        else if (node.RightNode == null)
        {
            if (currentNodeSide == Side.Left)
            {
                node.ParentNode.LeftNode = node.LeftNode;
            }
            else
            {
                node.ParentNode.RightNode = node.LeftNode;
            }

            node.LeftNode.ParentNode = node.ParentNode;
        }
        //если оба дочерних присутствуют, 
        //то правый становится на место удаляемого,
        //а левый вставляется в правый
        else
        {
            switch (currentNodeSide)
            {
                case Side.Left:
                    node.ParentNode.LeftNode = node.RightNode;
                    node.RightNode.ParentNode = node.ParentNode;
                    Add(node.LeftNode, node.RightNode);
                    break;
                case Side.Right:
                    node.ParentNode.RightNode = node.RightNode;
                    node.RightNode.ParentNode = node.ParentNode;
                    Add(node.LeftNode, node.RightNode);
                    break;
                default:
                    var bufLeft = node.LeftNode;
                    var bufRightLeft = node.RightNode.LeftNode;
                    var bufRightRight = node.RightNode.RightNode;
                    node.Data = node.RightNode.Data;
                    node.RightNode = bufRightRight;
                    node.LeftNode = bufRightLeft;
                    Add(bufLeft, node);
                    break;
            }
        }
    }
    // Удаление узла дерева
    // <param name="data">Данные для удаления</param>
    public void Remove(T data)
    {
        var foundNode = FindNode(data);
        Remove(foundNode);
    }

}