public class StatInheritanceTest : PlayerStats {
    public override void InheritedAwake() { print("Awake"); }
    public override void InheritedStart() { print("Start"); }
    public override void InheritedUpdate(float time) { print("Update"); }
}
