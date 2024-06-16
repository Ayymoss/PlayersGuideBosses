using TheLockedDoor.Enums;

namespace TheLockedDoor;

public class Door(string password)
{
    public DoorState DoorState { get; private set; } = DoorState.Closed;
    public LockState LockState { get; private set; } = LockState.Unlocked;

    public bool ToggleLock(string userPassword)
    {
        if (!string.Equals(userPassword, password, StringComparison.CurrentCulture)) return false;
        LockState = LockState == LockState.Locked ? LockState.Unlocked : LockState.Locked;
        return true;
    }

    public bool ToggleDoor()
    {
        if (LockState.Locked == LockState) return false;
        DoorState = DoorState == DoorState.Open ? DoorState.Closed : DoorState.Open;
        return true;
    }

    public bool SetPassword(string oldPassword, string newPassword)
    {
        if (!string.Equals(oldPassword, password, StringComparison.CurrentCulture)) return false;
        password = newPassword;
        return true;
    }
}
