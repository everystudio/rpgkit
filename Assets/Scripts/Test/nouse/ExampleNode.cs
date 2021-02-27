using UnityEditor.Experimental.GraphView;

public class ExampleNode : Node
{
    public ExampleNode()
    {
        title = "Example";

        // 入力用のポートを作成
        var inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(float)); // 第三引数をPort.Capacity.Multipleにすると複数のポートへの接続が可能になる
        inputPort.portName = "Input";
        inputContainer.Add(inputPort); // 入力用ポートはinputContainerに追加する

        // 出力用のポートを作る
        var outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(float));
        outputPort.portName = "Value";
        outputContainer.Add(outputPort); // 出力用ポートはoutputContainerに追加する
    }
}