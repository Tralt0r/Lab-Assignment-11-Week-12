using UnityEngine;

public class TargetBuilder
{
    private float speed; private int points; private Vector3 size; private Color color; private Vector3 position;

    public TargetBuilder() { Reset(); }
    public TargetBuilder Reset()
    {
        speed = 1f; points = 10; size = Vector3.one; color = Color.white; position = Vector3.zero;
        return this;
    }
    //Set enemy settings
    public TargetBuilder SetSpeed(float targetSpeed)      { speed = targetSpeed; return this; }
    public TargetBuilder SetPoints(int targetPoints)       { points = targetPoints; return this; }
    public TargetBuilder SetSize(float targetSize)       { size = new Vector3(targetSize, targetSize, 1f); return this; }
    public TargetBuilder SetColor(Color targetColor)      { color = targetColor; return this; }
    public TargetBuilder SetPosition(Vector3 targetPosition) { position = targetPosition; return this; }

    //Build enemies with size, position, color
    public Target Build()
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = "Target";
        go.transform.position = position;
        go.transform.localScale = size;
        go.GetComponent<Renderer>().material.color = color;
        go.GetComponent<Collider>().isTrigger = true;
        Target target = go.AddComponent<Target>();
        target.speed = speed; target.points = points;
        Reset();
        return target;
    }
}

//Enemy types
public class TargetDirector
{
    private readonly TargetBuilder targetBuilder;
    public TargetDirector(TargetBuilder builder) { targetBuilder = builder; }

    public Target Enemy1(Vector3 pos, int dir)   { return targetBuilder.SetSpeed(2f * dir).SetSize(0.9f).SetPoints(10).SetColor(Color.yellow).SetPosition(pos).Build(); }
    public Target Enemy2(Vector3 pos, int dir) { return targetBuilder.SetSpeed(3.5f * dir).SetSize(0.7f).SetPoints(25).SetColor(Color.white).SetPosition(pos).Build(); }
    public Target Enemy3(Vector3 pos, int dir)    { return targetBuilder.SetSpeed(6f * dir).SetSize(0.45f).SetPoints(50).SetColor(Color.red).SetPosition(pos).Build(); }
}
