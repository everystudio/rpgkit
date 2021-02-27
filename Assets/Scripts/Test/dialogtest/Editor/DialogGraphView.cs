using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogGraphView : GraphView
{
	private readonly Vector2 defaultNodeSize = new Vector2(150, 200);

	public DialogGraphView()
	{
		styleSheets.Add(Resources.Load<StyleSheet>("DialogGraph"));
		SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

		this.AddManipulator(new ContentDragger());
		this.AddManipulator(new SelectionDragger());
		this.AddManipulator(new RectangleSelector());

		var grid = new GridBackground();
		Insert(0, grid);
		grid.StretchToParentSize();

		AddElement(GenerateEntryPointNode());
	}
	private Port GeneratePort(DialogNode node , Direction portDirection,Port.Capacity capacity = Port.Capacity.Single)
	{
		return node.InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(float));
	}

	private DialogNode GenerateEntryPointNode()
	{
		var node = new DialogNode
		{
			title = "Start",
			GUID = System.Guid.NewGuid().ToString(),
			DialogText = "ENTRYPOINT",
			EntryPoint = true
		};

		var generatePort = GeneratePort(node, Direction.Output);
		generatePort.portName = "Next";
		node.outputContainer.Add(generatePort);

		var button = new Button(() =>
		{
			AddChoicePort(node);
		});
		button.text = "New Choice";
		node.titleContainer.Add(button);

		node.RefreshExpandedState();
		node.RefreshPorts();

		node.SetPosition(new Rect(100, 200, 100, 150));
		return node;
	}

	private void AddChoicePort(DialogNode node)
	{
		var generatePort = GeneratePort(node, Direction.Output);

		var outputPortCount = node.outputContainer.Query("connector").ToList().Count;
		generatePort.portName = $"Choice {outputPortCount}";

		node.outputContainer.Add(generatePort);
		node.RefreshExpandedState();
		node.RefreshPorts();
	}

	public void CreateNode(string nodeName)
	{
		AddElement(CreateDialogNode(nodeName));
	}

	public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
	{
		var compartiblePorts = new List<Port>();

		ports.ForEach(port =>
		{
			if (startPort != port && startPort.node != port.node)
			{
				compartiblePorts.Add(port);
			}
		});

		return compartiblePorts;
	}

	public DialogNode CreateDialogNode(string nodename)
	{
		var dialogNode = new DialogNode
		{
			title = "nodeName",
			DialogText = "nodeName",
			GUID = System.Guid.NewGuid().ToString()
		};
		var inputPort = GeneratePort(dialogNode, Direction.Input, Port.Capacity.Multi);
		inputPort.portName = "Input";
		dialogNode.inputContainer.Add(inputPort);
		dialogNode.RefreshExpandedState();
		dialogNode.RefreshPorts();
		dialogNode.SetPosition(new Rect(Vector2.zero, defaultNodeSize));
		dialogNode.SetPosition(new Rect(Vector2.zero, defaultNodeSize));
		return dialogNode;
	}
}
