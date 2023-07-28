using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Collisions : CoreComponent
{
    public Vector2 ColliderSize { get => _boxCollider.size; }
    public Vector2 DefaultSize { get; private set; }

    private BoxCollider2D _boxCollider;

    public override void Initialize(Core entity)
    {
        base.Initialize(entity);

        _boxCollider = GetComponent<BoxCollider2D>();
        DefaultSize = _boxCollider.size;
    }

    public void SetColliderSize(Vector2 size) => _boxCollider.size = size;
    public void SetColliderSize(float width, float height) => _boxCollider.size = new Vector2(width, height);
    public void SetColliderSizeX(float width) => _boxCollider.size = new Vector2(width, _boxCollider.size.y);
    public void SetColliderHeight(float height) => _boxCollider.size = new Vector2(_boxCollider.size.x, height);
    public void SetColliderOffset(Vector2 offset) => _boxCollider.offset = offset;
    public void SetColliderOffset(float offsetX, float offsetY) => _boxCollider.offset = new Vector2(offsetX, offsetY);
    public void SetColliderOffsetX(float offsetX) => _boxCollider.offset = new Vector2(offsetX, _boxCollider.offset.y);
    public void SetColliderOffsetY(float offsetY) => _boxCollider.offset = new Vector2(_boxCollider.offset.x, offsetY);
}
