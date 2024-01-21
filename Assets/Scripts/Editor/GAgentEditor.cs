using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using RPG.GOAP;

[CustomEditor(typeof(GAgentVisual))]
[CanEditMultipleObjects]
public class GAgentVisualEditor : Editor
{
    void OnEnable()
    {

    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        serializedObject.Update();
        GAgentVisual agent = (GAgentVisual)target;
        GUILayout.Label("Name: " + agent.name);
        GUILayout.Label("Current Action: " + agent.thisAgent.CurrentAction);
        GUILayout.Label("Actions: ");
        foreach (GAction a in agent.thisAgent.actions)
        {
            string pre = "";
            string eff = "";

            foreach (KeyValuePair<States, int> p in a.Conditions)
                pre += p.Key + ", ";
            foreach (KeyValuePair<States, int> e in a.Effects)
                eff += e.Key + ", ";

            GUILayout.Label("====  " + a.GetType().Name + "(" + pre + ")(" + eff + ")");
        }
        GUILayout.Label("Goals: ");
        foreach (KeyValuePair<SubGoal, int> g in agent.thisAgent.goals)
        {
            GUILayout.Label("---: ");
            foreach (KeyValuePair<States, int> sg in g.Key.sGoals)
                GUILayout.Label("=====  " + sg.Key);
        }
        GUILayout.Label("Beliefs: ");
        foreach (KeyValuePair<States, int> sg in agent.thisAgent.Beliefs.States)
        {
            GUILayout.Label("=====  " + sg.Key);
        }

        GUILayout.Label("Inventory: ");
        foreach (GameObject g in agent.thisAgent.Inventory.Items)
        {
            GUILayout.Label("====  " + g.GetComponent<GPlaceOfInterest>().ResorceType);
        }

        serializedObject.ApplyModifiedProperties();
    }
}