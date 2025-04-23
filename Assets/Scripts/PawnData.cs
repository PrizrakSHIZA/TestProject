
public class PawnData
{
    public int hp = 100;
    public float moveSpeed = 3f;
    public float maxSpeed = 7f;
    public float rotationSpeed = 0;

    public PawnData()
    {
    }

    public PawnData(int hp, float moveSpeed, float maxSpeed, float rotationSpeed)
    {
        this.hp = hp;
        this.moveSpeed = moveSpeed;
        this.maxSpeed = maxSpeed;
        this.rotationSpeed = rotationSpeed;
    }
}