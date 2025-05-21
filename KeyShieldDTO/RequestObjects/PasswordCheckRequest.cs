namespace KeyShieldDTO.RequestObjects;

public class PasswordCheckRequest(byte[] passwordHash)
{
    public byte[] PasswordHash { get; set; } = passwordHash;
}