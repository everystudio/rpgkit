using UnityEditor.Experimental.GraphView;

public class OutputNode : Node
{
    public OutputNode()
    {
        title = "Output";
        var port = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(float));
        port.portName = "Value";
        inputContainer.Add(port);
    }
}



