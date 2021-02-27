using UnityEditor.Experimental.GraphView;

public class AddNode : Node
{
    public AddNode()
    {
        title = "Add";

        var inputPort1 = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float));
        inputPort1.portName = "A";
        inputContainer.Add(inputPort1);

        var inputPort2 = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(float));
        inputPort2.portName = "B";
        inputContainer.Add(inputPort2);

        var outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(float));
        outputPort.portName = "Out";
        outputContainer.Add(outputPort);
    }
}