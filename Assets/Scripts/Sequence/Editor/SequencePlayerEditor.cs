using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace sequence
{
    [CustomEditor(typeof(SequencePlayer))]
    public class SequencePlayerEditor : Editor
    {
        public class SequenceTypePair
        {
            public System.Type SequenceType;
            public string SequenceName;
        }

        static class SequenceCopy
        {
            static public System.Type Type { get; private set; }
            static List<SerializedProperty> Properties = new List<SerializedProperty>();
            static string[] IgnoreList = new string[]
            {
                "m_GameObject",
                "m_Enabled",
                "m_EditorHideFlags",
                "m_Script",
                "m_Name",
                "m_EditorClassIdentifier"
            };

            static public void Copy(SerializedObject serializedObject)
            {
                Type = serializedObject.targetObject.GetType();
                Properties.Clear();

                SerializedProperty property = serializedObject.GetIterator();
                property.Next(true);
                do
                {
                    if (!IgnoreList.Contains(property.name))
                    {
                        Properties.Add(property.Copy());
                    }
                } while (property.Next(false));
            }
            static public void Paste(SerializedObject target)
            {
                if (target.targetObject.GetType() == Type)
                {
                    for (int i = 0; i < Properties.Count; i++)
                    {
                        target.CopyFromSerializedProperty(Properties[i]);
                    }
                }
            }
            static public bool HasCopy()
            {
                return Properties != null && Properties.Count > 0;
            }

            static public SequencePlayer CopyAllSourceSequencePlayer;


            static public void CopyAll(SequencePlayer sourceSequencePlayer)
            {
                CopyAllSourceSequencePlayer = sourceSequencePlayer;
            }

            static public bool HasMultipleCopies()
            {
                return (CopyAllSourceSequencePlayer != null);
            }

            static public void PasteAll(SequencePlayerEditor targetEditor)
            {
                var sourceFeedbacks = new SerializedObject(CopyAllSourceSequencePlayer);
                SerializedProperty feedbacks = sourceFeedbacks.FindProperty("SequencePlayer");

                for (int i = 0; i < feedbacks.arraySize; i++)
                {
                    SequenceBase arrayFeedback = (feedbacks.GetArrayElementAtIndex(i).objectReferenceValue as SequenceBase);

                    SequenceCopy.Copy(new SerializedObject(arrayFeedback));
                    SequenceBase newFeedback = targetEditor.AddSequence(arrayFeedback.GetType());
                    SerializedObject serialized = new SerializedObject(newFeedback);
                    serialized.Update();
                    SequenceCopy.Paste(serialized);
                    serialized.ApplyModifiedProperties();
                }

                CopyAllSourceSequencePlayer = null;
            }
        }
        protected SerializedProperty _mmfeedbacks;
        protected SerializedProperty _mmfeedbacksInitializationMode;
        protected SerializedProperty _mmfeedbacksSafeMode;
        protected SerializedProperty _mmfeedbacksAutoPlayOnStart;
        protected Dictionary<SequenceBase, Editor> _editors;
        protected List<SequenceTypePair> _typesAndNames = new List<SequenceTypePair>();
        protected string[] _typeDisplays;
        protected int _draggedStartID = -1;
        protected int _draggedEndID = -1;
        private static bool _debugView = false;

        void OnEnable()
        {
            // Get properties
            _mmfeedbacks = serializedObject.FindProperty("Sequences");

            _mmfeedbacksInitializationMode = serializedObject.FindProperty("InitializationMode");
            _mmfeedbacksSafeMode = serializedObject.FindProperty("SafeMode");
            _mmfeedbacksAutoPlayOnStart = serializedObject.FindProperty("AutoPlayOnStart");

            // Repair routine to catch feedbacks that may have escaped due to Unity's serialization issues
            RepairRoutine();

            // Create editors
            _editors = new Dictionary<SequenceBase, Editor>();
            for (int i = 0; i < _mmfeedbacks.arraySize; i++)
            {
                AddEditor(_mmfeedbacks.GetArrayElementAtIndex(i).objectReferenceValue as SequenceBase);
            }

            // Retrieve available feedbacks
            List<System.Type> types = (from domainAssembly in System.AppDomain.CurrentDomain.GetAssemblies()
                                       from assemblyType in domainAssembly.GetTypes()
                                       where assemblyType.IsSubclassOf(typeof(SequenceBase))
                                       select assemblyType).ToList();

            // Create display list from types
            List<string> typeNames = new List<string>();
            for (int i = 0; i < types.Count; i++)
            {
                SequenceTypePair newType = new SequenceTypePair();
                newType.SequenceType = types[i];
                newType.SequenceName = SequencePathAttribute.GetFeedbackDefaultPath(types[i]);
                if (newType.SequenceName == "SequenceBase")
                {
                    continue;
                }
                _typesAndNames.Add(newType);
            }

            _typesAndNames = _typesAndNames.OrderBy(t => t.SequenceName).ToList();

            typeNames.Add("Add new Sequence...");
            for (int i = 0; i < _typesAndNames.Count; i++)
            {
                typeNames.Add(_typesAndNames[i].SequenceName);
            }

            _typeDisplays = typeNames.ToArray();
        }
        protected virtual void RepairRoutine()
        {
            SequencePlayer player = target as SequencePlayer;

            if ((player.SafeMode == SequencePlayer.SafeModes.EditorOnly) || (player.SafeMode == SequencePlayer.SafeModes.Full))
            {
                player.AutoRepair();
            }

            serializedObject.ApplyModifiedProperties();
        }

        public override void OnInspectorGUI()
        {
            var e = Event.current;

            // Update object

            serializedObject.Update();

            Undo.RecordObject(target, "Modified Feedback Manager");

            EditorGUILayout.Space();

            /*
            if (!SequencePlayer.GlobalMMFeedbacksActive)
            {
                Color baseColor = GUI.color;
                GUI.color = Color.red;
                EditorGUILayout.HelpBox("All MMFeedbacks, including this one, are currently disabled. This is done via script, by changing the value of the MMFeedbacks.GlobalMMFeedbacksActive boolean. Right now this value has been set to false. Setting it back to true will allow MMFeedbacks to play again.", MessageType.Warning);
                EditorGUILayout.Space();
                GUI.color = baseColor;
            }

            EditorGUILayout.HelpBox("Select feedbacks from the 'add a feedback' dropdown and customize them. Remember, if you don't use auto initialization (Awake or Start), " +
                                    "you'll need to initialize them via script.", MessageType.None);
            */


            // Initialisation

            SequenceStyle.DrawSection("Settings");

            EditorGUILayout.PropertyField(_mmfeedbacksInitializationMode);
            EditorGUILayout.PropertyField(_mmfeedbacksAutoPlayOnStart);
            EditorGUILayout.PropertyField(_mmfeedbacksSafeMode);

            // Draw list
            SequenceStyle.DrawSection("Sequences");

            for (int i = 0; i < _mmfeedbacks.arraySize; i++)
            {
                SequenceStyle.DrawSplitter();

                SerializedProperty property = _mmfeedbacks.GetArrayElementAtIndex(i);

                // Failsafe but should not happen
                if (property.objectReferenceValue == null)
                {
                    continue;
                }

                // Retrieve feedback

                SequenceBase feedback = property.objectReferenceValue as SequenceBase;
                feedback.hideFlags = _debugView ? HideFlags.None : HideFlags.HideInInspector;

                Undo.RecordObject(feedback, "Modified Feedback");

                // Draw header

                int id = i;
                bool isExpanded = property.isExpanded;
                string label = feedback.Label;
                bool pause = false;

                if (feedback.Pause != null)
                {
                    pause = true;
                }
                if ((feedback.LooperPause == true) && (Application.isPlaying))
                {
                    if ((feedback as SequenceLooper).InfiniteLoop)
                    {
                        label = label + "[Infinite Loop] ";
                    }
                    else
                    {
                        label = label + "[ " + (feedback as SequenceLooper).NumberOfLoopsLeft + " loops left ] ";
                    }
                }

                Rect headerRect = SequenceStyle.DrawHeader(
                        ref isExpanded,
                        ref feedback.Config.Active,
                        label,
                        feedback.FeedbackColor,
                        (GenericMenu menu) =>
                        {
                            if (Application.isPlaying)
                            {
                                menu.AddItem(new GUIContent("Play"), false, () => PlaySequence(id));
                            }
                            else
                            {
                                menu.AddDisabledItem(new GUIContent("Play"));
                            }
                            menu.AddSeparator(null);
                            //menu.AddItem(new GUIContent("Reset"), false, () => ResetFeedback(id));
                            menu.AddItem(new GUIContent("Remove"), false, () => RemoveSequence(id));
                            menu.AddSeparator(null);
                            menu.AddItem(new GUIContent("Copy"), false, () => CopySequence(id));
                            if (SequenceCopy.HasCopy() && SequenceCopy.Type == feedback.GetType())
                                menu.AddItem(new GUIContent("Paste"), false, () => PasteSequence(id));
                            else
                                menu.AddDisabledItem(new GUIContent("Paste"));
                        },
                        feedback.SequenceStartedAt,
                        feedback.SequenceDuration,
                        feedback.Timing,
                        pause
                        );

                // Check if we start dragging this feedback

                switch (e.type)
                {
                    case EventType.MouseDown:
                        if (headerRect.Contains(e.mousePosition))
                        {
                            _draggedStartID = i;
                            e.Use();
                        }
                        break;
                    default:
                        break;
                }

                // Draw blue rect if feedback is being dragged

                if (_draggedStartID == i && headerRect != Rect.zero)
                {
                    Color color = new Color(0, 1, 1, 0.2f);
                    EditorGUI.DrawRect(headerRect, color);
                }

                // If hovering at the top of the feedback while dragging one, check where the feedback should be dropped : top or bottom

                if (headerRect.Contains(e.mousePosition))
                {
                    if (_draggedStartID >= 0)
                    {
                        _draggedEndID = i;

                        Rect headerSplit = headerRect;
                        headerSplit.height *= 0.5f;
                        headerSplit.y += headerSplit.height;
                        if (headerSplit.Contains(e.mousePosition))
                            _draggedEndID = i + 1;
                    }
                }

                // If expanded, draw feedback editor

                property.isExpanded = isExpanded;
                if (isExpanded)
                {
                    EditorGUI.BeginDisabledGroup(!feedback.Config.Active);

                    string helpText = SequenceHelpAttribute.GetSequenceHelpText(feedback.GetType());
                    if (!string.IsNullOrEmpty(helpText))
                    {
                        GUIStyle style = new GUIStyle(EditorStyles.helpBox);
                        style.richText = true;
                        float newHeight = style.CalcHeight(new GUIContent(helpText), EditorGUIUtility.currentViewWidth);
                        EditorGUILayout.LabelField(helpText, style);
                    }

                    EditorGUILayout.Space();

                    if (!_editors.ContainsKey(feedback))
                        AddEditor(feedback);

                    Editor editor = _editors[feedback];
                    CreateCachedEditor(feedback, feedback.GetType(), ref editor);

                    editor.OnInspectorGUI();

                    EditorGUI.EndDisabledGroup();

                    EditorGUILayout.Space();

                    EditorGUI.BeginDisabledGroup(!Application.isPlaying);
                    EditorGUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("Play", EditorStyles.miniButtonMid))
                        {
                            PlaySequence(id);
                        }
                        if (GUILayout.Button("Stop", EditorStyles.miniButtonMid))
                        {
                            StopSequence(id);
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUI.EndDisabledGroup();

                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                }
            }

            // Draw add new item

            if (_mmfeedbacks.arraySize > 0)
                SequenceStyle.DrawSplitter();

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            {
                // Feedback list

                int newItem = EditorGUILayout.Popup(0, _typeDisplays) - 1;
                if (newItem >= 0)
                {
                    AddSequence(_typesAndNames[newItem].SequenceType);
                }

                // Paste feedback copy as new

                if (SequenceCopy.HasCopy())
                {
                    if (GUILayout.Button("Paste as new", EditorStyles.miniButton, GUILayout.Width(EditorStyles.miniButton.CalcSize(new GUIContent("Paste as new")).x)))
                        PasteAsNew();
                }

                if (SequenceCopy.HasMultipleCopies())
                {
                    if (GUILayout.Button("Paste all as new", EditorStyles.miniButton, GUILayout.Width(EditorStyles.miniButton.CalcSize(new GUIContent("Paste all as new")).x)))
                        PasteAllAsNew();
                }
            }

            if (!SequenceCopy.HasMultipleCopies())
            {
                if (GUILayout.Button("Copy all", EditorStyles.miniButton, GUILayout.Width(EditorStyles.miniButton.CalcSize(new GUIContent("Paste as new")).x)))
                {
                    CopyAll();
                }
            }

            EditorGUILayout.EndHorizontal();

            // Reorder

            if (_draggedStartID >= 0 && _draggedEndID >= 0)
            {
                if (_draggedEndID != _draggedStartID)
                {
                    if (_draggedEndID > _draggedStartID)
                        _draggedEndID--;
                    _mmfeedbacks.MoveArrayElement(_draggedStartID, _draggedEndID);
                    _draggedStartID = _draggedEndID;
                }
            }

            if (_draggedStartID >= 0 || _draggedEndID >= 0)
            {
                switch (e.type)
                {
                    case EventType.MouseUp:
                        _draggedStartID = -1;
                        _draggedEndID = -1;
                        e.Use();
                        break;
                    default:
                        break;
                }
            }

            // Clean up

            bool wasRemoved = false;
            for (int i = _mmfeedbacks.arraySize - 1; i >= 0; i--)
            {
                if (_mmfeedbacks.GetArrayElementAtIndex(i).objectReferenceValue == null)
                {
                    wasRemoved = true;
                    _mmfeedbacks.DeleteArrayElementAtIndex(i);
                }
            }

            if (wasRemoved)
            {
                GameObject gameObject = (target as SequencePlayer).gameObject;
                foreach (var c in gameObject.GetComponents<Component>())
                {
                    c.hideFlags = HideFlags.None;
                }
            }

            // Apply changes

            serializedObject.ApplyModifiedProperties();

            // Draw debug

            SequenceStyle.DrawSection("All Feedbacks Debug");

            // Testing buttons

            EditorGUI.BeginDisabledGroup(!Application.isPlaying);
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Initialize", EditorStyles.miniButtonLeft))
                {
                    (target as SequencePlayer).Initialization();
                }
                if (GUILayout.Button("Play", EditorStyles.miniButtonMid))
                {
                    //(target as SequencePlayer).PlaySequences();
                }
                if (GUILayout.Button("Stop", EditorStyles.miniButtonMid))
                {
                    (target as SequencePlayer).StopSequences();
                }
                if (GUILayout.Button("Reset", EditorStyles.miniButtonMid))
                {
                    (target as SequencePlayer).ResetSequences();
                }
                EditorGUI.EndDisabledGroup();
                EditorGUI.BeginChangeCheck();
                {
                    _debugView = GUILayout.Toggle(_debugView, "Debug View", EditorStyles.miniButtonRight);

                    if (EditorGUI.EndChangeCheck())
                    {
                        foreach (var f in (target as SequencePlayer).Sequences)
                        {
                            f.hideFlags = _debugView ? HideFlags.HideInInspector : HideFlags.None;
                        }
                        UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            // Debug draw



            if (_debugView)
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.PropertyField(_mmfeedbacks, true);
                EditorGUI.EndDisabledGroup();
            }
        }

        protected virtual void InitializeSequence(int id)
        {
            SerializedProperty property = _mmfeedbacks.GetArrayElementAtIndex(id);
            SequenceBase seqence = property.objectReferenceValue as SequenceBase;
            seqence.Initialization(seqence.gameObject);
        }

        protected virtual void PlaySequence(int id)
        {
            SerializedProperty property = _mmfeedbacks.GetArrayElementAtIndex(id);
            SequenceBase seqence = property.objectReferenceValue as SequenceBase;
            seqence.Play(seqence.transform.position, 1f);
        }
        protected virtual void StopSequence(int id)
        {
            SerializedProperty property = _mmfeedbacks.GetArrayElementAtIndex(id);
            SequenceBase seqence = property.objectReferenceValue as SequenceBase;
            seqence.Stop(seqence.transform.position);
        }
        protected virtual void ResetSequence(int id)
        {
            SerializedProperty property = _mmfeedbacks.GetArrayElementAtIndex(id);
            SequenceBase seqence = property.objectReferenceValue as SequenceBase;
            seqence.ResetFeedback();
        }
        protected virtual void RemoveSequence(int id)
        {
            SerializedProperty property = _mmfeedbacks.GetArrayElementAtIndex(id);
            SequenceBase seqence = property.objectReferenceValue as SequenceBase;

            (target as SequencePlayer).Sequences.Remove(seqence);

            _editors.Remove(seqence);
            Undo.DestroyObjectImmediate(seqence);
        }
        protected virtual void CopySequence(int id)
        {
            SerializedProperty property = _mmfeedbacks.GetArrayElementAtIndex(id);
            SequenceBase seqence = property.objectReferenceValue as SequenceBase;

            SequenceCopy.Copy(new SerializedObject(seqence));
        }
        protected virtual void CopyAll()
        {
            SequenceCopy.CopyAll(target as SequencePlayer);
        }
        protected virtual void PasteSequence(int id)
        {
            SerializedProperty property = _mmfeedbacks.GetArrayElementAtIndex(id);
            SequenceBase seqence = property.objectReferenceValue as SequenceBase;

            SerializedObject serialized = new SerializedObject(seqence);

            SequenceCopy.Paste(serialized);
            serialized.ApplyModifiedProperties();
        }
        protected virtual void PasteAsNew()
        {
            SequenceBase newFeedback = AddSequence(SequenceCopy.Type);
            SerializedObject serialized = new SerializedObject(newFeedback);

            serialized.Update();
            SequenceCopy.Paste(serialized);
            serialized.ApplyModifiedProperties();
        }

        protected virtual void PasteAllAsNew()
        {
            serializedObject.Update();
            Undo.RecordObject(target, "Paste all Sequences");
            SequenceCopy.PasteAll(this);
            serializedObject.ApplyModifiedProperties();
        }


        protected virtual SequenceBase AddSequence(System.Type type)
        {
            GameObject gameObject = (target as SequencePlayer).gameObject;

            SequenceBase newFeedback = Undo.AddComponent(gameObject, type) as SequenceBase;
            newFeedback.hideFlags = _debugView ? HideFlags.None : HideFlags.HideInInspector;
            newFeedback.Label = SequencePathAttribute.GetFeedbackDefaultName(type);

            AddEditor(newFeedback);

            _mmfeedbacks.arraySize++;
            _mmfeedbacks.GetArrayElementAtIndex(_mmfeedbacks.arraySize - 1).objectReferenceValue = newFeedback;

            return newFeedback;
        }

            protected virtual void AddEditor(SequenceBase feedback)
            {
                if (feedback == null)
                {
                    return;
                }

                if (!_editors.ContainsKey(feedback))
                {
                    Editor editor = null;
                    CreateCachedEditor(feedback, null, ref editor);

                    _editors.Add(feedback, editor as Editor);
                }
            }
        }

    
}
