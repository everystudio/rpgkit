using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;

public class ValueNode : Node
{
    public ValueNode()
    {
        title = "Value";

        var port = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(float));
        port.portName = "Value";
        outputContainer.Add(port);

        extensionContainer.Add(new FloatField());
        RefreshExpandedState();
    }
}



